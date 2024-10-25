using Merchanter;
using Merchanter.Classes.Settings;
using System.ComponentModel.DataAnnotations;

namespace MerchanterApp.CMS.Services {
    public class MerchanterService {
        public int customer_id { get; set; }
        public DbHelper? db_helper { get; set; }
        public SettingsMerchanter? global { get; set; } = null;

        private readonly IServiceProvider _serviceProvider;

        public MerchanterService( IServiceProvider serviceProvider ) {
            _serviceProvider = serviceProvider;
            Connect();
        }

        public void Connect() {
            using var scope = _serviceProvider.CreateScope();
            db_helper = scope.ServiceProvider.GetService<DbHelper>();
            if( db_helper == null ) {
                db_helper = new( Constants.Server, Constants.User, Constants.Password, Constants.Database, Constants.Port );
                //db_helper.LoadSettings( customer_id, global );
            }
        }
    }
}
