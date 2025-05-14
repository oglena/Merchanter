using Merchanter.Classes.Settings;
using MerchanterApp.ApiService.Models;
using MerchanterApp.ApiService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MerchanterApp.ApiService.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class SettingsController(ISettingsService settingsService) : ControllerBase {
        [HttpGet("{CID}/GetCustomerSettings")]
        [Authorize]
        public async Task<ActionResult<BaseResponseModel<SettingsMerchanter>>> GetCustomerSettings(string CID) {
            int customer_id;
            if (int.TryParse(CID, out customer_id) && customer_id > 0) {
                SettingsMerchanter settings = await settingsService.GetCustomerSettings(customer_id);
                if (settings != null) {
                    return Ok(new BaseResponseModel<SettingsMerchanter>() { Success = true, Data = settings, ErrorMessage = "" });
                }
                else {
                    return Ok(new BaseResponseModel<SettingsMerchanter>() { Success = false, Data = null, ErrorMessage = "Error getting settings." });
                }
            }
            return BadRequest("Invalid customer ID.");
        }

        [HttpPut("{CID}/SaveGeneralSettings")]
        [Authorize]
        public async Task<ActionResult<BaseResponseModel<SettingsGeneral>>> SaveGeneralSettings(string CID, [FromBody] SettingsGeneral _settings) {
            int customer_id;
            if (int.TryParse(CID, out customer_id) && customer_id > 0) {
                if (await settingsService.SaveGeneralSettings(customer_id, _settings)) {
                    var saved_settings = settingsService.GetCustomerSettings(customer_id).Result.settings;
                    if (saved_settings != null) {
                        return Ok(new BaseResponseModel<SettingsGeneral>() { Success = true, Data = saved_settings, ErrorMessage = "" });
                    }
                }
                else {
                    return Ok(new BaseResponseModel<SettingsGeneral>() { Success = false, Data = null, ErrorMessage = "Error saving general settings." });
                }
            }
            return BadRequest("Invalid customer ID.");
        }

        [HttpPut("{CID}/SaveEntegraSettings")]
        [Authorize]
        public async Task<ActionResult<BaseResponseModel<SettingsEntegra>>> SaveEntegraSettings(string CID, [FromBody] SettingsEntegra _settings) {
            int customer_id;
            if (int.TryParse(CID, out customer_id) && customer_id > 0) {
                if (await settingsService.SaveEntegraSettings(customer_id, _settings)) {
                    var saved_settings = settingsService.GetCustomerSettings(customer_id).Result.entegra;
                    if (saved_settings != null) {
                        return Ok(new BaseResponseModel<SettingsEntegra>() { Success = true, Data = saved_settings, ErrorMessage = "" });
                    }
                }
                else {
                    return Ok(new BaseResponseModel<SettingsEntegra>() { Success = false, Data = null, ErrorMessage = "Error saving entegra settings." });
                }
            }
            return BadRequest("Invalid customer ID.");
        }

        [HttpPut("{CID}/SaveMagentoSettings")]
        [Authorize]
        public async Task<ActionResult<BaseResponseModel<SettingsMagento>>> SaveMagentoSettings(string CID, [FromBody] SettingsMagento _settings) {
            int customer_id;
            if (int.TryParse(CID, out customer_id) && customer_id > 0) {
                if (await settingsService.SaveMagentoSettings(customer_id, _settings)) {
                    var saved_settings = settingsService.GetCustomerSettings(customer_id).Result.magento;
                    if (saved_settings != null) {
                        return Ok(new BaseResponseModel<SettingsMagento>() { Success = true, Data = saved_settings, ErrorMessage = "" });
                    }
                }
                else {
                    return Ok(new BaseResponseModel<SettingsMagento>() { Success = false, Data = null, ErrorMessage = "Error saving magento settings." });
                }
            }
            return BadRequest("Invalid customer ID.");
        }

        [HttpPut("{CID}/SaveNetsisSettings")]
        [Authorize]
        public async Task<ActionResult<BaseResponseModel<SettingsNetsis>>> SaveNetsisSettings(string CID, [FromBody] SettingsNetsis _settings) {
            int customer_id;
            if (int.TryParse(CID, out customer_id) && customer_id > 0) {
                if (await settingsService.SaveNetsisSettings(customer_id, _settings)) {
                    var saved_settings = settingsService.GetCustomerSettings(customer_id).Result.netsis;
                    if (saved_settings != null) {
                        return Ok(new BaseResponseModel<SettingsNetsis>() { Success = true, Data = saved_settings, ErrorMessage = "" });
                    }
                }
                else {
                    return Ok(new BaseResponseModel<SettingsNetsis>() { Success = false, Data = null, ErrorMessage = "Error saving netsis settings." });
                }
            }
            return BadRequest("Invalid customer ID.");
        }

        [HttpPut("{CID}/SaveShipmentSettings")]
        [Authorize]
        public async Task<ActionResult<BaseResponseModel<SettingsShipment>>> SaveShipmentSettings(string CID, [FromBody] SettingsShipment _settings) {
            int customer_id;
            if (int.TryParse(CID, out customer_id) && customer_id > 0) {
                if (await settingsService.SaveShipmentSettings(customer_id, _settings)) {
                    var saved_settings = settingsService.GetCustomerSettings(customer_id).Result.shipment;
                    if (saved_settings != null) {
                        return Ok(new BaseResponseModel<SettingsShipment>() { Success = true, Data = saved_settings, ErrorMessage = "" });
                    }
                }
                else {
                    return Ok(new BaseResponseModel<SettingsShipment>() { Success = false, Data = null, ErrorMessage = "Error saving shipment settings." });
                }
            }
            return BadRequest("Invalid customer ID.");
        }

        [HttpPut("{CID}/SaveOrderSettings")]
        [Authorize]
        public async Task<ActionResult<BaseResponseModel<SettingsOrder>>> SaveOrderSettings(string CID, [FromBody] SettingsOrder _settings) {
            int customer_id;
            if (int.TryParse(CID, out customer_id) && customer_id > 0) {
                if (await settingsService.SaveOrderSettings(customer_id, _settings)) {
                    SettingsOrder? saved_settings = settingsService.GetCustomerSettings(customer_id).Result.order;
                    if (saved_settings != null) {
                        return Ok(new BaseResponseModel<SettingsOrder>() { Success = true, Data = saved_settings, ErrorMessage = "" });
                    }
                }
                else {
                    return Ok(new BaseResponseModel<SettingsOrder>() { Success = false, Data = null, ErrorMessage = "Error saving order settings." });
                }
            }
            return BadRequest("Invalid customer ID.");
        }

        [HttpPut("{CID}/SaveProductSettings")]
        [Authorize]
        public async Task<ActionResult<BaseResponseModel<SettingsProduct>>> SaveProductSettings(string CID, [FromBody] SettingsProduct _settings) {
            int customer_id;
            if (int.TryParse(CID, out customer_id) && customer_id > 0) {
                if (await settingsService.SaveProductSettings(customer_id, _settings)) {
                    SettingsProduct? saved_settings = settingsService.GetCustomerSettings(customer_id).Result.product;
                    if (saved_settings != null) {
                        return Ok(new BaseResponseModel<SettingsProduct>() { Success = true, Data = saved_settings, ErrorMessage = "" });
                    }
                }
                else {
                    return Ok(new BaseResponseModel<SettingsProduct>() { Success = false, Data = null, ErrorMessage = "Error saving product settings." });
                }
            }
            return BadRequest("Invalid customer ID.");
        }

        [HttpPut("{CID}/SaveInvoiceSettings")]
        [Authorize]
        public async Task<ActionResult<BaseResponseModel<SettingsInvoice>>> SaveInvoiceSettings(string CID, [FromBody] SettingsInvoice _settings) {
            int customer_id;
            if (int.TryParse(CID, out customer_id) && customer_id > 0) {
                if (await settingsService.SaveInvoiceSettings(customer_id, _settings)) {
                    SettingsInvoice? saved_settings = settingsService.GetCustomerSettings(customer_id).Result.invoice;
                    if (saved_settings != null) {
                        return Ok(new BaseResponseModel<SettingsInvoice>() { Success = true, Data = saved_settings, ErrorMessage = "" });
                    }
                }
                else {
                    return Ok(new BaseResponseModel<SettingsInvoice>() { Success = false, Data = null, ErrorMessage = "Error saving invoice settings." });
                }
            }
            return BadRequest("Invalid customer ID.");
        }

        [HttpPut("{CID}/SaveN11Settings")]
        [Authorize]
        public async Task<ActionResult<BaseResponseModel<SettingsN11>>> SaveN11Settings(string CID, [FromBody] SettingsN11 _settings) {
            int customer_id;
            if (int.TryParse(CID, out customer_id) && customer_id > 0) {
                if (await settingsService.SaveN11Settings(customer_id, _settings)) {
                    SettingsN11? saved_settings = settingsService.GetCustomerSettings(customer_id).Result.n11;
                    if (saved_settings != null) {
                        return Ok(new BaseResponseModel<SettingsN11>() { Success = true, Data = saved_settings, ErrorMessage = "" });
                    }
                }
                else {
                    return Ok(new BaseResponseModel<SettingsN11>() { Success = false, Data = null, ErrorMessage = "Error saving N11 settings." });
                }
            }
            return BadRequest("Invalid customer ID.");
        }

        [HttpPut("{CID}/SaveHBSettings")]
        [Authorize]
        public async Task<ActionResult<BaseResponseModel<SettingsHB>>> SaveHBSettings(string CID, [FromBody] SettingsHB _settings) {
            int customer_id;
            if (int.TryParse(CID, out customer_id) && customer_id > 0) {
                if (await settingsService.SaveHBSettings(customer_id, _settings)) {
                    SettingsHB? saved_settings = settingsService.GetCustomerSettings(customer_id).Result.hb;
                    if (saved_settings != null) {
                        return Ok(new BaseResponseModel<SettingsHB>() { Success = true, Data = saved_settings, ErrorMessage = "" });
                    }
                }
                else {
                    return Ok(new BaseResponseModel<SettingsHB>() { Success = false, Data = null, ErrorMessage = "Error saving HB settings." });
                }
            }
            return BadRequest("Invalid customer ID.");
        }

        [HttpPut("{CID}/SaveTYSettings")]
        [Authorize]
        public async Task<ActionResult<BaseResponseModel<SettingsTY>>> SaveTYSettings(string CID, [FromBody] SettingsTY _settings) {
            int customer_id;
            if (int.TryParse(CID, out customer_id) && customer_id > 0) {
                if (await settingsService.SaveTYSettings(customer_id, _settings)) {
                    SettingsTY? saved_settings = settingsService.GetCustomerSettings(customer_id).Result.ty;
                    if (saved_settings != null) {
                        return Ok(new BaseResponseModel<SettingsTY>() { Success = true, Data = saved_settings, ErrorMessage = "" });
                    }
                }
                else {
                    return Ok(new BaseResponseModel<SettingsTY>() { Success = false, Data = null, ErrorMessage = "Error saving TY settings." });
                }
            }
            return BadRequest("Invalid customer ID.");
        }

        [HttpPut("{CID}/SaveAnkERPSettings")]
        [Authorize]
        public async Task<ActionResult<BaseResponseModel<SettingsAnkaraErp>>> SaveAnkERPSettings(string CID, [FromBody] SettingsAnkaraErp _settings) {
            int customer_id;
            if (int.TryParse(CID, out customer_id) && customer_id > 0) {
                if (await settingsService.SaveAnkErpSettings(customer_id, _settings)) {
                    SettingsAnkaraErp? saved_settings = settingsService.GetCustomerSettings(customer_id).Result.ank_erp;
                    if (saved_settings != null) {
                        return Ok(new BaseResponseModel<SettingsAnkaraErp>() { Success = true, Data = saved_settings, ErrorMessage = "" });
                    }
                }
                else {
                    return Ok(new BaseResponseModel<SettingsAnkaraErp>() { Success = false, Data = null, ErrorMessage = "Error saving ANK_ERP settings." });
                }
            }
            return BadRequest("Invalid customer ID.");
        }

        [HttpPut("{CID}/SaveIdeasoftSettings")]
        [Authorize]
        public async Task<ActionResult<BaseResponseModel<SettingsIdeasoft>>> SaveIdeasoftSettings(string CID, [FromBody] SettingsIdeasoft _settings) {
            int customer_id;
            if (int.TryParse(CID, out customer_id) && customer_id > 0) {
                if (await settingsService.SaveIdeasoftSettings(customer_id, _settings)) {
                    SettingsIdeasoft? saved_settings = settingsService.GetCustomerSettings(customer_id).Result.ideasoft;
                    if (saved_settings != null) {
                        return Ok(new BaseResponseModel<SettingsIdeasoft>() { Success = true, Data = saved_settings, ErrorMessage = "" });
                    }
                }
                else {
                    return Ok(new BaseResponseModel<SettingsIdeasoft>() { Success = false, Data = null, ErrorMessage = "Error saving IDEASOFT settings." });
                }
            }
            return BadRequest("Invalid customer ID.");
        }

        [HttpPut("{CID}/SaveGoogleSettings")]
        [Authorize]
        public async Task<ActionResult<BaseResponseModel<SettingsGoogle>>> SaveGoogleSettings(string CID, [FromBody] SettingsGoogle _settings) {
            int customer_id;
            if (int.TryParse(CID, out customer_id) && customer_id > 0) {
                if (await settingsService.SaveGoogleSettings(customer_id, _settings)) {
                    SettingsGoogle? saved_settings = settingsService.GetCustomerSettings(customer_id).Result.google;
                    if (saved_settings != null) {
                        return Ok(new BaseResponseModel<SettingsGoogle>() { Success = true, Data = saved_settings, ErrorMessage = "" });
                    }
                }
                else {
                    return Ok(new BaseResponseModel<SettingsGoogle>() { Success = false, Data = null, ErrorMessage = "Error saving Google settings." });
                }
            }
            return BadRequest("Invalid customer ID.");
        }
    }
}
