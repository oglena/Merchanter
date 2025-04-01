// NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
using System.Xml.Serialization;
namespace MerchanterHelpers.Classes {
    /// <remarks/>
    [System.SerializableAttribute()]
    [XmlRoot(ElementName = "Products")]
    public partial class XProducts_FSP {

        private XProducts_FSP_Product[] productField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Product")]
        public XProducts_FSP_Product[] Product {
            get {
                return this.productField;
            }
            set {
                this.productField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class XProducts_FSP_Product {

        private string product_codeField;

        private ushort product_idField;

        private string barcodeField;

        private string nameField;

        private string mainCategoryField;

        private string mainCategory_idField;

        private string categoryField;

        private string category_idField;

        private string subCategoryField;

        private string subCategory_idField;

        private decimal bAYİfiyatField;

        private ushort tESKfiyatField;

        private string currencyTypeField;

        private byte taxField;

        private ushort? stockField;

        private string brandField;

        private string image1Field;

        private string image2Field;

        private string image3Field;

        private string image4Field;

        private string image5Field;

        private string descriptionField;

        /// <remarks/>
        public string Product_code {
            get {
                return this.product_codeField;
            }
            set {
                this.product_codeField = value;
            }
        }

        /// <remarks/>
        public ushort Product_id {
            get {
                return this.product_idField;
            }
            set {
                this.product_idField = value;
            }
        }

        /// <remarks/>
        public string Barcode {
            get {
                return this.barcodeField;
            }
            set {
                this.barcodeField = value;
            }
        }

        /// <remarks/>
        public string Name {
            get {
                return this.nameField;
            }
            set {
                this.nameField = value;
            }
        }

        /// <remarks/>
        public string mainCategory {
            get {
                return this.mainCategoryField;
            }
            set {
                this.mainCategoryField = value;
            }
        }

        /// <remarks/>
        public string mainCategory_id {
            get {
                return this.mainCategory_idField;
            }
            set {
                this.mainCategory_idField = value;
            }
        }

        /// <remarks/>
        public string category {
            get {
                return this.categoryField;
            }
            set {
                this.categoryField = value;
            }
        }

        /// <remarks/>
        public string category_id {
            get {
                return this.category_idField;
            }
            set {
                this.category_idField = value;
            }
        }

        /// <remarks/>
        public string subCategory {
            get {
                return this.subCategoryField;
            }
            set {
                this.subCategoryField = value;
            }
        }

        /// <remarks/>
        public string subCategory_id {
            get {
                return this.subCategory_idField;
            }
            set {
                this.subCategory_idField = value;
            }
        }

        /// <remarks/>
        public decimal BAYİfiyat {
            get {
                return this.bAYİfiyatField;
            }
            set {
                this.bAYİfiyatField = value;
            }
        }

        /// <remarks/>
        public ushort TESKfiyat {
            get {
                return this.tESKfiyatField;
            }
            set {
                this.tESKfiyatField = value;
            }
        }

        /// <remarks/>
        public string CurrencyType {
            get {
                return this.currencyTypeField;
            }
            set {
                this.currencyTypeField = value;
            }
        }

        /// <remarks/>
        public byte Tax {
            get {
                return this.taxField;
            }
            set {
                this.taxField = value;
            }
        }

        /// <remarks/>
        public ushort? Stock {
            get {
                return this.stockField;
            }
            set {
                this.stockField = value;
            }
        }

        /// <remarks/>
        public string Brand {
            get {
                return this.brandField;
            }
            set {
                this.brandField = value;
            }
        }

        /// <remarks/>
        public string Image1 {
            get {
                return this.image1Field;
            }
            set {
                this.image1Field = value;
            }
        }

        /// <remarks/>
        public string Image2 {
            get {
                return this.image2Field;
            }
            set {
                this.image2Field = value;
            }
        }

        /// <remarks/>
        public string Image3 {
            get {
                return this.image3Field;
            }
            set {
                this.image3Field = value;
            }
        }

        /// <remarks/>
        public string Image4 {
            get {
                return this.image4Field;
            }
            set {
                this.image4Field = value;
            }
        }

        /// <remarks/>
        public string Image5 {
            get {
                return this.image5Field;
            }
            set {
                this.image5Field = value;
            }
        }

        /// <remarks/>
        public string Description {
            get {
                return this.descriptionField;
            }
            set {
                this.descriptionField = value;
            }
        }
    }

}