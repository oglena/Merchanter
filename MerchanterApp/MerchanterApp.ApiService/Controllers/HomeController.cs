using Microsoft.AspNetCore.Mvc;

namespace MerchanterApp.ApiService.Controllers {
    public class HomeController :Controller {
        [Route( "" ), HttpGet]
        [ApiExplorerSettings( IgnoreApi = true )]
        public RedirectResult Index() {
            return Redirect( "/swagger/" );
        }
    }
}
