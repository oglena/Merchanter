// NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
using System.Xml.Serialization;

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute( "code" )]
[System.Xml.Serialization.XmlTypeAttribute( AnonymousType = true )]
[XmlRoot( ElementName = "ArrayOfProductDto", Namespace = "http://schemas.datacontract.org/2004/07/KoyuncuXml.Data.ComplexTypes" )]
public partial class XProducts_KOYUNCU {

    private XProducts_KOYUNCU_ProductDto[] productDtoField;

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute( "ProductDto" )]
    public XProducts_KOYUNCU_ProductDto[] ProductDto {
        get {
            return this.productDtoField;
        }
        set {
            this.productDtoField = value;
        }
    }
}

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute( "code" )]
[System.Xml.Serialization.XmlTypeAttribute( AnonymousType = true )]
public partial class XProducts_KOYUNCU_ProductDto {

    private uint brandCodeField;

    private string brandNameField;

    private decimal campaignPriceField;

    private ushort categoryCodeField;

    private string categoryNameField;

    private decimal clientPriceField;

    private string currencyField;

    private decimal customerPriceField;

    private decimal defaultPriceField;

    private string eANCodeField;

    private ushort idField;

    private string pNCodeField;

    private string productCodeField;

    private ArrayOfProductDtoProductDtoProductSpec[] productDetailsField;

    private string productImageField;

    private string productNameField;

    private string stockField;

    private string taxRateField;

    private byte topCategoryCodeField;

    private string topCategoryNameField;

    /// <remarks/>
    public uint BrandCode {
        get {
            return this.brandCodeField;
        }
        set {
            this.brandCodeField = value;
        }
    }

    /// <remarks/>
    public string BrandName {
        get {
            return this.brandNameField;
        }
        set {
            this.brandNameField = value;
        }
    }

    /// <remarks/>
    public decimal CampaignPrice {
        get {
            return this.campaignPriceField;
        }
        set {
            this.campaignPriceField = value;
        }
    }

    /// <remarks/>
    public ushort CategoryCode {
        get {
            return this.categoryCodeField;
        }
        set {
            this.categoryCodeField = value;
        }
    }

    /// <remarks/>
    public string CategoryName {
        get {
            return this.categoryNameField;
        }
        set {
            this.categoryNameField = value;
        }
    }

    /// <remarks/>
    public decimal ClientPrice {
        get {
            return this.clientPriceField;
        }
        set {
            this.clientPriceField = value;
        }
    }

    /// <remarks/>
    public string Currency {
        get {
            return this.currencyField;
        }
        set {
            this.currencyField = value;
        }
    }

    /// <remarks/>
    public decimal CustomerPrice {
        get {
            return this.customerPriceField;
        }
        set {
            this.customerPriceField = value;
        }
    }

    /// <remarks/>
    public decimal DefaultPrice {
        get {
            return this.defaultPriceField;
        }
        set {
            this.defaultPriceField = value;
        }
    }

    /// <remarks/>
    public string EANCode {
        get {
            return this.eANCodeField;
        }
        set {
            this.eANCodeField = value;
        }
    }

    /// <remarks/>
    public ushort Id {
        get {
            return this.idField;
        }
        set {
            this.idField = value;
        }
    }

    /// <remarks/>
    public string PNCode {
        get {
            return this.pNCodeField;
        }
        set {
            this.pNCodeField = value;
        }
    }

    /// <remarks/>
    public string ProductCode {
        get {
            return this.productCodeField;
        }
        set {
            this.productCodeField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlArrayItemAttribute( "ProductSpec", IsNullable = false )]
    public ArrayOfProductDtoProductDtoProductSpec[] ProductDetails {
        get {
            return this.productDetailsField;
        }
        set {
            this.productDetailsField = value;
        }
    }

    /// <remarks/>
    public string ProductImage {
        get {
            return this.productImageField;
        }
        set {
            this.productImageField = value;
        }
    }

    /// <remarks/>
    public string ProductName {
        get {
            return this.productNameField;
        }
        set {
            this.productNameField = value;
        }
    }

    /// <remarks/>
    public string Stock {
        get {
            return this.stockField;
        }
        set {
            this.stockField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute( IsNullable = true )]
    public string TaxRate {
        get {
            return this.taxRateField;
        }
        set {
            this.taxRateField = value;
        }
    }

    /// <remarks/>
    public byte TopCategoryCode {
        get {
            return this.topCategoryCodeField;
        }
        set {
            this.topCategoryCodeField = value;
        }
    }

    /// <remarks/>
    public string TopCategoryName {
        get {
            return this.topCategoryNameField;
        }
        set {
            this.topCategoryNameField = value;
        }
    }
}

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute( "code" )]
[System.Xml.Serialization.XmlTypeAttribute( AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/KoyuncuXml.Data.ComplexTypes" )]
public partial class ArrayOfProductDtoProductDtoProductSpec {

    private string specField;

    private string valueField;

    /// <remarks/>
    public string Spec {
        get {
            return this.specField;
        }
        set {
            this.specField = value;
        }
    }

    /// <remarks/>
    public string Value {
        get {
            return this.valueField;
        }
        set {
            this.valueField = value;
        }
    }
}

