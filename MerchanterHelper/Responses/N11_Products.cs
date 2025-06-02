namespace MerchanterHelper.Responses {

    public class N11_Products {
        public N11_Content[] content { get; set; }
        public N11_Pageable pageable { get; set; }
        public int totalElements { get; set; }
        public int totalPages { get; set; }
        public bool last { get; set; }
        public int numberOfElements { get; set; }
        public bool first { get; set; }
        public object sort { get; set; }
        public int size { get; set; }
        public int number { get; set; }
        public bool empty { get; set; }
    }

    public class N11_Pageable {
        public N11_Sort sort { get; set; }
        public int pageNumber { get; set; }
        public int pageSize { get; set; }
        public int offset { get; set; }
        public bool paged { get; set; }
        public bool unpaged { get; set; }
    }

    public class N11_Content {
        public int n11ProductId { get; set; }
        public int sellerId { get; set; }
        public string sellerNickname { get; set; }
        public string stockCode { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public long categoryId { get; set; }
        public int productMainId { get; set; }
        public string status { get; set; }
        public string saleStatus { get; set; }
        public int preparingDay { get; set; }
        public string 
            Template { get; set; }
        public int maxPurchaseQuantity { get; set; }
        public object[] customTextOptions { get; set; }
        public int catalogId { get; set; }
        public string barcode { get; set; }
        public int groupId { get; set; }
        public string currencyType { get; set; }
        public float salePrice { get; set; }
        public float listPrice { get; set; }
        public int quantity { get; set; }
        public N11_Attribute[] attributes { get; set; }
        public string[] imageUrls { get; set; }
        public int vatRate { get; set; }
        public int commissionRate { get; set; }
    }

    public class N11_Attribute {
        public int attributeId { get; set; }
        public string attributeName { get; set; }
        public string attributeValue { get; set; }
    }

}
