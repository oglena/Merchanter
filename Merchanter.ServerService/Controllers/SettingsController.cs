using Merchanter.Classes;
using Merchanter.Classes.Settings;
using Merchanter.ServerService.Classes;
using Merchanter.ServerService.Models;
using Merchanter.ServerService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Merchanter.ServerService.Controllers {
    [Route( "api/[controller]" )]
    [ApiController]
    public class SettingsController( ISettingsService settingsService ) :ControllerBase {

        [HttpGet( "{CID}/GetCustomerSettings" )]
        [Authorize]
        public async Task<ActionResult<BaseResponseModel<SettingsMerchanter>>> GetCustomerSettings( string CID ) {
            int customer_id;
            if( int.TryParse( CID, out customer_id ) && customer_id > 0 ) {
                SettingsMerchanter settings = await settingsService.GetSettings( customer_id );
                if( settings != null ) {
                    return Ok( new BaseResponseModel<SettingsMerchanter>() { Success = true, Data = settings, ErrorMessage = "" } );
                }
                else {
                    return Ok( new BaseResponseModel<SettingsMerchanter>() { Success = false, Data = null, ErrorMessage = "Error getting settings." } );
                }
            }
            return BadRequest( "Invalid customer ID." );
        }
    }
}
