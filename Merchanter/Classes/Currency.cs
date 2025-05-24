using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merchanter.Classes {
    public class Currency {
        public string code { get; set; } = "TL";
        public string symbol { get; set; } = "₺";
        public string name { get; set; } = "Türk Lirası";
        public string format { get; set; } = "{0}{1}";

        public static Currency DefaultCurrency => GetCurrency("TL");

        /// <summary>
        /// Creates a Currency object based on the provided currency code.
        /// </summary>
        /// <param name="_code">Currency code (e.g., "TL", "USD", "EUR").</param>
        /// <returns>Currency object with normalized values.</returns>
        public static Currency GetCurrency(string _code) {
            _code = _code.Trim();
            _code = _code.ToUpperInvariant();
            _code = (_code == "TRY" || _code == "TRL") ? "TL" : _code; // Normalize TRY and TRL to TL
            _code = (_code == "EURO") ? "EUR" : _code; // Normalize EURO to EUR
            return new Currency {
                code = _code,
                symbol = _code switch {
                    "TL" => "₺",
                    "USD" => "$",
                    "EUR" => "€",
                    _ => _code
                },
                name = _code switch {
                    "TL" => "Türk Lirası",
                    "USD" => "Amerikan Doları",
                    "EUR" => "Euro",
                    _ => _code
                },
                format = _code switch {
                    "TL" => "{0}{1}",
                    "USD" => "{1}{0}",
                    "EUR" => "{1}{0}",
                    _ => "{0}{1}"
                }
            };
        }

        public string Format(decimal amount) {
            return string.Format(format, amount.ToString("N2"), symbol);
        }

        public override string ToString() {
            return code;
        }
    }
}
