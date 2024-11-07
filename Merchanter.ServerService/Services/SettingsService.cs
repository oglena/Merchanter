using Merchanter.Classes;
using Merchanter.Classes.Settings;
using Merchanter.CustomerService.Repositories;
using Merchanter.ServerService.Classes;
using Merchanter.ServerService.Repositories;

namespace Merchanter.ServerService.Services {
    public interface ISettingsService {
        Task<SettingsMerchanter> GetSettings( int _customer_id );
    }
    public class SettingsService(ISettingsRepository settingsRepository): ISettingsService {
        public Task<SettingsMerchanter> GetSettings( int _customer_id ) {
            return settingsRepository.GetSettings( _customer_id );
        }
    }
}
