using Bot.Integration.Gitlab.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;

namespace Bot.App.Controllers
{
    [Route("[controller]")]
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
            _logger.LogInformation("Recived message");
            try
            {
                await _updateHandler.HandleUpdateAsync(_client, update, cancellationToken);
                return Ok();
            }
            catch (ApiRequestException exception)
            {
                _logger.LogError($"Telegram API error. Error code: {exception.ErrorCode}. Message: {exception.Message}");
                return BadRequest();
            }
            catch (ValidationException exception)
            {
                _logger.LogError($"Exception occured: {exception.Message}.");
                return BadRequest(exception.Message);
            }
            catch (AuthentificationException exception)
            {
                _logger.LogError($"Exception occured: {exception.Message}.");
                return Unauthorized(exception.Message);
            }
            catch (Exception exception)
            {
                _logger.LogCritical($"Exception occured: {exception.Message}.");
                throw;
            }
        }
    }
}
