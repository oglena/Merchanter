using Merchanter;
using Merchanter.Classes.Settings;
using System.Diagnostics.Eventing.Reader;

namespace Merchanter.ServerService.Services {
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
            this.helper = new( Constants.Server, Constants.User, Constants.Password, Constants.Database, Constants.Port );
        }

        public bool ChangeCustomer( int _customer_id ) {
            if( _customer_id > 0 ) {
                this.global = this.helper.LoadSettings( _customer_id );
                return true;
            }
            else
                return false;
        }
    }
}
