namespace Merchanter.Requests {
    public record class M2_StockItemRequest {
        public M2_StockItemRequest() {
        }
        public int qty { get; set; }
        public bool is_in_stock { get; set; }
    }
}