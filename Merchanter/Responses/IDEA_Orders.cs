namespace Merchanter.Responses {
    public record class IDEA_Orders {
        public IDEA_Order[] orders { get; set; }
    }

    public record class IDEA_Order {
        public int id { get; set; }
        public string customerFirstname { get; set; }
        public string customerSurname { get; set; }
        public string customerEmail { get; set; }
        public string customerPhone { get; set; }
        public string paymentTypeName { get; set; }
        public string paymentProviderCode { get; set; }
        public string paymentProviderName { get; set; }
        public string paymentGatewayCode { get; set; }
        public string paymentGatewayName { get; set; }
        public string bankName { get; set; }
        public string clientIp { get; set; }
        public string userAgent { get; set; }
        public string currency { get; set; }
        public string currencyRates { get; set; }
        public float amount { get; set; }
        public float couponDiscount { get; set; }
        public float taxAmount { get; set; }
        public float totalCustomTaxAmount { get; set; }
        public float promotionDiscount { get; set; }
        public float generalAmount { get; set; }
        public float shippingAmount { get; set; }
        public float additionalServiceAmount { get; set; }
        public float finalAmount { get; set; }
        public float sumOfGainedPoints { get; set; }
        public int installment { get; set; }
        public float installmentRate { get; set; }
        public int extraInstallment { get; set; }
        public string transactionId { get; set; }
        public int hasUserNote { get; set; }
        public string status { get; set; }
        public string paymentStatus { get; set; }
        public object errorMessage { get; set; }
        public string deviceType { get; set; }
        public string referrer { get; set; }
        public int invoicePrintCount { get; set; }
        public int useGiftPackage { get; set; }
        public string giftNote { get; set; }
        public string memberGroupName { get; set; }
        public int usePromotion { get; set; }
        public string shippingProviderCode { get; set; }
        public string shippingProviderName { get; set; }
        public string shippingCompanyName { get; set; }
        public string shippingPaymentType { get; set; }
        public string shippingTrackingCode { get; set; }
        public string source { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
        public IDEA_Maillist maillist { get; set; }
        public IDEA_Member member { get; set; }
        public IDEA_Orderdetail[] orderDetails { get; set; }
        public object[] orderCustomTaxLines { get; set; }
        public IDEA_Orderitem[] orderItems { get; set; }
        public IDEA_Shippingaddress shippingAddress { get; set; }
        public IDEA_Billingaddress billingAddress { get; set; }
        public object fraudOrder { get; set; }
        public object promotion { get; set; }
    }

    public record class IDEA_Maillist {
        public int id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
    }

    public record class IDEA_Member {
        public int id { get; set; }
        public string firstname { get; set; }
        public string surname { get; set; }
        public string email { get; set; }
    }

    public record class IDEA_Shippingaddress {
        public int id { get; set; }
        public string firstname { get; set; }
        public string surname { get; set; }
        public string country { get; set; }
        public string location { get; set; }
        public string subLocation { get; set; }
        public string address { get; set; }
        public string phoneNumber { get; set; }
        public string mobilePhoneNumber { get; set; }
        public string zipCode { get; set; }
    }

    public record class IDEA_Billingaddress {
        public int id { get; set; }
        public string firstname { get; set; }
        public string surname { get; set; }
        public string country { get; set; }
        public string location { get; set; }
        public string subLocation { get; set; }
        public string address { get; set; }
        public string phoneNumber { get; set; }
        public string mobilePhoneNumber { get; set; }
        public object zipCode { get; set; }
        public string invoiceType { get; set; }
        public object taxNo { get; set; }
        public object taxOffice { get; set; }
        public string identityRegistrationNumber { get; set; }
    }

    public record class IDEA_Orderdetail {
        public int id { get; set; }
        public string varKey { get; set; }
        public string varValue { get; set; }
    }

    public record class IDEA_Orderitem {
        public int id { get; set; }
        public string productName { get; set; }
        public string productSku { get; set; }
        public string productBarcode { get; set; }
        public float productPrice { get; set; }
        public string productCurrency { get; set; }
        public float productQuantity { get; set; }
        public float productTax { get; set; }
        public float productDiscount { get; set; }
        public float productMoneyOrderDiscount { get; set; }
        public float productWeight { get; set; }
        public string productStockTypeLabel { get; set; }
        public int isProductPromotioned { get; set; }
        public float discount { get; set; }
        public IDEA_Product2 product { get; set; }
        public object[] orderItemCustomizations { get; set; }
        public object orderItemSubscription { get; set; }
    }

    public record class IDEA_Product2 {
        public int id { get; set; }
        public string fullName { get; set; }
        public string slug { get; set; }
        public object[] distributors { get; set; }
    }
}