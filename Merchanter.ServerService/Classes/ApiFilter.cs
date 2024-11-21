using Swashbuckle.AspNetCore.Annotations;

namespace Merchanter.ServerService.Classes {
	[Serializable]
	public class ApiFilter {
		public List<Filter<dynamic>>? Filters { get; set; }
		public Pager? Pager { get; set; }
		//public Sort? Sort { get; set; }

		[SwaggerSchema( ReadOnly = true )]
		public int? TotalCount { get; set; }
	}

	public class Pager {
		public int ItemsPerPage { get; set; }
		public int CurrentPageIndex { get; set; }
	}

	public class Filter<T> {
		public string Field { get; set; } = string.Empty;
		public T? Value { get; set; } = default;
		//public string Operator { get; set; } = "equal";
	}

	//public class Sort {
	//	public string Field { get; set; } = string.Empty;
	//	public bool Descending { get; set; }
	//}
}
