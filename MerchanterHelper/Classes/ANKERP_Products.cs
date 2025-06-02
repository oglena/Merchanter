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
public class Gonderen {

    [XmlElement(ElementName = "VergiNo", Namespace = "http://www.ankarayazilim.com/TicariDokumanZarfi")]
    public string VergiNo;

    [XmlElement(ElementName = "Unvani", Namespace = "http://www.ankarayazilim.com/TicariDokumanZarfi")]
    public string Unvani;

    [XmlElement(ElementName = "Tanimlayici", Namespace = "http://www.ankarayazilim.com/TicariDokumanZarfi")]
    public string Tanimlayici;
}

[XmlRoot(ElementName = "Alici", Namespace = "http://www.ankarayazilim.com/TicariDokumanZarfi")]
public class Alici {

    [XmlElement(ElementName = "VergiNo", Namespace = "http://www.ankarayazilim.com/TicariDokumanZarfi")]
    public string VergiNo;

    [XmlElement(ElementName = "Unvani", Namespace = "http://www.ankarayazilim.com/TicariDokumanZarfi")]
    public string Unvani;

    [XmlElement(ElementName = "Tanimlayici", Namespace = "http://www.ankarayazilim.com/TicariDokumanZarfi")]
    public string Tanimlayici;
}

[XmlRoot(ElementName = "DokumanTanimi", Namespace = "http://www.ankarayazilim.com/TicariDokumanZarfi")]
public class DokumanTanimi {

    [XmlElement(ElementName = "Turu", Namespace = "http://www.ankarayazilim.com/TicariDokumanZarfi")]
    public string Turu;

    [XmlElement(ElementName = "Versiyon", Namespace = "http://www.ankarayazilim.com/TicariDokumanZarfi")]
    public string Versiyon;

    [XmlElement(ElementName = "DosyaAdi", Namespace = "http://www.ankarayazilim.com/TicariDokumanZarfi")]
    public string DosyaAdi;

    [XmlElement(ElementName = "OlusturulmaZamani", Namespace = "http://www.ankarayazilim.com/TicariDokumanZarfi")]
    public DateTime OlusturulmaZamani;
}

[XmlRoot(ElementName = "DokumanBaslik", Namespace = "http://www.ankarayazilim.com/TicariDokumanZarfi")]
public class DokumanBaslik {

    [XmlElement(ElementName = "Versiyon", Namespace = "http://www.ankarayazilim.com/TicariDokumanZarfi")]
    public string Versiyon;

    [XmlElement(ElementName = "Gonderen", Namespace = "http://www.ankarayazilim.com/TicariDokumanZarfi")]
    public Gonderen Gonderen;

    [XmlElement(ElementName = "Alici", Namespace = "http://www.ankarayazilim.com/TicariDokumanZarfi")]
    public Alici Alici;

    [XmlElement(ElementName = "DokumanTanimi", Namespace = "http://www.ankarayazilim.com/TicariDokumanZarfi")]
    public DokumanTanimi DokumanTanimi;
}

[XmlRoot(ElementName = "UrunTanim", Namespace = "http://www.ankarayazilim.com/UrunBilgisi")]
public class UrunTanim {

    [XmlElement(ElementName = "UrKodu", Namespace = "http://www.ankarayazilim.com/UrunBilgisi")]
    public string UrKodu;

    [XmlElement(ElementName = "UrAdi", Namespace = "http://www.ankarayazilim.com/UrunBilgisi")]
    public string UrAdi;

    [XmlElement(ElementName = "SicilKodu", Namespace = "http://www.ankarayazilim.com/UrunBilgisi")]
    public string SicilKodu;

    [XmlElement(ElementName = "Crud", Namespace = "http://www.ankarayazilim.com/UrunBilgisi")]
    public string Crud;

    [XmlElement(ElementName = "SicilAdi", Namespace = "http://www.ankarayazilim.com/UrunBilgisi", IsNullable = true)]
    public string? SicilAdi;

    [XmlElement(ElementName = "SicilAdi1", Namespace = "http://www.ankarayazilim.com/UrunBilgisi")]
    public string SicilAdi1;

    [XmlElement(ElementName = "SicilAdiy", Namespace = "http://www.ankarayazilim.com/UrunBilgisi")]
    public string SicilAdiy;

    [XmlElement(ElementName = "OzelKod1", Namespace = "http://www.ankarayazilim.com/UrunBilgisi", IsNullable = true)]
    public string? OzelKod1;

    [XmlElement(ElementName = "OzelKod2", Namespace = "http://www.ankarayazilim.com/UrunBilgisi", IsNullable = true)]
    public string? OzelKod2;

    [XmlElement(ElementName = "OzelKod3", Namespace = "http://www.ankarayazilim.com/UrunBilgisi", IsNullable = true)]
    public string? OzelKod3;

    [XmlElement(ElementName = "OzelKod4", Namespace = "http://www.ankarayazilim.com/UrunBilgisi", IsNullable = true)]
    public string? OzelKod4;

    [XmlElement(ElementName = "OzelKod5", Namespace = "http://www.ankarayazilim.com/UrunBilgisi", IsNullable = true)]
    public string? OzelKod5;

    [XmlElement(ElementName = "OzelKod6", Namespace = "http://www.ankarayazilim.com/UrunBilgisi", IsNullable = true)]
    public string? OzelKod6;

    [XmlElement(ElementName = "OzelKod7", Namespace = "http://www.ankarayazilim.com/UrunBilgisi", IsNullable = true)]
    public string? OzelKod7;

    [XmlElement(ElementName = "OzelKod8", Namespace = "http://www.ankarayazilim.com/UrunBilgisi", IsNullable = true)]
    public string? OzelKod8;

    [XmlElement(ElementName = "Sinif", Namespace = "http://www.ankarayazilim.com/UrunBilgisi", IsNullable = true)]
    public string? Sinif;

    [XmlElement(ElementName = "PerSatFiyat", Namespace = "http://www.ankarayazilim.com/UrunBilgisi")]
    public decimal PerSatFiyat;

    [XmlElement(ElementName = "ParaCinsi", Namespace = "http://www.ankarayazilim.com/UrunBilgisi")]
    public string ParaCinsi;

    [XmlElement(ElementName = "KamSatFiyat", Namespace = "http://www.ankarayazilim.com/UrunBilgisi", IsNullable = true)]
    public string? KamSatFiyat;

    [XmlElement(ElementName = "KamParaCinsi", Namespace = "http://www.ankarayazilim.com/UrunBilgisi", IsNullable = true)]
    public string? KamParaCinsi;

    [XmlElement(ElementName = "KamKodu", Namespace = "http://www.ankarayazilim.com/UrunBilgisi", IsNullable = true)]
    public string? KamKodu;

    [XmlElement(ElementName = "KamBasTar", Namespace = "http://www.ankarayazilim.com/UrunBilgisi", IsNullable = true)]
    public string? KamBasTar;

    [XmlElement(ElementName = "KamBitTar", Namespace = "http://www.ankarayazilim.com/UrunBilgisi", IsNullable = true)]
    public string? KamBitTar;

    [XmlElement(ElementName = "Dsf1", Namespace = "http://www.ankarayazilim.com/UrunBilgisi", IsNullable = true)]
    public string? Dsf1;

    [XmlElement(ElementName = "Dsk1", Namespace = "http://www.ankarayazilim.com/UrunBilgisi", IsNullable = true)]
    public string? Dsk1;

    [XmlElement(ElementName = "KdvOrani", Namespace = "http://www.ankarayazilim.com/UrunBilgisi")]
    public int KdvOrani;

    [XmlElement(ElementName = "IndOran", Namespace = "http://www.ankarayazilim.com/UrunBilgisi")]
    public int IndOran;

    [XmlElement(ElementName = "OlcuBirimi", Namespace = "http://www.ankarayazilim.com/UrunBilgisi")]
    public string OlcuBirimi;

    [XmlElement(ElementName = "BarkodKodu", Namespace = "http://www.ankarayazilim.com/UrunBilgisi", IsNullable = true)]
    public string? BarkodKodu;

    [XmlElement(ElementName = "PaketMiktari", Namespace = "http://www.ankarayazilim.com/UrunBilgisi")]
    public int PaketMiktari;

    [XmlElement(ElementName = "AgirligiKg", Namespace = "http://www.ankarayazilim.com/UrunBilgisi")]
    public int AgirligiKg;

    [XmlElement(ElementName = "HacmiM3", Namespace = "http://www.ankarayazilim.com/UrunBilgisi")]
    public int HacmiM3;

    [XmlElement(ElementName = "StokMevcudu", Namespace = "http://www.ankarayazilim.com/UrunBilgisi")]
    public int StokMevcudu;

    [XmlElement(ElementName = "TeminSuresi", Namespace = "http://www.ankarayazilim.com/UrunBilgisi", IsNullable = true)]
    public int? TeminSuresi;

    [XmlElement(ElementName = "KayitTarihi", Namespace = "http://www.ankarayazilim.com/UrunBilgisi", IsNullable = true)]
    public string? KayitTarihi;

    [XmlElement(ElementName = "Notlar", Namespace = "http://www.ankarayazilim.com/UrunBilgisi", IsNullable = true)]
    public string? Notlar;

    [XmlElement(ElementName = "KategoriKodu", Namespace = "http://www.ankarayazilim.com/UrunBilgisi", IsNullable = true)]
    public string? KategoriKodu;

    [XmlElement(ElementName = "MuadilKodu", Namespace = "http://www.ankarayazilim.com/UrunBilgisi", IsNullable = true)]
    public string? MuadilKodu;
}

[XmlRoot(ElementName = "Image", Namespace = "http://www.ankarayazilim.com/UrunBilgisi")]
public class Image {

    [XmlElement(ElementName = "IFilename", Namespace = "http://www.ankarayazilim.com/UrunBilgisi", IsNullable = true)]
    public string? IFilename;

    [XmlElement(ElementName = "IBase64Value", Namespace = "http://www.ankarayazilim.com/UrunBilgisi", IsNullable = true)]
    public string? IBase64Value;
}

[XmlRoot(ElementName = "Images", Namespace = "http://www.ankarayazilim.com/UrunBilgisi")]
public class Images {

    [XmlElement(ElementName = "Image", Namespace = "http://www.ankarayazilim.com/UrunBilgisi")]
    public List<Image> Image;
}

[XmlRoot(ElementName = "Muadil", Namespace = "http://www.ankarayazilim.com/UrunBilgisi")]
public class Muadil {

    [XmlElement(ElementName = "MUreticiAdi", Namespace = "http://www.ankarayazilim.com/UrunBilgisi", IsNullable = true)]
    public string? MUreticiAdi;

    [XmlElement(ElementName = "MUreticiKodu", Namespace = "http://www.ankarayazilim.com/UrunBilgisi", IsNullable = true)]
    public string? MUreticiKodu;
}

[XmlRoot(ElementName = "Muadils", Namespace = "http://www.ankarayazilim.com/UrunBilgisi")]
public class Muadils {

    [XmlElement(ElementName = "Muadil", Namespace = "http://www.ankarayazilim.com/UrunBilgisi")]
    public Muadil Muadil;
}

[XmlRoot(ElementName = "Oem", Namespace = "http://www.ankarayazilim.com/UrunBilgisi")]
public class Oem {

    [XmlElement(ElementName = "Marka", Namespace = "http://www.ankarayazilim.com/UrunBilgisi", IsNullable = true)]
    public string? Marka;

    [XmlElement(ElementName = "OemNo", Namespace = "http://www.ankarayazilim.com/UrunBilgisi", IsNullable = true)]
    public string? OemNo;
}

[XmlRoot(ElementName = "Oems", Namespace = "http://www.ankarayazilim.com/UrunBilgisi")]
public class Oems {

    [XmlElement(ElementName = "Oem", Namespace = "http://www.ankarayazilim.com/UrunBilgisi")]
    public Oem Oem;
}

[XmlRoot(ElementName = "Sky", Namespace = "http://www.ankarayazilim.com/UrunBilgisi")]
public class Sky {

    [XmlElement(ElementName = "SkyMarkaAdi", Namespace = "http://www.ankarayazilim.com/UrunBilgisi", IsNullable = true)]
    public string? SkyMarkaAdi;

    [XmlElement(ElementName = "SkyMarkaModelAdi", Namespace = "http://www.ankarayazilim.com/UrunBilgisi", IsNullable = true)]
    public string? SkyMarkaModelAdi;

    [XmlElement(ElementName = "SkyOemNo", Namespace = "http://www.ankarayazilim.com/UrunBilgisi", IsNullable = true)]
    public string? SkyOemNo;

    [XmlElement(ElementName = "SkyOzelKod4", Namespace = "http://www.ankarayazilim.com/UrunBilgisi", IsNullable = true)]
    public string? SkyOzelKod4;

    [XmlElement(ElementName = "SkyOzelKod5", Namespace = "http://www.ankarayazilim.com/UrunBilgisi", IsNullable = true)]
    public string? SkyOzelKod5;

    [XmlElement(ElementName = "SkyOzelKod6", Namespace = "http://www.ankarayazilim.com/UrunBilgisi", IsNullable = true)]
    public string? SkyOzelKod6;

    [XmlElement(ElementName = "SkyOzelKod7", Namespace = "http://www.ankarayazilim.com/UrunBilgisi", IsNullable = true)]
    public string? SkyOzelKod7;

    [XmlElement(ElementName = "SkyOzelKod8", Namespace = "http://www.ankarayazilim.com/UrunBilgisi", IsNullable = true)]
    public string? SkyOzelKod8;

    [XmlElement(ElementName = "SkyOzelKod9", Namespace = "http://www.ankarayazilim.com/UrunBilgisi", IsNullable = true)]
    public string? SkyOzelKod9;

    [XmlElement(ElementName = "SkyOzelKod10", Namespace = "http://www.ankarayazilim.com/UrunBilgisi", IsNullable = true)]
    public string? SkyOzelKod10;

    [XmlElement(ElementName = "SkyOtoak", Namespace = "http://www.ankarayazilim.com/UrunBilgisi", IsNullable = true)]
    public string? SkyOtoak;

    [XmlElement(ElementName = "SkyAktifmi", Namespace = "http://www.ankarayazilim.com/UrunBilgisi", IsNullable = true)]
    public string? SkyAktifmi;
}

[XmlRoot(ElementName = "Skys", Namespace = "http://www.ankarayazilim.com/UrunBilgisi")]
public class Skys {

    [XmlElement(ElementName = "Sky", Namespace = "http://www.ankarayazilim.com/UrunBilgisi")]
    public List<Sky> Sky;
}

[XmlRoot(ElementName = "Gsozellik", Namespace = "http://www.ankarayazilim.com/UrunBilgisi")]
public class Gsozellik {

    [XmlElement(ElementName = "Ozkod", Namespace = "http://www.ankarayazilim.com/UrunBilgisi", IsNullable = true)]
    public string? Ozkod;

    [XmlElement(ElementName = "Ozadi", Namespace = "http://www.ankarayazilim.com/UrunBilgisi", IsNullable = true)]
    public string? Ozadi;

    [XmlElement(ElementName = "Ozdeger", Namespace = "http://www.ankarayazilim.com/UrunBilgisi", IsNullable = true)]
    public string? Ozdeger;

    [XmlElement(ElementName = "IBase64Value", Namespace = "http://www.ankarayazilim.com/UrunBilgisi", IsNullable = true)]
    public string? IBase64Value;
}

[XmlRoot(ElementName = "Gsozelliks", Namespace = "http://www.ankarayazilim.com/UrunBilgisi")]
public class Gsozelliks {

    [XmlElement(ElementName = "Gsozellik", Namespace = "http://www.ankarayazilim.com/UrunBilgisi")]
    public List<Gsozellik> Gsozellik;
}

[XmlRoot(ElementName = "UrunSicil", Namespace = "http://www.ankarayazilim.com/UrunBilgisi")]
public class UrunSicil {

    [XmlElement(ElementName = "UrunTanim", Namespace = "http://www.ankarayazilim.com/UrunBilgisi")]
    public UrunTanim UrunTanim;

    [XmlElement(ElementName = "Images", Namespace = "http://www.ankarayazilim.com/UrunBilgisi")]
    public Images Images;

    [XmlElement(ElementName = "Muadils", Namespace = "http://www.ankarayazilim.com/UrunBilgisi")]
    public Muadils Muadils;

    [XmlElement(ElementName = "Oems", Namespace = "http://www.ankarayazilim.com/UrunBilgisi")]
    public Oems Oems;

    [XmlElement(ElementName = "Skys", Namespace = "http://www.ankarayazilim.com/UrunBilgisi")]
    public Skys Skys;

    [XmlElement(ElementName = "Gsozelliks", Namespace = "http://www.ankarayazilim.com/UrunBilgisi")]
    public Gsozelliks Gsozelliks;
}

[XmlRoot(ElementName = "ElemanListe", Namespace = "")]
public class ElemanListeUrun {

    [XmlElement(ElementName = "UrunSicil", Namespace = "http://www.ankarayazilim.com/UrunBilgisi")]
    public List<UrunSicil> UrunSicil;
}

[XmlRoot(ElementName = "Eleman", Namespace = "")]
public class ElemanUrun {

    [XmlElement(ElementName = "ElemanTipi", Namespace = "")]
    public string ElemanTipi;

    [XmlElement(ElementName = "ElemanSayisi", Namespace = "")]
    public int ElemanSayisi;

    [XmlElement(ElementName = "ElemanListe", Namespace = "")]
    public ElemanListeUrun ElemanListe;
}

[XmlRoot(ElementName = "DokumanPaket", Namespace = "http://www.ankarayazilim.com/DokumanPaket")]
public class DokumanPaketUrun {

    [XmlElement(ElementName = "Eleman", Namespace = "")]
    public ElemanUrun Eleman;
}

[XmlRoot(ElementName = "TicariDokuman", Namespace = "http://www.ankarayazilim.com/TicariDokumanZarfi")]
public class TicariDokumanUrun {

    [XmlElement(ElementName = "DokumanBaslik", Namespace = "http://www.ankarayazilim.com/TicariDokumanZarfi")]
    public DokumanBaslik DokumanBaslik;

    [XmlElement(ElementName = "DokumanPaket", Namespace = "http://www.ankarayazilim.com/DokumanPaket")]
    public DokumanPaketUrun DokumanPaketUrun;

    [XmlAttribute(AttributeName = "td", Namespace = "http://www.w3.org/2000/xmlns/")]
    public string Td = "http://www.ankarayazilim.com/TicariDokumanZarfi";

    [XmlAttribute(AttributeName = "dp", Namespace = "http://www.w3.org/2000/xmlns/")]
    public string Dp = "http://www.ankarayazilim.com/DokumanPaket";

    [XmlAttribute(AttributeName = "ub", Namespace = "http://www.w3.org/2000/xmlns/")]
    public string Ub = "http://www.ankarayazilim.com/UrunBilgisi";

    [XmlAttribute(AttributeName = "xsi", Namespace = "http://www.w3.org/2000/xmlns/")]
    public string Xsi = "http://www.w3.org/2001/XMLSchema-instance";
}

