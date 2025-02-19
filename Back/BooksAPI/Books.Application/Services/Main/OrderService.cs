using AutoMapper;
using Books.Application.Exceptions;
using Books.Application.Validators.Create;
using Books.Application.Validators.Update;
using Books.Core.Abstractions.Repositories;
using Books.Core.Abstractions.Repositories.Main;
using Books.Core.Abstractions.Services.Main;
using Books.Core.Abstractions.UOW;
using Books.Core.Dtos.Create;
using Books.Core.Dtos.Read;
using Books.Core.Dtos.Update;
using Books.Core.Entities;
using Books.Core.Enums;

namespace Books.Application.Services.Main;

public class OrderService : IOrderService
{
    private readonly IMapper _mapper;
    private readonly CreateOrderValidator _createOrderValidator;
    private readonly UpdateOrderValidator _updateOrderValidator;
    private readonly IOrderRepository _orderRepository;
    private readonly IUnitOfWork _unitOfWork;

    public OrderService(IMapper mapper, CreateOrderValidator createOrderValidator,
        UpdateOrderValidator updateOrderValidator, IOrderRepository orderRepository, IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _createOrderValidator = createOrderValidator;
        _updateOrderValidator = updateOrderValidator;
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
    }
    public async Task<IEnumerable<OrderDto>> GetAllOrdersAsync()
    {
        var orders = await _orderRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<OrderDto>>(orders);
    }

    public async Task<OrderDto?> GetOrderByIdAsync(Guid id)
    {
        var order = await _orderRepository.GetByIdAsync(id);
        return _mapper.Map<OrderDto>(order);
    }

    public async Task<OrderDto> CreateOrderAsync(CreateOrderDto createOrderDto)
    {
        var validator = await _createOrderValidator.ValidateAsync(createOrderDto);
        if (!validator.IsValid)
            throw new BookException(ExceptionType.InvalidRequest,
                string.Join(", ", validator.Errors.Select(e => e.ErrorMessage).FirstOrDefault()));

        await _unitOfWork.BeginTransactionAsync();
        try
        {
            var order = _mapper.Map<Order>(createOrderDto);
            if (!Enum.TryParse(createOrderDto.StatusId.ToString(), out OrderStatus orderStatus))
                throw new BookException(ExceptionType.InvalidRequest, "InvalidBookStatus");
           
            order.Status = orderStatus;
            await _orderRepository.AddAsync(order);
            await _unitOfWork.CommitTransactionAsync();

            return _mapper.Map<OrderDto>(order);
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }

    public async Task<OrderDto> UpdateOrderAsync(Guid id, UpdateOrderDto updateOrderDto)
    {
        var existingOrder = await _orderRepository.GetByIdAsync(id);
        
        var validator = await _updateOrderValidator.ValidateAsync(updateOrderDto);
        if (!validator.IsValid)
            throw new BookException(ExceptionType.InvalidRequest,
                string.Join(", ", validator.Errors.Select(e => e.ErrorMessage).FirstOrDefault()));
        
        await _unitOfWork.BeginTransactionAsync();
        try
        {
            _mapper.Map(updateOrderDto, existingOrder);
            await _orderRepository.UpdateAsync(new[] { existingOrder });
            await _unitOfWork.CommitTransactionAsync();

            return _mapper.Map<OrderDto>(existingOrder);
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }

    public async Task<bool> DeleteOrderAsync(Guid id)
    {
        await _unitOfWork.BeginTransactionAsync();

        try
        {
            await _orderRepository.DeleteAsync(id);
            await _unitOfWork.CommitTransactionAsync();

            return true;
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }

    public async Task<int> GetOrdersCountAsync()
        => await _orderRepository.CountAsync();

    public async Task<IEnumerable<OrderDto>> GetOrdersPageAsync(int pageNumber, int pageSize)
    {
        if (pageNumber <= 0 || pageSize <= 0)
            throw new BookException(ExceptionType.BadRequest, "PaginationError");

        var orders = await _orderRepository.GetAllAsync();
        var pagedOrders = orders
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize);

        return _mapper.Map<IEnumerable<OrderDto>>(pagedOrders);
    }
}