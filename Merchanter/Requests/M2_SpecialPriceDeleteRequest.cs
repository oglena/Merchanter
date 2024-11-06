public class M2_SpecialPriceDeleteRequest {
    public M2_SpecialPriceDeleteRequest() {
    }

    public decimal price { get; set; }
    public int store_id { get; set; }
    public string sku { get; set; }
    public M2_PriceRequestPriceExtensionAttributes[] extension_attributes { get; set; }
    public string price_from { get; set; }
    public string price_to { get; set; }
}