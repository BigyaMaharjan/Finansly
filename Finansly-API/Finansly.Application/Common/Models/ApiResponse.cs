namespace Finansly.Application.Common.Models;

public sealed record ApiResponse<T>
{
    public bool Success { get; init; }
    public int StatusCode { get; init; }
    public string? Message { get; init; }
    public T? Data { get; init; }

    public static ApiResponse<T> Ok(T data, string? message = null) =>
        new() { Success = true, StatusCode = 200, Data = data, Message = message };

    public static ApiResponse<T> Created(T data) =>
        new() { Success = true, StatusCode = 201, Data = data };

    public static ApiResponse<T> Fail(int statusCode, string message) =>
        new() { Success = false, StatusCode = statusCode, Message = message };
}