// NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
using System.Xml.Serialization;

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute( "code" )]
[System.Xml.Serialization.XmlTypeAttribute( AnonymousType = true )]
[XmlRoot( ElementName = "Products" )]
public partial class XProducts_PENTA {

    private XProducts_PENTA_Stok[] stokField;

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute( "Stok" )]
    public XProducts_PENTA_Stok[] Stok {
        get {
            return this.stokField;
        }
        set {
            this.stokField = value;
        }
    }
}

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute( "code" )]
[System.Xml.Serialization.XmlTypeAttribute( AnonymousType = true )]
public partial class XProducts_PENTA_Stok {

    private string ustGrup_KodField;

    private string ustGrup_AdField;

    private string anaGrup_KodField;

    private string anaGrup_AdField;

    private string altGrup_KodField;

    private string altGrup_AdField;

    private uint kodField;

    private string adField;

    private string urunGrubuField;

    private string urunGrubuKoduField;

    private string dovizField;

    private decimal fiyat_SKullaniciField;

    private decimal fiyat_BayiField;

    private decimal fiyat_OzelField;

    private string miktarField;

    private byte garantiField;

    private string markaField;

    private string markaIsimField;

    private decimal vergiField;

    private string desiField;

    private string ureticiKodField;

    private string ureticiBarkodNoField;

    private string eski_KodField;

    private string boyutField;

    private string boyut_BirimField;

    private string net_AgirlikField;

    private string brut_AgirlikField;

    private string menseiField;

    private string ozelKategoriField;

    private byte ozel_StokField;

    private string iskontoYuzdeField;

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string UstGrup_Kod {
        get {
            return this.ustGrup_KodField;
        }
        set {
            this.ustGrup_KodField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string UstGrup_Ad {
        get {
            return this.ustGrup_AdField;
        }
        set {
            this.ustGrup_AdField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string AnaGrup_Kod {
        get {
            return this.anaGrup_KodField;
        }
        set {
            this.anaGrup_KodField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string AnaGrup_Ad {
        get {
            return this.anaGrup_AdField;
        }
        set {
            this.anaGrup_AdField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string AltGrup_Kod {
        get {
            return this.altGrup_KodField;
        }
        set {
            this.altGrup_KodField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string AltGrup_Ad {
        get {
            return this.altGrup_AdField;
        }
        set {
            this.altGrup_AdField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public uint Kod {
        get {
            return this.kodField;
        }
        set {
            this.kodField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Ad {
        get {
            return this.adField;
        }
        set {
            this.adField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string UrunGrubu {
        get {
            return this.urunGrubuField;
        }
        set {
            this.urunGrubuField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string UrunGrubuKodu {
        get {
            return this.urunGrubuKoduField;
        }
        set {
            this.urunGrubuKoduField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Doviz {
        get {
            return this.dovizField;
        }
        set {
            this.dovizField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public decimal Fiyat_SKullanici {
        get {
            return this.fiyat_SKullaniciField;
        }
        set {
            this.fiyat_SKullaniciField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public decimal Fiyat_Bayi {
        get {
            return this.fiyat_BayiField;
        }
        set {
            this.fiyat_BayiField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public decimal Fiyat_Ozel {
        get {
            return this.fiyat_OzelField;
        }
        set {
            this.fiyat_OzelField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Miktar {
        get {
            return this.miktarField;
        }
        set {
            this.miktarField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public byte Garanti {
        get {
            return this.garantiField;
        }
        set {
            this.garantiField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Marka {
        get {
            return this.markaField;
        }
        set {
            this.markaField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string MarkaIsim {
        get {
            return this.markaIsimField;
        }
        set {
            this.markaIsimField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public decimal Vergi {
        get {
            return this.vergiField;
        }
        set {
            this.vergiField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Desi {
        get {
            return this.desiField;
        }
        set {
            this.desiField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string UreticiKod {
        get {
            return this.ureticiKodField;
        }
        set {
            this.ureticiKodField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string UreticiBarkodNo {
        get {
            return this.ureticiBarkodNoField;
        }
        set {
            this.ureticiBarkodNoField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Eski_Kod {
        get {
            return this.eski_KodField;
        }
        set {
            this.eski_KodField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Boyut {
        get {
            return this.boyutField;
        }
        set {
            this.boyutField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Boyut_Birim {
        get {
            return this.boyut_BirimField;
        }
        set {
            this.boyut_BirimField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Net_Agirlik {
        get {
            return this.net_AgirlikField;
        }
        set {
            this.net_AgirlikField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Brut_Agirlik {
        get {
            return this.brut_AgirlikField;
        }
        set {
            this.brut_AgirlikField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Mensei {
        get {
            return this.menseiField;
        }
        set {
            this.menseiField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string OzelKategori {
        get {
            return this.ozelKategoriField;
        }
        set {
            this.ozelKategoriField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public byte Ozel_Stok {
        get {
            return this.ozel_StokField;
        }
        set {
            this.ozel_StokField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string IskontoYuzde {
        get {
            return this.iskontoYuzdeField;
        }
        set {
            this.iskontoYuzdeField = value;
        }
    }
}