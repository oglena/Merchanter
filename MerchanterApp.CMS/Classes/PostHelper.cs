using Merchanter.Classes;
using MerchanterApp.CMS.Models;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using System.Text;
using static MerchanterApp.CMS.Classes.PostHelper;

namespace MerchanterApp.CMS.Classes {
    public interface IPostHelper {
        public Task<BaseResponseModel<T>?> Request<T>(string? _token, PostMethod _method, string _url, StringContent? _body = null);
    }

    public class PostHelper : IPostHelper {
        [Inject]
        private IConfiguration? configuration { get; set; }
        private readonly ILogger logger;
        public PostHelper(IConfiguration _configuration, ILogger<PostHelper> _logger) {
            configuration = _configuration;
            logger = _logger;
        }
        public async Task<BaseResponseModel<T>?> Request<T>(string? _token, PostMethod _method, string _url, StringContent? _body = null) {
            BaseResponseModel<T>? model = null;
            var baseAddress = configuration?.GetSection("AppSettings:MerchanterServerUrl")?.Value;

            if (string.IsNullOrWhiteSpace(baseAddress)) {
                logger.LogError("Base address is not configured.");
                return null;
            }

            switch (_method) {
                case PostMethod.Login:
                    using (HttpClient httpClient = new()) {
                        httpClient.BaseAddress = new Uri(baseAddress);
                        using HttpResponseMessage response = await httpClient.PostAsync(_url, _body);
                        if (response.IsSuccessStatusCode) {
                            var login_response = JsonConvert.DeserializeObject<UserLoginResponseModel>(response.Content.ReadAsStringAsync().Result);
                            if (login_response != null && login_response.AuthenticateResult && !string.IsNullOrWhiteSpace(login_response.AuthToken) && login_response.AdminInformation != null) {
                                model = new BaseResponseModel<T>() {
                                    Success = true,
                                    ErrorMessage = "",
                                    Data = (T)(object)login_response
                                };
                                logger.LogInformation("LOGIN[" + response.StatusCode.ToString() + " " + login_response.AdminInformation.name + "]: " + _url, DateTime.UtcNow.ToLongTimeString());
                            }
                            else {
                                logger.LogInformation("LOGIN[FAIL]: " + response.StatusCode.ToString() + " " + _url, DateTime.UtcNow.ToLongTimeString());
                            }
                        }
                    }
                    break;
                case PostMethod.Get:
                    if (!string.IsNullOrWhiteSpace(_token)) {
                        using (HttpClient httpClient = new()) {
                            httpClient.BaseAddress = new Uri(baseAddress);
                            httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + _token);
                            using HttpResponseMessage response = await httpClient.GetAsync(_url);
                            if (response.IsSuccessStatusCode) {
                                var response_json = response.Content.ReadAsStringAsync().Result;
                                model = JsonConvert.DeserializeObject<BaseResponseModel<T>>(response_json);
                                logger.LogInformation("GET: " + response.StatusCode.ToString() + " " + _url, DateTime.UtcNow.ToLongTimeString());
                            }
                        }
                    }
                    break;
                case PostMethod.Post:
                    if (!string.IsNullOrWhiteSpace(_token)) {
                        using HttpClient httpClient = new();
                        httpClient.BaseAddress = new Uri(baseAddress);
                        httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + _token);
                        using HttpResponseMessage response = await httpClient.PostAsync(_url, _body);
                        if (response.IsSuccessStatusCode) {
                            model = JsonConvert.DeserializeObject<BaseResponseModel<T>>(response.Content.ReadAsStringAsync().Result);
                            logger.LogInformation("POST: " + response.StatusCode.ToString() + " " + _url, DateTime.UtcNow.ToLongTimeString());
                        }

                    }
                    break;
                case PostMethod.Put:
                    if (!string.IsNullOrWhiteSpace(_token)) {
                        using (HttpClient httpClient = new()) {
                            httpClient.BaseAddress = new Uri(baseAddress);
                            httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + _token);
                            using HttpResponseMessage response = await httpClient.PutAsync(_url, _body);
                            if (response.IsSuccessStatusCode) {
                                model = JsonConvert.DeserializeObject<BaseResponseModel<T>>(response.Content.ReadAsStringAsync().Result);
                                logger.LogInformation("PUT: " + response.StatusCode.ToString() + " " + _url, DateTime.UtcNow.ToLongTimeString());
                            }
                        }

                    }
                    break;
                case PostMethod.Delete:
                    break;
            }
            return model;
        }

        public enum PostMethod {
            Get = 0,
            Post = 1,
            Put = 2,
            Delete = 3,
            Login = 4
        }
    }
}
