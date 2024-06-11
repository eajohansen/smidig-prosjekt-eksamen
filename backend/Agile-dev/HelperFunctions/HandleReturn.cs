namespace agile_dev.Models;

public class HandleReturn<T>
{
    public bool IsSuccess { get; private set; }
    public T? Value { get; private set; }
    public string ErrorMessage{ get; private set; }

    private HandleReturn(bool isSuccess, T? value, string errorMessage)
    {
        IsSuccess = isSuccess;
        Value = value;
        ErrorMessage = errorMessage;
    }
    
    private HandleReturn(bool isSuccess, T? value)
    {
        IsSuccess = isSuccess;
        Value = value;
    }
    
    private HandleReturn(bool isSuccess, string errorMessage)
    {
        IsSuccess = isSuccess;
        ErrorMessage = errorMessage;
    }
    
    private HandleReturn(bool isSuccess)
    {
        IsSuccess = isSuccess;
    }

    public static HandleReturn<T> Success(T? value)
    {
        return new HandleReturn<T>(true, value);
    }
    public static HandleReturn<T> Success()
    {
        return new HandleReturn<T>(true);
    }

    public static HandleReturn<T> Failure(string errorMessage)
    {
        return new HandleReturn<T>(false, errorMessage);
    }

    public static HandleReturn<T> Failure(T? value, string errorMessage)
    {
        return new HandleReturn<T>(true, value, errorMessage);
    }
}