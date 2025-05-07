using Merchanter.Classes.Settings;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text;

namespace Merchanter {
    public static partial class Helper {
        public static SettingsMerchanter global { get; set; }

        #region Helper Functions
        public static void PostPageAll(object _url) {
            try {
                if (!string.IsNullOrWhiteSpace(_url?.ToString())) {
                    Console.WriteLine("[" + DateTime.Now.ToString() + "] " + "Magento Indexer Started");
                    Debug.WriteLine("[" + DateTime.Now.ToString() + "] " + "Magento Indexer Started");

                    using (var client = new HttpClient()) {
                        var response = client.GetAsync(_url.ToString()).Result;
                        if (response.IsSuccessStatusCode) {
                            var res = response.Content.ReadAsStringAsync().Result;
                            Console.WriteLine("[" + DateTime.Now.ToString() + "] " + "Re:index Done." + Environment.NewLine + res);
                        }
                    }
                }
            }
            catch (Exception ex) {
                Console.WriteLine("[" + DateTime.Now.ToString() + "] " + ex.ToString());
                Debug.WriteLine("[" + DateTime.Now.ToString() + "] " + ex.ToString());
            }
            finally {
                Console.WriteLine("[" + DateTime.Now.ToString() + "] " + "Magento Indexer Ended");
                Debug.WriteLine("[" + DateTime.Now.ToString() + "] " + "Magento Indexer Ended");
            }
        }

        private static string ConvertFriendly(string _str) {
            return _str.Replace("/", "&#47;");
        }

        private static string ConvertDateToString(DateTime date, bool is_ts) {
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

        public static string GetProductFinalPrice(decimal _price_value, ENT_CurrencyRates _ent_currency_rates, string _currency, bool _tax_included, int _tax) {
            return Math.Round(_price_value * (_tax_included ? 1 : (1 + ((decimal)_tax / 100m))) * ((_currency == "USD") ? _ent_currency_rates.USD : ((_currency == "EUR") ? _ent_currency_rates.EUR : 1)), 2, MidpointRounding.AwayFromZero).ToString() + " TL";
        }

        public static string GetProductFinalPrice(decimal _price_value, string _currency, bool _tax_included, int _tax) {
            return Math.Round(_price_value * (_tax_included ? 1 : (1 + ((decimal)_tax / 100m))), 2, MidpointRounding.AwayFromZero).ToString() + " " + _currency;
        }

        public static void UploadFileToFtp(string url, string filePath, string username, string password) {
            var fileName = Path.GetFileName(filePath);
            var uri = new Uri(url + fileName);

            using (var client = new HttpClient()) {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                    "Basic", Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes($"{username}:{password}")));

                using (var fileStream = File.OpenRead(filePath)) {
                    var content = new StreamContent(fileStream);
                    var response = client.PutAsync(uri, content).Result;

                    if (response.IsSuccessStatusCode) {
                        Console.WriteLine("Upload done: {0}", response.ReasonPhrase);
                    }
                    else {
                        Console.WriteLine("Upload failed: {0}", response.ReasonPhrase);
                    }
                }
            }
        }

        /// <summary>
        /// Writes console and debug messages.
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="is_console">Is Console ?</param>
        /// <param name="is_debug">Is Debug ?</param>
        private static void PrintConsole(string message, bool is_console = true, bool is_debug = true) {
            if (is_console)
                Console.WriteLine("#" + DateTime.Now.ToString("dd.MM.yy HH:mm:ss") + "," + global.settings.company_name + ", " + message);
            if (is_debug)
                Debug.WriteLine("#" + DateTime.Now.ToString("dd.MM.yy HH:mm:ss") + "," + global.settings.company_name + ", " + message);
        }

        /// <summary>
        /// Converts Base64 to String
        /// </summary>
        /// <param name="base64EncodedData">Source</param>
        /// <returns>Decoded Data</returns>
        public static string Base64ToString(string base64EncodedData) {
            if (!string.IsNullOrWhiteSpace(base64EncodedData)) {
                byte[] base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
                return Encoding.GetEncoding("ISO-8859-9").GetString(base64EncodedBytes); // ISO-8859-9 supports Turkish characters
            }
            return string.Empty;
        }

        /// <summary>
        /// Converts image file to Base64 string
        /// </summary>
        /// <param name="folderUrl">Image Folder</param>
        /// <param name="imageName">Image Name</param>
        /// <returns>Base64 string</returns>
        /// <exception cref="FileNotFoundException"></exception>
        public static string? GetImageAsBase64(string folderUrl, string imageName) {
            try {
                var filePath = Path.Combine(folderUrl, imageName);
                if (File.Exists(filePath)) {
                    byte[] imageBytes = File.ReadAllBytes(filePath);
                    return Convert.ToBase64String(imageBytes);
                }
                else {
                    //throw new FileNotFoundException("Image not found at the specified path.");
                    return null;
                }
            }
            catch (Exception ex) {
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }
        #endregion
    }
}
