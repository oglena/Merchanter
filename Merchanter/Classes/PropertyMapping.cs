namespace Merchanter.Classes {
    public record class PropertyMapping {
        public int id { get; set; }
        public string property { get; set; }
        public string source { get; set; }
        public SyncType sync_type { get; set; } = SyncType.None;

        public enum SyncType {
            None = 0,
            FullSync = 1,
            OnlyInsert = 2,
            SKU = 3,
            Brand = 4,
            Category = 5,
            Image = 6,
            TargetPrice = 7,
            Attribute = 8,
            Currency = 9
        }
    }
}
