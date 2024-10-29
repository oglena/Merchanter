using Merchanter.Classes;
using MerchanterApp.CMS.Models;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using System.Text;
using static MerchanterApp.CMS.Classes.PostHelper;

namespace MerchanterApp.CMS.Classes {
    public interface IPostHelper {
        public Task<BaseResponseModel?> Request( int _admin_id, string? _token, PostMethod _method, string _url, StringContent? _body = null );
    }
    public class PostHelper :IPostHelper {
        [Inject]
        private IConfiguration? configuration { get; set; }
        public PostHelper( IConfiguration _configuration ) {
            configuration = _configuration;
        }
        public async Task<BaseResponseModel?> Request( int _admin_id, string? _token, PostMethod _method, string _url, StringContent? _body = null ) {
            BaseResponseModel? model = null;
            switch( _method ) {
                case PostMethod.Get:
                    if( _admin_id > 0 && !string.IsNullOrWhiteSpace( _token ) ) {
                        using( HttpClient httpClient = new HttpClient() ) {
                            httpClient.BaseAddress = new Uri( configuration[ "AppSettings:MerchanterServerUrl" ] );
                            httpClient.DefaultRequestHeaders.Add( "Authorization", "Bearer " + _token );
                            using HttpResponseMessage response = await httpClient.GetAsync( _url );
                            if( response.IsSuccessStatusCode ) {
                                model = JsonConvert.DeserializeObject<BaseResponseModel>( response.Content.ReadAsStringAsync().Result );
                            }
                        }
                    }
                    break;
                case PostMethod.Post:
                    if( _admin_id > 0 && !string.IsNullOrWhiteSpace( _token ) ) {
                        using( HttpClient httpClient = new HttpClient() ) {
                            httpClient.BaseAddress = new Uri( configuration[ "AppSettings:MerchanterServerUrl" ] );
                            httpClient.DefaultRequestHeaders.Add( "Authorization", "Bearer " + _token );
                            using HttpResponseMessage response = await httpClient.PostAsync( _url, _body );
                            if( response.IsSuccessStatusCode ) {
                                model = JsonConvert.DeserializeObject<BaseResponseModel>( response.Content.ReadAsStringAsync().Result );
                            }
                        }

                    }
                    break;
                case PostMethod.Put:
                    return null;
                case PostMethod.Delete:
                    return null;
                default:
                    return null;
            }
            return model;
        }

        public enum PostMethod {
            Get = 0,
            Post = 1,
            Put = 2,
            Delete = 3
        }
    }
}
