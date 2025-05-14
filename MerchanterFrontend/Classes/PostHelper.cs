using MerchanterFrontend.Models;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using RestSharp;
using static MerchanterFrontend.Classes.PostHelper;

namespace MerchanterFrontend.Classes {
    public interface IPostHelper {
        public Task<BaseResponseModel<T>?> Request<T>(string? _token, PostMethod _method, PostDestination _destination, string _url, StringContent? _body = null);
    }

    public class PostHelper : IPostHelper {
        [Inject]
        private IConfiguration? configuration { get; set; }
        private readonly ILogger logger;
        public PostHelper(IConfiguration _configuration, ILogger<PostHelper> _logger) {
            configuration = _configuration;
            logger = _logger;
        }
        public async Task<BaseResponseModel<T>?> Request<T>(string? _token, PostMethod _method, PostDestination _destination, string _url, StringContent? _body = null) {
            BaseResponseModel<T>? model = null;
            var baseAddress = configuration?.GetSection("AppSettings:MerchanterApiUrl")?.Value;
            switch (_destination) {
                case PostDestination.Server:
                    baseAddress = configuration?.GetSection("AppSettings:MerchanterServerUrl")?.Value;
                    break;
            }

            if (string.IsNullOrWhiteSpace(baseAddress)) {
                logger.LogError("Base address is not configured.");
                return null;
            }

            switch (_method) {
                case PostMethod.Login:
                    try {
                        var client = new RestClient(baseAddress);
                        var request = new RestRequest(_url, Method.Post) {
                            Timeout = TimeSpan.FromSeconds(10)
                        };
                        request.AddHeader("Authorization", "Bearer " + _token);
                        if (_body != null) {
                            var bodyContent = await _body.ReadAsStringAsync();
                            request.AddStringBody(bodyContent, DataFormat.Json);
                        }
                        var response = client.ExecutePost<T>(request); var login_response = JsonConvert.DeserializeObject<UserLoginResponseModel>(response.Content);
                        if (login_response != null && login_response.AuthenticateResult && !string.IsNullOrWhiteSpace(login_response.AuthToken) && 
                            login_response.Settings != null && login_response.Settings.customer != null) {
                            model = new BaseResponseModel<T>() {
                                Success = true,
                                ErrorMessage = "",
                                Data = (T)(object)login_response
                            };
                            logger.LogInformation("LOGIN[" + response.StatusCode.ToString() + " " + login_response.Settings.customer.user_name + "]: " + _url, DateTime.UtcNow.ToLongTimeString());
                        }
                        else {
                            logger.LogInformation("LOGIN[FAIL]: " + response.StatusCode.ToString() + " " + _url, DateTime.UtcNow.ToLongTimeString());
                        }
                    }
                    catch (Exception ex) {
                        logger.LogError("LOGIN: " + ex.Message + " " + _url, DateTime.UtcNow.ToLongTimeString());
                        model = new BaseResponseModel<T>() {
                            Success = false,
                            ErrorMessage = ex.Message,
                            Data = default
                        };
                    }
                    break;
                case PostMethod.Get:
                    if (!string.IsNullOrWhiteSpace(_token)) {
                        try {
                            var client = new RestClient(baseAddress);
                            var request = new RestRequest(_url, Method.Get) {
                                Timeout = TimeSpan.FromSeconds(10)
                            };
                            request.AddHeader("Authorization", "Bearer " + _token);
                            var response = await client.ExecuteAsync<T>(request);
                            if (response.IsSuccessStatusCode) {
                                model = JsonConvert.DeserializeObject<BaseResponseModel<T>>(response.Content);
                                logger.LogInformation("GET: " + response.StatusCode.ToString() + " " + _url, DateTime.UtcNow.ToLongTimeString());
                            }
                            else {
                                model = new BaseResponseModel<T>() {
                                    Success = false,
                                    ErrorMessage = response.StatusCode.ToString(),
                                    Data = default
                                };
                            }
                        }
                        catch (Exception ex) {
                            logger.LogError("GET: " + ex.Message + " " + _url, DateTime.UtcNow.ToLongTimeString());
                            model = new BaseResponseModel<T>() {
                                Success = false,
                                ErrorMessage = ex.Message,
                                Data = default
                            };
                        }
                    }
                    break;
                case PostMethod.Post:
                    if (!string.IsNullOrWhiteSpace(_token)) {
                        try {
                            var client = new RestClient(baseAddress);
                            var request = new RestRequest(_url, Method.Post) {
                                Timeout = TimeSpan.FromSeconds(10)
                            };
                            request.AddHeader("Authorization", "Bearer " + _token);
                            if (_body != null) {
                                var bodyContent = await _body.ReadAsStringAsync();
                                request.AddStringBody(bodyContent, DataFormat.Json);
                            }
                            var response = await client.ExecutePostAsync<T>(request);
                            if (response.IsSuccessStatusCode) {
                                model = JsonConvert.DeserializeObject<BaseResponseModel<T>>(response.Content);
                                logger.LogInformation("POST: " + response.StatusCode.ToString() + " " + _url, DateTime.UtcNow.ToLongTimeString());
                            }
                            else {
                                model = new BaseResponseModel<T>() {
                                    Success = false,
                                    ErrorMessage = response.StatusCode.ToString(),
                                    Data = default
                                };
                            }
                        }
                        catch (Exception ex) {
                            logger.LogError("POST: " + ex.Message + " " + _url, DateTime.UtcNow.ToLongTimeString());
                            model = new BaseResponseModel<T>() {
                                Success = false,
                                ErrorMessage = ex.Message,
                                Data = default
                            };
                        }
                    }
                    break;
                case PostMethod.Put:
                    if (!string.IsNullOrWhiteSpace(_token)) {
                        try {
                            var client = new RestClient(baseAddress);
                            var request = new RestRequest(_url, Method.Put) {
                                Timeout = TimeSpan.FromSeconds(10)
                            };
                            request.AddHeader("Authorization", "Bearer " + _token);
                            if (_body != null) {
                                var bodyContent = await _body.ReadAsStringAsync();
                                request.AddStringBody(bodyContent, DataFormat.Json);
                            }
                            var response = await client.ExecutePutAsync<T>(request);
                            if (response.IsSuccessStatusCode) {
                                model = JsonConvert.DeserializeObject<BaseResponseModel<T>>(response.Content);
                                logger.LogInformation("PUT: " + response.StatusCode.ToString() + " " + _url, DateTime.UtcNow.ToLongTimeString());
                            }
                            else {
                                model = new BaseResponseModel<T>() {
                                    Success = false,
                                    ErrorMessage = response.StatusCode.ToString(),
                                    Data = default
                                };
                            }
                        }
                        catch (Exception ex) {
                            logger.LogError("PUT: " + ex.Message + " " + _url, DateTime.UtcNow.ToLongTimeString());
                            model = new BaseResponseModel<T>() {
                                Success = false,
                                ErrorMessage = ex.Message,
                                Data = default
                            };
                        }
                    }
                    break;
                case PostMethod.Delete:
                    if (!string.IsNullOrWhiteSpace(_token)) {
                        try {
                            var client = new RestClient(baseAddress);
                            var request = new RestRequest(_url, Method.Delete) {
                                Timeout = TimeSpan.FromSeconds(10)
                            };
                            request.AddHeader("Authorization", "Bearer " + _token);
                            if (_body != null) {
                                var bodyContent = await _body.ReadAsStringAsync();
                                request.AddStringBody(bodyContent, DataFormat.Json);
                            }
                            var response = await client.ExecuteDeleteAsync<T>(request);
                            if (response.IsSuccessStatusCode) {
                                model = JsonConvert.DeserializeObject<BaseResponseModel<T>>(response.Content);
                                logger.LogInformation("DELETE: " + response.StatusCode.ToString() + " " + _url, DateTime.UtcNow.ToLongTimeString());
                            }
                            else {
                                model = new BaseResponseModel<T>() {
                                    Success = false,
                                    ErrorMessage = response.StatusCode.ToString(),
                                    Data = default
                                };
                            }
                        }
                        catch (Exception ex) {
                            logger.LogError("DELETE: " + ex.Message + " " + _url, DateTime.UtcNow.ToLongTimeString());
                            model = new BaseResponseModel<T>() {
                                Success = false,
                                ErrorMessage = ex.Message,
                                Data = default
                            };
                        }
                    }
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

        public enum PostDestination {
            Api = 0,
            Server = 1
        }
    }
}
