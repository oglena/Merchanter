using Merchanter.Classes;
using Merchanter.ServerService.Classes;
using Merchanter.ServerService.Models;
using Merchanter.ServerService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;

namespace Merchanter.ServerService.Controllers {
    [Route( "api/[controller]" )]
    [ApiController]
    public class ServerController( IServerService serverService ) :ControllerBase {

        [HttpGet( "GetServers" )]
        [Authorize]
        public async Task<ActionResult<BaseResponseModel>> GetServers() {
            //await Task.Run( () => {
            List<MerchanterServer> servers = await serverService.GetServers();
            if( servers != null ) {
                return Ok( new BaseResponseModel() { Success = servers != null, Data = servers != null ? servers : [], ErrorMessage = servers != null ? "" : "Error -1" } );
                //return Ok( products );
            }
            else {
                return BadRequest();
            }
        }
    }
}
