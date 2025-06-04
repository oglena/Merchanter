using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json.Serialization;

namespace Merchanter.Classes {
    /// <summary>
    /// Represents filtering, paging, and sorting criteria for API queries.
    /// </summary>
    [Serializable]
    public class ApiFilter {
        /// <summary>
        /// Gets or sets the list of filters applied to the query.
        /// </summary>
        public List<Filter<dynamic>>? Filters { get; set; }

        /// <summary>
        /// Gets or sets the paging information for the query.
        /// </summary>
        public Pager? Pager { get; set; }

        /// <summary>
        /// Gets or sets the sorting information for the query.
        /// </summary>
        public Sort? Sort { get; set; }

        /// <summary>
        /// Gets extended query responses, which can include additional data or metadata related to the query.
        /// </summary>
        [SwaggerSchema(ReadOnly = true)]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Dictionary<string, dynamic>? ExtendedQueryResponses { get; set; } = null;
    }

    /// <summary>
    /// Represents paging information for API queries.
    /// </summary>
    public class Pager {
        /// <summary>
        /// Gets or sets the current page index.
        /// </summary>
        public int CurrentPageIndex { get; set; } = 0;

        /// <summary>
        /// Gets or sets the number of items per page.
        /// </summary>
        public int ItemsPerPage { get; set; } = 25;
    }

    /// <summary>
    /// Represents sorting information for API queries.
    /// </summary>
    public class Sort {
        public string Field { get; set; }
        public SortDirection Direction { get; set; } =  SortDirection.Descending;

        public enum SortDirection {
            Ascending = 1,
            Descending = 2
        }
    }

    public interface IFilter {

        /// <summary>
        /// Represents database operators for filtering.
        /// </summary>
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public enum DBOperator {
            /// <summary>
            /// Equal operator.
            /// </summary>
            Equal = 1,

            /// <summary>
            /// Not equal operator.
            /// </summary>
            NotEqual = 2,

            /// <summary>
            /// Like operator.
            /// </summary>
            Like = 3,

            /// <summary>
            /// In operator.
            /// </summary>
            In = 4,

            /// <summary>
            /// Greater than operator.
            /// </summary>
            GreaterThan = 5,

            /// <summary>
            /// Less than operator.
            /// </summary>
            LessThan = 6,

            /// <summary>
            /// Greater than or equal operator.
            /// </summary>
            GreaterThanOrEqual = 7,

            /// <summary>
            /// Less than or equal operator.
            /// </summary>
            LessThanOrEqual = 8,

            /// <summary>
            /// Not in operator.
            /// </summary>
            NotIn = 9
        }
    }

    /// <summary>
    /// Represents a filter applied to a query.
    /// </summary>
    /// <typeparam name="T">The type of the filter value.</typeparam>
    public partial class Filter<T> : IFilter {
        /// <summary>
        /// Gets or sets the field to filter on.
        /// </summary>
        public string Field { get; set; }

        /// <summary>
        /// Gets or sets the value to filter by.
        /// </summary>
        public T? Value { get; set; } = default;

        /// <summary>
        /// Gets or sets the operator used for filtering.
        /// </summary>
        public IFilter.DBOperator Operator { get; set; } = IFilter.DBOperator.Equal;
    }
}
