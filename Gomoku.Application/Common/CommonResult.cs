namespace Gomoku.Application.Common;

public record CommonResult
{
    public object[] Data { get; init; }
    
    public static CommonResult Create(object[]? data)
    {
        return new CommonResult()
        {
            Data = data ?? Array.Empty<object>()
        };
    }
    
  
}

