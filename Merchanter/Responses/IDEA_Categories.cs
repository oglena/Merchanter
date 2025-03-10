
public class IDEA_Categories {
    public IDEA_Category[] categories { get; set; }
}

public class IDEA_Category {
    public int id { get; set; }
    public string name { get; set; }
    public string slug { get; set; }
    public int sortOrder { get; set; }
    public int status { get; set; }
    public object distributor { get; set; }
    public string distributorCode { get; set; }
    public float percent { get; set; }
    public string imageFile { get; set; }
    public int displayShowcaseContent { get; set; }
    public string showcaseContent { get; set; }
    public int showcaseContentDisplayType { get; set; }
    public int displayShowcaseFooterContent { get; set; }
    public string showcaseFooterContent { get; set; }
    public int showcaseFooterContentDisplayType { get; set; }
    public int hasChildren { get; set; }
    public string pageTitle { get; set; }
    public string metaDescription { get; set; }
    public string metaKeywords { get; set; }
    public object canonicalUrl { get; set; }
    public Parent parent { get; set; }
    public object[] children { get; set; }
    public string imageUrl { get; set; }
    public int isSearchable { get; set; }
    public object seoSetting { get; set; }
    public DateTime createdAt { get; set; }
}

public class Parent {
    public int id { get; set; }
    public string name { get; set; }
    public string slug { get; set; }
    public int sortOrder { get; set; }
    public int status { get; set; }
    public object distributor { get; set; }
    public string distributorCode { get; set; }
    public float percent { get; set; }
    public string imageFile { get; set; }
    public int displayShowcaseContent { get; set; }
    public string showcaseContent { get; set; }
    public int showcaseContentDisplayType { get; set; }
    public int displayShowcaseFooterContent { get; set; }
    public string showcaseFooterContent { get; set; }
    public int showcaseFooterContentDisplayType { get; set; }
    public int hasChildren { get; set; }
    public object pageTitle { get; set; }
    public string metaDescription { get; set; }
    public string metaKeywords { get; set; }
    public object canonicalUrl { get; set; }
    public object parent { get; set; }
    public object[] children { get; set; }
    public string imageUrl { get; set; }
    public int isSearchable { get; set; }
    public object seoSetting { get; set; }
    public DateTime createdAt { get; set; }
}
