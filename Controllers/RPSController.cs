using Microsoft.AspNetCore.Mvc;
using RPS.Network.Models;
using RPS.Network.Services;
using TodoApp.Services;
using TodoDB.Todos;

namespace RPS.Network.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RPSController : ControllerBase
    {
        private readonly RPSService _rpsService;

        public RPSController(RPSService RPSsService) =>
            _rpsService = RPSsService;

        #region Admin
        [HttpGet]
        public async Task<List<RPSServerInstance>> GetAllServers() =>
            await _rpsService.GetAsync();

        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<RPSServerInstance>> GetServer(string id)
        {
            if (id == null)
            {
                return BadRequest("Invalid request. Todo data is missing.");
            }
            var server = await _rpsService.GetAsync(id);

            if (server is null)
            {
                return NotFound();
            }

            return server;
        }

        [HttpDelete]
        public async Task<ActionResult<RPSServerInstance>> Delete([FromBody] string id)
        {
            if (id == null)
            {
                return BadRequest("Invalid request. Todo data is missing.");
            }

            var server = await _rpsService.GetAsync(id);

            if (server is null)
            {
                return NotFound("ID passed is not found.");
            }

            await _rpsService.RemoveAsync(id);

            return server;
        }
        #endregion

        #region For Client

        private async Task<RPSServerInstance> GetBaseServer()
        {
            List<RPSServerInstance> serverList = await _rpsService.GetAsync();

            if (serverList.Count == 0)
            {
                await _rpsService.CreateAsync(new RPSServerInstance());
                serverList = await _rpsService.GetAsync();
                serverList[0].IsP1Ready = true;
                await _rpsService.UpdateAsync(serverList[0].Id, serverList[0]);
            }
            else
            {
                serverList[0].IsP2Ready = true;
                await _rpsService.UpdateAsync(serverList[0].Id, serverList[0]);
            }
            return serverList[0];
        }


        [HttpGet]
        public async Task<ActionResult<RPSServerInstance>> GetRPSGame()
        {
            RPSServerInstance instance = await GetBaseServer();
            return instance;
        }

        [HttpGet("{id:length(24)},{isPlayerOne:bool}")]
        public async Task<ActionResult<bool>> IsOpponentReady([FromBody] string id, [FromBody] bool isPlayerOne)
        {
            if (id == null)
            {
                return BadRequest("Invalid request. Todo data is missing.");
            }
            var server = await _rpsService.GetAsync(id);

            if (server is null)
            {
                return NotFound();
            }
            
            if(isPlayerOne)
                return server.IsP2Ready;
            else
                return server.IsP1Ready;
        }

        [HttpGet("{id:length(24)}, {myRole:length(24)},{isPlayerOne:bool}")]
        public async Task<ActionResult<String>> GetOpponentHand([FromBody] string id, [FromBody] string myRole, [FromBody] bool isPlayerOne)
        {
            if (id == null || myRole == null)
            {
                return BadRequest("Invalid request. Todo data is missing.");
            }
            var server = await _rpsService.GetAsync(id);

            if (server is null)
            {
                return NotFound();
            }

            if (isPlayerOne)
            {
                if(server.P1Role == "" || server.P2Role != myRole)
                {
                    server.P1Role = myRole;
                    await _rpsService.UpdateAsync(id, server);
                }
            }
            else
            {
                if(server.P2Role == "" || server.P2Role != myRole)
                {
                    server.P2Role = myRole;
                    await _rpsService.UpdateAsync(id, server);
                }
            }

            
            if (isPlayerOne)
            {
                return server.P2Role;
            }
            else
                return server.P1Role;
        }

        [HttpGet]
        public async Task<ActionResult<bool>> IsGameEnded(string id)
        {
            if(id == null)
            {
                return BadRequest("Invalid request. Todo data is missing.");
            }
            var server = await _rpsService.GetAsync(id);
            if (server == null)
            {
                return NotFound();
            }
            if(server.IsP1Ready && server.IsP2Ready)
            {
                if (!server.InResult)
                {
                    server.InResult = true;
                    await _rpsService.UpdateAsync(id, server);
                    return false;
                }
                else
                {
                    server.ResetContent();
                    await _rpsService.UpdateAsync(id, server);
                    return true;
                }
            }
            
            return false;
        }

        #endregion
    }
}
