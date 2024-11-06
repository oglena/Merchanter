using Merchanter.ServerService.Classes;
using System.Collections;

namespace Merchanter.ServerService.Models {
    public class BaseResponseModel<T> {
        public bool Success { get; set; }
        public string ErrorMessage { get; set; } = "";
        public T? Data { get; set; }
        public ApiFilter? ApiFilter { get; set; }
    }
}
