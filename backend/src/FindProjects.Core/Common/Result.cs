namespace FindProjects.Core.Common;

public class Result
{
    public bool IsSuccess { get; set; }
    public string? ErrorMessage { get; set; }

    public static Result Success()
    {
        return new Result { IsSuccess = true, ErrorMessage = null};
    }
    
    public static Result Failed(string errorMessage)
    {
        return new Result { IsSuccess = false, ErrorMessage = errorMessage};
    }
}