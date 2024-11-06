public class M2_ShippingRequest {
    public bool notify { get; set; }
    public bool appendComment { get; set; }
    public M2_ShippingRequestShipment_Comment comment { get; set; }
    public M2_ShippingRequestShipment_Track[] tracks { get; set; }
}

public class M2_ShippingRequestShipment_Comment {
    public M2_ShippingRequestShipment_Extension_Attributes extension_attributes { get; set; }
    public string comment { get; set; }
    public int is_visible_on_front { get; set; }
}

public class M2_ShippingRequestShipment_Track {
    public M2_ShippingRequestShipment_Extension_Attributes extension_attributes { get; set; }
    public string track_number { get; set; }
    public string title { get; set; }
    public string carrier_code { get; set; }
}

public class M2_ShippingRequestShipment_Extension_Attributes {

}