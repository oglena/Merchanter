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
        /// <param name="id">Customer ID as string.</param>
        /// <returns>Customer settings wrapped in a BaseResponseModel.</returns>
        [HttpGet("{id}/GetCustomerSettings")]
        [Authorize]
        public async Task<ActionResult<BaseResponseModel<SettingsMerchanter>>> GetCustomerSettings(string id) {
            int customer_id;
            if (int.TryParse(id, out customer_id) && customer_id > 0) {
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
        /// <param name="id">Customer ID as string.</param>
        /// <returns>List of active integrations wrapped in a BaseResponseModel.</returns>
        [HttpGet("{id}/GetActiveIntegrations")]
        [Authorize]
        public async Task<ActionResult<BaseResponseModel<List<ActiveIntegration>?>>> GetActiveIntegrations(string id) {
            int customer_id;
            if (int.TryParse(id, out customer_id) && customer_id > 0) {
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
        /// <param name="id">Customer ID as string.</param>
        /// <param name="_settings">General settings object.</param>
        /// <returns>Saved general settings wrapped in a BaseResponseModel.</returns>
        [HttpPut("{id}/SaveGeneralSettings")]
        [Authorize]
        public async Task<ActionResult<BaseResponseModel<SettingsGeneral>>> SaveGeneralSettings(string id, [FromBody] SettingsGeneral _settings) {
            int customer_id;
            if (int.TryParse(id, out customer_id) && customer_id > 0) {
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

        /// <summary>
        /// Saves Entegra integration settings for the specified customer.
        /// </summary>
        /// <param name="id">Customer ID as string.</param>
        /// <param name="_settings">Entegra settings object.</param>
        /// <returns>Saved Entegra settings wrapped in a BaseResponseModel.</returns>
        [HttpPut("{id}/SaveEntegraSettings")]
        [Authorize]
        public async Task<ActionResult<BaseResponseModel<SettingsEntegra>>> SaveEntegraSettings(string id, [FromBody] SettingsEntegra _settings) {
            int customer_id;
            if (int.TryParse(id, out customer_id) && customer_id > 0) {
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

        /// <summary>
        /// Saves Magento integration settings for the specified customer.
        /// </summary>
        /// <param name="id">Customer ID as string.</param>
        /// <param name="_settings">Magento settings object.</param>
        /// <returns>Saved Magento settings wrapped in a BaseResponseModel.</returns>
        [HttpPut("{id}/SaveMagentoSettings")]
        [Authorize]
        public async Task<ActionResult<BaseResponseModel<SettingsMagento>>> SaveMagentoSettings(string id, [FromBody] SettingsMagento _settings) {
            int customer_id;
            if (int.TryParse(id, out customer_id) && customer_id > 0) {
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

        /// <summary>
        /// Saves Netsis integration settings for the specified customer.
        /// </summary>
        /// <param name="id">Customer ID as string.</param>
        /// <param name="_settings">Netsis settings object.</param>
        /// <returns>Saved Netsis settings wrapped in a BaseResponseModel.</returns>
        [HttpPut("{id}/SaveNetsisSettings")]
        [Authorize]
        public async Task<ActionResult<BaseResponseModel<SettingsNetsis>>> SaveNetsisSettings(string id, [FromBody] SettingsNetsis _settings) {
            int customer_id;
            if (int.TryParse(id, out customer_id) && customer_id > 0) {
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

        /// <summary>
        /// Saves shipment settings for the specified customer.
        /// </summary>
        /// <param name="id">Customer ID as string.</param>
        /// <param name="_settings">Shipment settings object.</param>
        /// <returns>Saved shipment settings wrapped in a BaseResponseModel.</returns>
        [HttpPut("{id}/SaveShipmentSettings")]
        [Authorize]
        public async Task<ActionResult<BaseResponseModel<SettingsShipment>>> SaveShipmentSettings(string id, [FromBody] SettingsShipment _settings) {
            int customer_id;
            if (int.TryParse(id, out customer_id) && customer_id > 0) {
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

        /// <summary>
        /// Saves order settings for the specified customer.
        /// </summary>
        /// <param name="id">Customer ID as string.</param>
        /// <param name="_settings">Order settings object.</param>
        /// <returns>Saved order settings wrapped in a BaseResponseModel.</returns>
        [HttpPut("{id}/SaveOrderSettings")]
        [Authorize]
        public async Task<ActionResult<BaseResponseModel<SettingsOrder>>> SaveOrderSettings(string id, [FromBody] SettingsOrder _settings) {
            int customer_id;
            if (int.TryParse(id, out customer_id) && customer_id > 0) {
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

        /// <summary>
        /// Saves product settings for the specified customer.
        /// </summary>
        /// <param name="id">Customer ID as string.</param>
        /// <param name="_settings">Product settings object.</param>
        /// <returns>Saved product settings wrapped in a BaseResponseModel.</returns>
        [HttpPut("{id}/SaveProductSettings")]
        [Authorize]
        public async Task<ActionResult<BaseResponseModel<SettingsProduct>>> SaveProductSettings(string id, [FromBody] SettingsProduct _settings) {
            int customer_id;
            if (int.TryParse(id, out customer_id) && customer_id > 0) {
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

        /// <summary>
        /// Saves invoice settings for the specified customer.
        /// </summary>
        /// <param name="id">Customer ID as string.</param>
        /// <param name="_settings">Invoice settings object.</param>
        /// <returns>Saved invoice settings wrapped in a BaseResponseModel.</returns>
        [HttpPut("{id}/SaveInvoiceSettings")]
        [Authorize]
        public async Task<ActionResult<BaseResponseModel<SettingsInvoice>>> SaveInvoiceSettings(string id, [FromBody] SettingsInvoice _settings) {
            int customer_id;
            if (int.TryParse(id, out customer_id) && customer_id > 0) {
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

        /// <summary>
        /// Saves N11 integration settings for the specified customer.
        /// </summary>
        /// <param name="id">Customer ID as string.</param>
        /// <param name="_settings">N11 settings object.</param>
        /// <returns>Saved N11 settings wrapped in a BaseResponseModel.</returns>
        [HttpPut("{id}/SaveN11Settings")]
        [Authorize]
        public async Task<ActionResult<BaseResponseModel<SettingsN11>>> SaveN11Settings(string id, [FromBody] SettingsN11 _settings) {
            int customer_id;
            if (int.TryParse(id, out customer_id) && customer_id > 0) {
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

        /// <summary>
        /// Saves Hepsiburada integration settings for the specified customer.
        /// </summary>
        /// <param name="id">Customer ID as string.</param>
        /// <param name="_settings">HB settings object.</param>
        /// <returns>Saved HB settings wrapped in a BaseResponseModel.</returns>
        [HttpPut("{id}/SaveHBSettings")]
        [Authorize]
        public async Task<ActionResult<BaseResponseModel<SettingsHB>>> SaveHBSettings(string id, [FromBody] SettingsHB _settings) {
            int customer_id;
            if (int.TryParse(id, out customer_id) && customer_id > 0) {
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

        /// <summary>
        /// Saves Trendyol integration settings for the specified customer.
        /// </summary>
        /// <param name="id">Customer ID as string.</param>
        /// <param name="_settings">TY settings object.</param>
        /// <returns>Saved TY settings wrapped in a BaseResponseModel.</returns>
        [HttpPut("{id}/SaveTYSettings")]
        [Authorize]
        public async Task<ActionResult<BaseResponseModel<SettingsTY>>> SaveTYSettings(string id, [FromBody] SettingsTY _settings) {
            int customer_id;
            if (int.TryParse(id, out customer_id) && customer_id > 0) {
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

        /// <summary>
        /// Saves Ankara ERP integration settings for the specified customer.
        /// </summary>
        /// <param name="id">Customer ID as string.</param>
        /// <param name="_settings">Ankara ERP settings object.</param>
        /// <returns>Saved Ankara ERP settings wrapped in a BaseResponseModel.</returns>
        [HttpPut("{id}/SaveAnkERPSettings")]
        [Authorize]
        public async Task<ActionResult<BaseResponseModel<SettingsAnkaraErp>>> SaveAnkERPSettings(string id, [FromBody] SettingsAnkaraErp _settings) {
            int customer_id;
            if (int.TryParse(id, out customer_id) && customer_id > 0) {
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

        /// <summary>
        /// Saves Ideasoft integration settings for the specified customer.
        /// </summary>
        /// <param name="id">Customer ID as string.</param>
        /// <param name="_settings">Ideasoft settings object.</param>
        /// <returns>Saved Ideasoft settings wrapped in a BaseResponseModel.</returns>
        [HttpPut("{id}/SaveIdeasoftSettings")]
        [Authorize]
        public async Task<ActionResult<BaseResponseModel<SettingsIdeasoft>>> SaveIdeasoftSettings(string id, [FromBody] SettingsIdeasoft _settings) {
            int customer_id;
            if (int.TryParse(id, out customer_id) && customer_id > 0) {
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

        /// <summary>
        /// Saves Google integration settings for the specified customer.
        /// </summary>
        /// <param name="id">Customer ID as string.</param>
        /// <param name="_settings">Google settings object.</param>
        /// <returns>Saved Google settings wrapped in a BaseResponseModel.</returns>
        [HttpPut("{id}/SaveGoogleSettings")]
        [Authorize]
        public async Task<ActionResult<BaseResponseModel<SettingsGoogle>>> SaveGoogleSettings(string id, [FromBody] SettingsGoogle _settings) {
            int customer_id;
            if (int.TryParse(id, out customer_id) && customer_id > 0) {
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
