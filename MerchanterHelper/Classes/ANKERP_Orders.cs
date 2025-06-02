// using System.Xml.Serialization;
// XmlSerializer serializer = new XmlSerializer(typeof(TicariDokuman));
// using (StringReader reader = new StringReader(xml))
// {
//    var test = (TicariDokuman)serializer.Deserialize(reader);
// }

using System.Collections.Generic;
using System.Xml.Serialization;
using System;

[XmlRoot(ElementName = "Gonderen", Namespace = "http://www.ankarayazilim.com/TicariDokumanZarfi")]
public class GonderenSip {

    [XmlElement(ElementName = "VergiNo", Namespace = "http://www.ankarayazilim.com/TicariDokumanZarfi")]
    public string VergiNo;

    [XmlElement(ElementName = "Unvani", Namespace = "http://www.ankarayazilim.com/TicariDokumanZarfi")]
    public string Unvani;

    [XmlElement(ElementName = "Tanimlayici", Namespace = "http://www.ankarayazilim.com/TicariDokumanZarfi")]
    public string Tanimlayici;
}

[XmlRoot(ElementName = "Alici", Namespace = "http://www.ankarayazilim.com/TicariDokumanZarfi")]
public class AliciSip {

    [XmlElement(ElementName = "VergiNo", Namespace = "http://www.ankarayazilim.com/TicariDokumanZarfi")]
    public string VergiNo;

    [XmlElement(ElementName = "Unvani", Namespace = "http://www.ankarayazilim.com/TicariDokumanZarfi")]
    public string Unvani;

    [XmlElement(ElementName = "Tanimlayici", Namespace = "http://www.ankarayazilim.com/TicariDokumanZarfi")]
    public string Tanimlayici;
}

[XmlRoot(ElementName = "DokumanTanimi", Namespace = "http://www.ankarayazilim.com/TicariDokumanZarfi")]
public class DokumanTanimiSip {

    [XmlElement(ElementName = "Turu", Namespace = "http://www.ankarayazilim.com/TicariDokumanZarfi")]
    public string Turu;

    [XmlElement(ElementName = "Versiyon", Namespace = "http://www.ankarayazilim.com/TicariDokumanZarfi")]
    public string Versiyon;

    [XmlElement(ElementName = "DosyaAdi", Namespace = "http://www.ankarayazilim.com/TicariDokumanZarfi")]
    public string DosyaAdi;

    [XmlElement(ElementName = "OlusturulmaZamani", Namespace = "http://www.ankarayazilim.com/TicariDokumanZarfi")]
    public string OlusturulmaZamani;
}

[XmlRoot(ElementName = "DokumanBaslik", Namespace = "http://www.ankarayazilim.com/TicariDokumanZarfi")]
public class DokumanBaslikSip {

    [XmlElement(ElementName = "Versiyon", Namespace = "http://www.ankarayazilim.com/TicariDokumanZarfi")]
    public string Versiyon;

    [XmlElement(ElementName = "Gonderen", Namespace = "http://www.ankarayazilim.com/TicariDokumanZarfi")]
    public GonderenSip Gonderen;

    [XmlElement(ElementName = "Alici", Namespace = "http://www.ankarayazilim.com/TicariDokumanZarfi")]
    public AliciSip Alici;

    [XmlElement(ElementName = "DokumanTanimi", Namespace = "http://www.ankarayazilim.com/TicariDokumanZarfi")]
    public DokumanTanimiSip DokumanTanimi;
}

[XmlRoot(ElementName = "Baslik", Namespace = "http://www.ankarayazilim.com/TicariBelge")]
public class BaslikSip {

    [XmlElement(ElementName = "Aciliyet", Namespace = "http://www.ankarayazilim.com/TicariBelge")]
    public string Aciliyet;

    [XmlElement(ElementName = "HareketKodu", Namespace = "http://www.ankarayazilim.com/TicariBelge")]
    public string HareketKodu;

    [XmlElement(ElementName = "ProjeNo", Namespace = "http://www.ankarayazilim.com/TicariBelge")]
    public string ProjeNo;

    [XmlElement(ElementName = "BelgeNo", Namespace = "http://www.ankarayazilim.com/TicariBelge")]
    public int BelgeNo;

    [XmlElement(ElementName = "MBelgeNo", Namespace = "http://www.ankarayazilim.com/TicariBelge")]
    public int MBelgeNo;

    [XmlElement(ElementName = "DovizKodu", Namespace = "http://www.ankarayazilim.com/TicariBelge")]
    public string DovizKodu;

    [XmlElement(ElementName = "DovizKuru", Namespace = "http://www.ankarayazilim.com/TicariBelge")]
    public int DovizKuru;

    [XmlElement(ElementName = "HariciNumara", Namespace = "http://www.ankarayazilim.com/TicariBelge")]
    public string HariciNumara;

    [XmlElement(ElementName = "TanzimTarihi", Namespace = "http://www.ankarayazilim.com/TicariBelge")]
    public string TanzimTarihi;

    [XmlElement(ElementName = "TanzimSaati", Namespace = "http://www.ankarayazilim.com/TicariBelge")]
    public string TanzimSaati;

    [XmlElement(ElementName = "TeslimTarihi", Namespace = "http://www.ankarayazilim.com/TicariBelge")]
    public string TeslimTarihi;

    [XmlElement(ElementName = "OzelKod", Namespace = "http://www.ankarayazilim.com/TicariBelge")]
    public string OzelKod;

    [XmlElement(ElementName = "PlasiyerKodu", Namespace = "http://www.ankarayazilim.com/TicariBelge")]
    public string PlasiyerKodu;

    [XmlElement(ElementName = "OdemeSekli", Namespace = "http://www.ankarayazilim.com/TicariBelge")]
    public string OdemeSekli;

    [XmlElement(ElementName = "OdemeTarihi", Namespace = "http://www.ankarayazilim.com/TicariBelge")]
    public string OdemeTarihi;

    [XmlElement(ElementName = "OdemeRefNo", Namespace = "http://www.ankarayazilim.com/TicariBelge")]
    public string OdemeRefNo;

    [XmlElement(ElementName = "OdemeTutar", Namespace = "http://www.ankarayazilim.com/TicariBelge")]
    public decimal OdemeTutar;

    [XmlElement(ElementName = "OdemeNotu", Namespace = "http://www.ankarayazilim.com/TicariBelge")]
    public string OdemeNotu;

    [XmlElement(ElementName = "MalTutar", Namespace = "http://www.ankarayazilim.com/TicariBelge")]
    public decimal MalTutar;

    [XmlElement(ElementName = "MalIndTutar", Namespace = "http://www.ankarayazilim.com/TicariBelge")]
    public decimal MalIndTutar;

    [XmlElement(ElementName = "HizmetTutar", Namespace = "http://www.ankarayazilim.com/TicariBelge")]
    public decimal HizmetTutar;

    [XmlElement(ElementName = "HizmetIndTutar", Namespace = "http://www.ankarayazilim.com/TicariBelge")]
    public decimal HizmetIndTutar;

    [XmlElement(ElementName = "OtvMatrah", Namespace = "http://www.ankarayazilim.com/TicariBelge")]
    public decimal OtvMatrah;

    [XmlElement(ElementName = "OtvOran", Namespace = "http://www.ankarayazilim.com/TicariBelge")]
    public decimal OtvOran;

    [XmlElement(ElementName = "OtvTutar", Namespace = "http://www.ankarayazilim.com/TicariBelge")]
    public decimal OtvTutar;

    [XmlElement(ElementName = "KdvMatrah", Namespace = "http://www.ankarayazilim.com/TicariBelge")]
    public decimal KdvMatrah;

    [XmlElement(ElementName = "KdvTutar", Namespace = "http://www.ankarayazilim.com/TicariBelge")]
    public decimal KdvTutar;

    [XmlElement(ElementName = "BelgeTutar", Namespace = "http://www.ankarayazilim.com/TicariBelge")]
    public decimal BelgeTutar;
}

[XmlRoot(ElementName = "Items", Namespace = "http://www.ankarayazilim.com/TicariBelge")]
public class ItemsSip {

    [XmlElement(ElementName = "BarkodKodu", Namespace = "http://www.ankarayazilim.com/TicariBelge")]
    public string BarkodKodu;

    [XmlElement(ElementName = "UrunGrubu", Namespace = "http://www.ankarayazilim.com/TicariBelge")]
    public string UrunGrubu;

    [XmlElement(ElementName = "UrunKodu", Namespace = "http://www.ankarayazilim.com/TicariBelge")]
    public string UrunKodu;

    [XmlElement(ElementName = "UrunTanim", Namespace = "http://www.ankarayazilim.com/TicariBelge")]
    public string UrunTanim;

    [XmlElement(ElementName = "Miktar", Namespace = "http://www.ankarayazilim.com/TicariBelge")]
    public int Miktar;

    [XmlElement(ElementName = "OlcuBirim", Namespace = "http://www.ankarayazilim.com/TicariBelge")]
    public string OlcuBirim;

    [XmlElement(ElementName = "BirimFiyat", Namespace = "http://www.ankarayazilim.com/TicariBelge")]
    public decimal BirimFiyat;

    [XmlElement(ElementName = "Tutar", Namespace = "http://www.ankarayazilim.com/TicariBelge")]
    public decimal Tutar;

    [XmlElement(ElementName = "IndOran", Namespace = "http://www.ankarayazilim.com/TicariBelge")]
    public int IndOran;

    [XmlElement(ElementName = "KdvOran", Namespace = "http://www.ankarayazilim.com/TicariBelge")]
    public byte KdvOran;

    [XmlElement(ElementName = "Notlar", Namespace = "http://www.ankarayazilim.com/TicariBelge")]
    public string Notlar;
}

[XmlRoot(ElementName = "StokSicil", Namespace = "http://www.ankarayazilim.com/TicariBelge")]
public class StokSicilSip {

    [XmlElement(ElementName = "UrunGrubu", Namespace = "http://www.ankarayazilim.com/TicariBelge")]
    public string UrunGrubu;

    [XmlElement(ElementName = "UrunKodu", Namespace = "http://www.ankarayazilim.com/TicariBelge")]
    public string UrunKodu;

    [XmlElement(ElementName = "UrunTanim", Namespace = "http://www.ankarayazilim.com/TicariBelge")]
    public string UrunTanim;

    [XmlElement(ElementName = "OlcuBirim", Namespace = "http://www.ankarayazilim.com/TicariBelge")]
    public string OlcuBirim;

    [XmlElement(ElementName = "KdvOran", Namespace = "http://www.ankarayazilim.com/TicariBelge")]
    public int KdvOran;
}

[XmlRoot(ElementName = "HizmasSicil", Namespace = "http://www.ankarayazilim.com/TicariBelge")]
public class HizmasSicilSip {

    [XmlElement(ElementName = "UrunGrubu", Namespace = "http://www.ankarayazilim.com/TicariBelge")]
    public string UrunGrubu;

    [XmlElement(ElementName = "UrunKodu", Namespace = "http://www.ankarayazilim.com/TicariBelge")]
    public string UrunKodu;

    [XmlElement(ElementName = "UrunTanim", Namespace = "http://www.ankarayazilim.com/TicariBelge")]
    public string UrunTanim;

    [XmlElement(ElementName = "OlcuBirim", Namespace = "http://www.ankarayazilim.com/TicariBelge")]
    public string OlcuBirim;

    [XmlElement(ElementName = "KdvOran", Namespace = "http://www.ankarayazilim.com/TicariBelge")]
    public int KdvOran;
}

[XmlRoot(ElementName = "SatirDetay", Namespace = "http://www.ankarayazilim.com/TicariBelge")]
public class SatirDetaySip {

    [XmlElement(ElementName = "Items", Namespace = "http://www.ankarayazilim.com/TicariBelge")]
    public ItemsSip Items;

    [XmlElement(ElementName = "StokSicil", Namespace = "http://www.ankarayazilim.com/TicariBelge")]
    public StokSicilSip StokSicil;

    [XmlElement(ElementName = "HizmasSicil", Namespace = "http://www.ankarayazilim.com/TicariBelge")]
    public HizmasSicilSip HizmasSicil;
}

[XmlRoot(ElementName = "CariSicil", Namespace = "http://www.ankarayazilim.com/TicariBelge")]
public class CariSicilSip {

    [XmlElement(ElementName = "HesapNo", Namespace = "http://www.ankarayazilim.com/TicariBelge")]
    public string HesapNo;

    [XmlElement(ElementName = "Adi", Namespace = "http://www.ankarayazilim.com/TicariBelge")]
    public string Adi;

    [XmlElement(ElementName = "Soyadi", Namespace = "http://www.ankarayazilim.com/TicariBelge")]
    public string Soyadi;

    [XmlElement(ElementName = "Unvani", Namespace = "http://www.ankarayazilim.com/TicariBelge")]
    public string Unvani;

    [XmlElement(ElementName = "Adres1", Namespace = "http://www.ankarayazilim.com/TicariBelge")]
    public string Adres1;

    [XmlElement(ElementName = "Adres2", Namespace = "http://www.ankarayazilim.com/TicariBelge")]
    public string Adres2;

    [XmlElement(ElementName = "Adres3", Namespace = "http://www.ankarayazilim.com/TicariBelge")]
    public string Adres3;

    [XmlElement(ElementName = "Ilce", Namespace = "http://www.ankarayazilim.com/TicariBelge")]
    public string Ilce;

    [XmlElement(ElementName = "Sehir", Namespace = "http://www.ankarayazilim.com/TicariBelge")]
    public string Sehir;

    [XmlElement(ElementName = "PostaKodu", Namespace = "http://www.ankarayazilim.com/TicariBelge")]
    public string PostaKodu;

    [XmlElement(ElementName = "Ulke", Namespace = "http://www.ankarayazilim.com/TicariBelge")]
    public string Ulke;

    [XmlElement(ElementName = "Telefon", Namespace = "http://www.ankarayazilim.com/TicariBelge")]
    public string Telefon;

    [XmlElement(ElementName = "FaksNo", Namespace = "http://www.ankarayazilim.com/TicariBelge")]
    public string FaksNo;

    [XmlElement(ElementName = "GsmNo", Namespace = "http://www.ankarayazilim.com/TicariBelge")]
    public string GsmNo;

    [XmlElement(ElementName = "EPosta", Namespace = "http://www.ankarayazilim.com/TicariBelge")]
    public string EPosta;

    [XmlElement(ElementName = "WebSiteURL", Namespace = "http://www.ankarayazilim.com/TicariBelge")]
    public string WebSiteURL;

    [XmlElement(ElementName = "VergiNo", Namespace = "http://www.ankarayazilim.com/TicariBelge")]
    public string VergiNo;

    [XmlElement(ElementName = "VergiDaireAdi", Namespace = "http://www.ankarayazilim.com/TicariBelge")]
    public string VergiDaireAdi;

    [XmlElement(ElementName = "OzelKod", Namespace = "http://www.ankarayazilim.com/TicariBelge")]
    public string OzelKod;

    [XmlElement(ElementName = "OzelKod1", Namespace = "http://www.ankarayazilim.com/TicariBelge")]
    public string OzelKod1;

    [XmlElement(ElementName = "OzelKod2", Namespace = "http://www.ankarayazilim.com/TicariBelge")]
    public string OzelKod2;

    [XmlElement(ElementName = "OzelKod3", Namespace = "http://www.ankarayazilim.com/TicariBelge")]
    public string OzelKod3;

    [XmlElement(ElementName = "Notlar", Namespace = "http://www.ankarayazilim.com/TicariBelge")]
    public string Notlar;
}

[XmlRoot(ElementName = "SevkYeri", Namespace = "http://www.ankarayazilim.com/TicariBelge")]
public class SevkYeriSip {

    [XmlElement(ElementName = "HesapNo", Namespace = "http://www.ankarayazilim.com/TicariBelge")]
    public string HesapNo;

    [XmlElement(ElementName = "Adi", Namespace = "http://www.ankarayazilim.com/TicariBelge")]
    public string Adi;

    [XmlElement(ElementName = "Soyadi", Namespace = "http://www.ankarayazilim.com/TicariBelge")]
    public string Soyadi;

    [XmlElement(ElementName = "Unvani", Namespace = "http://www.ankarayazilim.com/TicariBelge")]
    public string Unvani;

    [XmlElement(ElementName = "Adres1", Namespace = "http://www.ankarayazilim.com/TicariBelge")]
    public string Adres1;

    [XmlElement(ElementName = "Adres2", Namespace = "http://www.ankarayazilim.com/TicariBelge")]
    public string Adres2;

    [XmlElement(ElementName = "Adres3", Namespace = "http://www.ankarayazilim.com/TicariBelge")]
    public string Adres3;

    [XmlElement(ElementName = "Ilce", Namespace = "http://www.ankarayazilim.com/TicariBelge")]
    public string Ilce;

    [XmlElement(ElementName = "Sehir", Namespace = "http://www.ankarayazilim.com/TicariBelge")]
    public string Sehir;

    [XmlElement(ElementName = "PostaKodu", Namespace = "http://www.ankarayazilim.com/TicariBelge")]
    public string PostaKodu;

    [XmlElement(ElementName = "Ulke", Namespace = "http://www.ankarayazilim.com/TicariBelge")]
    public string Ulke;

    [XmlElement(ElementName = "Telefon", Namespace = "http://www.ankarayazilim.com/TicariBelge")]
    public string Telefon;

    [XmlElement(ElementName = "FaksNo", Namespace = "http://www.ankarayazilim.com/TicariBelge")]
    public string FaksNo;

    [XmlElement(ElementName = "GsmNo", Namespace = "http://www.ankarayazilim.com/TicariBelge")]
    public string GsmNo;

    [XmlElement(ElementName = "EPosta", Namespace = "http://www.ankarayazilim.com/TicariBelge")]
    public string EPosta;

    [XmlElement(ElementName = "WebSiteURL", Namespace = "http://www.ankarayazilim.com/TicariBelge")]
    public string WebSiteURL;

    [XmlElement(ElementName = "VergiNo", Namespace = "http://www.ankarayazilim.com/TicariBelge")]
    public string VergiNo;

    [XmlElement(ElementName = "VergiDaireAdi", Namespace = "http://www.ankarayazilim.com/TicariBelge")]
    public string VergiDaireAdi;

    [XmlElement(ElementName = "KargoKodu", Namespace = "http://www.ankarayazilim.com/TicariBelge")]
    public string KargoKodu;

    [XmlElement(ElementName = "OzelKod", Namespace = "http://www.ankarayazilim.com/TicariBelge")]
    public string OzelKod;

    [XmlElement(ElementName = "Notlar", Namespace = "http://www.ankarayazilim.com/TicariBelge")]
    public string Notlar;
}

[XmlRoot(ElementName = "BelgeSicil", Namespace = "http://www.ankarayazilim.com/TicariBelge")]
public class BelgeSicilSip {

    [XmlElement(ElementName = "Baslik", Namespace = "http://www.ankarayazilim.com/TicariBelge")]
    public BaslikSip Baslik;

    [XmlElement(ElementName = "BaslikNot", Namespace = "http://www.ankarayazilim.com/TicariBelge")]
    public string BaslikNot;

    [XmlElement(ElementName = "Dipnot", Namespace = "http://www.ankarayazilim.com/TicariBelge")]
    public string Dipnot;

    [XmlElement(ElementName = "SatirDetay", Namespace = "http://www.ankarayazilim.com/TicariBelge")]
    public List<SatirDetaySip> SatirDetay;

    [XmlElement(ElementName = "CariSicil", Namespace = "http://www.ankarayazilim.com/TicariBelge")]
    public CariSicilSip CariSicil;

    [XmlElement(ElementName = "SevkYeri", Namespace = "http://www.ankarayazilim.com/TicariBelge")]
    public SevkYeriSip SevkYeri;

    [XmlElement(ElementName = "Irsaliye", Namespace = "http://www.ankarayazilim.com/TicariBelge")]
    public string Irsaliye;

    [XmlElement(ElementName = "Siparis", Namespace = "http://www.ankarayazilim.com/TicariBelge")]
    public string Siparis;

    [XmlElement(ElementName = "IsEmri", Namespace = "http://www.ankarayazilim.com/TicariBelge")]
    public string IsEmri;

    [XmlAttribute(AttributeName = "BELTUR", Namespace = "")]
    public string BELTUR;

    [XmlAttribute(AttributeName = "MS", Namespace = "")]
    public string MS;
}

[XmlRoot(ElementName = "ElemanListe", Namespace = "")]
public class ElemanListeSip {

    [XmlElement(ElementName = "BelgeSicil", Namespace = "http://www.ankarayazilim.com/TicariBelge")]
    public BelgeSicilSip BelgeSicil;
}

[XmlRoot(ElementName = "Eleman", Namespace = "")]
public class ElemanSip {

    [XmlElement(ElementName = "ElemanTipi", Namespace = "")]
    public string ElemanTipi;

    [XmlElement(ElementName = "ElemanSayisi", Namespace = "")]
    public int ElemanSayisi;

    [XmlElement(ElementName = "ElemanListe", Namespace = "")]
    public ElemanListeSip ElemanListe;
}

[XmlRoot(ElementName = "DokumanPaket", Namespace = "http://www.ankarayazilim.com/DokumanPaket")]
public class DokumanPaketSip {

    [XmlElement(ElementName = "Eleman", Namespace = "")]
    public ElemanSip Eleman;
}

[XmlRoot(ElementName = "TicariDokuman", Namespace = "http://www.ankarayazilim.com/TicariDokumanZarfi")]
public class TicariDokumanSip {

    [XmlElement(ElementName = "DokumanBaslik", Namespace = "http://www.ankarayazilim.com/TicariDokumanZarfi")]
    public DokumanBaslikSip DokumanBaslik;

    [XmlElement(ElementName = "DokumanPaket", Namespace = "http://www.ankarayazilim.com/DokumanPaket")]
    public DokumanPaketSip DokumanPaket;

    [XmlAttribute(AttributeName = "td", Namespace = "http://www.w3.org/2000/xmlns/")]
    public string Td = "http://www.ankarayazilim.com/TicariDokumanZarfi";

    [XmlAttribute(AttributeName = "dp", Namespace = "http://www.w3.org/2000/xmlns/")]
    public string Dp = "http://www.ankarayazilim.com/DokumanPaket";

    [XmlAttribute(AttributeName = "tb", Namespace = "http://www.w3.org/2000/xmlns/")]
    public string Tb = "http://www.ankarayazilim.com/TicariBelge";

    [XmlAttribute(AttributeName = "xsi", Namespace = "http://www.w3.org/2000/xmlns/")]
    public string Xsi = "http://www.w3.org/2001/XMLSchema-instance";
}

