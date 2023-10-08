using System.Text.Json.Serialization;

namespace Gomoku_WebApp.Models;

public record ResponseDto
{
    [JsonPropertyName("error_message")]
    public string? ErrorMessage { get; init; }
    
    [JsonPropertyName("is_success")]
    public bool IsSuccess { get; init; }
    [JsonPropertyName("data")]
    public object[]? Data { get; init; }

    public static ResponseDto Success(object[] obj) => new ResponseDto()
    {
        Data = obj, ErrorMessage = string.Empty, IsSuccess = true
    };
    
    public static ResponseDto Success() => new ResponseDto()
    {
        Data = Array.Empty<object>(), ErrorMessage = string.Empty, IsSuccess = true
    };
    
    public static ResponseDto Success(object obj) => new ResponseDto()
    {
        Data = new []{obj}, ErrorMessage = string.Empty, IsSuccess = true
    };

    public static ResponseDto Failed(string errorMessage) => new()
    {
        Data = Array.Empty<object>(), ErrorMessage = errorMessage, IsSuccess = false
    };
}