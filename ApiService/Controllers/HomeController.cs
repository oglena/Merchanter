using Microsoft.AspNetCore.Mvc;

namespace ApiService.Controllers {
    /// <summary>
    /// HomeController for redirecting to the main application page.
    /// </summary>
    public class HomeController :Controller {
        /// <summary>
        /// Redirects to the main application page.
        /// </summary>
        [Route( "" ), HttpGet]
        [ApiExplorerSettings( IgnoreApi = true )]
        public RedirectResult Index() {
            return Redirect( "/scalar/" );
        }
    }
}
