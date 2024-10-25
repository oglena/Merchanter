public class ENT_Product {
    public int ProductId { get; set; }
    public string Sku { get; set; }
    public string Barcode { get; set; }
    public int Qty { get; set; }
    public decimal Price { get; set; }
    public decimal Special_Price { get; set; }
    public decimal Custom_Price { get; set; }
    public string Currency { get; set; }
    public int Tax { get; set; } = 20;
    public bool TaxIncluded { get; set; } = false;
    public string? Name { get; set; }
    public string? BrandName { get; set; }
}