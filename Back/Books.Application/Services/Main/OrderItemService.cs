using AutoMapper;
using Books.Application.Exceptions;
using Books.Application.Validators.Create;
using Books.Application.Validators.Update;
using Books.Core.Abstractions.Repositories;
using Books.Core.Abstractions.Services.Main;
using Books.Core.Abstractions.UOW;
using Books.Core.Dtos.Create;
using Books.Core.Dtos.Read;
using Books.Core.Dtos.Update;
using Books.Core.Models;

namespace Books.Application.Services.Main;

public class OrderItemService : IOrderItemService
{
    private readonly IMapper _mapper;
    private readonly CreateOrderItemValidator _createOrderItemValidator;
    private readonly UpdateOrderItemValidator _updateOrderItemValidator;
    private readonly IOrderItemRepository _orderItemRepository;
    private readonly IUnitOfWork _unitOfWork;

    public OrderItemService(IMapper mapper, CreateOrderItemValidator createOrderItemValidator,
        UpdateOrderItemValidator updateOrderItemValidator, IOrderItemRepository orderItemRepository, IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _createOrderItemValidator = createOrderItemValidator;
        _updateOrderItemValidator = updateOrderItemValidator;
        _orderItemRepository = orderItemRepository;
        _unitOfWork = unitOfWork;
    }
    public async Task<IEnumerable<OrderItemDto>> GetAllOrderItemsAsync()
    {
        var orderItems = await _orderItemRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<OrderItemDto>>(orderItems);
    }

    public async Task<OrderItemDto?> GetOrderItemByIdAsync(Guid id)
    {
        var orderItem = await _orderItemRepository.GetByIdAsync(id);
        return _mapper.Map<OrderItemDto>(orderItem);
    }

    public async Task<OrderItemDto> CreateOrderItemAsync(CreateOrderItemDto createOrderItemDto)
    {
        var validator = await _createOrderItemValidator.ValidateAsync(createOrderItemDto);
        if (!validator.IsValid)
            throw new BookException(ExceptionType.InvalidRequest,
                string.Join(", ", validator.Errors));
        
        await _unitOfWork.BeginTransactionAsync();

        try
        {
            var orderItem = _mapper.Map<OrderItem>(createOrderItemDto);
            await _orderItemRepository.AddAsync(orderItem);
            await _unitOfWork.BeginTransactionAsync();
            
            return _mapper.Map<OrderItemDto>(orderItem);
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }

    public async Task<OrderItemDto> UpdateOrderItemAsync(Guid id, UpdateOrderItemDto updateOrderItemDto)
    {
        var existingOrderItem = await _orderItemRepository.GetByIdAsync(id);
        
        var validator = await _updateOrderItemValidator.ValidateAsync(updateOrderItemDto);
        if (!validator.IsValid)
            throw new BookException(ExceptionType.InvalidRequest,
                string.Join(", ", validator.Errors));
        
        await _unitOfWork.BeginTransactionAsync();

        try
        {
            _mapper.Map(updateOrderItemDto, existingOrderItem);
            await _orderItemRepository.UpdateAsync(new[] { existingOrderItem });
            await _unitOfWork.BeginTransactionAsync();
            
            return _mapper.Map<OrderItemDto>(existingOrderItem);
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }

    public async Task<bool> DeleteOrderItemAsync(Guid id)
    {
        await _unitOfWork.BeginTransactionAsync();

        try
        {
            await _orderItemRepository.DeleteAsync(id);
            await _unitOfWork.CommitTransactionAsync();

            return true;
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }

    public async Task<int> GetOrderItemsCountAsync()
        => await _orderItemRepository.CountAsync();

    public async Task<IEnumerable<OrderItemDto>> GetOrderItemsPageAsync(int pageNumber, int pageSize)
    {
        if (pageNumber <= 0 || pageSize <= 0)
            throw new BookException(ExceptionType.BadRequest, "PaginationError");

        var orderItems = await _orderItemRepository.GetAllAsync();
        var pagedOrderItems = orderItems
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize);

        return _mapper.Map<IEnumerable<OrderItemDto>>(pagedOrderItems);
    }
}