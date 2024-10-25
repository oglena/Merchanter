using Merchanter;
using Merchanter.Classes.Settings;

namespace MerchanterApp.ApiService.Services {
    public interface IMerchanterService {
        public DbHelper helper { get; set; }
        public SettingsMerchanter? global { get; set; }
        public int customer_id { get; set; }
    }

    public class MerchanterService :IMerchanterService {
        public int customer_id { get; set; }
        public DbHelper helper { get; set; }
        public SettingsMerchanter? global { get; set; }

        public MerchanterService() {
            //if( customer_id > 0 )
            helper = new( Constants.Server, Constants.User, Constants.Password, Constants.Database, Constants.Port );
        }
    }
}
