using ankaraerp;
using MerchanterHelpers.Classes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace MerchanterHelpers {
    public class ANKERP {
        public string company_code { get; set; }
        public string user_name { get; set; }
        public string password { get; set; }
        public string work_year { get; set; }
        public string url { get; set; }
        public string temp_folder_url { get; set; }

        public ANKERP(string p_company_code, string p_user_name, string p_password, string p_work_year, string p_url, string p_temp_folder_url) {
            this.company_code = p_company_code;
            this.user_name = p_user_name;
            this.password = p_password;
            this.work_year = p_work_year;
            this.url = p_url;
            this.temp_folder_url = p_temp_folder_url;
        }

        public async Task<List<TicariDokumanUrun>?> GetProducts() {
            List<TicariDokumanUrun>? dokumans = null;
            using (DataShareClient client = new DataShareClient(DataShareClient.EndpointConfiguration.BasicHttpBinding_IDataShare,
                url)) {
                try {
                    ClientUser cuserdetail = new ClientUser {
                        CompanyCode = this.company_code,
                        MethodName = "B2CExportFilePrepare",  //stok bilgilerini hazırla ve getir
                        UserName = this.user_name,
                        PassWord = this.password,
                        WorkYear = this.work_year.ToString()
                    };

                    List<string> xmlreq = new List<string>();
                    xmlreq.Add("<root></root>"); //boş xml dokumanı gönder
                    client.InnerChannel.OperationTimeout = new TimeSpan(10, 0, 0);  //timeout 8 saat
                    BlobFile? res = await client.InvokeAsync(cuserdetail, string.Join("", xmlreq.ToArray())); //web servis çağrılıyor...
                    if (res.errormessages != null) {
                        PrintConsole(res.errormessages);
                        return null;
                    }

                    Directory.CreateDirectory(this.temp_folder_url);
                    File.WriteAllBytes(this.temp_folder_url + "\\" + res.filename, res.blob_data);
                    UnZip(this.temp_folder_url + "\\" + res.filename, this.temp_folder_url + "\\", "", true);
                    PrintConsole("Ana dosya indirildi ve açıldı.");

                    cuserdetail.MethodName = "B2CDownloadZipFile"; //paketlenen zip dosyasını indir.

                    XmlDocument xmldoc = new XmlDocument();
                    xmldoc.Load(this.temp_folder_url + "\\" + Path.GetFileNameWithoutExtension(res.filename) + ".xml");

                    dokumans = new List<TicariDokumanUrun>();
                    foreach (XmlNode node in xmldoc.SelectSingleNode("/root")) {
                        xmlreq.Clear();
                        xmlreq.Add("<root>");
                        xmlreq.Add("<item>" + node.InnerText + "</item>"); //alınacak zip paket ismi 
                        xmlreq.Add("</root>");
                        var rsp = await client.InvokeAsync(cuserdetail, string.Join("", xmlreq.ToArray()));

                        if (res.errormessages != null) {
                            PrintConsole(res.errormessages);
                            return null;
                        }
                        else {
                            File.WriteAllBytes(this.temp_folder_url + "\\" + rsp.filename, rsp.blob_data);
                            UnZip(this.temp_folder_url + "\\" + rsp.filename, this.temp_folder_url + "\\", "", true);
                            PrintConsole(rsp.filename + " dosyası indirildi ve açıldı.");

                            string xmlFilePath = this.temp_folder_url + "\\" + Path.GetFileNameWithoutExtension(rsp.filename) + ".xml";
                            if (File.Exists(xmlFilePath)) {
                                XmlSerializer serializer = new XmlSerializer(typeof(TicariDokumanUrun));
                                using FileStream fs = new FileStream(xmlFilePath, FileMode.Open);
                                TicariDokumanUrun? dokuman = (TicariDokumanUrun?)serializer.Deserialize(fs);
                                if (dokuman != null) {
                                    dokumans.Add(dokuman);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex) {
                    PrintConsole(ex.Message);
                    return null;
                }
            }

            return dokumans;
        }

        public TicariDokumanKategori? GetCategoriesFromFolder(string _folder_url) {
            string[] files = Directory.GetFiles(_folder_url, "*.xml");
            if (files.Length == 0) return null;

            TicariDokumanKategori? dokuman = null;
            try {
                string xmlFilePath = files[0];
                if (File.Exists(files[0])) {
                    XmlSerializer serializer = new XmlSerializer(typeof(TicariDokumanKategori));
                    using FileStream fs = new FileStream(files[0], FileMode.Open);
                    dokuman = (TicariDokumanKategori?)serializer.Deserialize(fs);
                    if (dokuman != null) {
                        return dokuman;
                    }
                }
            }
            catch (Exception ex) {
                PrintConsole(ex.Message);
                return null;
            }
            return dokuman;
        }

        public List<TicariDokumanUrun>? GetProductsFromFolder(string _folder_url) {
            string[] files = Directory.GetFiles(_folder_url, "*.xml");
            if (files.Length == 0) return null;

            List<TicariDokumanUrun> dokumans = new List<TicariDokumanUrun>();
            foreach (var item in files) {
                try {
                    string xmlFilePath = item;
                    if (File.Exists(item)) {
                        XmlSerializer serializer = new XmlSerializer(typeof(TicariDokumanUrun));
                        using FileStream fs = new FileStream(item, FileMode.Open);
                        TicariDokumanUrun? dokuman = (TicariDokumanUrun?)serializer.Deserialize(fs);
                        if (dokuman != null) {
                            dokumans.Add(dokuman);
                        }
                    }
                }
                catch (Exception ex) {
                    PrintConsole(ex.Message);
                    return null;
                }
            }
            return dokumans;
        }

        public async Task<TicariDokumanKategori?> GetCategories() {
            TicariDokumanKategori? dokuman = null;
            using (DataShareClient client = new DataShareClient(DataShareClient.EndpointConfiguration.BasicHttpBinding_IDataShare,
                url)) {
                try {
                    ClientUser cuserdetail = new ClientUser {
                        CompanyCode = this.company_code,
                        MethodName = "GetUrunKatKatalog",  //ürün kategori kataloğunu getir.
                        UserName = this.user_name,
                        PassWord = this.password,
                        WorkYear = this.work_year.ToString()
                    };

                    List<string> xmlreq = new List<string>();
                    xmlreq.Add("<root></root>"); //boş xml dokumanı gönder
                    client.InnerChannel.OperationTimeout = new TimeSpan(8, 0, 0);  //timeout 8 saat
                    BlobFile? res = await client.InvokeAsync(cuserdetail, string.Join("", xmlreq.ToArray())); //web servis çağrılıyor...
                    if (res.errormessages != null) {
                        PrintConsole(res.errormessages);
                        return null;
                    }

                    Directory.CreateDirectory(this.temp_folder_url);
                    File.WriteAllBytes(this.temp_folder_url + "\\" + res.filename, res.blob_data);
                    UnZip(this.temp_folder_url + "\\" + res.filename, this.temp_folder_url + "\\", "", true);
                    PrintConsole(res.filename + " dosyası indirildi ve açıldı.");

                    string xmlFilePath = this.temp_folder_url + "\\" + Path.GetFileNameWithoutExtension(res.filename) + ".xml";
                    if (File.Exists(xmlFilePath)) {
                        XmlSerializer serializer = new XmlSerializer(typeof(TicariDokumanKategori));
                        using FileStream fs = new FileStream(xmlFilePath, FileMode.Open);
                        dokuman = (TicariDokumanKategori?)serializer.Deserialize(fs);
                    }
                }
                catch (Exception ex) {
                    PrintConsole(ex.Message);
                    return null;
                }
            }

            return dokuman;
        }

        public async Task<string> SendOrder(string _guid, string _order_xml) {
            using (DataShareClient client = new DataShareClient(DataShareClient.EndpointConfiguration.BasicHttpBinding_IDataShare,
                url)) {
                try {
                    ClientUser cuserdetail = new ClientUser {
                        CompanyCode = this.company_code,
                        MethodName = "SendCustomerOrder",  //sipariş bilgilerini gönder
                        UserName = this.user_name,
                        PassWord = this.password,
                        WorkYear = this.work_year.ToString()
                    };

                    Directory.CreateDirectory(this.temp_folder_url + "\\Orders");
                    Directory.CreateDirectory(this.temp_folder_url + "\\Orders\\" + _guid.ToString());
                    File.WriteAllText(this.temp_folder_url + "\\Orders\\" + _guid.ToString() + "\\" + _guid.ToString() + ".xml", _order_xml);
                    Zip(this.temp_folder_url + "\\Orders\\" + _guid.ToString(), this.temp_folder_url + "\\Orders\\" + _guid.ToString() + ".zip");
                    BlobFile zipdoc = new BlobFile { filename = _guid.ToString() + ".zip" };
                    zipdoc.blob_data = File.ReadAllBytes(this.temp_folder_url + "\\Orders\\" + zipdoc.filename);
                    zipdoc.filesize = zipdoc.blob_data.Length;

                    ReturnMessage? rem = await client.SendDocumentAsync(cuserdetail, zipdoc); //web servis çağrılıyor...
                    if (rem?.Number != null) {
                        PrintConsole(rem.Number + " - " + rem.Message);
                        File.Move(this.temp_folder_url + "\\Orders\\" + _guid.ToString() + ".xml",
                            this.temp_folder_url + "\\Orders\\Processed\\" + _guid.ToString() + ".xml");
                        Directory.Delete(this.temp_folder_url + "\\Orders\\" + _guid.ToString(), true);
                        return rem.Number.ToString();
                    }
                }
                catch (Exception ex) {
                    PrintConsole(ex.Message);
                    return string.Empty;
                }
            }
            return string.Empty;
        }

        private void UnZip(string zipPath, string extractPath, string password, bool overwrite) {
            using (ZipArchive archive = ZipFile.OpenRead(zipPath)) {
                foreach (ZipArchiveEntry entry in archive.Entries) {
                    string destinationPath = Path.Combine(extractPath, entry.FullName);
                    if (overwrite || !File.Exists(destinationPath)) {
                        entry.ExtractToFile(destinationPath, overwrite);
                    }
                }
            }
        }

        private void Zip(string sourcePath, string zipPath) {
            if (File.Exists(zipPath)) {
                File.Delete(zipPath);
            }

            ZipFile.CreateFromDirectory(sourcePath, zipPath);
        }

        /// <summary>
        /// Writes console and debug messages.
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="is_console">Is Console ?</param>
        /// <param name="is_debug">Is Debug ?</param>
        private static void PrintConsole(string message, bool is_console = true, bool is_debug = true) {
            if (is_console)
                Console.WriteLine("#" + DateTime.Now.ToString("dd.MM.yy HH:mm:ss") + "," + "ANKARA_ERP" + "] " + message);
            if (is_debug)
                Debug.WriteLine("#" + DateTime.Now.ToString("dd.MM.yy HH:mm:ss") + "," + "ANKARA_ERP" + "] " + message);
        }
    }
}
