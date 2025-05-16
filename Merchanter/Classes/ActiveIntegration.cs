namespace Merchanter.Classes {
    public class ActiveIntegration {
        public int customer_id { get; set; }
        public Work work { get; set; }
        public Platform platform { get; set; }
        public Integration integration { get; set; }
        public string platform_name { get; set; }
        public string work_name { get; set; }
        public Work.WorkType platform_work_type { get; set; }
        public List<Platform.PlatformType> available_platform_types { get; set; }
        public bool platform_status { get; set; }
        public Work.WorkType work_type { get; set; }
        public Work.WorkDirection work_direction { get; set; }
        public bool work_status { get; set; }
        public string version { get; set; }
        public bool integration_status { get; set; }
    }
}
