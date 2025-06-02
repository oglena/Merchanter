using Microsoft.AspNetCore.Mvc;

namespace MerchanterApp.ServerService.Controllers {
    public class HomeController :Controller {
        [Route( "" ), HttpGet]
        [ApiExplorerSettings( IgnoreApi = true )]
        public RedirectResult Index() {
            return Redirect( "/swagger/" );
        }
    }
}
