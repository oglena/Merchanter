namespace MerchanterApp.CMS.Classes {
    public class ApiFilter {
        public Dictionary<string,string?>? filters { get; set; }
        public Pager? pager { get; set; }
    }

    public class Pager {
        public int items_per_page { get; set; }
        public int current_page_index { get; set; }
    }
}
