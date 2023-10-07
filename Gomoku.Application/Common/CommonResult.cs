namespace Gomoku.Application.Common;

public record CommonResult
{
    public object?[] Data { get; init; }
    
    public static CommonResult Create(object? data)
    {
        return new CommonResult()
        {
            Data = data == null ? new []{data} : Array.Empty<object>()
        };
    }
    
    public static CommonResult Create(params object[] data)
    {
        return new CommonResult()
        {
            Data = data 
        };
    }
}

