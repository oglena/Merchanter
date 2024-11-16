using Merchanter.Classes.Settings;
using Merchanter.ServerService.Models;
using Merchanter.ServerService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

        [HttpPut( "{CID}/SaveEntegraSettings" )]
        [Authorize]
        public async Task<ActionResult<BaseResponseModel<SettingsEntegra>>> SaveEntegraSettings( string CID, [FromBody] SettingsEntegra _settings ) {
            int customer_id;
            if( int.TryParse( CID, out customer_id ) && customer_id > 0 ) {
                if( await settingsService.SaveEntegraSettings( customer_id, _settings ) ) {
                    var saved_settings = settingsService.GetCustomerSettings( customer_id ).Result.entegra;
                    if( saved_settings != null ) {
                        return Ok( new BaseResponseModel<SettingsEntegra>() { Success = true, Data = saved_settings, ErrorMessage = "" } );
                    }
                }
                else {
                    return Ok( new BaseResponseModel<SettingsEntegra>() { Success = false, Data = null, ErrorMessage = "Error saving entegra settings." } );
                }
            }
            return BadRequest( "Invalid customer ID." );
        }

        [HttpPut( "{CID}/SaveMagentoSettings" )]
        [Authorize]
        public async Task<ActionResult<BaseResponseModel<SettingsMagento>>> SaveMagentoSettings( string CID, [FromBody] SettingsMagento _settings ) {
            int customer_id;
            if( int.TryParse( CID, out customer_id ) && customer_id > 0 ) {
                if( await settingsService.SaveMagentoSettings( customer_id, _settings ) ) {
                    var saved_settings = settingsService.GetCustomerSettings( customer_id ).Result.magento;
                    if( saved_settings != null ) {
                        return Ok( new BaseResponseModel<SettingsMagento>() { Success = true, Data = saved_settings, ErrorMessage = "" } );
                    }
                }
                else {
                    return Ok( new BaseResponseModel<SettingsMagento>() { Success = false, Data = null, ErrorMessage = "Error saving magento settings." } );
                }
            }
            return BadRequest( "Invalid customer ID." );
        }

        [HttpPut( "{CID}/SaveNetsisSettings" )]
        [Authorize]
        public async Task<ActionResult<BaseResponseModel<SettingsNetsis>>> SaveNetsisSettings( string CID, [FromBody] SettingsNetsis _settings ) {
            int customer_id;
            if( int.TryParse( CID, out customer_id ) && customer_id > 0 ) {
                if( await settingsService.SaveNetsisSettings( customer_id, _settings ) ) {
                    var saved_settings = settingsService.GetCustomerSettings( customer_id ).Result.netsis;
                    if( saved_settings != null ) {
                        return Ok( new BaseResponseModel<SettingsNetsis>() { Success = true, Data = saved_settings, ErrorMessage = "" } );
                    }
                }
                else {
                    return Ok( new BaseResponseModel<SettingsNetsis>() { Success = false, Data = null, ErrorMessage = "Error saving netsis settings." } );
                }
            }
            return BadRequest( "Invalid customer ID." );
        }

        [HttpPut( "{CID}/SaveShipmentSettings" )]
        [Authorize]
        public async Task<ActionResult<BaseResponseModel<SettingsShipment>>> SaveShipmentSettings( string CID, [FromBody] SettingsShipment _settings ) {
            int customer_id;
            if( int.TryParse( CID, out customer_id ) && customer_id > 0 ) {
                if( await settingsService.SaveShipmentSettings( customer_id, _settings ) ) {
                    var saved_settings = settingsService.GetCustomerSettings( customer_id ).Result.shipment;
                    if( saved_settings != null ) {
                        return Ok( new BaseResponseModel<SettingsShipment>() { Success = true, Data = saved_settings, ErrorMessage = "" } );
                    }
                }
                else {
                    return Ok( new BaseResponseModel<SettingsShipment>() { Success = false, Data = null, ErrorMessage = "Error saving shipment settings." } );
                }
            }
            return BadRequest( "Invalid customer ID." );
		}

		[HttpPut( "{CID}/SaveOrderSettings" )]
		[Authorize]
		public async Task<ActionResult<BaseResponseModel<SettingsOrder>>> SaveOrderSettings( string CID, [FromBody] SettingsOrder _settings ) {
			int customer_id;
			if( int.TryParse( CID, out customer_id ) && customer_id > 0 ) {
				if( await settingsService.SaveOrderSettings( customer_id, _settings ) ) {
					SettingsOrder? saved_settings = settingsService.GetCustomerSettings( customer_id ).Result.order;
					if( saved_settings != null ) {
						return Ok( new BaseResponseModel<SettingsOrder>() { Success = true, Data = saved_settings, ErrorMessage = "" } );
					}
				}
				else {
					return Ok( new BaseResponseModel<SettingsOrder>() { Success = false, Data = null, ErrorMessage = "Error saving order settings." } );
				}
			}
			return BadRequest( "Invalid customer ID." );
		}
	}
}
