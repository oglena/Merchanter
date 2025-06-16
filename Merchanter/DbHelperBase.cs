using Merchanter.Classes;
using MySql.Data.MySqlClient;

namespace Merchanter {
    /// <summary>
    /// Provides a collection of utility methods for interacting with database-related operations.
    /// </summary>
    /// <remarks>This static class serves as a base for common database helper methods, including query
    /// construction, column name translation, and filter parameter creation. It is designed to simplify database
    /// interactions by abstracting repetitive tasks such as SQL operator conversion, column mapping, and query
    /// generation.</remarks>
    public static class DbHelperBase {
        private static readonly string[] pq = ["sku", "barcode", "name", "brand_name", "type"];

        /// <summary>
        /// Gets setting value from the database
        /// </summary>
        /// <param name="_setting">Setting</param>
        /// <param name="_filter">Filter</param>
        /// <returns>[No data] returns 'null'</returns>
        public static string? GetSettingValue(string _setting, string _filter) {
            var temp = _setting.Split('|');
            foreach (var item in temp) {
                if (item.Split('=')[0] == _filter)
                    return item.Split('=')[1];
            }
            return temp[0].Split('=')[1];
        }

        /// <summary>
        /// Writes the specified text to the console with the specified foreground color.
        /// </summary>
        /// <remarks>The text is padded to fit the width of the console window, ensuring it spans the
        /// entire line. After writing the text, the console's foreground color is reset to its default value.</remarks>
        /// <param name="value">The text to display in the console.</param>
        /// <param name="_color">The foreground color to use when displaying the text.</param>
        internal static void PrintConsole(string value, ConsoleColor _color) {
            Console.ForegroundColor = _color;
            Console.WriteLine(value.PadRight(Console.WindowWidth - 1));
            Console.ResetColor();
        }

        /// <summary>
        /// Converts a database operator enumeration value to its corresponding SQL operator string.
        /// </summary>
        /// <param name="_operator">The database operator to convert. Must be a valid <see cref="IFilter.DBOperator"/> value.</param>
        /// <returns>A string representation of the specified database operator. For example,  <see
        /// cref="IFilter.DBOperator.Equal"/> returns "=" and <see cref="IFilter.DBOperator.Like"/> returns "LIKE". If
        /// the operator is not recognized, the method returns "=" by default.</returns>
        internal static string GetOperatorString(IFilter.DBOperator _operator) => _operator switch {
            IFilter.DBOperator.Equal => "=",
            IFilter.DBOperator.NotEqual => "<>",
            IFilter.DBOperator.GreaterThan => ">",
            IFilter.DBOperator.LessThan => "<",
            IFilter.DBOperator.GreaterThanOrEqual => ">=",
            IFilter.DBOperator.LessThanOrEqual => "<=",
            IFilter.DBOperator.Like => "LIKE",
            IFilter.DBOperator.In => "IN",
            IFilter.DBOperator.NotIn => "NOT IN",
            _ => "="
        };

        /// <summary>
        /// Converts a <see cref="Sort.SortDirection"/> value to its corresponding SQL sort direction string.
        /// </summary>
        /// <param name="_direction">The sort direction to convert. Must be either <see cref="Sort.SortDirection.Ascending"/> or <see
        /// cref="Sort.SortDirection.Descending"/>.</param>
        /// <returns>A string representing the SQL sort direction: <see langword="ASC"/> for ascending or <see langword="DESC"/>
        /// for descending.</returns>
        internal static string GetDirectionString(Sort.SortDirection _direction) => _direction switch {
            Sort.SortDirection.Ascending => "ASC",
            Sort.SortDirection.Descending => "DESC",
            _ => "DESC"
        };

        /// <summary>
        /// Translates a given column name from the application domain to its corresponding database column name.
        /// </summary>
        /// <remarks>This method maps specific application-level column names to their respective database
        /// column names based on predefined mappings. If the input column name does not match any of the predefined
        /// mappings, the method returns <see langword="null"/>.</remarks>
        /// <param name="_column_name">The name of the column in the application domain to be translated. This parameter is case-sensitive.</param>
        /// <returns>The corresponding database column name as a string, or <see langword="null"/> if the input column name does
        /// not have a defined mapping.</returns>
        internal static string? TranslateToDatabase(string _column_name, Type _type) {
            switch (_type) {
                case Type t when t == typeof(Product):
                    return _column_name switch {
                        "id" => "p_id",
                        "source_product_id" => "p_source_product_id",
                        "sku" => "p_sku",
                        "type" => "p_type",
                        "name" => "p_name",
                        "barcode" => "p_barcode",
                        "total_qty" => "p_total_qty",
                        "price" => "p_price",
                        "special_price" => "p_special_price",
                        "custom_price" => "p_custom_price",
                        "currency" => "p_currency",
                        "tax" => "p_tax",
                        "tax_included" => "p_tax_included",
                        "sources" => "p_sources",
                        "update_date" => "p_update_date",
                        "brand_id" => "pe_brand_id",
                        "brand_name" => "b_brand_name",
                        "category_ids" => "pe_category_ids",
                        "is_xml_enabled" => "pe_is_xml_enabled",
                        "xml_sources" => "pe_xml_sources",
                        "main_source_name" => "ps_name",
                        "main_source_update_date" => "ps_update_date",
                        "weight" => "pe_weight",
                        "volume" => "pe_volume",
                        "status" => "pe_is_enabled",
                        "description" => "pe_description",
                        _ => _column_name,
                    };
                case Type t when t == typeof(Category):
                    return _column_name;
                case Type t when t == typeof(Brand):
                    return _column_name;
                case Type t when t == typeof(Log):
                    return _column_name;
                case Type t when t == typeof(Notification):
                    return _column_name;
                default:
                    break;
            }
            throw new Exception(_column_name + " cannot translate to database column!");
        }

        /// <summary>
        /// Builds a database query string based on the specified filters, type, and sorting options.
        /// </summary>
        /// <remarks>This method dynamically constructs a SQL query based on the provided filters, entity
        /// type, and optional sorting and pagination. It also adds the necessary parameters to the <paramref
        /// name="_cmd"/> object to prevent SQL injection.  The behavior of the query generation depends on the
        /// specified <paramref name="_type"/>: - For <see cref="Product"/>, special handling is applied to prioritize
        /// product query a.k.a. "pq" filters. - For other types, filters are applied directly without reordering.  If <paramref
        /// name="_OFA"/> is <see langword="true"/>, the query will not include sorting or
        /// pagination.</remarks>
        /// <param name="_filters">The <see cref="ApiFilter"/> object containing filter, sort, and pagination criteria.</param>
        /// <param name="_query">A reference to the query string that will be modified to include the generated SQL conditions.</param>
        /// <param name="_cmd">A reference to the <see cref="MySqlCommand"/> object that will be populated with parameters for the query.</param>
        /// <param name="_type">The type of the entity (e.g., <see cref="Product"/>, <see cref="Category"/>, etc.) for which the query is
        /// being built.</param>
        /// <param name="_OFA">Only filters active. A boolean value indicating whether to include only filter conditions in the query.  If <see
        /// langword="true"/>, sorting and pagination are excluded from the query.</param>
        /// <returns>A string representing the constructed SQL query with the applied filters, sorting, and pagination.</returns>
        internal static string BuildDBQuery(ApiFilter _filters, ref string _query, ref MySqlCommand _cmd, Type _type,
            bool _OFA = false) {
            if (_query.Length > 0 && _query[^1] == ';') _query = _query[..^1]; // Remove the last semicolon(;) if it exists
            #region Filters
            if (_filters.Filters is not null && _filters.Filters.Count > 0) {
                int index = 0;
                switch (_type) {
                    case Type t when t == typeof(Product):
                        index = 0;
                        //move pq filter to top of the list if exists
                        if (_filters.Filters.Any(x => x.Field == "pq")) {
                            var pq_filter = _filters.Filters.FirstOrDefault(x => x.Field == "pq");
                            _filters.Filters.Remove(pq_filter);
                            _filters.Filters.Insert(0, pq_filter);
                        }
                        foreach (var filter in _filters.Filters) {
                            if (Equals(filter.Field, "pq")) {
                                _query += $" AND ( ";
                                _query += TranslateToDatabase("sku", _type) + " " + GetOperatorString(IFilter.DBOperator.Like) + " " +
                                    (filter.Value is not null ? $"@{TranslateToDatabase("sku", _type)}" : "NULL") + " OR ";
                                _query += TranslateToDatabase("barcode", _type) + " " + GetOperatorString(IFilter.DBOperator.Like) + " " +
                                    (filter.Value is not null ? $"@{TranslateToDatabase("barcode", _type)}" : "NULL") + " OR ";
                                _query += TranslateToDatabase("name", _type) + " " + GetOperatorString(IFilter.DBOperator.Like) + " " +
                                    (filter.Value is not null ? $"@{TranslateToDatabase("name", _type)}" : "NULL") + " OR ";
                                _query += TranslateToDatabase("brand_name", _type) + " " + GetOperatorString(IFilter.DBOperator.Like) + " " +
                                    (filter.Value is not null ? $"@{TranslateToDatabase("brand_name", _type)}" : "NULL") + " OR ";
                                _query += TranslateToDatabase("type", _type) + " " + GetOperatorString(IFilter.DBOperator.Like) + " " +
                                    (filter.Value is not null ? $"@{TranslateToDatabase("type", _type)}" : "NULL");
                                _query += $" )";
                                _cmd.Parameters.Add(new MySqlParameter(TranslateToDatabase("sku", _type), "%" + filter.Value + "%"));
                                _cmd.Parameters.Add(new MySqlParameter(TranslateToDatabase("barcode", _type), "%" + filter.Value + "%"));
                                _cmd.Parameters.Add(new MySqlParameter(TranslateToDatabase("name", _type), "%" + filter.Value + "%"));
                                _cmd.Parameters.Add(new MySqlParameter(TranslateToDatabase("brand_name", _type), "%" + filter.Value + "%"));
                                _cmd.Parameters.Add(new MySqlParameter(TranslateToDatabase("type", _type), "%" + filter.Value + "%"));
                                index++;
                            }
                            else {
                                _query += $" AND " + TranslateToDatabase(filter.Field, _type) + " " + GetOperatorString(filter.Operator) + " " +
                                    (filter.Value is not null ? $"@{TranslateToDatabase(filter.Field, _type)}" + "_" + index.ToString() : "NULL");

                                if (filter.Value is not null) {
                                    _cmd.Parameters.Add(CreateFilterParameter(_type, filter, index));
                                }
                                index++;
                            }
                        }
                        break;
                    case Type t when t == typeof(Category):
                        index = 0;
                        foreach (var filter in _filters.Filters) {
                            _query += $" AND " + TranslateToDatabase(filter.Field, _type) + " " + GetOperatorString(filter.Operator) + " " +
                                (filter.Value is not null ? $"@{TranslateToDatabase(filter.Field, _type)}" + "_" + index.ToString() : "NULL");

                            if (filter.Value is not null) {
                                _cmd.Parameters.Add(CreateFilterParameter(_type, filter, index));
                            }
                            index++;
                        }
                        break;
                    case Type t when t == typeof(Brand):
                        index = 0;
                        foreach (var filter in _filters.Filters) {
                            _query += $" AND " + TranslateToDatabase(filter.Field, _type) + " " + GetOperatorString(filter.Operator) + " " +
                                (filter.Value is not null ? $"@{TranslateToDatabase(filter.Field, _type)}" + "_" + index.ToString() : "NULL");

                            if (filter.Value is not null) {
                                _cmd.Parameters.Add(CreateFilterParameter(_type, filter, index));
                            }
                            index++;
                        }
                        break;
                    case Type t when t == typeof(Log):
                        index = 0;
                        foreach (var filter in _filters.Filters) {
                            _query += $" AND " + TranslateToDatabase(filter.Field, _type) + " " + GetOperatorString(filter.Operator) + " " +
                                (filter.Value is not null ? $"@{TranslateToDatabase(filter.Field, _type)}" + "_" + index.ToString() : "NULL");

                            if (filter.Value is not null) {
                                _cmd.Parameters.Add(CreateFilterParameter(_type, filter, index));
                            }
                            index++;
                        }
                        break;
                    case Type t when t == typeof(Notification):
                        index = 0;
                        foreach (var filter in _filters.Filters) {
                            _query += $" AND " + TranslateToDatabase(filter.Field, _type) + " " + GetOperatorString(filter.Operator) + " " +
                                (filter.Value is not null ? $"@{TranslateToDatabase(filter.Field, _type)}" + "_" + index.ToString() : "NULL");

                            if (filter.Value is not null) {
                                _cmd.Parameters.Add(CreateFilterParameter(_type, filter, index));
                            }
                            index++;
                        }
                        break;
                }
            }
            #endregion
            if (!_OFA) {
                #region Sorting
                if (_filters.Sort is not null) {
                    _query += " ORDER BY " + TranslateToDatabase(_filters.Sort.Field, _type) + " " + GetDirectionString(_filters.Sort.Direction) + " LIMIT @start,@end;";
                }
                else {
                    switch (_type) {
                        case Type t when t == typeof(Product):
                            _filters.Sort = new Sort() { Field = "update_date", Direction = Sort.SortDirection.Descending };
                            _query += " ORDER BY " + "p_update_date" + " DESC LIMIT @start,@end;";
                            break;
                        case Type t when t == typeof(Category):
                            _filters.Sort = new Sort() { Field = "id", Direction = Sort.SortDirection.Descending };
                            _query += " ORDER BY " + "id" + " DESC LIMIT @start,@end;";
                            break;
                        case Type t when t == typeof(Brand):
                            _filters.Sort = new Sort() { Field = "id", Direction = Sort.SortDirection.Descending };
                            _query += " ORDER BY " + "id" + " DESC LIMIT @start,@end;";
                            break;
                        case Type t when t == typeof(Log):
                            _filters.Sort = new Sort() { Field = "update_date", Direction = Sort.SortDirection.Descending };
                            _query += " ORDER BY " + "update_date" + " DESC LIMIT @start,@end;";
                            break;
                        case Type t when t == typeof(Notification):
                            _filters.Sort = new Sort() { Field = "update_date", Direction = Sort.SortDirection.Descending };
                            _query += " ORDER BY " + "update_date" + " DESC LIMIT @start,@end;";
                            break;
                    }
                }
                #endregion

                #region Pager
                _filters.Pager ??= new Pager() { ItemsPerPage = 10, CurrentPageIndex = 0 };
                _cmd.Parameters.Add(new MySqlParameter("start", _filters.Pager.ItemsPerPage * _filters.Pager.CurrentPageIndex));
                _cmd.Parameters.Add(new MySqlParameter("end", _filters.Pager.ItemsPerPage));
                #endregion
            }

            return _query;
        }

        /// <summary>
        /// Creates a MySQL parameter for a database query based on the specified filter and type.
        /// </summary>
        /// <remarks>The method supports various filter operators, including <see
        /// cref="IFilter.DBOperator.Like"/>, <see cref="IFilter.DBOperator.In"/>, and <see
        /// cref="IFilter.DBOperator.NotIn"/>. For "In" and "NotIn" operators, the filter value must be an array of
        /// values. String arrays are automatically formatted with single quotes around each value.</remarks>
        /// <param name="_type">The type of the entity being filtered, used to translate the field name to a database-specific format.</param>
        /// <param name="filter">The filter containing the field, operator, and value to construct the parameter.</param>
        /// <param name="_index">An optional index used to ensure unique parameter names in cases where multiple filters are applied.
        /// Defaults to 0.</param>
        /// <returns>A <see cref="MySqlParameter"/> representing the filter condition, with the parameter name and value
        /// formatted according to the filter's operator and value.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the filter value is invalid or missing, and a parameter cannot be created.</exception>
        private static MySqlParameter CreateFilterParameter(Type _type, Filter<dynamic> filter, int _index = 0) {
            switch (filter.Operator) {
                case IFilter.DBOperator.Like:
                    return new MySqlParameter(TranslateToDatabase(filter.Field, _type) + "_" + _index.ToString(),
                        "%" + filter.Value + "%");
                case IFilter.DBOperator op when op == IFilter.DBOperator.In || op == IFilter.DBOperator.NotIn:
                    dynamic[]? filter_values = filter.Value as dynamic[];
                    if (filter_values is not null) {
                        if (filter_values.GetType() == typeof(string[])) {
                            for (int i = 0; i < filter_values.Length; i++) {
                                filter_values[i] = "'" + filter_values[i] + "'";
                            }
                        }
                        return new MySqlParameter(TranslateToDatabase(filter.Field, _type) + "_" + _index.ToString(),
                            string.Join(",", filter_values));
                    }
                    break;
                default:
                    return new MySqlParameter(TranslateToDatabase(filter.Field, _type) + "_" + _index.ToString(),
                        filter.Value);
            }
            // Add a default return statement to handle cases where no value is returned.  
            throw new InvalidOperationException("Unable to create filter parameter due to invalid or missing filter value.");
        }
    }
}