using vvs_knjiga_recepata_v2.Models;
using vvs_knjiga_recepata_v2.Services.KnjigaRecepataServices;
using vvs_knjiga_recepata_v2.Services.OcjenaServices;
using vvs_knjiga_recepata_v2.Services.ReceptServices;
using vvs_knjiga_recepata_v2;
using vvs_knjiga_recepata_v2.Exceptions;

[TestClass]
public class KnjigaRecepataServiceTest
{
    [TestMethod]
    public void DodajRecept_ValidanUnos_TrebaDodatiRecept()
    {
        var knjigaRecepata = new KnjigaRecepata();
        var receptService = new KnjigaRecepataService(new ReceptService(), new OcjenaService());

        string naziv = "Palaèinke";
        int vrijemePripreme = 20;
        int kalorije = 300;
        Kategorija kategorija = Kategorija.Slatko;
        int brojOsoba = 4;
        List<KolSastojaka> sastojci = new List<KolSastojaka>
            {
                new KolSastojaka { Sastojak = new Sastojak("Brašno", MjernaJedinica.Kilogram), Kolicina = 0.2 },
                new KolSastojaka { Sastojak = new Sastojak("Jaja", MjernaJedinica.Komad), Kolicina = 2 }
            };

        var noviRecept = new Recept(naziv, vrijemePripreme, kalorije, kategorija, sastojci, brojOsoba, new List<Ocjena>());
        receptService.dodajRecept(knjigaRecepata, noviRecept);
        Assert.AreEqual(1, knjigaRecepata.Recepti.Count);
        Assert.AreEqual("Palaèinke", knjigaRecepata.Recepti[0].Naziv);
    }

    [TestMethod]
    public void Recept_NevalidanUnos_TrebaBacitiNevalidanUnosExc()
    {
        string naziv = "Palaèinke";
        int vrijemePripreme = 20;
        int kalorije = -100; 
        Kategorija kategorija = Kategorija.Slatko;
        int brojOsoba = 4;
        List<KolSastojaka> sastojci = new List<KolSastojaka>
        {
            new KolSastojaka { Sastojak = new Sastojak("Brašno", MjernaJedinica.Kilogram), Kolicina = 0.2 },
            new KolSastojaka { Sastojak = new Sastojak("Jaja", MjernaJedinica.Komad), Kolicina = 2 }
        };

        var exception = Assert.ThrowsException<NevalidanUnosException>(() =>
        {
            var noviRecept = new Recept(naziv, vrijemePripreme, kalorije, kategorija, sastojci, brojOsoba, new List<Ocjena>());
        });

        Assert.AreEqual("Kalorije ne mogu biti negativne.", exception.Message);
    }

    [TestMethod]
    public void IzbrisiRecept_ValidanRecept_TrebaIzbrisatiRecept()
    {
        var knjigaRecepata = new KnjigaRecepata();
        var receptService = new KnjigaRecepataService(new ReceptService(), new OcjenaService());
        var recept = new Recept(
            "Palaèinke",
            20,
            300,
            Kategorija.Slatko,
            new List<KolSastojaka>
            {
            new KolSastojaka { Sastojak = new Sastojak("Brašno", MjernaJedinica.Kilogram), Kolicina = 0.2 },
            new KolSastojaka { Sastojak = new Sastojak("Jaja", MjernaJedinica.Komad), Kolicina = 2 }
            },
            4,
            new List<Ocjena>()
        );

        knjigaRecepata.recepti.Add(recept);
        receptService.IzbrisiRecept(knjigaRecepata, recept);
        Assert.AreEqual(0, knjigaRecepata.recepti.Count);
    }

    [TestMethod]
    [DataRow(800, Kategorija.Slano, 2)]
    [DataRow(400, Kategorija.Slatko, 1)]
    [DataRow(1000, null, 3)]
    public void FiltrirajPoKalorijama_ValidanUnos_VracaIspravanRezultat(int maxKalorije, Kategorija? kategorija, int expectedCount)
    {
        var knjigaRecepata = new KnjigaRecepata();
        var receptService = new KnjigaRecepataService(new ReceptService(), new OcjenaService());
        knjigaRecepata.Recepti = new List<Recept>
    {
        new Recept(
            "Palaèinke",
            20,
            300,
            Kategorija.Slatko,
            new List<KolSastojaka>
            {
                new KolSastojaka { Sastojak = new Sastojak("Brašno", MjernaJedinica.Kilogram), Kolicina = 0.2 },
                new KolSastojaka { Sastojak = new Sastojak("Jaja", MjernaJedinica.Komad), Kolicina = 2 }
            },
            4,
            new List<Ocjena>()
        ),
        new Recept(
            "Pizza",
            30,
            800,
            Kategorija.Slano,
            new List<KolSastojaka>
            {
                new KolSastojaka { Sastojak = new Sastojak("Brašno", MjernaJedinica.Kilogram), Kolicina = 0.3 },
                new KolSastojaka { Sastojak = new Sastojak("Sir", MjernaJedinica.Kilogram), Kolicina = 0.2 },
                new KolSastojaka { Sastojak = new Sastojak("Paradajz sos", MjernaJedinica.Litar), Kolicina = 0.15 }
            },
            2,
            new List<Ocjena>()
        ),
        new Recept(
            "Gulaš",
            60,
            600,
            Kategorija.Slano,
            new List<KolSastojaka>
            {
                new KolSastojaka { Sastojak = new Sastojak("Meso", MjernaJedinica.Kilogram), Kolicina = 1 },
                new KolSastojaka { Sastojak = new Sastojak("Luk", MjernaJedinica.Kilogram), Kolicina = 0.5 },
                new KolSastojaka { Sastojak = new Sastojak("Voda", MjernaJedinica.Litar), Kolicina = 1 }
            },
            6,
            new List<Ocjena>()
        )
    };
        var filtriraniRecepti = receptService.FiltrirajPoKalorijama(knjigaRecepata, maxKalorije, kategorija);

        Assert.AreEqual(expectedCount, filtriraniRecepti.Count);
    }

    [TestMethod]
    [ExpectedException(typeof(NevalidanUnosException))]
    public void FiltrirajPoKalorijama_NevalidneKalorije_TrebaBacitiNevalidanUnosExc()
    {
        var knjigaRecepata = new KnjigaRecepata();
        var receptService = new KnjigaRecepataService(new ReceptService(), new OcjenaService());
        receptService.FiltrirajPoKalorijama(knjigaRecepata, -100, Kategorija.Slatko); 
    }
    [TestMethod]
    public void PreracunajSastojke_ValidanUnos_TrebaVratitiIspravneVrijednosti()
    {

        var recept = new Recept(
            "Palaèinke",
            20,
            300,
            Kategorija.Slatko,
            new List<KolSastojaka>
            {
            new KolSastojaka { Sastojak = new Sastojak("Brašno", MjernaJedinica.Kilogram), Kolicina = 0.2 },
            new KolSastojaka { Sastojak = new Sastojak("Jaja", MjernaJedinica.Komad), Kolicina = 2 }
            },
            4,
            new List<Ocjena>()
        );

        var receptService = new KnjigaRecepataService(new ReceptService(), new OcjenaService());

        var preracunatiSastojci = receptService.PreracunajSastojke(recept, 8); 

        Assert.AreEqual(0.4, preracunatiSastojci[0].Kolicina); 
        Assert.AreEqual(4, preracunatiSastojci[1].Kolicina); 
    }
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void PreracunajSastojke_NevalidanBrojOsoba_TrebaVratitiExc()
    {
        var recept = new Recept(
            "Palaèinke",
            20,
            300,
            Kategorija.Slatko,
            new List<KolSastojaka>
            {
            new KolSastojaka { Sastojak = new Sastojak("Brašno", MjernaJedinica.Kilogram), Kolicina = 0.2 },
            new KolSastojaka { Sastojak = new Sastojak("Jaja", MjernaJedinica.Komad), Kolicina = 2 }
            },
            4,
            new List<Ocjena>()
        );

        var receptService = new KnjigaRecepataService(new ReceptService(), new OcjenaService());
        var preracunatiSastojci = receptService.PreracunajSastojke(recept, 0);
    }
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void PreracunajSastojke_NegativanBrojOsoba_ShouldThrowException()
    {
        var recept = new Recept(
            "Palaèinke",
            20,
            300,
            Kategorija.Slatko,
            new List<KolSastojaka>
            {
            new KolSastojaka { Sastojak = new Sastojak("Brašno", MjernaJedinica.Kilogram), Kolicina = 0.2 },
            new KolSastojaka { Sastojak = new Sastojak("Jaja", MjernaJedinica.Komad), Kolicina = 2 }
            },
            4,
            new List<Ocjena>()
        );

        var receptService = new KnjigaRecepataService(new ReceptService(), new OcjenaService());
        var preracunatiSastojci = receptService.PreracunajSastojke(recept, -4); 
    }
    [TestMethod]
    [ExpectedException(typeof(NevalidanUnosException))]
    public void PreracunajSastojke_PraznaListaSastojaka_ShouldThrowException()
    {
        // Arrange
        var recept = new Recept(
            "Palaèinke",
            20,
            300,
            Kategorija.Slatko,
            new List<KolSastojaka>(), 
            4,
            new List<Ocjena>()
        );

        var receptService = new KnjigaRecepataService(new ReceptService(), new OcjenaService());
        var preracunatiSastojci = receptService.PreracunajSastojke(recept, 8);
    }
}