namespace Books.Core.Dtos.Read;

public class ResponseDto<T>
{
    public T Data { get; set; }
    public bool Success { get; set; }
    public int Code { get; set; }
    public string? Message { get; set; }
    public DateTime RequestDate { get; set; }
    public long Ticks { get; set; }
}