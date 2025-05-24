using Merchanter.Classes;
using static Merchanter.Classes.Product;

public class CurrencyRate {
    public Currency currency { get; set; } = Currency.GetCurrency("TL");
    public decimal rate { get; set; } = 1.0m; // Default rate is 1.0 for the base currency
}