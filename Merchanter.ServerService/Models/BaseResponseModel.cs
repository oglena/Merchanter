using System.Collections;

namespace Merchanter.ServerService.Models {
    public class BaseResponseModel: IEnumerable {
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
        public object Data { get; set; }

        public IEnumerator GetEnumerator() {
            throw new NotImplementedException();
        }
    }
}
