using System.Runtime.Serialization;

namespace MerchanterApp.CMS.Classes {
    [Serializable]
    public class ApiFilter {
        public List<Filter<dynamic>>? Filters { get; set; }
        public Pager? Pager { get; set; }
        public int? TotalCount { get; set; }
    }

    public class Pager {
        public int ItemsPerPage { get; set; }
        public int CurrentPageIndex { get; set; }
    }

    public class Filter<T> {
        public string Field { get; set; } = string.Empty;
        public T? Value { get; set; } = default;
    }
}
