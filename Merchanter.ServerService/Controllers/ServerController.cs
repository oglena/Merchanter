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
        public async Task<ActionResult<BaseResponseModel<List<MerchanterServer>>>> GetServers() {
            //await Task.Run( () => {
            List<MerchanterServer> servers = await serverService.GetServers();
            if( servers != null ) {
                return Ok( new BaseResponseModel<List<MerchanterServer>>() { Success = servers != null, Data = servers != null ? servers : [], ErrorMessage = servers != null ? "" : "Error -1" } );
                //return Ok( products );
            }
            else {
                return BadRequest();
            }
        }

        [HttpGet( "{CID}/StartServer" )]
        [Authorize]
        public async Task<ActionResult<BaseResponseModel<MerchanterServer>>> StartServer( string CID ) {
            int customer_id;
            if( int.TryParse( CID, out customer_id ) && customer_id > 0 ) {
                MerchanterServer started_server = await serverService.StartServer( customer_id );
                if( started_server != null ) {
                    return Ok( new BaseResponseModel<MerchanterServer>() { Success = started_server != null, Data = started_server != null ? started_server : new(), ErrorMessage = started_server != null ? "" : "Error -1" } );
                    //return Ok( products );
                }
                else {
                    return BadRequest();
                }
            }
            return BadRequest();
        }

        [HttpGet( "{CID}/StopServer" )]
        [Authorize]
        public async Task<ActionResult<BaseResponseModel<MerchanterServer>>> StopServer( string CID ) {
            int customer_id;
            if( int.TryParse( CID, out customer_id ) && customer_id > 0 ) {
                MerchanterServer stopped_server = await serverService.StopServer( customer_id );
                if( stopped_server != null ) {
                    return Ok( new BaseResponseModel<MerchanterServer>() { Success = stopped_server != null, Data = stopped_server != null ? stopped_server : new(), ErrorMessage = stopped_server != null ? "" : "Error -1" } );
                    //return Ok( products );
                }
                else {
                    return BadRequest();
                }
            }
            return BadRequest();
        }
    }
}
