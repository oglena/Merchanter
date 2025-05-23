using MerchanterHelpers.Classes;
using RestSharp;
using System;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MerchanterHelpers {
    public class Executioner : IDisposable {

        RestClient restClient = new RestClient();


        public virtual string? ExecuteHB(string _url, Method _method, object? _json, string _token) {
            try {
                restClient = new RestClient();
                var request = new RestRequest(_url, _method);
                request.AddHeader("Accept", "application/json");
                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("Authorization", "Basic " + Base64Encode(_token));
                if (_method == Method.Post) {
                    if (_json != null) {
                        request.AddJsonBody(_json);
                    }
                    var response = restClient.Execute(request);
                    if (response.StatusCode == System.Net.HttpStatusCode.OK || response.StatusCode == System.Net.HttpStatusCode.Created) {
                        return response.Content;
                    }
                    else {
                        return null;
                    }
                }
                else if (_method == Method.Get) {
                    var response = restClient.Execute(request);
                    if (response.StatusCode == System.Net.HttpStatusCode.OK) {
                        return response.Content;
                    }
                    else {
                        return null;
                    }
                }
                else if (_method == Method.Delete) {
                    var response = restClient.Execute(request);
                    if (response.StatusCode == System.Net.HttpStatusCode.OK) {
                        return response.Content;
                    }
                    else {
                        return null;
                    }
                }
                else if (_method == Method.Put) {
                    if (_json != null) {
                        request.AddJsonBody(_json);
                    }
                    var response = restClient.Execute(request);
                    if (response.StatusCode == System.Net.HttpStatusCode.OK) {
                        return response.Content;
                    }
                    else {
                        return null;
                    }
                }
                else {
                    return null;
                }
            }
            catch (Exception ex) {
                return null;
            }
        }

        public virtual string? ExecuteTY(string _url, Method _method, object? _json, string _token) {
            try {
                restClient = new RestClient();
                var request = new RestRequest(_url, _method);
                request.AddHeader("Accept", "application/json");
                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("Authorization", "Basic " + Base64Encode(_token));
                if (_method == Method.Post) {
                    if (_json != null) {
                        request.AddJsonBody(_json);
                    }
                    var response = restClient.Execute(request);
                    if (response.StatusCode == System.Net.HttpStatusCode.OK || response.StatusCode == System.Net.HttpStatusCode.Created) {
                        return response.Content;
                    }
                    else {
                        return null;
                    }
                }
                else if (_method == Method.Get) {
                    var response = restClient.Execute(request);
                    if (response.StatusCode == System.Net.HttpStatusCode.OK) {
                        return response.Content;
                    }
                    else {
                        return null;
                    }
                }
                else if (_method == Method.Delete) {
                    var response = restClient.Execute(request);
                    if (response.StatusCode == System.Net.HttpStatusCode.OK) {
                        return response.Content;
                    }
                    else {
                        return null;
                    }
                }
                else if (_method == Method.Put) {
                    if (_json != null) {
                        request.AddJsonBody(_json);
                    }
                    var response = restClient.Execute(request);
                    if (response.StatusCode == System.Net.HttpStatusCode.OK) {
                        return response.Content;
                    }
                    else {
                        return null;
                    }
                }
                else {
                    return null;
                }
            }
            catch (Exception ex) {
                return null;
            }
        }

        public virtual string? ExecuteN11(string _url, Method _method, object? _json, string? _appkey, string? _appsecret) {
            try {
                restClient = new RestClient();
                var request = new RestRequest(_url, _method);
                request.AddHeader("Accept", "application/json");
                request.AddHeader("Content-Type", "application/json");
                if (_appkey != null) request.AddHeader("appkey", _appkey);
                if (_appsecret != null) request.AddHeader("appsecret", _appsecret);

                if (_method == Method.Post) {
                    if (_json != null) {
                        request.AddJsonBody(_json);
                    }
                    var response = restClient.Execute(request);
                    if (response.StatusCode == System.Net.HttpStatusCode.OK || response.StatusCode == System.Net.HttpStatusCode.Created) {
                        return response.Content;
                    }
                    else {
                        return null;
                    }
                }
                else if (_method == Method.Get) {
                    var response = restClient.Execute(request);
                    if (response.StatusCode == System.Net.HttpStatusCode.OK) {
                        return response.Content;
                    }
                    else {
                        return null;
                    }
                }
                else if (_method == Method.Delete) {
                    var response = restClient.Execute(request);
                    if (response.StatusCode == System.Net.HttpStatusCode.OK) {
                        return response.Content;
                    }
                    else {
                        return null;
                    }
                }
                else if (_method == Method.Put) {
                    if (_json != null) {
                        request.AddJsonBody(_json);
                    }
                    var response = restClient.Execute(request);
                    if (response.StatusCode == System.Net.HttpStatusCode.OK) {
                        return response.Content;
                    }
                    else {
                        return null;
                    }
                }
                else {
                    return null;
                }
            }
            catch (Exception ex) {
                return null;
            }
        }

        public virtual string? ExecutePENTA(string _url, Method _method) {
            try {
                restClient = new RestClient();
                var request = new RestRequest(_url, _method);
                request.AddHeader("Accept", "text/xml");
                request.AddHeader("Content-Type", "text/xml");
                var response = restClient.Execute(request);
                if (response.StatusCode == System.Net.HttpStatusCode.OK) {
                    return response.Content;
                }
                else {
                    return null;
                }
            }
            catch (Exception _ex) {
                throw _ex;
            }
        }

        public virtual string? ExecuteFSP(string _url, Method _method) {
            try {
                restClient = new RestClient();
                var request = new RestRequest(_url, _method);
                request.AddHeader("Accept", "text/xml");
                request.AddHeader("Content-Type", "text/xml");
                var response = restClient.Execute(request);
                if (response.StatusCode == System.Net.HttpStatusCode.OK) {
                    return response.Content;
                }
                else {
                    return null;
                }
            }
            catch (Exception _ex) {
                throw _ex;
            }
        }

        public virtual string? ExecuteOKSID(string _url, Method _method) {
            try {
                restClient = new RestClient();
                var request = new RestRequest(_url, _method);
                request.AddHeader("Accept", "text/xml");
                request.AddHeader("Content-Type", "text/xml");
                var response = restClient.Execute(request);
                if (response.StatusCode == System.Net.HttpStatusCode.OK) {
                    return response.Content;
                }
                else {
                    return null;
                }
            }
            catch (Exception _ex) {
                throw _ex;
            }
        }

        public virtual string? ExecuteKOYUNCU(string _url, Method _method) {
            try {
                restClient = new RestClient();
                var request = new RestRequest(_url, _method);
                request.AddHeader("Accept", "text/xml");
                request.AddHeader("Content-Type", "text/xml");
                var response = restClient.Execute(request);
                if (response.StatusCode == System.Net.HttpStatusCode.OK) {
                    return response.Content;
                }
                else {
                    return null;
                }
            }
            catch (Exception _ex) {
                throw _ex;
            }
        }

        public string ConvertDateToString(DateTime date, bool is_ts) {
            DateTime utcTime;
            if (date.Kind == DateTimeKind.Local) {
                utcTime = new DateTime(
                    date.Year,
                    date.Month,
                    date.Day,
                    date.Hour,
                    date.Minute,
                    date.Second,
                    date.Millisecond,
                    DateTimeKind.Local).ToUniversalTime();
            }
            else {
                utcTime = date;
            }
            if (is_ts)
                return utcTime.ToString("yyyy-MM-dd\\THH:mm:ss.ff\\Z", System.Globalization.CultureInfo.InvariantCulture);
            else
                return utcTime.ToString("yyyy-MM-dd\\T21:00:00.00\\Z", System.Globalization.CultureInfo.InvariantCulture);
        }

        private string Base64Encode(string plainText) {
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }

        void IDisposable.Dispose() {
            restClient.Dispose();
        }
    }
}
