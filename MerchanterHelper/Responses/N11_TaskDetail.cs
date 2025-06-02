namespace MerchanterHelper.Responses {

    public class N11_TaskDetail {
        public int taskId { get; set; }
        public N11_Skus skus { get; set; }
        public string createdDate { get; set; }
        public string modifiedDate { get; set; }
        public string status { get; set; }
    }

    public class N11_Skus {
        public N11_Content[] content { get; set; }
        public N11_Pageable pageable { get; set; }
        public bool last { get; set; }
        public int totalElements { get; set; }
        public int totalPages { get; set; }
        public N11_Sort sort { get; set; }
        public bool first { get; set; }
        public int number { get; set; }
        public int numberOfElements { get; set; }
        public int size { get; set; }
        public bool empty { get; set; }
    }

    public class N11_Sort {
        public bool empty { get; set; }
        public bool sorted { get; set; }
        public bool unsorted { get; set; }
    }

    public class N11_Sku {
        public float salePrice { get; set; }
        public float listPrice { get; set; }
        public string currencyType { get; set; }
        public string[] reasons { get; set; }
        public int stock { get; set; }
    }
}
