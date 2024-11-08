using Merchanter.Classes;
using Merchanter.Classes.Settings;
using Merchanter.CustomerService.Repositories;
using Merchanter.ServerService.Classes;
using Merchanter.ServerService.Repositories;

namespace Merchanter.ServerService.Services {
    public interface ISettingsService {
        Task<SettingsMerchanter> GetCustomerSettings( int _customer_id );
        Task<bool> SaveGeneralSettings( int _customer_id, SettingsGeneral _settings );
    }
    public class SettingsService(ISettingsRepository settingsRepository): ISettingsService {
        public Task<SettingsMerchanter> GetCustomerSettings( int _customer_id ) {
            return settingsRepository.GetSettings( _customer_id );
        }
        public Task<bool> SaveGeneralSettings( int _customer_id, SettingsGeneral _settings ) {
            return settingsRepository.SaveGeneralSettings( _customer_id, _settings );
        }
    }
}
