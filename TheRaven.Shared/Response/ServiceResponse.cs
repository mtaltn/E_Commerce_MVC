namespace TheRaven.Shared.Response;

public class ServiceResponse<T>
{
    public string Message { get; set; } = string.Empty;
    public bool Success { get; set; } = false;
    public T Data { get; set; }
}