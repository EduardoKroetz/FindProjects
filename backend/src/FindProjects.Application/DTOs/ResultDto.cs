namespace FindProjects.Application.DTOs;

public class ResultDto<T>
{
    public int StatusCode { get; set; }
    public bool Success { get; set; }
    public List<string> Errors { get; set; } = [];
    public T? Data { get; set; } 

    public static ResultDto<T> SuccessResult(T data, int statusCode = 200)
    {
        return new ResultDto<T>() { Data = data, StatusCode = statusCode, Success = true, Errors = [] };
    }
    
    public static ResultDto<T> BadResult(List<string> errors, int statusCode = 400)
    {
        return new ResultDto<T>() { Data = default, StatusCode = statusCode, Success = false, Errors = errors };
    }
    
    public static ResultDto<T> BadResult(string error, int statusCode = 400)
    {
        return new ResultDto<T>()
        {
            Data = default, 
            StatusCode = statusCode, 
            Success = false, 
            Errors = new List<string>() { error }
        };
    }
}