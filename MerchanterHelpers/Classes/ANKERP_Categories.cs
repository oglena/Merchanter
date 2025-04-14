// using System.Xml.Serialization;
// XmlSerializer serializer = new XmlSerializer(typeof(TicariDokuman));
// using (StringReader reader = new StringReader(xml))
// {
//    var test = (TicariDokuman)serializer.Deserialize(reader);
// }

using System.Collections.Generic;
using System.Xml.Serialization;
using System;

[XmlRoot(ElementName = "KategoriItem", Namespace = "http://www.ankarayazilim.com/UrunBilgisi")]
public class KategoriItem {

    [XmlElement(ElementName = "Kodu", Namespace = "http://www.ankarayazilim.com/UrunBilgisi")]
    public string Kodu;

    [XmlElement(ElementName = "Adi", Namespace = "http://www.ankarayazilim.com/UrunBilgisi")]
    public string Adi;

    [XmlElement(ElementName = "UstBaslik", Namespace = "http://www.ankarayazilim.com/UrunBilgisi")]
    public string UstBaslik;

    [XmlElement(ElementName = "Pasif", Namespace = "http://www.ankarayazilim.com/UrunBilgisi")]
    public string Pasif;

    [XmlElement(ElementName = "Ozkod1", Namespace = "http://www.ankarayazilim.com/UrunBilgisi")]
    public string Ozkod1;

    [XmlElement(ElementName = "Ozkod2", Namespace = "http://www.ankarayazilim.com/UrunBilgisi")]
    public string Ozkod2;

    [XmlElement(ElementName = "Ozkod3", Namespace = "http://www.ankarayazilim.com/UrunBilgisi")]
    public string Ozkod3;

    [XmlElement(ElementName = "Ozkod4", Namespace = "http://www.ankarayazilim.com/UrunBilgisi")]
    public string Ozkod4;

    [XmlElement(ElementName = "Ozkod5", Namespace = "http://www.ankarayazilim.com/UrunBilgisi")]
    public string Ozkod5;
}

[XmlRoot(ElementName = "ElemanListe", Namespace = "")]
public class ElemanListeKategori {

    [XmlElement(ElementName = "KategoriItem", Namespace = "http://www.ankarayazilim.com/UrunBilgisi")]
    public List<KategoriItem> KategoriItem;
}

[XmlRoot(ElementName = "Eleman", Namespace = "")]
public class ElemanKategori {

    [XmlElement(ElementName = "ElemanTipi", Namespace = "")]
    public string ElemanTipi;

    [XmlElement(ElementName = "ElemanSayisi", Namespace = "")]
    public int ElemanSayisi;

    [XmlElement(ElementName = "ElemanListe", Namespace = "")]
    public ElemanListeKategori ElemanListe;
}

[XmlRoot(ElementName = "DokumanPaket", Namespace = "http://www.ankarayazilim.com/DokumanPaket")]
public class DokumanPaketKategori {

    [XmlElement(ElementName = "Eleman", Namespace = "")]
    public ElemanKategori Eleman;
}

[XmlRoot(ElementName = "TicariDokuman", Namespace = "http://www.ankarayazilim.com/TicariDokumanZarfi")]
public class TicariDokumanKategori {

    [XmlElement(ElementName = "DokumanBaslik", Namespace = "http://www.ankarayazilim.com/TicariDokumanZarfi")]
    public DokumanBaslik DokumanBaslik;

    [XmlElement(ElementName = "DokumanPaket", Namespace = "http://www.ankarayazilim.com/DokumanPaket")]
    public DokumanPaketKategori DokumanPaketKategori;

    [XmlAttribute(AttributeName = "td", Namespace = "http://www.w3.org/2000/xmlns/")]
    public string Td = "http://www.ankarayazilim.com/TicariDokumanZarfi";

    [XmlAttribute(AttributeName = "dp", Namespace = "http://www.w3.org/2000/xmlns/")]
    public string Dp = "http://www.ankarayazilim.com/DokumanPaket";

    [XmlAttribute(AttributeName = "ub", Namespace = "http://www.w3.org/2000/xmlns/")]
    public string Ub = "http://www.ankarayazilim.com/UrunBilgisi";

    [XmlAttribute(AttributeName = "xsi", Namespace = "http://www.w3.org/2000/xmlns/")]
    public string Xsi = "http://www.w3.org/2001/XMLSchema-instance";
}
