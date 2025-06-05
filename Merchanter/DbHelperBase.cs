using Merchanter.Classes;
using MySql.Data.MySqlClient;

namespace Merchanter {
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
                    switch (_column_name) {
                        case "id":
                            return "p_id";
                        case "source_product_id":
                            return "p_source_product_id";
                        case "sku":
                            return "p_sku";
                        case "type":
                            return "p_type";
                        case "name":
                            return "p_name";
                        case "barcode":
                            return "p_barcode";
                        case "total_qty":
                            return "p_total_qty";
                        case "price":
                            return "p_price";
                        case "special_price":
                            return "p_special_price";
                        case "custom_price":
                            return "p_custom_price";
                        case "currency":
                            return "p_currency";
                        case "tax":
                            return "p_tax";
                        case "tax_included":
                            return "p_tax_included";
                        case "sources":
                            return "p_sources";
                        case "update_date":
                            return "p_update_date";
                        case "brand_id":
                            return "pe_brand_id";
                        case "brand_name":
                            return "b_brand_name";
                        case "category_ids":
                            return "pe_category_ids";
                        case "is_xml_enabled":
                            return "pe_is_xml_enabled";
                        case "xml_sources":
                            return "pe_xml_sources";
                        case "main_source_name":
                            return "ps_name";
                        case "main_source_update_date":
                            return "ps_update_date";
                        case "weight":
                            return "pe_weight";
                        case "volume":
                            return "pe_volume";
                        case "status":
                            return "pe_is_enabled";
                        case "description":
                            return "pe_description";
                        default:
                            return _column_name;
                    }
                case Type t when t == typeof(Category):
                    return _column_name;
                case Type t when t == typeof(Brand):
                    return _column_name;
                case Type t when t == typeof(Log):
                    return _column_name;
                default:
                    break;
            }
            throw new Exception(_column_name + " cannot translate to database column!");
        }

        internal static string BuildDBQuery(ApiFilter _filters, ref string _query, ref MySqlCommand _cmd, Type _type, bool _only_filters_active = false) {
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
                }
            }
            #endregion
            if (!_only_filters_active) {
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
                        case Type t when t == typeof(Log):
                            _filters.Sort = new Sort() { Field = "update_date", Direction = Sort.SortDirection.Descending };
                            _query += " ORDER BY " + "p_update_date" + " DESC LIMIT @start,@end;";
                            break;
                        case Type t when t == typeof(Brand):
                            _filters.Sort = new Sort() { Field = "id", Direction = Sort.SortDirection.Descending };
                            _query += " ORDER BY " + "id" + " DESC LIMIT @start,@end;";
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