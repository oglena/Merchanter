﻿using ankaraerp;
using MerchanterHelpers.Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
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

        public async Task<List<US_TicariDokuman>?> GetProducts() {
            List<US_TicariDokuman>? dokumans = null;
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
                        Console.WriteLine(res.errormessages);
                        return null;
                    }

                    Directory.CreateDirectory(this.temp_folder_url);
                    File.WriteAllBytes(this.temp_folder_url + "\\" + res.filename, res.blob_data);
                    UnZip(this.temp_folder_url + "\\" + res.filename, this.temp_folder_url + "\\", "", true);
                    Console.WriteLine("Ana dosya indirildi ve açıldı.");

                    cuserdetail.MethodName = "B2CDownloadZipFile"; //paketlenen zip dosyasını indir.

                    XmlDocument xmldoc = new XmlDocument();
                    xmldoc.Load(this.temp_folder_url + "\\" + Path.GetFileNameWithoutExtension(res.filename) + ".xml");

                    dokumans = new List<US_TicariDokuman>();
                    foreach (XmlNode node in xmldoc.SelectSingleNode("/root")) {
                        xmlreq.Clear();
                        xmlreq.Add("<root>");
                        xmlreq.Add("<item>" + node.InnerText + "</item>"); //alınacak zip paket ismi 
                        xmlreq.Add("</root>");
                        var rsp = await client.InvokeAsync(cuserdetail, string.Join("", xmlreq.ToArray()));

                        if (res.errormessages != null) {
                            Console.WriteLine(res.errormessages);
                            return null;
                        }
                        else {
                            File.WriteAllBytes(this.temp_folder_url + "\\" + rsp.filename, rsp.blob_data);
                            UnZip(this.temp_folder_url + "\\" + rsp.filename, this.temp_folder_url + "\\", "", true);
                            Console.WriteLine(rsp.filename + " dosyası indirildi ve açıldı.");

                            string xmlFilePath = this.temp_folder_url + "\\" + Path.GetFileNameWithoutExtension(rsp.filename) + ".xml";
                            if (File.Exists(xmlFilePath)) {
                                XmlSerializer serializer = new XmlSerializer(typeof(US_TicariDokuman));
                                using FileStream fs = new FileStream(xmlFilePath, FileMode.Open);
                                US_TicariDokuman? dokuman = (US_TicariDokuman?)serializer.Deserialize(fs);
                                if (dokuman != null) {
                                    dokumans.Add(dokuman);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex) {
                    Console.WriteLine(ex.Message);
                    return null;
                }
            }

            return dokumans;
        }

        public List<US_TicariDokuman>? GetProductsFromFolder(string _folder_url) {
            string[] files = Directory.GetFiles(_folder_url, "*.xml");
            if (files.Length == 0) return null;

            List<US_TicariDokuman> dokumans = new List<US_TicariDokuman>();
            foreach (var item in files) {
                try {
                    string xmlFilePath = item;
                    if (File.Exists(item)) {
                        XmlSerializer serializer = new XmlSerializer(typeof(US_TicariDokuman));
                        using FileStream fs = new FileStream(item, FileMode.Open);
                        US_TicariDokuman? dokuman = (US_TicariDokuman?)serializer.Deserialize(fs);
                        if (dokuman != null) {
                            dokumans.Add(dokuman);
                        }
                    }
                }
                catch (Exception ex) {
                    return null;
                }
            }
            return dokumans;
        }

        public async Task<CAT_TicariDokuman?> GetCategories() {
            CAT_TicariDokuman? dokuman = null;
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
                        Console.WriteLine(res.errormessages);
                        return null;
                    }

                    Directory.CreateDirectory(this.temp_folder_url);
                    File.WriteAllBytes(this.temp_folder_url + "\\" + res.filename, res.blob_data);
                    UnZip(this.temp_folder_url + "\\" + res.filename, this.temp_folder_url + "\\", "", true);
                    Console.WriteLine(res.filename + " dosyası indirildi ve açıldı.");

                    string xmlFilePath = this.temp_folder_url + "\\" + Path.GetFileNameWithoutExtension(res.filename) + ".xml";
                    if (File.Exists(xmlFilePath)) {
                        XmlSerializer serializer = new XmlSerializer(typeof(CAT_TicariDokuman));
                        using FileStream fs = new FileStream(xmlFilePath, FileMode.Open);
                        dokuman = (CAT_TicariDokuman?)serializer.Deserialize(fs);
                    }
                }
                catch (Exception ex) {
                    Console.WriteLine(ex.Message);
                    return null;
                }
            }

            return dokuman;
        }

        public async Task<bool> SendOrder(string _guid, string _order_xml) {
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
                        Console.WriteLine(rem.Message);
                        Directory.Delete(this.temp_folder_url + "\\Orders\\" + _guid.ToString());
                        return true;
                    }
                }
                catch (Exception ex) {
                    Console.WriteLine(ex.Message);
                    return false;
                }
            }
            return false;
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
    }
}
