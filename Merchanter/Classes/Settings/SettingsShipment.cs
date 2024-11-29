namespace Merchanter.Classes.Settings {
	public class SettingsShipment {
        public int id { get; set; }
        public int customer_id { get; set; }

        #region Yurtiçi Kargo
        public bool yurtici_kargo { get; set; } = false;
        public string? yurtici_kargo_user_name { get; set; } = null;
        public string? yurtici_kargo_password { get; set; } = null;
		#endregion

		#region MNG Kargo
		public bool mng_kargo { get; set; } = false;
		public string? mng_kargo_customer_number { get; set; } = null;
		public string? mng_kargo_password { get; set; } = null;
		public string? mng_kargo_client_id { get; set; } = null;
		public string? mng_kargo_client_secret { get; set; } = null;
		#endregion

		#region Aras Kargo
		public bool aras_kargo { get; set; } = false;
		#endregion
	}
}
