namespace MerchanterFrontend.Classes {
    public class N11_Categories {
        public N11_Category[] categories { get; set; }
    }

    public class N11_Category {
        public int id { get; set; }
        public int? parentId { get; set; }
        public string name { get; set; }
        public N11_Subcategory[] subCategories { get; set; }
    }

    public class N11_Subcategory {
        public int id { get; set; }
        public int parentId { get; set; }
        public string name { get; set; }
        public N11_Subcategory[] subCategories { get; set; }
    }
}
