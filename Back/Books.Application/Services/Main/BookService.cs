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
using Books.Core.Models;

namespace Books.Application.Services.Main;

public class BookService : IBookService
{
    private readonly IMapper _mapper;
    private readonly CreateBookValidator _createBookValidator;
    private readonly UpdateBookValidator _updateBookValidator;
    private readonly IBookRepository _bookRepository;
    private readonly IUnitOfWork _unitOfWork;

    public BookService(IMapper mapper, CreateBookValidator createBookValidator, UpdateBookValidator updateBookValidator,
        IBookRepository bookRepository, IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _createBookValidator = createBookValidator;
        _updateBookValidator = updateBookValidator;
        _bookRepository = bookRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<IEnumerable<BookDto>> GetAllBooksAsync()
    {
        var books = await _bookRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<BookDto>>(books);
    }

    public async Task<BookDto?> GetBookByIdAsync(Guid id)
    {
        var book = await _bookRepository.GetByIdAsync(id);
        return _mapper.Map<BookDto>(book);
    }

    public async Task<BookDto?> GetBookByTitleAsync(string title)
    {
        var book = await _bookRepository.FindAsync(b => b.Title == title);
        return _mapper.Map<BookDto>(book);
    }

    public async Task<BookDto> CreateBookAsync(CreateBookDto createBookDto)
    {
        if (await ExistsByTitleAsync(createBookDto.Title))
            throw new BookException(ExceptionType.CredentialsAlreadyExists, "BookAlreadyExists");
        
        var validator = await _createBookValidator.ValidateAsync(createBookDto);
        if (!validator.IsValid)
            throw new BookException(ExceptionType.InvalidRequest, 
                string.Join(", ", validator.Errors));

        await _unitOfWork.BeginTransactionAsync();
        try
        {
           var book = _mapper.Map<Book>(createBookDto);
           await _bookRepository.AddAsync(book);
           await _unitOfWork.CommitTransactionAsync();
           
           return _mapper.Map<BookDto>(book);
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
        
    }

    public async Task<BookDto> UpdateBookAsync(Guid id, UpdateBookDto updateBookDto)
    {
        var existingBook = await _bookRepository.GetByIdAsync(id);
        
        if (existingBook.Title != updateBookDto.Title && await ExistsByTitleAsync(updateBookDto.Title))
            throw new BookException(ExceptionType.CredentialsAlreadyExists, "BookAlreadyExists");
        
        var validator = await _updateBookValidator.ValidateAsync(updateBookDto);
        if (!validator.IsValid)
            throw new BookException(ExceptionType.InvalidRequest, 
                string.Join(", ", validator.Errors));
        
        await _unitOfWork.BeginTransactionAsync();

        try
        {
            _mapper.Map(updateBookDto, existingBook);
            await _bookRepository.UpdateAsync(new[] {existingBook});
            await _unitOfWork.CommitTransactionAsync();
            
            return _mapper.Map<BookDto>(existingBook);
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }

    public async Task<bool> DeleteBookAsync(Guid id)
    {
        await _bookRepository.DeleteAsync(id);

        return true;
    }

    public async Task<bool> ExistsByTitleAsync(string title)
        => await _bookRepository.AnyAsync(b => b.Title == title);

    public async Task<int> GetBooksCountAsync()
    {
        var books = await _bookRepository.GetAllAsync();
        return books.Count();
    }

    public async Task<IEnumerable<BookDto>> GetBooksPageAsync(int pageNumber, int pageSize)
    {
        var books = await _bookRepository.GetAllAsync();
        if (pageNumber <= 0 || pageSize <= 0)
            throw new BookException(ExceptionType.BadRequest, "PaginationError");
        
        var pagedBooks = books
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize);
        
        return _mapper.Map<IEnumerable<BookDto>>(pagedBooks);
    }
}