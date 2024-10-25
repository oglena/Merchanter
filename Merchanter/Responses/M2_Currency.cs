
public class M2_Currency {
    public string base_currency_code { get; set; }
    public string base_currency_symbol { get; set; }
    public string default_display_currency_code { get; set; }
    public string default_display_currency_symbol { get; set; }
    public string[] available_currency_codes { get; set; }
    public Exchange_Rates[] exchange_rates { get; set; }
}

public class Exchange_Rates {
    public string currency_to { get; set; }
    public decimal rate { get; set; }
}
