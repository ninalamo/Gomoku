using System.Text.Json.Serialization;

namespace Gomoku_WebApp.Models;

public record CreateGameDto
{
    [JsonPropertyName("player_one")]
    [JsonRequired]
    public required string PlayerOne { get; init; }
    [JsonPropertyName("player_two")]
    [JsonRequired]
    public required string PlayerTwo { get; init; }
}