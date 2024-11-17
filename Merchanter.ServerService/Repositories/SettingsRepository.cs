using Merchanter.Classes.Settings;
using Merchanter.CustomerService.Repositories;
using Merchanter.ServerService.Services;

namespace Merchanter.ServerService.Repositories {
    public interface ISettingsRepository {
        Task<SettingsMerchanter> GetSettings( int _customer_id );
        Task<bool> SaveGeneralSettings( int _customer_id, SettingsGeneral _settings );
        Task<bool> SaveEntegraSettings( int _customer_id, SettingsEntegra _settings );
        Task<bool> SaveNetsisSettings( int _customer_id, SettingsNetsis _settings );
        Task<bool> SaveMagentoSettings( int _customer_id, SettingsMagento _settings );
        Task<bool> SaveShipmentSettings( int _customer_id, SettingsShipment _settings );
        Task<bool> SaveOrderSettings( int _customer_id, SettingsOrder _settings );
        Task<bool> SaveProductSettings( int _customer_id, SettingsProduct _settings );
        Task<bool> SaveInvoiceSettings( int _customer_id, SettingsInvoice _settings );
        MerchanterService merchanterService { get; set; }
    }

    public class SettingsRepository( MerchanterService merchanterService ) :ISettingsRepository {
        public SettingsMerchanter? settings { get; set; }
        MerchanterService ISettingsRepository.merchanterService { get; set; } = new MerchanterService();

        public async Task<SettingsMerchanter> GetSettings( int _customer_id ) {
            return await Task.Run( () => settings = merchanterService.helper.LoadSettings( _customer_id ) );
        }

        public async Task<bool> SaveGeneralSettings( int _customer_id, SettingsGeneral _settings ) {
            return await Task.Run( () => merchanterService.helper.SaveCustomerSettings( _customer_id, _settings ) );
        }

        public async Task<bool> SaveEntegraSettings( int _customer_id, SettingsEntegra _settings ) {
            return await Task.Run( () => merchanterService.helper.SaveEntegraSettings( _customer_id, _settings ) );
        }

        public async Task<bool> SaveNetsisSettings( int _customer_id, SettingsNetsis _settings ) {
            return await Task.Run( () => merchanterService.helper.SaveNetsisSettings( _customer_id, _settings ) );
        }

        public async Task<bool> SaveMagentoSettings( int _customer_id, SettingsMagento _settings ) {
            return await Task.Run( () => merchanterService.helper.SaveMagentoSettings( _customer_id, _settings ) );
        }

        public async Task<bool> SaveShipmentSettings( int _customer_id, SettingsShipment _settings ) {
            return await Task.Run( () => merchanterService.helper.SaveShipmentSettings( _customer_id, _settings ) );
        }

        public async Task<bool> SaveOrderSettings( int _customer_id, SettingsOrder _settings ) {
            return await Task.Run( () => merchanterService.helper.SaveOrderSettings( _customer_id, _settings ) );
        }

        public async Task<bool> SaveProductSettings( int _customer_id, SettingsProduct _settings ) {
            return await Task.Run( () => merchanterService.helper.SaveProductSettings( _customer_id, _settings ) );
        }

        public async Task<bool> SaveInvoiceSettings( int _customer_id, SettingsInvoice _settings ) {
            return await Task.Run( () => merchanterService.helper.SaveInvoiceSettings( _customer_id, _settings ) );
        }
    }
}
