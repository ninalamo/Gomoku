using Gomoku_WebApp.Models;
using Gomoku.Application.Commands.AddGame;
using Gomoku.Application.Commands.AddPebble;
using Gomoku.Application.Commands.UpdateNextPlayer;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Gomoku_WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<GameController> _logger;

        public GameController(IMediator mediator, ILogger<GameController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> CreateGame([FromBody] CreateGameDto model)
        {
            _logger.LogInformation($"Request received in {nameof(CreateGameDto)}");

            var result = await _mediator.Send(new AddGameCommand(model.PlayerOne, model.PlayerTwo));

            return Ok(ResponseDto.Success(new []
            {
                new {result.GameId, Board = result.Cells}
            }));

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutPebble([FromRoute]Guid id, [FromQuery]int row, [FromQuery]int column,  [FromQuery]Guid playerId)
        {
            _logger.LogInformation($"Request received in {nameof(PutPebble)}");

            var result = await _mediator.Send(new AddPebbleCommand(row, column, id, playerId));

            return Ok(ResponseDto.Success(result));

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetNextPlayer(Guid id)
        {
            _logger.LogInformation($"Request received in {nameof(GetNextPlayer)}");

            var result = await _mediator.Send(new UpdateGameCurrentPlayerCommand(id));

            return Ok(ResponseDto.Success(result));

        }
        
        
    }
}
