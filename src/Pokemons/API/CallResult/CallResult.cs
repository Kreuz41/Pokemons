namespace Pokemons.API.CallResult;

public class CallResult<T>
{
    private CallResult()
    { }
    
    public bool Status { get; set; }
    public string? Message { get; set; }
    public T? Data { get; set; }

    public static CallResult<T> Success(T data) => 
        new() { Status = true, Data = data};
    
    public static CallResult<T> Failure(string message) => 
        new() { Status = false, Message = message };
}