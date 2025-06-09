using Merchanter;
using Merchanter.Classes.Settings;

namespace MerchanterApp.ServerService.Services {
    public interface IMerchanterService {
        public DbHelper helper { get; set; }
        public SettingsMerchanter? global { get; set; }
        public int admin_id { get; set; }
    }

    public class MerchanterService :IMerchanterService {
        public int admin_id { get; set; }
        public DbHelper helper { get; set; }
        public SettingsMerchanter? global { get; set; }

        public MerchanterService() {
            helper = new( Constants.Server, Constants.User, Constants.Password, Constants.Database, Constants.Port );
        }
    }
}
