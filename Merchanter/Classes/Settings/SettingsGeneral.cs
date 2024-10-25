using System.ComponentModel.DataAnnotations;
using System.Configuration;

namespace Merchanter.Classes.Settings {
    public class SettingsGeneral {
        [Required]
        public int id { get; set; }
        [Required]
        public int customer_id { get; set; }
        [Required]
        public string company_name { get; set; } = "DEMO FİRMA";
        [Required]
        public decimal rate_TL { get; set; } = 1;
        [Required]
        public decimal rate_USD { get; set; } = 0;
        [Required]
        public decimal rate_EUR { get; set; } = 0;
        [Required]
        public int daysto_ordersync { get; set; } = 7;
        [Required]
        public int daysto_invoicesync { get; set; } = 7;
        [Required]
        public bool yurtici_kargo { get; set; } = false;
        [Required]
        public bool mng_kargo { get; set; } = false;
        [Required]
        public bool aras_kargo { get; set; } = false;
        [Required]
        public bool xml_qty_addictive_enable { get; set; } = true;
    }
}
