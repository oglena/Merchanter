// NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
using System.Xml.Serialization;

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute( "code" )]
[System.Xml.Serialization.XmlTypeAttribute( AnonymousType = true )]
[XmlRoot( ElementName = "root" )]
public partial class XProducts_OKSID {

    private XProducts_OKSID_Err errField;

    private XProducts_OKSID_Stok[] stokField;

    /// <remarks/>
    public XProducts_OKSID_Err err {
        get {
            return this.errField;
        }
        set {
            this.errField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute( "Stok" )]
    public XProducts_OKSID_Stok[] Stok {
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
public partial class XProducts_OKSID_Err {

    private byte typeField;

    private string descField;

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public byte Type {
        get {
            return this.typeField;
        }
        set {
            this.typeField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Desc {
        get {
            return this.descField;
        }
        set {
            this.descField = value;
        }
    }
}

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute( "code" )]
[System.Xml.Serialization.XmlTypeAttribute( AnonymousType = true )]
public partial class XProducts_OKSID_Stok {

    private ushort kategori_IdField;

    private string anaGrup_KodField;

    private string anaGrup_AdField;

    private string altGrup_KodField;

    private string altGrup_AdField;

    private string kodField;

    private string adField;

    private byte dovizField;

    private string doviz_CinsiField;

    private decimal fiat_SKullaniciField;

    private decimal fiat_BayiField;

    private byte miktarField;

    private byte garantiField;

    private string markaField;

    private decimal desiField;

    private string marka_IsmiField;

    private byte kdvField;

    private string barkodField;

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public ushort Kategori_Id {
        get {
            return this.kategori_IdField;
        }
        set {
            this.kategori_IdField = value;
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
    public string Kod {
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
    public byte Doviz {
        get {
            return this.dovizField;
        }
        set {
            this.dovizField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Doviz_Cinsi {
        get {
            return this.doviz_CinsiField;
        }
        set {
            this.doviz_CinsiField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public decimal Fiat_SKullanici {
        get {
            return this.fiat_SKullaniciField;
        }
        set {
            this.fiat_SKullaniciField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public decimal Fiat_Bayi {
        get {
            return this.fiat_BayiField;
        }
        set {
            this.fiat_BayiField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public byte Miktar {
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
    public decimal Desi {
        get {
            return this.desiField;
        }
        set {
            this.desiField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Marka_Ismi {
        get {
            return this.marka_IsmiField;
        }
        set {
            this.marka_IsmiField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public byte Kdv {
        get {
            return this.kdvField;
        }
        set {
            this.kdvField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string barkod {
        get {
            return this.barkodField;
        }
        set {
            this.barkodField = value;
        }
    }
}

