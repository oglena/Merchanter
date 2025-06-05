using ApiService.Services;
using Merchanter.Classes;
using Merchanter.Classes.Settings;
using ApiService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ApiService.Controllers {
    /// <summary>
    /// Provides settings-related API endpoints for Merchanter.
    /// </summary>
    [Route("api/[controller]")]
    [SwaggerTag("Settings endpoint for Merchanter.")]
    [ApiController]
    public class SettingsController(ISettingsService settingsService) : ControllerBase {
        /// <summary>
        /// Retrieves all settings for the specified customer.
        /// </summary>
        /// <returns>Customer settings wrapped in a BaseResponseModel.</returns>
        [HttpGet("GetCustomerSettings")]
        [Authorize]
        public async Task<ActionResult<BaseResponseModel<SettingsMerchanter>>> GetCustomerSettings() {
            if (int.TryParse(HttpContext.User.FindFirst("customerId")?.Value, out int customer_id) && customer_id > 0) {
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

        /// <summary>
        /// Retrieves the list of active integrations for the specified customer.
        /// </summary>
        /// <returns>List of active integrations wrapped in a BaseResponseModel.</returns>
        [HttpGet("GetActiveIntegrations")]
        [Authorize]
        public async Task<ActionResult<BaseResponseModel<List<ActiveIntegration>?>>> GetActiveIntegrations() {
            if (int.TryParse(HttpContext.User.FindFirst("customerId")?.Value, out int customer_id) && customer_id > 0) {
                var active_integrations = await settingsService.GetActiveIntegrations(customer_id);
                if (active_integrations != null) {
                    return Ok(new BaseResponseModel<List<ActiveIntegration>?>() { Success = true, Data = active_integrations, ErrorMessage = "" });
                }
                else {
                    return Ok(new BaseResponseModel<List<ActiveIntegration>?>() { Success = false, Data = null, ErrorMessage = "Error getting active integrations." });
                }
            }
            return BadRequest("Invalid customer ID.");
        }

        /// <summary>
        /// Saves general settings for the specified customer.
        /// </summary>
        /// <param name="_settings">General settings object.</param>
        /// <returns>Saved general settings wrapped in a BaseResponseModel.</returns>
        [HttpPut("SaveGeneralSettings")]
        [Authorize]
        public async Task<ActionResult<BaseResponseModel<SettingsGeneral>>> SaveGeneralSettings( [FromBody] SettingsGeneral _settings) {
            if (int.TryParse(HttpContext.User.FindFirst("customerId")?.Value, out int customer_id) && customer_id > 0) {
                if (await settingsService.SaveGeneralSettings(customer_id, _settings)) {
                    var saved_settings = (await settingsService.GetCustomerSettings(customer_id))?.settings;
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

        /// <summary>
        /// Saves Entegra integration settings for the specified customer.
        /// </summary>
        /// <param name="_settings">Entegra settings object.</param>
        /// <returns>Saved Entegra settings wrapped in a BaseResponseModel.</returns>
        [HttpPut("SaveEntegraSettings")]
        [Authorize]
        public async Task<ActionResult<BaseResponseModel<SettingsEntegra>>> SaveEntegraSettings( [FromBody] SettingsEntegra _settings) {
            if (int.TryParse(HttpContext.User.FindFirst("customerId")?.Value, out int customer_id) && customer_id > 0) {
                if (await settingsService.SaveEntegraSettings(customer_id, _settings)) {
                    var saved_settings = (await settingsService.GetCustomerSettings(customer_id))?.entegra;
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

        /// <summary>
        /// Saves Magento integration settings for the specified customer.
        /// </summary>
        /// <param name="_settings">Magento settings object.</param>
        /// <returns>Saved Magento settings wrapped in a BaseResponseModel.</returns>
        [HttpPut("SaveMagentoSettings")]
        [Authorize]
        public async Task<ActionResult<BaseResponseModel<SettingsMagento>>> SaveMagentoSettings([FromBody] SettingsMagento _settings) {
            if (int.TryParse(HttpContext.User.FindFirst("customerId")?.Value, out int customer_id) && customer_id > 0) {
                if (await settingsService.SaveMagentoSettings(customer_id, _settings)) {
                    var saved_settings = (await settingsService.GetCustomerSettings(customer_id))?.magento;
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

        /// <summary>
        /// Saves Netsis integration settings for the specified customer.
        /// </summary>
        /// <param name="_settings">Netsis settings object.</param>
        /// <returns>Saved Netsis settings wrapped in a BaseResponseModel.</returns>
        [HttpPut("SaveNetsisSettings")]
        [Authorize]
        public async Task<ActionResult<BaseResponseModel<SettingsNetsis>>> SaveNetsisSettings( [FromBody] SettingsNetsis _settings) {
            if (int.TryParse(HttpContext.User.FindFirst("customerId")?.Value, out int customer_id) && customer_id > 0) {
                if (await settingsService.SaveNetsisSettings(customer_id, _settings)) {
                    var saved_settings = (await settingsService.GetCustomerSettings(customer_id))?.netsis;
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

        /// <summary>
        /// Saves shipment settings for the specified customer.
        /// </summary>
        /// <param name="_settings">Shipment settings object.</param>
        /// <returns>Saved shipment settings wrapped in a BaseResponseModel.</returns>
        [HttpPut("SaveShipmentSettings")]
        [Authorize]
        public async Task<ActionResult<BaseResponseModel<SettingsShipment>>> SaveShipmentSettings( [FromBody] SettingsShipment _settings) {
            if (int.TryParse(HttpContext.User.FindFirst("customerId")?.Value, out int customer_id) && customer_id > 0) {
                if (await settingsService.SaveShipmentSettings(customer_id, _settings)) {
                    var saved_settings = (await settingsService.GetCustomerSettings(customer_id))?.shipment;
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

        /// <summary>
        /// Saves order settings for the specified customer.
        /// </summary>
        /// <param name="_settings">Order settings object.</param>
        /// <returns>Saved order settings wrapped in a BaseResponseModel.</returns>
        [HttpPut("SaveOrderSettings")]
        [Authorize]
        public async Task<ActionResult<BaseResponseModel<SettingsOrder>>> SaveOrderSettings([FromBody] SettingsOrder _settings) {
            if (int.TryParse(HttpContext.User.FindFirst("customerId")?.Value, out int customer_id) && customer_id > 0) {
                if (await settingsService.SaveOrderSettings(customer_id, _settings)) {
                    SettingsOrder? saved_settings = (await settingsService.GetCustomerSettings(customer_id))?.order;
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

        /// <summary>
        /// Saves product settings for the specified customer.
        /// </summary>
        /// <param name="_settings">Product settings object.</param>
        /// <returns>Saved product settings wrapped in a BaseResponseModel.</returns>
        [HttpPut("SaveProductSettings")]
        [Authorize]
        public async Task<ActionResult<BaseResponseModel<SettingsProduct>>> SaveProductSettings( [FromBody] SettingsProduct _settings) {
            if (int.TryParse(HttpContext.User.FindFirst("customerId")?.Value, out int customer_id) && customer_id > 0) {
                if (await settingsService.SaveProductSettings(customer_id, _settings)) {
                    SettingsProduct? saved_settings = (await settingsService.GetCustomerSettings(customer_id))?.product;
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

        /// <summary>
        /// Saves invoice settings for the specified customer.
        /// </summary>
        /// <param name="_settings">Invoice settings object.</param>
        /// <returns>Saved invoice settings wrapped in a BaseResponseModel.</returns>
        [HttpPut("SaveInvoiceSettings")]
        [Authorize]
        public async Task<ActionResult<BaseResponseModel<SettingsInvoice>>> SaveInvoiceSettings( [FromBody] SettingsInvoice _settings) {
            if (int.TryParse(HttpContext.User.FindFirst("customerId")?.Value, out int customer_id) && customer_id > 0) {
                if (await settingsService.SaveInvoiceSettings(customer_id, _settings)) {
                    SettingsInvoice? saved_settings = (await settingsService.GetCustomerSettings(customer_id))?.invoice;
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

        /// <summary>
        /// Saves N11 integration settings for the specified customer.
        /// </summary>
        /// <param name="_settings">N11 settings object.</param>
        /// <returns>Saved N11 settings wrapped in a BaseResponseModel.</returns>
        [HttpPut("SaveN11Settings")]
        [Authorize]
        public async Task<ActionResult<BaseResponseModel<SettingsN11>>> SaveN11Settings( [FromBody] SettingsN11 _settings) {
            if (int.TryParse(HttpContext.User.FindFirst("customerId")?.Value, out int customer_id) && customer_id > 0) {
                if (await settingsService.SaveN11Settings(customer_id, _settings)) {
                    SettingsN11? saved_settings = (await settingsService.GetCustomerSettings(customer_id))?.n11;
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

        /// <summary>
        /// Saves Hepsiburada integration settings for the specified customer.
        /// </summary>
        /// <param name="_settings">HB settings object.</param>
        /// <returns>Saved HB settings wrapped in a BaseResponseModel.</returns>
        [HttpPut("SaveHBSettings")]
        [Authorize]
        public async Task<ActionResult<BaseResponseModel<SettingsHB>>> SaveHBSettings( [FromBody] SettingsHB _settings) {
            if (int.TryParse(HttpContext.User.FindFirst("customerId")?.Value, out int customer_id) && customer_id > 0) {
                if (await settingsService.SaveHBSettings(customer_id, _settings)) {
                    SettingsHB? saved_settings = (await settingsService.GetCustomerSettings(customer_id))?.hb;
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

        /// <summary>
        /// Saves Trendyol integration settings for the specified customer.
        /// </summary>
        /// <param name="_settings">TY settings object.</param>
        /// <returns>Saved TY settings wrapped in a BaseResponseModel.</returns>
        [HttpPut("SaveTYSettings")]
        [Authorize]
        public async Task<ActionResult<BaseResponseModel<SettingsTY>>> SaveTYSettings( [FromBody] SettingsTY _settings) {
            if (int.TryParse(HttpContext.User.FindFirst("customerId")?.Value, out int customer_id) && customer_id > 0) {
                if (await settingsService.SaveTYSettings(customer_id, _settings)) {
                    SettingsTY? saved_settings = (await settingsService.GetCustomerSettings(customer_id))?.ty;
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

        /// <summary>
        /// Saves Ankara ERP integration settings for the specified customer.
        /// </summary>
        /// <param name="_settings">Ankara ERP settings object.</param>
        /// <returns>Saved Ankara ERP settings wrapped in a BaseResponseModel.</returns>
        [HttpPut("SaveAnkERPSettings")]
        [Authorize]
        public async Task<ActionResult<BaseResponseModel<SettingsAnkaraErp>>> SaveAnkERPSettings( [FromBody] SettingsAnkaraErp _settings) {
            if (int.TryParse(HttpContext.User.FindFirst("customerId")?.Value, out int customer_id) && customer_id > 0) {
                if (await settingsService.SaveAnkErpSettings(customer_id, _settings)) {
                    SettingsAnkaraErp? saved_settings = (await settingsService.GetCustomerSettings(customer_id))?.ank_erp;
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

        /// <summary>
        /// Saves Ideasoft integration settings for the specified customer.
        /// </summary>
        /// <param name="_settings">Ideasoft settings object.</param>
        /// <returns>Saved Ideasoft settings wrapped in a BaseResponseModel.</returns>
        [HttpPut("SaveIdeasoftSettings")]
        [Authorize]
        public async Task<ActionResult<BaseResponseModel<SettingsIdeasoft>>> SaveIdeasoftSettings( [FromBody] SettingsIdeasoft _settings) {
            if (int.TryParse(HttpContext.User.FindFirst("customerId")?.Value, out int customer_id) && customer_id > 0) {
                if (await settingsService.SaveIdeasoftSettings(customer_id, _settings)) {
                    SettingsIdeasoft? saved_settings = (await settingsService.GetCustomerSettings(customer_id))?.ideasoft;
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

        /// <summary>
        /// Saves Google integration settings for the specified customer.
        /// </summary>
        /// <param name="_settings">Google settings object.</param>
        /// <returns>Saved Google settings wrapped in a BaseResponseModel.</returns>
        [HttpPut("SaveGoogleSettings")]
        [Authorize]
        public async Task<ActionResult<BaseResponseModel<SettingsGoogle>>> SaveGoogleSettings( [FromBody] SettingsGoogle _settings) {
            if (int.TryParse(HttpContext.User.FindFirst("customerId")?.Value, out int customer_id) && customer_id > 0) {
                if (await settingsService.SaveGoogleSettings(customer_id, _settings)) {
                    SettingsGoogle? saved_settings = (await settingsService.GetCustomerSettings(customer_id))?.google;
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
