using Swashbuckle.AspNetCore.Annotations;

namespace Merchanter.Classes {
    [Serializable]
    public class ApiFilter {
        public List<Filter<dynamic>>? Filters { get; set; }
        public Pager? Pager { get; set; }
        public Sort? Sort { get; set; }

        [SwaggerSchema(ReadOnly = true)]
        public int? TotalCount { get; set; }
    }

    public class Pager {
        public int CurrentPageIndex { get; set; } = 1;
        public int ItemsPerPage { get; set; } = 25;
    }

    public class Filter<T> {
        public string Field { get; set; }
        public T? Value { get; set; } = default;
        public string Operator { get; set; } = "equal"; // equal, like, in, greater, less, greaterorequal, lessorequal
    }

    public class Sort {
        public string Field { get; set; }
        public string Direction { get; set; } = "desc";
    }
}
