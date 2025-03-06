
public class IDEA_Products {
    public IDEA_Product[] products { get; set; }
}

public class IDEA_Product {
    public int id { get; set; }
    public string name { get; set; }
    public string fullName { get; set; }
    public string slug { get; set; }
    public string sku { get; set; }
    public string barcode { get; set; }
    public float stockAmount { get; set; }
    public decimal price1 { get; set; }
    public IDEA_Currency currency { get; set; }
    public float discount { get; set; }
    public int discountType { get; set; }
    public float moneyOrderDiscount { get; set; }
    public float buyingPrice { get; set; }
    public string marketPriceDetail { get; set; }
    public int taxIncluded { get; set; }
    public int tax { get; set; }
    public int warranty { get; set; }
    public float volumetricWeight { get; set; }
    public string stockTypeLabel { get; set; }
    public int customShippingDisabled { get; set; }
    public float customShippingCost { get; set; }
    public object distributor { get; set; }
    public int hasGift { get; set; }
    public object gift { get; set; }
    public int status { get; set; }
    public int hasOption { get; set; }
    public string installmentThreshold { get; set; }
    public object homeSortOrder { get; set; }
    public object popularSortOrder { get; set; }
    public object brandSortOrder { get; set; }
    public object featuredSortOrder { get; set; }
    public object campaignedSortOrder { get; set; }
    public object newSortOrder { get; set; }
    public object discountedSortOrder { get; set; }
    public int categoryShowcaseStatus { get; set; }
    public object midblockSortOrder { get; set; }
    public string pageTitle { get; set; }
    public string metaDescription { get; set; }
    public string metaKeywords { get; set; }
    public string canonicalUrl { get; set; }
    public object parent { get; set; }
    public IDEA_Brand brand { get; set; }
    public IDEA_Detail detail { get; set; }
    public IDEA_Category[] categories { get; set; }
    public object[] prices { get; set; }
    public IDEA_Image[] images { get; set; }
    public object[] optionGroups { get; set; }
    public DateTime updatedAt { get; set; }
    public DateTime createdAt { get; set; }
}

public class IDEA_Currency {
    public int id { get; set; }
    public string label { get; set; }
    public string abbr { get; set; }
}

public class IDEA_Brand {
    public int id { get; set; }
    public string name { get; set; }
    public string pageTitle { get; set; }
    public string metaDescription { get; set; }
    public string metaKeywords { get; set; }
    public string canonicalUrl { get; set; }
    public string imageUrl { get; set; }
}

public class IDEA_Detail {
    public int id { get; set; }
    public string details { get; set; }
    public object extraDetails { get; set; }
}

public class IDEA_Category {
    public int id { get; set; }
    public string name { get; set; }
    public int sortOrder { get; set; }
    public int? showcaseSortOrder { get; set; }
    public string pageTitle { get; set; }
    public string metaDescription { get; set; }
    public string metaKeywords { get; set; }
    public string canonicalUrl { get; set; }
    public string tree { get; set; }
    public string imageUrl { get; set; }
}

public class IDEA_Image {
    public int id { get; set; }
    public string filename { get; set; }
    public string extension { get; set; }
    public string thumbUrl { get; set; }
    public string originalUrl { get; set; }
}
