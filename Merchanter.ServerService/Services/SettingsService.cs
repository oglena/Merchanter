using Merchanter.Classes;
using Merchanter.Classes.Settings;
using Merchanter.CustomerService.Repositories;
using Merchanter.ServerService.Classes;
using Merchanter.ServerService.Repositories;

namespace Merchanter.ServerService.Services {
    public interface ISettingsService {
        Task<SettingsMerchanter> GetCustomerSettings( int _customer_id );
        Task<bool> SaveGeneralSettings( int _customer_id, SettingsGeneral _settings );
        Task<bool> SaveEntegraSettings( int _customer_id, SettingsEntegra _settings );
        Task<bool> SaveNetsisSettings( int _customer_id, SettingsNetsis _settings );
        Task<bool> SaveMagentoSettings( int _customer_id, SettingsMagento _settings );
        Task<bool> SaveShipmentSettings( int _customer_id, SettingsShipment _settings );
        Task<bool> SaveOrderSettings( int _customer_id, SettingsOrder _settings );
    }
    public class SettingsService(ISettingsRepository settingsRepository): ISettingsService {
        public Task<SettingsMerchanter> GetCustomerSettings( int _customer_id ) {
            return settingsRepository.GetSettings( _customer_id );
        }

        public Task<bool> SaveGeneralSettings( int _customer_id, SettingsGeneral _settings ) {
            return settingsRepository.SaveGeneralSettings( _customer_id, _settings );
        }

        public Task<bool> SaveEntegraSettings( int _customer_id, SettingsEntegra _settings ) {
            return settingsRepository.SaveEntegraSettings( _customer_id, _settings );
        }

        public Task<bool> SaveNetsisSettings( int _customer_id, SettingsNetsis _settings ) {
            return settingsRepository.SaveNetsisSettings( _customer_id, _settings );
        }

        public Task<bool> SaveMagentoSettings( int _customer_id, SettingsMagento _settings ) {
            return settingsRepository.SaveMagentoSettings( _customer_id, _settings );
        }

        public Task<bool> SaveShipmentSettings( int _customer_id, SettingsShipment _settings ) {
            return settingsRepository.SaveShipmentSettings( _customer_id, _settings );
        }

        public Task<bool> SaveOrderSettings( int _customer_id, SettingsOrder _settings ) {
            return settingsRepository.SaveOrderSettings( _customer_id, _settings );
        }
    }
}
