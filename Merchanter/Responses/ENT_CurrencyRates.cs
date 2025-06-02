namespace Merchanter.Responses {
    public record class ENT_CurrencyRates {
        public decimal TL { get; set; } = 1;
        public decimal EUR { get; set; }
        public decimal USD { get; set; }
    }
}