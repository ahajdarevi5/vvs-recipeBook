using vvs_knjiga_recepata_v2.Models;
using vvs_knjiga_recepata_v2.Services.KnjigaRecepataServices;

namespace vvs_knjiga_recepata_v2
{
    public class KnjigaRecepata
    {
        public List<Recept> recepti { get; set; }

        public KnjigaRecepata()
        {
            Recepti = new List<Recept>();

            //PREDEFINISANE VRIJEDNOSTI ZA PROGRAM.CS 

             /*recepti = new List<Recept>
             {
                 new Recept(
                     "Palačinke",
                     20,
                     300,
                     Kategorija.Slatko,
                     new List<KolSastojaka>
                     {
                         new KolSastojaka { Sastojak = new Sastojak("Brašno", MjernaJedinica.Kilogram), Kolicina = 0.2 },
                         new KolSastojaka { Sastojak = new Sastojak("Jaja", MjernaJedinica.Komad), Kolicina = 2 },
                         new KolSastojaka { Sastojak = new Sastojak("Mlijeko", MjernaJedinica.Litar), Kolicina = 0.5 }
                     },
                     4,
                     new List<Ocjena> { new Ocjena(1,5,"Odlican recept"), new Ocjena(2,4,"Vrlo dobro") }
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
                      new List<Ocjena> { new Ocjena(1,5,"Odlican recept"), new Ocjena(2,4,"Vrlo dobro") }
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
                      new List<Ocjena> { new Ocjena(3,5,"Odlican recept"), new Ocjena(4,4,"Vrlo dobro") }
                 ),

                 new Recept(
                     "Špagete Carbonara",
                     25,
                     700,
                     Kategorija.Slano,
                     new List<KolSastojaka>
                     {
                         new KolSastojaka { Sastojak = new Sastojak("Špagete", MjernaJedinica.Kilogram), Kolicina = 0.4 },
                         new KolSastojaka { Sastojak = new Sastojak("Jaja", MjernaJedinica.Komad), Kolicina = 3 },
                         new KolSastojaka { Sastojak = new Sastojak("Sir", MjernaJedinica.Kilogram), Kolicina = 0.1 }
                     },
                     4,
                      new List<Ocjena> { new Ocjena(5,4,"Solidno"), new Ocjena(6,2,"Lose") }
                 ),

                 new Recept(
                     "Čokoladna torta",
                     90,
                     1200,
                     Kategorija.Slatko,
                     new List<KolSastojaka>
                     {
                         new KolSastojaka { Sastojak = new Sastojak("Brašno", MjernaJedinica.Kilogram), Kolicina = 0.3 },
                         new KolSastojaka { Sastojak = new Sastojak("Šećer", MjernaJedinica.Kilogram), Kolicina = 0.2 },
                         new KolSastojaka { Sastojak = new Sastojak("Čokolada", MjernaJedinica.Kilogram), Kolicina = 0.4 }
                     },
                     8,
                      new List<Ocjena> { new Ocjena(7,4,"Solidno"), new Ocjena(8,2,"Lose") }
                 ),

                 new Recept(
                     "Musaka",
                     45,
                     500,
                     Kategorija.Slano,
                     new List<KolSastojaka>
                     {
                         new KolSastojaka { Sastojak = new Sastojak("Meso", MjernaJedinica.Kilogram), Kolicina = 0.5 },
                         new KolSastojaka { Sastojak = new Sastojak("Krompir", MjernaJedinica.Kilogram), Kolicina = 1 },
                         new KolSastojaka { Sastojak = new Sastojak("Jaja", MjernaJedinica.Komad), Kolicina = 2 }
                     },
                     4,
                      new List<Ocjena> { new Ocjena(9,4,"Najbolji recept ikad"), new Ocjena(10,3,"Moze biti ukusnije") }
                 )
             };
             */

        }

        // Javno svojstvo za pristup listi recepata
        public List<Recept> Recepti
        {
            get { return recepti; }
            set { recepti = value; }
        }

    }
}
