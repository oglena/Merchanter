using Merchanter.Classes.Settings;
using Merchanter.CustomerService.Repositories;
using Merchanter.ServerService.Services;

namespace Merchanter.ServerService.Repositories {
    public interface ISettingsRepository {
        Task<SettingsMerchanter> GetSettings( int _customer_id );
        MerchanterService merchanterService { get; set; }
    }

    public class SettingsRepository( MerchanterService merchanterService ) :ISettingsRepository {
        public SettingsMerchanter settings { get; set; }
        MerchanterService ISettingsRepository.merchanterService { get; set; } = new MerchanterService();

        public async Task<SettingsMerchanter> GetSettings( int _customer_id ) {
            return await Task.Run( () => settings = merchanterService.helper.LoadSettings( _customer_id ) );
        }
    }
}
