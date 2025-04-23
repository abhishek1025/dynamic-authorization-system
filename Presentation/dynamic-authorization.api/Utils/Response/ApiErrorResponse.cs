namespace dynamic_authorization.api;

public class ApiErrorResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = "SERVER ERROR: Unexpected Error Occured.";
    public int StatusCode { get; set; }
}