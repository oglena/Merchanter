using System.Collections.Generic;
using System.Xml.Serialization;

namespace MerchanterHelpers.Classes {
    // NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.ankarayazilim.com/TicariDokumanZarfi")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.ankarayazilim.com/TicariDokumanZarfi", IsNullable = false)]
    public partial class BS_TicariDokuman {

        private BS_TicariDokumanDokumanBaslik dokumanBaslikField;

        private BS_DokumanPaket dokumanPaketField;

        /// <remarks/>
        public BS_TicariDokumanDokumanBaslik DokumanBaslik {
            get {
                return this.dokumanBaslikField;
            }
            set {
                this.dokumanBaslikField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.ankarayazilim.com/DokumanPaket")]
        public BS_DokumanPaket DokumanPaket {
            get {
                return this.dokumanPaketField;
            }
            set {
                this.dokumanPaketField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.ankarayazilim.com/TicariDokumanZarfi")]
    public partial class BS_TicariDokumanDokumanBaslik {

        private string versiyonField;

        private BS_TicariDokumanDokumanBaslikGonderen gonderenField;

        private BS_TicariDokumanDokumanBaslikAlici aliciField;

        private BS_TicariDokumanDokumanBaslikDokumanTanimi dokumanTanimiField;

        /// <remarks/>
        public string Versiyon {
            get {
                return this.versiyonField;
            }
            set {
                this.versiyonField = value;
            }
        }

        /// <remarks/>
        public BS_TicariDokumanDokumanBaslikGonderen Gonderen {
            get {
                return this.gonderenField;
            }
            set {
                this.gonderenField = value;
            }
        }

        /// <remarks/>
        public BS_TicariDokumanDokumanBaslikAlici Alici {
            get {
                return this.aliciField;
            }
            set {
                this.aliciField = value;
            }
        }

        /// <remarks/>
        public BS_TicariDokumanDokumanBaslikDokumanTanimi DokumanTanimi {
            get {
                return this.dokumanTanimiField;
            }
            set {
                this.dokumanTanimiField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.ankarayazilim.com/TicariDokumanZarfi")]
    public partial class BS_TicariDokumanDokumanBaslikGonderen {

        private ulong vergiNoField;

        private string unvaniField;

        private string tanimlayiciField;

        /// <remarks/>
        public ulong VergiNo {
            get {
                return this.vergiNoField;
            }
            set {
                this.vergiNoField = value;
            }
        }

        /// <remarks/>
        public string Unvani {
            get {
                return this.unvaniField;
            }
            set {
                this.unvaniField = value;
            }
        }

        /// <remarks/>
        public string Tanimlayici {
            get {
                return this.tanimlayiciField;
            }
            set {
                this.tanimlayiciField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.ankarayazilim.com/TicariDokumanZarfi")]
    public partial class BS_TicariDokumanDokumanBaslikAlici {

        private ulong vergiNoField;

        private string unvaniField;

        private string tanimlayiciField;

        /// <remarks/>
        public ulong VergiNo {
            get {
                return this.vergiNoField;
            }
            set {
                this.vergiNoField = value;
            }
        }

        /// <remarks/>
        public string Unvani {
            get {
                return this.unvaniField;
            }
            set {
                this.unvaniField = value;
            }
        }

        /// <remarks/>
        public string Tanimlayici {
            get {
                return this.tanimlayiciField;
            }
            set {
                this.tanimlayiciField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.ankarayazilim.com/TicariDokumanZarfi")]
    public partial class BS_TicariDokumanDokumanBaslikDokumanTanimi {

        private string turuField;

        private string versiyonField;

        private string dosyaAdiField;

        private string olusturulmaZamaniField;

        /// <remarks/>
        public string Turu {
            get {
                return this.turuField;
            }
            set {
                this.turuField = value;
            }
        }

        /// <remarks/>
        public string Versiyon {
            get {
                return this.versiyonField;
            }
            set {
                this.versiyonField = value;
            }
        }

        /// <remarks/>
        public string DosyaAdi {
            get {
                return this.dosyaAdiField;
            }
            set {
                this.dosyaAdiField = value;
            }
        }

        /// <remarks/>
        public string OlusturulmaZamani {
            get {
                return this.olusturulmaZamaniField;
            }
            set {
                this.olusturulmaZamaniField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.ankarayazilim.com/DokumanPaket")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.ankarayazilim.com/DokumanPaket", IsNullable = false)]
    public partial class BS_DokumanPaket {

        private BS_Eleman elemanField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "")]
        public BS_Eleman Eleman {
            get {
                return this.elemanField;
            }
            set {
                this.elemanField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class BS_Eleman {

        private string elemanTipiField;

        private byte elemanSayisiField;

        private ElemanElemanListe elemanListeField;

        /// <remarks/>
        public string ElemanTipi {
            get {
                return this.elemanTipiField;
            }
            set {
                this.elemanTipiField = value;
            }
        }

        /// <remarks/>
        public byte ElemanSayisi {
            get {
                return this.elemanSayisiField;
            }
            set {
                this.elemanSayisiField = value;
            }
        }

        /// <remarks/>
        public ElemanElemanListe ElemanListe {
            get {
                return this.elemanListeField;
            }
            set {
                this.elemanListeField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ElemanElemanListe {

        private BelgeSicil belgeSicilField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.ankarayazilim.com/TicariBelge")]
        public BelgeSicil BelgeSicil {
            get {
                return this.belgeSicilField;
            }
            set {
                this.belgeSicilField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.ankarayazilim.com/TicariBelge")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.ankarayazilim.com/TicariBelge", IsNullable = false)]
    public partial class BelgeSicil {

        private BelgeSicilBaslik baslikField;

        private string baslikNotField;

        private string dipnotField;

        private List<BelgeSicilSatirDetay> satirDetayField;

        private BelgeSicilCariSicil cariSicilField;

        private BelgeSicilSevkYeri sevkYeriField;

        private string irsaliyeField;

        private string siparisField;

        private string isEmriField;

        [XmlAttribute("BELTUR")]
        private string bELTURField;

        [XmlAttribute("MS")]
        private string msField;

        /// <remarks/>
        public BelgeSicilBaslik Baslik {
            get {
                return this.baslikField;
            }
            set {
                this.baslikField = value;
            }
        }

        /// <remarks/>
        public string BaslikNot {
            get {
                return this.baslikNotField;
            }
            set {
                this.baslikNotField = value;
            }
        }

        /// <remarks/>
        public string Dipnot {
            get {
                return this.dipnotField;
            }
            set {
                this.dipnotField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("SatirDetay")]
        public List<BelgeSicilSatirDetay> SatirDetay {
            get {
                return this.satirDetayField;
            }
            set {
                this.satirDetayField = value;
            }
        }

        /// <remarks/>
        public BelgeSicilCariSicil CariSicil {
            get {
                return this.cariSicilField;
            }
            set {
                this.cariSicilField = value;
            }
        }

        /// <remarks/>
        public BelgeSicilSevkYeri SevkYeri {
            get {
                return this.sevkYeriField;
            }
            set {
                this.sevkYeriField = value;
            }
        }

        /// <remarks/>
        public string Irsaliye {
            get {
                return this.irsaliyeField;
            }
            set {
                this.irsaliyeField = value;
            }
        }

        /// <remarks/>
        public string Siparis {
            get {
                return this.siparisField;
            }
            set {
                this.siparisField = value;
            }
        }

        /// <remarks/>
        public string IsEmri {
            get {
                return this.isEmriField;
            }
            set {
                this.isEmriField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string BELTUR {
            get {
                return this.bELTURField;
            }
            set {
                this.bELTURField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string MS {
            get {
                return this.msField;
            }
            set {
                this.msField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.ankarayazilim.com/TicariBelge")]
    public partial class BelgeSicilBaslik {

        private string aciliyetField;

        private string hareketKoduField;

        private string projeNoField;

        private int belgeNoField;

        private int mBelgeNoField;

        private string dovizKoduField;

        private decimal dovizKuruField;

        private string hariciNumaraField;

        private string tanzimTarihiField;

        private string tanzimSaatiField;

        private string teslimTarihiField;

        private string ozelKodField;

        private string plasiyerKoduField;

        private string odemeSekliField;

        private string odemeTarihiField;

        private string odemeRefNoField;

        private decimal odemeTutarField;

        private string odemeNotuField;

        private decimal malTutarField;

        private decimal malIndTutarField;

        private decimal hizmetTutarField;

        private decimal hizmetIndTutarField;

        private decimal otvMatrahField;

        private decimal otvOranField;

        private decimal otvTutarField;

        private decimal kdvMatrahField;

        private decimal kdvTutarField;

        private decimal belgeTutarField;

        /// <remarks/>
        public string Aciliyet {
            get {
                return this.aciliyetField;
            }
            set {
                this.aciliyetField = value;
            }
        }

        /// <remarks/>
        public string HareketKodu {
            get {
                return this.hareketKoduField;
            }
            set {
                this.hareketKoduField = value;
            }
        }

        /// <remarks/>
        public string ProjeNo {
            get {
                return this.projeNoField;
            }
            set {
                this.projeNoField = value;
            }
        }

        /// <remarks/>
        public int BelgeNo {
            get {
                return this.belgeNoField;
            }
            set {
                this.belgeNoField = value;
            }
        }

        /// <remarks/>
        public int MBelgeNo {
            get {
                return this.mBelgeNoField;
            }
            set {
                this.mBelgeNoField = value;
            }
        }

        /// <remarks/>
        public string DovizKodu {
            get {
                return this.dovizKoduField;
            }
            set {
                this.dovizKoduField = value;
            }
        }

        /// <remarks/>
        public decimal DovizKuru {
            get {
                return this.dovizKuruField;
            }
            set {
                this.dovizKuruField = value;
            }
        }

        /// <remarks/>
        public string HariciNumara {
            get {
                return this.hariciNumaraField;
            }
            set {
                this.hariciNumaraField = value;
            }
        }

        /// <remarks/>
        public string TanzimTarihi {
            get {
                return this.tanzimTarihiField;
            }
            set {
                this.tanzimTarihiField = value;
            }
        }

        /// <remarks/>
        public string TanzimSaati {
            get {
                return this.tanzimSaatiField;
            }
            set {
                this.tanzimSaatiField = value;
            }
        }

        /// <remarks/>
        public string TeslimTarihi {
            get {
                return this.teslimTarihiField;
            }
            set {
                this.teslimTarihiField = value;
            }
        }

        /// <remarks/>
        public string OzelKod {
            get {
                return this.ozelKodField;
            }
            set {
                this.ozelKodField = value;
            }
        }

        /// <remarks/>
        public string PlasiyerKodu {
            get {
                return this.plasiyerKoduField;
            }
            set {
                this.plasiyerKoduField = value;
            }
        }

        /// <remarks/>
        public string OdemeSekli {
            get {
                return this.odemeSekliField;
            }
            set {
                this.odemeSekliField = value;
            }
        }

        /// <remarks/>
        public string OdemeTarihi {
            get {
                return this.odemeTarihiField;
            }
            set {
                this.odemeTarihiField = value;
            }
        }

        /// <remarks/>
        public string OdemeRefNo {
            get {
                return this.odemeRefNoField;
            }
            set {
                this.odemeRefNoField = value;
            }
        }

        /// <remarks/>
        public decimal OdemeTutar {
            get {
                return this.odemeTutarField;
            }
            set {
                this.odemeTutarField = value;
            }
        }

        /// <remarks/>
        public string OdemeNotu {
            get {
                return this.odemeNotuField;
            }
            set {
                this.odemeNotuField = value;
            }
        }

        /// <remarks/>
        public decimal MalTutar {
            get {
                return this.malTutarField;
            }
            set {
                this.malTutarField = value;
            }
        }

        /// <remarks/>
        public decimal MalIndTutar {
            get {
                return this.malIndTutarField;
            }
            set {
                this.malIndTutarField = value;
            }
        }

        /// <remarks/>
        public decimal HizmetTutar {
            get {
                return this.hizmetTutarField;
            }
            set {
                this.hizmetTutarField = value;
            }
        }

        /// <remarks/>
        public decimal HizmetIndTutar {
            get {
                return this.hizmetIndTutarField;
            }
            set {
                this.hizmetIndTutarField = value;
            }
        }

        /// <remarks/>
        public decimal OtvMatrah {
            get {
                return this.otvMatrahField;
            }
            set {
                this.otvMatrahField = value;
            }
        }

        /// <remarks/>
        public decimal OtvOran {
            get {
                return this.otvOranField;
            }
            set {
                this.otvOranField = value;
            }
        }

        /// <remarks/>
        public decimal OtvTutar {
            get {
                return this.otvTutarField;
            }
            set {
                this.otvTutarField = value;
            }
        }

        /// <remarks/>
        public decimal KdvMatrah {
            get {
                return this.kdvMatrahField;
            }
            set {
                this.kdvMatrahField = value;
            }
        }

        /// <remarks/>
        public decimal KdvTutar {
            get {
                return this.kdvTutarField;
            }
            set {
                this.kdvTutarField = value;
            }
        }

        /// <remarks/>
        public decimal BelgeTutar {
            get {
                return this.belgeTutarField;
            }
            set {
                this.belgeTutarField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.ankarayazilim.com/TicariBelge")]
    public partial class BelgeSicilSatirDetay {

        private BelgeSicilSatirDetayItems itemsField;

        private BelgeSicilSatirDetayStokSicil stokSicilField;

        private BelgeSicilSatirDetayHizmasSicil hizmasSicilField;

        /// <remarks/>
        public BelgeSicilSatirDetayItems Items {
            get {
                return this.itemsField;
            }
            set {
                this.itemsField = value;
            }
        }

        /// <remarks/>
        public BelgeSicilSatirDetayStokSicil StokSicil {
            get {
                return this.stokSicilField;
            }
            set {
                this.stokSicilField = value;
            }
        }

        /// <remarks/>
        public BelgeSicilSatirDetayHizmasSicil HizmasSicil {
            get {
                return this.hizmasSicilField;
            }
            set {
                this.hizmasSicilField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.ankarayazilim.com/TicariBelge")]
    public partial class BelgeSicilSatirDetayItems {

        private object barkodKoduField;

        private string urunGrubuField;

        private string urunKoduField;

        private string urunTanimField;

        private byte miktarField;

        private string olcuBirimField;

        private decimal birimFiyatField;

        private decimal tutarField;

        private byte indOranField;

        private byte kdvOranField;

        private object notlarField;

        /// <remarks/>
        public object BarkodKodu {
            get {
                return this.barkodKoduField;
            }
            set {
                this.barkodKoduField = value;
            }
        }

        /// <remarks/>
        public string UrunGrubu {
            get {
                return this.urunGrubuField;
            }
            set {
                this.urunGrubuField = value;
            }
        }

        /// <remarks/>
        public string UrunKodu {
            get {
                return this.urunKoduField;
            }
            set {
                this.urunKoduField = value;
            }
        }

        /// <remarks/>
        public string UrunTanim {
            get {
                return this.urunTanimField;
            }
            set {
                this.urunTanimField = value;
            }
        }

        /// <remarks/>
        public byte Miktar {
            get {
                return this.miktarField;
            }
            set {
                this.miktarField = value;
            }
        }

        /// <remarks/>
        public string OlcuBirim {
            get {
                return this.olcuBirimField;
            }
            set {
                this.olcuBirimField = value;
            }
        }

        /// <remarks/>
        public decimal BirimFiyat {
            get {
                return this.birimFiyatField;
            }
            set {
                this.birimFiyatField = value;
            }
        }

        /// <remarks/>
        public decimal Tutar {
            get {
                return this.tutarField;
            }
            set {
                this.tutarField = value;
            }
        }

        /// <remarks/>
        public byte IndOran {
            get {
                return this.indOranField;
            }
            set {
                this.indOranField = value;
            }
        }

        /// <remarks/>
        public byte KdvOran {
            get {
                return this.kdvOranField;
            }
            set {
                this.kdvOranField = value;
            }
        }

        /// <remarks/>
        public object Notlar {
            get {
                return this.notlarField;
            }
            set {
                this.notlarField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.ankarayazilim.com/TicariBelge")]
    public partial class BelgeSicilSatirDetayStokSicil {

        private string urunGrubuField;

        private string urunKoduField;

        private string urunTanimField;

        private string olcuBirimField;

        private byte kdvOranField;

        /// <remarks/>
        public string UrunGrubu {
            get {
                return this.urunGrubuField;
            }
            set {
                this.urunGrubuField = value;
            }
        }

        /// <remarks/>
        public string UrunKodu {
            get {
                return this.urunKoduField;
            }
            set {
                this.urunKoduField = value;
            }
        }

        /// <remarks/>
        public string UrunTanim {
            get {
                return this.urunTanimField;
            }
            set {
                this.urunTanimField = value;
            }
        }

        /// <remarks/>
        public string OlcuBirim {
            get {
                return this.olcuBirimField;
            }
            set {
                this.olcuBirimField = value;
            }
        }

        /// <remarks/>
        public byte KdvOran {
            get {
                return this.kdvOranField;
            }
            set {
                this.kdvOranField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.ankarayazilim.com/TicariBelge")]
    public partial class BelgeSicilSatirDetayHizmasSicil {

        private string urunGrubuField;

        private string urunKoduField;

        private string urunTanimField;

        private string olcuBirimField;

        private byte kdvOranField;

        /// <remarks/>
        public string UrunGrubu {
            get {
                return this.urunGrubuField;
            }
            set {
                this.urunGrubuField = value;
            }
        }

        /// <remarks/>
        public string UrunKodu {
            get {
                return this.urunKoduField;
            }
            set {
                this.urunKoduField = value;
            }
        }

        /// <remarks/>
        public string UrunTanim {
            get {
                return this.urunTanimField;
            }
            set {
                this.urunTanimField = value;
            }
        }

        /// <remarks/>
        public string OlcuBirim {
            get {
                return this.olcuBirimField;
            }
            set {
                this.olcuBirimField = value;
            }
        }

        /// <remarks/>
        public byte KdvOran {
            get {
                return this.kdvOranField;
            }
            set {
                this.kdvOranField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.ankarayazilim.com/TicariBelge")]
    public partial class BelgeSicilCariSicil {

        private string hesapNoField;

        private string adiField;

        private string soyadiField;

        private string unvaniField;

        private string adres1Field;

        private string adres2Field;

        private string adres3Field;

        private string ilceField;

        private string sehirField;

        private string postaKoduField;

        private string ulkeField;

        private string telefonField;

        private string faksNoField;

        private string gsmNoField;

        private string ePostaField;

        private string webSiteURLField;

        private string vergiNoField;

        private string vergiDaireAdiField;

        private string ozelKodField;

        private string ozelKod1Field;

        private string ozelKod2Field;

        private string ozelKod3Field;

        private string notlarField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("HesapNo")]
        public string HesapNo {
            get {
                return this.hesapNoField;
            }
            set {
                this.hesapNoField = value;
            }
        }

        /// <remarks/>
        public string Adi {
            get {
                return this.adiField;
            }
            set {
                this.adiField = value;
            }
        }

        /// <remarks/>
        public string Soyadi {
            get {
                return this.soyadiField;
            }
            set {
                this.soyadiField = value;
            }
        }

        /// <remarks/>
        public string Unvani {
            get {
                return this.unvaniField;
            }
            set {
                this.unvaniField = value;
            }
        }

        /// <remarks/>
        public string Adres1 {
            get {
                return this.adres1Field;
            }
            set {
                this.adres1Field = value;
            }
        }

        /// <remarks/>
        public string Adres2 {
            get {
                return this.adres2Field;
            }
            set {
                this.adres2Field = value;
            }
        }

        /// <remarks/>
        public string Adres3 {
            get {
                return this.adres3Field;
            }
            set {
                this.adres3Field = value;
            }
        }

        /// <remarks/>
        public string Ilce {
            get {
                return this.ilceField;
            }
            set {
                this.ilceField = value;
            }
        }

        /// <remarks/>
        public string Sehir {
            get {
                return this.sehirField;
            }
            set {
                this.sehirField = value;
            }
        }

        /// <remarks/>
        public string PostaKodu {
            get {
                return this.postaKoduField;
            }
            set {
                this.postaKoduField = value;
            }
        }

        /// <remarks/>
        public string Ulke {
            get {
                return this.ulkeField;
            }
            set {
                this.ulkeField = value;
            }
        }

        /// <remarks/>
        public string Telefon {
            get {
                return this.telefonField;
            }
            set {
                this.telefonField = value;
            }
        }

        /// <remarks/>
        public string FaksNo {
            get {
                return this.faksNoField;
            }
            set {
                this.faksNoField = value;
            }
        }

        /// <remarks/>
        public string GsmNo {
            get {
                return this.gsmNoField;
            }
            set {
                this.gsmNoField = value;
            }
        }

        /// <remarks/>
        public string EPosta {
            get {
                return this.ePostaField;
            }
            set {
                this.ePostaField = value;
            }
        }

        /// <remarks/>
        public string WebSiteURL {
            get {
                return this.webSiteURLField;
            }
            set {
                this.webSiteURLField = value;
            }
        }

        /// <remarks/>
        public string VergiNo {
            get {
                return this.vergiNoField;
            }
            set {
                this.vergiNoField = value;
            }
        }

        /// <remarks/>
        public string VergiDaireAdi {
            get {
                return this.vergiDaireAdiField;
            }
            set {
                this.vergiDaireAdiField = value;
            }
        }

        /// <remarks/>
        public string OzelKod {
            get {
                return this.ozelKodField;
            }
            set {
                this.ozelKodField = value;
            }
        }

        /// <remarks/>
        public string OzelKod1 {
            get {
                return this.ozelKod1Field;
            }
            set {
                this.ozelKod1Field = value;
            }
        }

        /// <remarks/>
        public string OzelKod2 {
            get {
                return this.ozelKod2Field;
            }
            set {
                this.ozelKod2Field = value;
            }
        }

        /// <remarks/>
        public string OzelKod3 {
            get {
                return this.ozelKod3Field;
            }
            set {
                this.ozelKod3Field = value;
            }
        }

        /// <remarks/>
        public string Notlar {
            get {
                return this.notlarField;
            }
            set {
                this.notlarField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.ankarayazilim.com/TicariBelge")]
    public partial class BelgeSicilSevkYeri {

        private string hesapNoField;

        private string adiField;

        private string soyadiField;

        private string unvaniField;

        private string adres1Field;

        private string adres2Field;

        private string adres3Field;

        private string ilceField;

        private string sehirField;

        private string postaKoduField;

        private string ulkeField;

        private string telefonField;

        private string faksNoField;

        private string gsmNoField;

        private string ePostaField;

        private string webSiteURLField;

        private string vergiNoField;

        private string vergiDaireAdiField;

        private string kargoKoduField;

        private string ozelKodField;

        private string notlarField;

        /// <remarks/>
        public string HesapNo {
            get {
                return this.hesapNoField;
            }
            set {
                this.hesapNoField = value;
            }
        }

        /// <remarks/>
        public string Adi {
            get {
                return this.adiField;
            }
            set {
                this.adiField = value;
            }
        }

        /// <remarks/>
        public string Soyadi {
            get {
                return this.soyadiField;
            }
            set {
                this.soyadiField = value;
            }
        }

        /// <remarks/>
        public string Unvani {
            get {
                return this.unvaniField;
            }
            set {
                this.unvaniField = value;
            }
        }

        /// <remarks/>
        public string Adres1 {
            get {
                return this.adres1Field;
            }
            set {
                this.adres1Field = value;
            }
        }

        /// <remarks/>
        public string Adres2 {
            get {
                return this.adres2Field;
            }
            set {
                this.adres2Field = value;
            }
        }

        /// <remarks/>
        public string Adres3 {
            get {
                return this.adres3Field;
            }
            set {
                this.adres3Field = value;
            }
        }

        /// <remarks/>
        public string Ilce {
            get {
                return this.ilceField;
            }
            set {
                this.ilceField = value;
            }
        }

        /// <remarks/>
        public string Sehir {
            get {
                return this.sehirField;
            }
            set {
                this.sehirField = value;
            }
        }

        /// <remarks/>
        public string PostaKodu {
            get {
                return this.postaKoduField;
            }
            set {
                this.postaKoduField = value;
            }
        }

        /// <remarks/>
        public string Ulke {
            get {
                return this.ulkeField;
            }
            set {
                this.ulkeField = value;
            }
        }

        /// <remarks/>
        public string Telefon {
            get {
                return this.telefonField;
            }
            set {
                this.telefonField = value;
            }
        }

        /// <remarks/>
        public string FaksNo {
            get {
                return this.faksNoField;
            }
            set {
                this.faksNoField = value;
            }
        }

        /// <remarks/>
        public string GsmNo {
            get {
                return this.gsmNoField;
            }
            set {
                this.gsmNoField = value;
            }
        }

        /// <remarks/>
        public string EPosta {
            get {
                return this.ePostaField;
            }
            set {
                this.ePostaField = value;
            }
        }

        /// <remarks/>
        public string WebSiteURL {
            get {
                return this.webSiteURLField;
            }
            set {
                this.webSiteURLField = value;
            }
        }

        /// <remarks/>
        public string VergiNo {
            get {
                return this.vergiNoField;
            }
            set {
                this.vergiNoField = value;
            }
        }

        /// <remarks/>
        public string VergiDaireAdi {
            get {
                return this.vergiDaireAdiField;
            }
            set {
                this.vergiDaireAdiField = value;
            }
        }

        /// <remarks/>
        public string KargoKodu {
            get {
                return this.kargoKoduField;
            }
            set {
                this.kargoKoduField = value;
            }
        }

        /// <remarks/>
        public string OzelKod {
            get {
                return this.ozelKodField;
            }
            set {
                this.ozelKodField = value;
            }
        }

        /// <remarks/>
        public string Notlar {
            get {
                return this.notlarField;
            }
            set {
                this.notlarField = value;
            }
        }
    }
}