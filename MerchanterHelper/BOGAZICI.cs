using MerchanterHelper.Classes;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace MerchanterHelper {
    public class BOGAZICI {
        public string bogazici_bayikodu { get; set; }
        public string bogazici_email { get; set; } 
        public string bogazici_sifre { get; set; }
        public BOGAZICI( string _bogazici_bayikodu, string _bogazici_email, string _bogazici_sifre ) {
            bogazici_bayikodu = _bogazici_bayikodu;
            bogazici_email = _bogazici_email;
            bogazici_sifre = _bogazici_sifre;
        }

        public List<XProducts_BOGAZICI>? getBogaziciProducts() {
            try {
                bogazici_service.BBXMLServicesSoapClient client = new bogazici_service.BBXMLServicesSoapClient( bogazici_service.BBXMLServicesSoapClient.EndpointConfiguration.BBXMLServicesSoap );
                XmlNode xxx = client.GetProducts( bogazici_bayikodu, bogazici_email, bogazici_sifre );
                XElement xmlProducts = XElement.Parse( xxx.OuterXml );

                var result = xmlProducts.Elements()
                    .Select( element => new XProducts_BOGAZICI {
                        LOGICALREF = element.Element( "LOGICALREF" ).Value,
                        PARENTREF = element.Element( "PARENTREF" ).Value,
                        SINIFREF = element.Element( "SINIFREF" ).Value,
                        KATEGORI = element.Element( "KATEGORI" ).Value,
                        SINIF = element.Element( "SINIF" ).Value,
                        STOKKOD = element.Element( "STOKKOD" ).Value,
                        PRODUCERCODE = element.Element( "PRODUCERCODE" ).Value,
                        STOKACIKLAMA = element.Element( "STOKACIKLAMA" ).Value,
                        MARKA = element.Element( "MARKA" ).Value,
                        KDVORAN = element.Element( "KDVORAN" ).Value,
                        B2BSABLONREF = element.Element( "B2BSABLONREF" ).Value,
                        RESIM = element.Element( "RESIM" ).Value,
                        EAN1 = (element.Element( "EAN1" ) != null) ? element.Element( "EAN1" ).Value : string.Empty
                    } )
                    .ToList();

                var stock_result = getBogaziciProductStocks();
                if( stock_result != null ) {
                    foreach( var item in stock_result ) {
                        var temp_product = result.Where( x => x.LOGICALREF == item.LOGICALREF ).FirstOrDefault();
                        if( temp_product != null ) {
                            temp_product.STOK = item.STOK;
                            temp_product.BIRIMFIYAT = item.BIRIMFIYAT;
                            temp_product.BIRIMDOVIZ = item.BIRIMDOVIZ;
                            temp_product.SKFIYAT = item.SKFIYAT;
                        }
                    }
                }
                return result;
            }
            catch {
                return null;
            }
        }

        private List<XProducts_BOGAZICI> getBogaziciProductStocks() {
            bogazici_service.BBXMLServicesSoapClient client = new bogazici_service.BBXMLServicesSoapClient( bogazici_service.BBXMLServicesSoapClient.EndpointConfiguration.BBXMLServicesSoap );
            XElement xmlPStock = XElement.Parse( client.GetPriceANDStock( bogazici_bayikodu, bogazici_email, bogazici_sifre ).OuterXml );

            return xmlPStock.Elements()
                .Select( element => new XProducts_BOGAZICI {
                    LOGICALREF = element.Element( "LOGICALREF" ).Value,
                    VAT = element.Element( "VAT" ).Value,
                    BIRIMFIYAT = element.Element( "BIRIMFIYAT" ).Value,
                    BIRIMDOVIZ = element.Element( "BIRIMDOVIZ" ).Value,
                    SKFIYAT = element.Element( "SKFIYAT" ).Value,
                    SKDOVIZ = element.Element( "SKDOVIZ" ).Value,
                    STOK = element.Element( "STOK" ).Value
                } )
                .ToList();
        }
    }
}
