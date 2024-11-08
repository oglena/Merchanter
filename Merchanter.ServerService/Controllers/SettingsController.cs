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
                SettingsMerchanter settings = await settingsService.GetCustomerSettings( customer_id );
                if( settings != null ) {
                    return Ok( new BaseResponseModel<SettingsMerchanter>() { Success = true, Data = settings, ErrorMessage = "" } );
                }
                else {
                    return Ok( new BaseResponseModel<SettingsMerchanter>() { Success = false, Data = null, ErrorMessage = "Error getting settings." } );
                }
            }
            return BadRequest( "Invalid customer ID." );
        }

        [HttpPut( "{CID}/SaveGeneralSettings" )]
        [Authorize]
        public async Task<ActionResult<BaseResponseModel<SettingsGeneral>>> SaveGeneralSettings( string CID, [FromBody] SettingsGeneral _settings ) {
            int customer_id;
            if( int.TryParse( CID, out customer_id ) && customer_id > 0 ) {
                if( await settingsService.SaveGeneralSettings( customer_id, _settings ) ) {
                    var saved_settings = settingsService.GetCustomerSettings( customer_id ).Result.settings;
                    if( saved_settings != null ) {
                        return Ok( new BaseResponseModel<SettingsGeneral>() { Success = true, Data = saved_settings, ErrorMessage = "" } );
                    }
                }
                else {
                    return Ok( new BaseResponseModel<SettingsGeneral>() { Success = false, Data = null, ErrorMessage = "Error saving general settings." } );
                }
            }
            return BadRequest( "Invalid customer ID." );
        }
    }
}
