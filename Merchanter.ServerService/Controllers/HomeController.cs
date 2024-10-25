using Microsoft.AspNetCore.Mvc;

namespace Merchanter.ServerService.Controllers {
    public class HomeController :Controller {
        [Route( "" ), HttpGet]
        [ApiExplorerSettings( IgnoreApi = true )]
        public RedirectResult Index() {
            return Redirect( "/swagger/" );
        }
    }
}
