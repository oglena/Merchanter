namespace MerchanterHelpers.Classes {
    // NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.ankarayazilim.com/TicariDokumanZarfi")]
    [System.Xml.Serialization.XmlRootAttribute("TicariDokuman", Namespace = "http://www.ankarayazilim.com/TicariDokumanZarfi", IsNullable = false)]
    public partial class CAT_TicariDokuman {

        private CAT_TicariDokumanDokumanBaslik dokumanBaslikField;

        private CAT_DokumanPaket dokumanPaketField;

        /// <remarks/>
        public CAT_TicariDokumanDokumanBaslik DokumanBaslik {
            get {
                return this.dokumanBaslikField;
            }
            set {
                this.dokumanBaslikField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("DokumanPaket", Namespace = "http://www.ankarayazilim.com/DokumanPaket")]
        public CAT_DokumanPaket DokumanPaket {
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
    public partial class CAT_TicariDokumanDokumanBaslik {

        private string versiyonField;

        private CAT_TicariDokumanDokumanBaslikGonderen gonderenField;

        private CAT_TicariDokumanDokumanBaslikAlici aliciField;

        private CAT_TicariDokumanDokumanBaslikDokumanTanimi dokumanTanimiField;

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
        public CAT_TicariDokumanDokumanBaslikGonderen Gonderen {
            get {
                return this.gonderenField;
            }
            set {
                this.gonderenField = value;
            }
        }

        /// <remarks/>
        public CAT_TicariDokumanDokumanBaslikAlici Alici {
            get {
                return this.aliciField;
            }
            set {
                this.aliciField = value;
            }
        }

        /// <remarks/>
        public CAT_TicariDokumanDokumanBaslikDokumanTanimi DokumanTanimi {
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
    public partial class CAT_TicariDokumanDokumanBaslikGonderen {

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
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.ankarayazilim.com/UrunBilgisi")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.ankarayazilim.com/UrunBilgisi", IsNullable = false)]
    public partial class KategoriItem {

        private string koduField;

        private string adiField;

        private string ustBaslikField;

        private string pasifField;

        private object ozkod1Field;

        private object ozkod2Field;

        private object ozkod3Field;

        private object ozkod4Field;

        private object ozkod5Field;

        /// <remarks/>
        public string Kodu {
            get {
                return this.koduField;
            }
            set {
                this.koduField = value;
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
        public string UstBaslik {
            get {
                return this.ustBaslikField;
            }
            set {
                this.ustBaslikField = value;
            }
        }

        /// <remarks/>
        public string Pasif {
            get {
                return this.pasifField;
            }
            set {
                this.pasifField = value;
            }
        }

        /// <remarks/>
        public object Ozkod1 {
            get {
                return this.ozkod1Field;
            }
            set {
                this.ozkod1Field = value;
            }
        }

        /// <remarks/>
        public object Ozkod2 {
            get {
                return this.ozkod2Field;
            }
            set {
                this.ozkod2Field = value;
            }
        }

        /// <remarks/>
        public object Ozkod3 {
            get {
                return this.ozkod3Field;
            }
            set {
                this.ozkod3Field = value;
            }
        }

        /// <remarks/>
        public object Ozkod4 {
            get {
                return this.ozkod4Field;
            }
            set {
                this.ozkod4Field = value;
            }
        }

        /// <remarks/>
        public object Ozkod5 {
            get {
                return this.ozkod5Field;
            }
            set {
                this.ozkod5Field = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.ankarayazilim.com/TicariDokumanZarfi")]
    public partial class CAT_TicariDokumanDokumanBaslikAlici {

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
    public partial class CAT_TicariDokumanDokumanBaslikDokumanTanimi {

        private string turuField;

        private string versiyonField;

        private string dosyaAdiField;

        private System.DateTime olusturulmaZamaniField;

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
        public System.DateTime OlusturulmaZamani {
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
    [System.Xml.Serialization.XmlRootAttribute("DokumanPaket", Namespace = "http://www.ankarayazilim.com/DokumanPaket", IsNullable = false)]
    public partial class CAT_DokumanPaket {

        private CAT_Eleman elemanField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "")]
        public CAT_Eleman Eleman {
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
    [System.Xml.Serialization.XmlRootAttribute("Eleman", Namespace = "", IsNullable = false)]
    public partial class CAT_Eleman {

        private string elemanTipiField;

        private ushort elemanSayisiField;

        private KategoriItem[] elemanListeField;

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
        [System.Xml.Serialization.XmlArrayItemAttribute("KategoriItem", Namespace = "http://www.ankarayazilim.com/UrunBilgisi", IsNullable = false)]
        public KategoriItem[] ElemanListe {
            get {
                return this.elemanListeField;
            }
            set {
                this.elemanListeField = value;
            }
        }
    }
}