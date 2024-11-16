using iText.Html2pdf;
using iText.Kernel.Pdf;
using Merchanter.Classes;
using Merchanter.Classes.Settings;
using Merchanter.Responses;
using NetOpenX.Rest.Client;
using NetOpenX.Rest.Client.BLL;
using NetOpenX.Rest.Client.Model;
using NetOpenX.Rest.Client.Model.Enums;
using System.Diagnostics;

namespace Merchanter {
    public static class NetOpenXHelper {
        public static oAuth2 _oAuth2 { get; set; } = null;
        public static NetsisBelgeTipleri belge_tipleri { get; set; } = new NetsisBelgeTipleri();

        private static oAuth2? LoginNetsis( SettingsMerchanter _setting ) {
            _oAuth2 = new( _setting.netsis.rest_url );
            try {
                AuthResult result = _oAuth2.Login( new JLogin() {
                    BranchCode = _setting.netsis.siparis_subekodu,
                    NetsisUser = _setting.netsis.netopenx_user,
                    NetsisPassword = _setting.netsis.netopenx_password,
                    DbType = JNVTTipi.vtMSSQL,
                    DbName = _setting.netsis.dbname,
                    DbPassword = _setting.netsis.dbpassword,
                    DbUser = _setting.netsis.dbuser
                } );
                if( result.IsSuccessStatusCode ) {
                    //Console.WriteLine( "[" + DateTime.Now.ToString() + "] " + "Netsis login success" );
                    Debug.WriteLine( "[" + DateTime.Now.ToString() + "] " + "Netsis login success" );
                    return _oAuth2;
                }
                else {
                    WriteLogLine( "[" + DateTime.Now.ToString() + "] " + "Netsis Login invalid" + Environment.NewLine + result.error, ConsoleColor.Magenta );
                    Debug.WriteLine( "[" + DateTime.Now.ToString() + "] " + "Netsis Login invalid" );
                    return null;
                }
            }
            catch( Exception ex ) {
                WriteLogLine( "[" + DateTime.Now.ToString() + "] " + "Netsis login error" + Environment.NewLine + ex.ToString(), ConsoleColor.Red );
                Debug.WriteLine( "[" + DateTime.Now.ToString() + "] " + "Netsis login error" );
                return null;
            }
        }

        public static string? GetNetsisCari( string _raw_cari_kodu ) {
            string _cari_kodu = GenerateCariKod( _raw_cari_kodu );
            var _ARPsManager = new ARPsManager( LoginNetsis( Helper.global ) );
            var resultCMById = _ARPsManager.GetInternalById( _cari_kodu );
            if( resultCMById.Data == null ) {
                return null;
            }
            else {
                return resultCMById.Data.CariTemelBilgi.CARI_KOD;
            }
        }

        public static string? GetNetsisSiparis( string _raw_fatura_no, bool is_need_generate = true ) {
            if( is_need_generate ) _raw_fatura_no = GenerateFaturaNo( _raw_fatura_no );
            var _item_slips_manager = new ItemSlipsManager( LoginNetsis( Helper.global ) );
            var resultCMById = _item_slips_manager.GetInternalById( belge_tipleri.MUSTERI_SIPARISI + ";" + _raw_fatura_no );
            if( resultCMById.Data == null ) {
                return null;
            }
            else {
                return resultCMById.Data.FatUst.FATIRS_NO;
            }
        }

        public static string? GetNetsisFatura( string _fatura_no ) {
            var _item_slips_manager = new ItemSlipsManager( LoginNetsis( Helper.global ) );
            var resultCMById = _item_slips_manager.GetInternalById( belge_tipleri.SATIS_FATURASI + ";" + _fatura_no );
            if( resultCMById.Data == null ) {
                return null;
            }
            else {
                return resultCMById.Data.FatUst.FATIRS_NO;
            }
        }

        public static int? GetNetsisFaturaCountByDate( int _days ) {
            var _item_slips_manager = new ItemSlipsManager( LoginNetsis( Helper.global ) );
            var resultCMById = _item_slips_manager.GetInternal( JTFaturaTip.ftSFat, new SelectFilter() {
                Filter = "Tarih BETWEEN '" + DateTime.Now.AddDays( -1 * _days ).ToString( "yyyy-MM-dd 00:00:00" ) + "' AND '" + DateTime.Now.ToString( "yyyy-MM-dd 23:59:59" ) + "'",
                Offset = 0,
                Limit = 100,
                Fields = [ "Tarih" ]
            } );
            if( resultCMById.Data == null ) {
                return null;
            }
            else {
                return resultCMById.Data.Count;
            }
        }

        public static List<NETSIS_InvoiceResponse> GetNetsisFaturas( int _days, List<Invoice>? _past_invoices, int _limit = 250 ) {
            List<NetOpenX.Rest.Client.Model.NetOpenX.ItemSlips> data = [];
            int offset = 0;
            var _item_slips_manager = new ItemSlipsManager( LoginNetsis( Helper.global ) );
        QUERY:
            var query = _item_slips_manager.GetInternal( JTFaturaTip.ftSFat, new SelectFilter() {
                Filter = "Tarih > '" + DateTime.Now.AddDays( -1 * _days ).ToString( "yyyy-MM-dd 00:00:00" ) + "' AND FTIRSIP = '1' AND TIPI = 2",
                Offset = offset,
                Limit = _limit,
                Fields = [ "FATIRS_NO", "FTIRSIP", "TIPI", "CariKod", "ACIK15" ]
            } ).Data;
            if( query != null )
                data.AddRange( query );
            if( data.Count == 0 ) {
                return null;
            }
            else {
                if( (data.Count == _limit + offset) ) {
                    offset += _limit;
                    goto QUERY;
                }

                List<NETSIS_InvoiceResponse> response = [];
                foreach( var item in data ) {
                    var temp_invoice = _past_invoices?.Where( x => x.invoice_no == item.FatUst.FATIRS_NO ).Where( x => x.is_belge_created ).FirstOrDefault();
                    if( temp_invoice == null ) {
                        try {
                            var selected_item = _item_slips_manager.GetInternalById( belge_tipleri.SATIS_FATURASI + ";" + item.FatUst.FATIRS_NO ).Data;
                            NETSIS_InvoiceResponse invoice = new() {
                                FATURANO = selected_item.FatUst.FATIRS_NO,
                                EKACK1 = selected_item.FatUst.EKACK1,
                                EKACK2 = selected_item.FatUst.EKACK2,
                                CARIKODU = selected_item.FatUst.CariKod,
                                CARIGRUP = selected_item.FatUst.CariKod.Split( "-" )[ 0 ],
                                GENELTOPLAM = Convert.ToDecimal( selected_item.FatUst.GENELTOPLAM ),
                                BRUTTUTAR = Convert.ToDecimal( selected_item.FatUst.BRUTTUTAR ),
                                KDV = Convert.ToDecimal( selected_item.FatUst.KDV ),
                                FATKALEM_ADEDI = selected_item.KalemAdedi.HasValue ? selected_item.KalemAdedi.Value : 0,
                                KDV_DAHILMI = selected_item.FatUst.KDV_DAHILMI.HasValue ? selected_item.FatUst.KDV_DAHILMI.Value : false,
                                TARIH = selected_item.FatUst.Tarih,
                                GIB_FATIRS_NO = selected_item.FatUst.GIB_FATIRS_NO,
                                KALEMS = []
                            };
                            List<string> sipnums = [];
                            foreach( var kalem in selected_item.Kalems ) {
                                invoice.KALEMS.Add( new NETSIS_InvoiceItemResponse() {
                                    FATURANO = selected_item.FatUst.FATIRS_NO,
                                    STOKKODU = kalem.StokKodu,
                                    MIKTAR = Convert.ToInt32( kalem.STra_GCMIK ),
                                    TARIH = kalem.Stra_FiiliTar,
                                    SIPARISNO = kalem.STra_SIPNUM,
                                    FIYAT = Convert.ToDecimal( kalem.STra_NF ),
                                    KDV_ORAN = Convert.ToInt32( kalem.SatisKDVOran ),
                                    SERILER = kalem.KalemSeri != null ? kalem.KalemSeri.Select( x => x.Seri1 ).ToList() : []
                                } );
                                if( !sipnums.Contains( kalem.STra_SIPNUM ) ) { }
                                sipnums.Add( kalem.STra_SIPNUM );
                            }
                            invoice.SIPARISNO = sipnums[ 0 ];
                            if( !string.IsNullOrWhiteSpace( invoice.FATURANO ) && !string.IsNullOrWhiteSpace( invoice.SIPARISNO ) ) {
                                response.Add( invoice );
                                WriteLogLine( "[" + DateTime.Now.ToString() + "] " + invoice.FATURANO + " processing", ConsoleColor.Blue );
                            }
                            else {
                                WriteLogLine( "[" + DateTime.Now.ToString() + "] " + invoice.FATURANO + " invalid invoice", ConsoleColor.Red );
                            }
                        }
                        catch( Exception ex ) {
                            WriteLogLine( "[" + DateTime.Now.ToString() + "] " + "Netsis invoice " + item.FatUst.FATIRS_NO + " response error" + Environment.NewLine + ex.ToString(), ConsoleColor.Red );
                            Debug.WriteLine( "[" + DateTime.Now.ToString() + "] " + "Netsis invoice " + item.FatUst.FATIRS_NO + " response error" );
                            continue;
                        }
                    }
                }

                return response;
            }
        }

        public static string? InsertNetsisCari( string _raw_cari_kodu, string _email, BillingAddress _billing_address, string _payment_method ) {
            var selected_cari_kodu = GetNetsisCari( _raw_cari_kodu );
            if( selected_cari_kodu == null ) {
                string _cari_kodu = GenerateCariKod( _raw_cari_kodu );
                var _ARPsManager = new ARPsManager( LoginNetsis( Helper.global ) );
                var _ARPs = new NetOpenX.Rest.Client.Model.NetOpenX.ARPs() {
                    CariTemelBilgi = new NetOpenX.Rest.Client.Model.NetOpenX.ARPsPrimInfo() {
                        CARI_KOD = _cari_kodu,
                        CARI_ISIM = _billing_address.is_corporate ? _billing_address.firma_ismi : _billing_address.firstname + " " + _billing_address.lastname,
                        CARI_TIP = Helper.global.netsis.sipari_caritip,
                        Sube_Kodu = Helper.global.netsis.siparis_subekodu,
                        ULKE_KODU = "TR",
                        CARI_ADRES = string.Concat( _billing_address.street ) + "    " + CheckNull( _billing_address.region ).ToUpper() + "/" + CheckNull( _billing_address.city ).ToUpper(),
                        CARI_IL = CheckNull( _billing_address.city ).ToUpper(),
                        CARI_TEL = _billing_address.telephone,
                        CARI_ILCE = CheckNull( _billing_address.region ).ToUpper(),
                        EMAIL = _email,
                        Grup_Kodu = DbHelper.GetSettingValue( Helper.global.netsis.cari_siparis_grupkodu, _payment_method ),
                        M_KOD = Helper.global.netsis.siparis_muhasebekodu,
                        VERGI_DAIRESI = _billing_address.is_corporate ? _billing_address.firma_vergidairesi : string.Empty,
                        VERGI_NUMARASI = _billing_address.is_corporate ? _billing_address.firma_vergino : string.Empty,
                        Onceki_Kod = null
                    },
                    CariEkBilgi = new NetOpenX.Rest.Client.Model.NetOpenX.ARPsSuppInfo() {
                        CARI_KOD = _cari_kodu,
                        TcKimlikNo = _billing_address.tc_no ?? string.Empty
                    }
                };
                var resultPostDataCM = _ARPsManager.PostInternal( _ARPs );
                if( resultPostDataCM.IsSuccessful ) {
                    WriteLogLine( "[" + DateTime.Now.ToString() + "] " + resultPostDataCM.Data.CariTemelBilgi.CARI_KOD + " Netsis Customer inserted", ConsoleColor.Green );
                    Debug.WriteLine( "[" + DateTime.Now.ToString() + "] " + resultPostDataCM.Data.CariTemelBilgi.CARI_KOD + " Netsis Customer inserted" );
                    return resultPostDataCM.Data.CariTemelBilgi.CARI_KOD;
                }
                else {
                    WriteLogLine( "[" + DateTime.Now.ToString() + "] " + "Netsis Customer insert fail => " + resultPostDataCM.Message, ConsoleColor.Red );
                    Debug.WriteLine( "[" + DateTime.Now.ToString() + "] " + "Netsis Customer insert fail => " + resultPostDataCM.Message );
                    return null;
                }
            }
            else {
                return selected_cari_kodu;
            }

        }

        public static string? InsertNetsisMusSiparis( Order _order, string _cari_kodu, string? _order_shipping_barcode ) {
            ItemSlipsManager _item_slips_manager = new( LoginNetsis( Helper.global ) );
            NetOpenX.Rest.Client.Model.NetOpenX.ItemSlips slip = new() {
                FaturaTip = JTFaturaTip.ftSSip,
                //Seri = _is_corporate ? settings.NetsisBelgeOnek_EFATURA : settings.NetsisBelgeOnek_EARSIV,
                //SonNumaraYazilsin = true,
                //KayitliNumaraOtomatikGuncellensin = true,
                SeriliHesapla = false,
                FatUst = new NetOpenX.Rest.Client.Model.NetOpenX.ItemSlipsHeader {
                    FATIRS_NO = GenerateFaturaNo( _order.order_id.ToString() ),
                    Sube_Kodu = Helper.global.netsis.siparis_subekodu,
                    TIPI = JTFaturaTipi.ft_YurtIci,
                    KDV_DAHILMI = Helper.global.order.siparis_kdvdahilmi,
                    Tip = JTFaturaTip.ftSSip,
                    Tarih = DateTime.Now,
                    FiiliTarih = DateTime.Now,
                    SIPARIS_TEST = DateTime.Now,
                    EfaturaCarisiMi = _order.billing_address.is_corporate,
                    CariKod = _cari_kodu,
                    KOD2 = Helper.global.netsis.siparis_kod2,
                    EKACK1 = _order.order_id.ToString(),
                    EKACK2 = _order.order_label,
                    EKACK3 = _order.shipping_address?.firstname + " " + _order.shipping_address?.lastname,
                    EKACK4 = DbHelper.GetSettingValue( Helper.global.netsis.siparis_ekack4, _order.payment_method ),
                    EKACK5 = _order.shipping_address?.telephone,
                    EKACK7 = _order.order_date.ToString(),
                    EKACK8 = _order_shipping_barcode,
                    EKACK10 = DbHelper.GetSettingValue( Helper.global.netsis.siparis_ekack10, _order.shipment_method ),
                    EKACK11 = Helper.global.netsis.siparis_ekack11,
                    EKACK12 = _order.comment,
                    EKACK15 = DbHelper.GetSettingValue( Helper.global.netsis.siparis_ekack15, _order.payment_method ),
                },
                Kalems = []
            };

            foreach( var item in _order.order_items ) {
                NetOpenX.Rest.Client.Model.NetOpenX.ItemSlipLines slipLine = new() {
                    StokKodu = item.sku,
                    STra_ACIK = item.parent_sku,
                    STra_KDV = Convert.ToDouble( item.tax ),
                    DEPO_KODU = Helper.global.netsis.siparis_depokodu,
                    STra_GCMIK = (double)item.qty_ordered,
                    STra_NF = Convert.ToDouble( item.price - item.tax_amount ),
                    STra_BF = Convert.ToDouble( item.price - item.tax_amount )
                };
                slip.Kalems.Add( slipLine );
            }

            if( _order.installment_amount > 0 ) {
                NetOpenX.Rest.Client.Model.NetOpenX.ItemSlipLines slipLine = new() {
                    StokKodu = Helper.global.order.siparis_taksitkomisyon_sku,
                    DEPO_KODU = Helper.global.netsis.siparis_depokodu,
                    STra_GCMIK = (double)1,
                    STra_NF = Convert.ToDouble( _order.installment_amount / 1.20f ), //TODO:burasını bi düşünmek lazım
                    STra_BF = Convert.ToDouble( _order.installment_amount / 1.20f )
                };
                slip.Kalems.Add( slipLine );
            }

            if( _order.discount_amount != 0 ) {
                slip.FatUst.GEN_ISK1O = Convert.ToDouble( (_order.discount_amount * -1) / _order.subtotal + _order.installment_amount ) * 100;
            }

            var result = _item_slips_manager.PostInternal( slip );
            if( result.IsSuccessful ) {
                string erp_no = result.Data.FatUst.FATIRS_NO;
                WriteLogLine( "[" + DateTime.Now.ToString() + "] " + result.Data.FatUst.FATIRS_NO + " Netsis Customer Order inserted", ConsoleColor.Green );
                Debug.WriteLine( "[" + DateTime.Now.ToString() + "] " + result.Data.FatUst.FATIRS_NO + " Netsis Customer Order inserted" );
                return result.Data.FatUst.FATIRS_NO;
            }
            else {
                WriteLogLine( "[" + DateTime.Now.ToString() + "] " + "Netsis Customer Order insert fail => " + result.Message, ConsoleColor.Red );
                Debug.WriteLine( "[" + DateTime.Now.ToString() + "] " + "Netsis Customer Order insert fail => " + result.Message );
                return null;
            }
        }

        /// <summary>
        /// Get Netsis Fatura
        /// </summary>
        /// <param name="_belgeno"></param>
        /// <param name="_belgetip">0=EARSIV,1=EFATURA</param>
        public static string? GetEbelge( string _belgeno ) {
            EDocumentManager ebelge = new( LoginNetsis( Helper.global ) );
            int belgetip = _belgeno.StartsWith( Helper.global.netsis.efatura_belge_onek ) ? 1 : _belgeno.StartsWith( Helper.global.netsis.earsiv_belge_onek ) ? 0 : -1;
            var response = ebelge.ShowEDocument( new NetOpenX.Rest.Client.Model.Custom.EDocumentShowParam() {
                HtmlPath = belgetip == 1 ? Helper.global.netsis.ebelge_dizayn_efatura : Helper.global.netsis.ebelge_dizayn_earsiv,
                GIBDocumentNumber = _belgeno,
                DocumentBoxType = JTEBelgeBoxType.ebAll,
                EDocumentType = (JTEBelgeTip)belgetip
            } );
            if( response != null && response.IsSuccessful ) {
                if( !string.IsNullOrWhiteSpace( response.Data ) ) {
                    if( !Directory.Exists( Environment.CurrentDirectory + @"\" + Helper.global.netsis.ebelge_klasorismi + @"\" + Helper.global.settings.company_name + @"\" ) ) {
                        Directory.CreateDirectory( Environment.CurrentDirectory + @"\" + Helper.global.netsis.ebelge_klasorismi + @"\" + Helper.global.settings.company_name + @"\" );
                    }
                    string fullpath = Environment.CurrentDirectory + @"\" + Helper.global.netsis.ebelge_klasorismi + @"\" + Helper.global.settings.company_name + @"\" +
                        _belgeno + ".pdf";
                    HtmlConverter.ConvertToPdf( response.Data, new PdfWriter( fullpath ) );
                    return fullpath;
                }
            }
            return null;
        }

        /// <summary>
        /// Order billing_id
        /// </summary>
        /// <param name="_raw_cari_kodu"></param>
        /// <returns></returns>
        private static string GenerateCariKod( string _raw_cari_kodu ) {
            while( _raw_cari_kodu.Length < Constants.carikod_length - Helper.global.netsis.siparis_carionek.Length ) {
                _raw_cari_kodu = _raw_cari_kodu.Insert( 0, "0" );
            }
            return _raw_cari_kodu.Insert( 0, Helper.global.netsis.siparis_carionek );
        }

        /// <summary>
        /// Order order_id
        /// </summary>
        /// <param name="_raw_fatura_no"></param>
        /// <returns></returns>
        private static string GenerateFaturaNo( string _raw_fatura_no ) {
            while( _raw_fatura_no.Length < Constants.erp_sipno_length - Helper.global.netsis.belgeonek_musterisiparisi.Length ) {
                _raw_fatura_no = _raw_fatura_no.Insert( 0, "0" );
            }
            return _raw_fatura_no.Insert( 0, Helper.global.netsis.belgeonek_musterisiparisi );
        }

        private static string CheckNull( string item ) {
            if( !string.IsNullOrWhiteSpace( item ) ) {
                return item;
            }
            else {
                return string.Empty;
            }
        }

        private static void WriteLogLine( string value, ConsoleColor _color ) {
            Console.ForegroundColor = _color;
            Console.WriteLine( value.PadRight( Console.WindowWidth - 1 ) );
            Console.ResetColor();
        }
    }

    public class NetsisBelgeTipleri {
        public string SATIS_FATURASI { get; set; } = "ftSFat";
        public string MUSTERI_SIPARISI { get; set; } = "ftSSip";
        public int ebtArsiv { get; set; } = 0;
        public int ebtEFatura { get; set; } = 1;
    }
}
