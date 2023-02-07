using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;

namespace Bot.App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UpdateController : ControllerBase
    {
        private readonly ITelegramBotClient _client;
        private readonly ILogger<UpdateController> _logger;
        private readonly IUpdateHandler _updateHandler;
        public UpdateController(ITelegramBotClient client, ILogger<UpdateController> logger, IUpdateHandler updateHandler)
        {
            _client = client;
            _logger = logger;
            _updateHandler = updateHandler;
        }

        [HttpPost]
        public async Task<IActionResult> PostUpdateAsync([FromBody] Update update, CancellationToken cancellationToken)
        {
            if (update is null)
                return BadRequest();

            await _updateHandler.HandleUpdateAsync(_client, update, cancellationToken);
            
            return Ok();
        }
    }
}
