namespace MerchanterHelpers.Classes {
    // NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.ankarayazilim.com/TicariDokumanZarfi")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.ankarayazilim.com/TicariDokumanZarfi", IsNullable = false)]
    public partial class US_TicariDokuman {

        private US_TicariDokumanDokumanBaslik dokumanBaslikField;

        private US_DokumanPaket dokumanPaketField;

        /// <remarks/>
        public US_TicariDokumanDokumanBaslik DokumanBaslik {
            get {
                return this.dokumanBaslikField;
            }
            set {
                this.dokumanBaslikField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.ankarayazilim.com/DokumanPaket")]
        public US_DokumanPaket DokumanPaket {
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
    public partial class US_TicariDokumanDokumanBaslik {

        private string versiyonField;

        private US_TicariDokumanDokumanBaslikGonderen gonderenField;

        private US_TicariDokumanDokumanBaslikAlici aliciField;

        private US_TicariDokumanDokumanBaslikDokumanTanimi dokumanTanimiField;

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
        public US_TicariDokumanDokumanBaslikGonderen Gonderen {
            get {
                return this.gonderenField;
            }
            set {
                this.gonderenField = value;
            }
        }

        /// <remarks/>
        public US_TicariDokumanDokumanBaslikAlici Alici {
            get {
                return this.aliciField;
            }
            set {
                this.aliciField = value;
            }
        }

        /// <remarks/>
        public US_TicariDokumanDokumanBaslikDokumanTanimi DokumanTanimi {
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
    public partial class US_TicariDokumanDokumanBaslikGonderen {

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
    public partial class US_TicariDokumanDokumanBaslikAlici {

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
    public partial class US_TicariDokumanDokumanBaslikDokumanTanimi {

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
    public partial class US_DokumanPaket {

        private US_Eleman elemanField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "")]
        public US_Eleman Eleman {
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
    public partial class US_Eleman {

        private string elemanTipiField;

        private ushort elemanSayisiField;

        private UrunSicil[] elemanListeField;

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
        public ushort ElemanSayisi {
            get {
                return this.elemanSayisiField;
            }
            set {
                this.elemanSayisiField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("UrunSicil", Namespace = "http://www.ankarayazilim.com/UrunBilgisi", IsNullable = false)]
        public UrunSicil[] ElemanListe {
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
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.ankarayazilim.com/UrunBilgisi")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.ankarayazilim.com/UrunBilgisi", IsNullable = false)]
    public partial class UrunSicil {

        private UrunSicilUrunTanim urunTanimField;

        private UrunSicilImage[] imagesField;

        private UrunSicilMuadils muadilsField;

        private UrunSicilOems oemsField;

        private UrunSicilSkys skysField;

        private UrunSicilGsozellik[] gsozelliksField;

        /// <remarks/>
        public UrunSicilUrunTanim UrunTanim {
            get {
                return this.urunTanimField;
            }
            set {
                this.urunTanimField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Image", IsNullable = false)]
        public UrunSicilImage[] Images {
            get {
                return this.imagesField;
            }
            set {
                this.imagesField = value;
            }
        }

        /// <remarks/>
        public UrunSicilMuadils Muadils {
            get {
                return this.muadilsField;
            }
            set {
                this.muadilsField = value;
            }
        }

        /// <remarks/>
        public UrunSicilOems Oems {
            get {
                return this.oemsField;
            }
            set {
                this.oemsField = value;
            }
        }

        /// <remarks/>
        public UrunSicilSkys Skys {
            get {
                return this.skysField;
            }
            set {
                this.skysField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Gsozellik", IsNullable = false)]
        public UrunSicilGsozellik[] Gsozelliks {
            get {
                return this.gsozelliksField;
            }
            set {
                this.gsozelliksField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.ankarayazilim.com/UrunBilgisi")]
    public partial class UrunSicilUrunTanim {

        private string urKoduField;

        private string urAdiField;

        private string sicilKoduField;

        private string crudField;

        private string sicilAdiField;

        private string sicilAdi1Field;

        private string sicilAdiyField;

        private string ozelKod1Field;

        private string ozelKod2Field;

        private string ozelKod3Field;

        private string ozelKod4Field;

        private string ozelKod5Field;

        private string ozelKod6Field;

        private string ozelKod7Field;

        private string ozelKod8Field;

        private string sinifField;

        private decimal perSatFiyatField;

        private string paraCinsiField;

        private string kamSatFiyatField;

        private string kamParaCinsiField;

        private string kamKoduField;

        private string kamBasTarField;

        private string kamBitTarField;

        private int kdvOraniField;

        private int indOranField;

        private string olcuBirimiField;

        private string barkodKoduField;

        private int paketMiktariField;

        private int agirligiKgField;

        private int hacmiM3Field;

        private int stokMevcuduField;

        private int teminSuresiField;

        private string kayitTarihiField;

        private string notlarField;

        private string kategoriKoduField;

        private string muadilKoduField;

        /// <remarks/>
        public string UrKodu {
            get {
                return this.urKoduField;
            }
            set {
                this.urKoduField = value;
            }
        }

        /// <remarks/>
        public string UrAdi {
            get {
                return this.urAdiField;
            }
            set {
                this.urAdiField = value;
            }
        }

        /// <remarks/>
        public string SicilKodu {
            get {
                return this.sicilKoduField;
            }
            set {
                this.sicilKoduField = value;
            }
        }

        /// <remarks/>
        public string Crud {
            get {
                return this.crudField;
            }
            set {
                this.crudField = value;
            }
        }

        /// <remarks/>
        public string SicilAdi {
            get {
                return this.sicilAdiField;
            }
            set {
                this.sicilAdiField = value;
            }
        }

        /// <remarks/>
        public string SicilAdi1 {
            get {
                return this.sicilAdi1Field;
            }
            set {
                this.sicilAdi1Field = value;
            }
        }

        /// <remarks/>
        public string SicilAdiy {
            get {
                return this.sicilAdiyField;
            }
            set {
                this.sicilAdiyField = value;
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
        public string OzelKod4 {
            get {
                return this.ozelKod4Field;
            }
            set {
                this.ozelKod4Field = value;
            }
        }

        /// <remarks/>
        public string OzelKod5 {
            get {
                return this.ozelKod5Field;
            }
            set {
                this.ozelKod5Field = value;
            }
        }

        /// <remarks/>
        public string OzelKod6 {
            get {
                return this.ozelKod6Field;
            }
            set {
                this.ozelKod6Field = value;
            }
        }

        /// <remarks/>
        public string OzelKod7 {
            get {
                return this.ozelKod7Field;
            }
            set {
                this.ozelKod7Field = value;
            }
        }

        /// <remarks/>
        public string OzelKod8 {
            get {
                return this.ozelKod8Field;
            }
            set {
                this.ozelKod8Field = value;
            }
        }

        /// <remarks/>
        public string Sinif {
            get {
                return this.sinifField;
            }
            set {
                this.sinifField = value;
            }
        }

        /// <remarks/>
        public decimal PerSatFiyat {
            get {
                return this.perSatFiyatField;
            }
            set {
                this.perSatFiyatField = value;
            }
        }

        /// <remarks/>
        public string ParaCinsi {
            get {
                return this.paraCinsiField;
            }
            set {
                this.paraCinsiField = value;
            }
        }

        /// <remarks/>
        public string KamSatFiyat {
            get {
                return this.kamSatFiyatField;
            }
            set {
                this.kamSatFiyatField = value;
            }
        }

        /// <remarks/>
        public string KamParaCinsi {
            get {
                return this.kamParaCinsiField;
            }
            set {
                this.kamParaCinsiField = value;
            }
        }

        /// <remarks/>
        public string KamKodu {
            get {
                return this.kamKoduField;
            }
            set {
                this.kamKoduField = value;
            }
        }

        /// <remarks/>
        public string KamBasTar {
            get {
                return this.kamBasTarField;
            }
            set {
                this.kamBasTarField = value;
            }
        }

        /// <remarks/>
        public string KamBitTar {
            get {
                return this.kamBitTarField;
            }
            set {
                this.kamBitTarField = value;
            }
        }

        /// <remarks/>
        public int KdvOrani {
            get {
                return this.kdvOraniField;
            }
            set {
                this.kdvOraniField = value;
            }
        }

        /// <remarks/>
        public int IndOran {
            get {
                return this.indOranField;
            }
            set {
                this.indOranField = value;
            }
        }

        /// <remarks/>
        public string OlcuBirimi {
            get {
                return this.olcuBirimiField;
            }
            set {
                this.olcuBirimiField = value;
            }
        }

        /// <remarks/>
        public string BarkodKodu {
            get {
                return this.barkodKoduField;
            }
            set {
                this.barkodKoduField = value;
            }
        }

        /// <remarks/>
        public int PaketMiktari {
            get {
                return this.paketMiktariField;
            }
            set {
                this.paketMiktariField = value;
            }
        }

        /// <remarks/>
        public int AgirligiKg {
            get {
                return this.agirligiKgField;
            }
            set {
                this.agirligiKgField = value;
            }
        }

        /// <remarks/>
        public int HacmiM3 {
            get {
                return this.hacmiM3Field;
            }
            set {
                this.hacmiM3Field = value;
            }
        }

        /// <remarks/>
        public int StokMevcudu {
            get {
                return this.stokMevcuduField;
            }
            set {
                this.stokMevcuduField = value;
            }
        }

        /// <remarks/>
        public int TeminSuresi {
            get {
                return this.teminSuresiField;
            }
            set {
                this.teminSuresiField = value;
            }
        }

        /// <remarks/>
        public string KayitTarihi {
            get {
                return this.kayitTarihiField;
            }
            set {
                this.kayitTarihiField = value;
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

        /// <remarks/>
        public string KategoriKodu {
            get {
                return this.kategoriKoduField;
            }
            set {
                this.kategoriKoduField = value;
            }
        }

        /// <remarks/>
        public string MuadilKodu {
            get {
                return this.muadilKoduField;
            }
            set {
                this.muadilKoduField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.ankarayazilim.com/UrunBilgisi")]
    public partial class UrunSicilImage {

        private string iFilenameField;

        private string iBase64ValueField;

        /// <remarks/>
        public string IFilename {
            get {
                return this.iFilenameField;
            }
            set {
                this.iFilenameField = value;
            }
        }

        /// <remarks/>
        public string IBase64Value {
            get {
                return this.iBase64ValueField;
            }
            set {
                this.iBase64ValueField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.ankarayazilim.com/UrunBilgisi")]
    public partial class UrunSicilMuadils {

        private UrunSicilMuadilsMuadil muadilField;

        /// <remarks/>
        public UrunSicilMuadilsMuadil Muadil {
            get {
                return this.muadilField;
            }
            set {
                this.muadilField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.ankarayazilim.com/UrunBilgisi")]
    public partial class UrunSicilMuadilsMuadil {

        private string mUreticiAdiField;

        private string mUreticiKoduField;

        /// <remarks/>
        public string MUreticiAdi {
            get {
                return this.mUreticiAdiField;
            }
            set {
                this.mUreticiAdiField = value;
            }
        }

        /// <remarks/>
        public string MUreticiKodu {
            get {
                return this.mUreticiKoduField;
            }
            set {
                this.mUreticiKoduField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.ankarayazilim.com/UrunBilgisi")]
    public partial class UrunSicilOems {

        private UrunSicilOemsOem oemField;

        /// <remarks/>
        public UrunSicilOemsOem Oem {
            get {
                return this.oemField;
            }
            set {
                this.oemField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.ankarayazilim.com/UrunBilgisi")]
    public partial class UrunSicilOemsOem {

        private string markaField;

        private string oemNoField;

        /// <remarks/>
        public string Marka {
            get {
                return this.markaField;
            }
            set {
                this.markaField = value;
            }
        }

        /// <remarks/>
        public string OemNo {
            get {
                return this.oemNoField;
            }
            set {
                this.oemNoField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.ankarayazilim.com/UrunBilgisi")]
    public partial class UrunSicilSkys {

        private UrunSicilSkysSky skyField;

        /// <remarks/>
        public UrunSicilSkysSky Sky {
            get {
                return this.skyField;
            }
            set {
                this.skyField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.ankarayazilim.com/UrunBilgisi")]
    public partial class UrunSicilSkysSky {

        private string skyMarkaAdiField;

        private string skyMarkaModelAdiField;

        private string skyOemNoField;

        private string skyOzelKod4Field;

        private string skyOzelKod5Field;

        private string skyOzelKod6Field;

        private string skyOzelKod7Field;

        private string skyOzelKod8Field;

        private string skyOzelKod9Field;

        private string skyOzelKod10Field;

        private string skyOtoakField;

        private string skyAktifmiField;

        /// <remarks/>
        public string SkyMarkaAdi {
            get {
                return this.skyMarkaAdiField;
            }
            set {
                this.skyMarkaAdiField = value;
            }
        }

        /// <remarks/>
        public string SkyMarkaModelAdi {
            get {
                return this.skyMarkaModelAdiField;
            }
            set {
                this.skyMarkaModelAdiField = value;
            }
        }

        /// <remarks/>
        public string SkyOemNo {
            get {
                return this.skyOemNoField;
            }
            set {
                this.skyOemNoField = value;
            }
        }

        /// <remarks/>
        public string SkyOzelKod4 {
            get {
                return this.skyOzelKod4Field;
            }
            set {
                this.skyOzelKod4Field = value;
            }
        }

        /// <remarks/>
        public string SkyOzelKod5 {
            get {
                return this.skyOzelKod5Field;
            }
            set {
                this.skyOzelKod5Field = value;
            }
        }

        /// <remarks/>
        public string SkyOzelKod6 {
            get {
                return this.skyOzelKod6Field;
            }
            set {
                this.skyOzelKod6Field = value;
            }
        }

        /// <remarks/>
        public string SkyOzelKod7 {
            get {
                return this.skyOzelKod7Field;
            }
            set {
                this.skyOzelKod7Field = value;
            }
        }

        /// <remarks/>
        public string SkyOzelKod8 {
            get {
                return this.skyOzelKod8Field;
            }
            set {
                this.skyOzelKod8Field = value;
            }
        }

        /// <remarks/>
        public string SkyOzelKod9 {
            get {
                return this.skyOzelKod9Field;
            }
            set {
                this.skyOzelKod9Field = value;
            }
        }

        /// <remarks/>
        public string SkyOzelKod10 {
            get {
                return this.skyOzelKod10Field;
            }
            set {
                this.skyOzelKod10Field = value;
            }
        }

        /// <remarks/>
        public string SkyOtoak {
            get {
                return this.skyOtoakField;
            }
            set {
                this.skyOtoakField = value;
            }
        }

        /// <remarks/>
        public string SkyAktifmi {
            get {
                return this.skyAktifmiField;
            }
            set {
                this.skyAktifmiField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.ankarayazilim.com/UrunBilgisi")]
    public partial class UrunSicilGsozellik {

        private string ozkodField;

        private string ozadiField;

        private string ozdegerField;

        private string iBase64ValueField;

        /// <remarks/>
        public string Ozkod {
            get {
                return this.ozkodField;
            }
            set {
                this.ozkodField = value;
            }
        }

        /// <remarks/>
        public string Ozadi {
            get {
                return this.ozadiField;
            }
            set {
                this.ozadiField = value;
            }
        }

        /// <remarks/>
        public string Ozdeger {
            get {
                return this.ozdegerField;
            }
            set {
                this.ozdegerField = value;
            }
        }

        /// <remarks/>
        public string IBase64Value {
            get {
                return this.iBase64ValueField;
            }
            set {
                this.iBase64ValueField = value;
            }
        }
    }
}
