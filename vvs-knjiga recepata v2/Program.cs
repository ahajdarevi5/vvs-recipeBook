using System;
using System.Runtime.Versioning;
using System.Security.Cryptography.X509Certificates;
using vvs_knjiga_recepata_v2.Models;
using vvs_knjiga_recepata_v2.Services.KnjigaRecepataServices;
using vvs_knjiga_recepata_v2.Services.OcjenaServices;
using vvs_knjiga_recepata_v2.Services.ReceptServices;
using vvs_knjiga_recepata_v2.Services.SastojakServices;
using vvs_knjiga_recepata_v2;

namespace vvs_knjiga_recepata_v2_KnjigaRecepata
{
    internal class Program
    {
        static void Main(string[] args)
        {

            KnjigaRecepata knjigaRecepata = new KnjigaRecepata();
            var receptService = new ReceptService();
            var ocjenaService = new OcjenaService();
            var knjigaRecepataService = new KnjigaRecepataService(receptService, ocjenaService);
            var sastojakService = new SastojakService();


            List<Sastojak> sastojci = new List<Sastojak>
            {
                new Sastojak("Brašno", MjernaJedinica.Kilogram),
                new Sastojak("Šećer", MjernaJedinica.Kilogram),
                new Sastojak("Mlijeko", MjernaJedinica.Litar),
                new Sastojak("Voda", MjernaJedinica.Litar),
                new Sastojak("Jaja", MjernaJedinica.Komad),
                new Sastojak("Sol", MjernaJedinica.Kilogram),
                new Sastojak("Kvasac", MjernaJedinica.Kilogram),
                new Sastojak("Paradajz", MjernaJedinica.Kilogram),
                new Sastojak("Sir", MjernaJedinica.Kilogram),
                new Sastojak("Ulje", MjernaJedinica.Litar)
            };

            
            bool izlaz = false;

            // Glavna petlja programa
            while (!izlaz)
            {
                Console.WriteLine("\nDobrodošli u Knjigu recepata!");
                Console.WriteLine("Odaberite opciju:");
                Console.WriteLine("1. Pogledajte sve recepte");
                Console.WriteLine("2. Pretražite recept po nazivu");
                Console.WriteLine("3. Pretražite i sortirajte recepte na osnovu kategorije");
                Console.WriteLine("4. Ocijenite recept");
                Console.WriteLine("5. Dodajte novi recept");
                Console.WriteLine("6. Dodajte sastojak receptu");
                Console.WriteLine("7. Preračunajte količinu sastojaka za broj osoba");
                Console.WriteLine("8. Dodajte novi sastojak");
                Console.WriteLine("9. Filtriraj po kalorijama");
                Console.WriteLine("0. Izlaz");
                Console.Write("\nUnesite opciju: ");

                int opcija;
                if (int.TryParse(Console.ReadLine(), out opcija))
                {
                    switch (opcija)
                    {
                        case 1:
                            // Prikaz svih recepata
                            var sviRecepti = knjigaRecepataService.DohvatiSveRecepte(knjigaRecepata);
                            if (sviRecepti.Count == 0)
                            {
                                Console.WriteLine("Nema recepata u knjizi.");
                            }
                            else
                            {
                                foreach (var recept in sviRecepti)
                                {
                                    receptService.prikazi(recept);
                                }
                            }
                            break;

                    case 2:
                        // Pretraga recepta po nazivu
                        Console.Write("Unesite naziv recepta: ");
                        string naziv = Console.ReadLine();

                        try
                        {
                            var receptZaPrikaz = knjigaRecepataService.pretraziPoNazivu(knjigaRecepata, naziv);
                            receptService.prikazi(receptZaPrikaz);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        break;


                        case 3:
                            Console.WriteLine("Izaberite kategoriju (1. Slatko, 2. Slano):");
                            string kategorija = Console.ReadLine() == "1" ? "slatko" : "slano";

                            Console.WriteLine("Izaberite sekundarni kriterijum:");
                            Console.WriteLine("1. Vrijeme pripreme");
                            Console.WriteLine("2. Kalorije");
                            Console.WriteLine("3. Sastojci (najmanje 50% poklapanja)");

                            string kriterijum = Console.ReadLine() switch
                            {
                                "1" => "vrijeme",
                                "2" => "kalorije",
                                "3" => "sastojci",
                                _ => throw new ArgumentException("Nepoznat kriterijum.")
                            };

                            (int min, int max)? vremenskiOpseg = null;
                            int? maksimalneKalorije = null;
                            List<string>? unetiSastojci = null;

                            if (kriterijum == "vrijeme")
                            {
                                Console.Write("Unesite minimalno vrijeme pripreme (u minutama): ");
                                int minVrijeme = int.Parse(Console.ReadLine());
                                Console.Write("Unesite maksimalno vrijeme pripreme (u minutama): ");
                                int maxVrijeme = int.Parse(Console.ReadLine());
                                vremenskiOpseg = (minVrijeme, maxVrijeme);
                            }
                            else if (kriterijum == "kalorije")
                            {
                                Console.Write("Unesite maksimalan broj kalorija: ");
                                maksimalneKalorije = int.Parse(Console.ReadLine());
                            }
                            else if (kriterijum == "sastojci")
                            {
                                Console.WriteLine("Unesite sastojke za pretragu (odvojene zarezom):");
                                unetiSastojci = Console.ReadLine().Split(',').Select(s => s.Trim()).ToList();
                            }

                            var sortiraniRecepti = knjigaRecepataService.SortirajRecepte(
                                knjigaRecepata,
                                kategorija,
                                kriterijum,
                                unetiSastojci,
                                vremenskiOpseg,
                                maksimalneKalorije
                            );

                            if (sortiraniRecepti.Any())
                            {
                                foreach (var recept in sortiraniRecepti)
                                {
                                    receptService.prikazi(recept);
                                }
                            }
                            else
                            {
                                Console.WriteLine("Nema rezultata za zadati kriterijum.");
                            }
                            break;


                        case 4:
                            // Ocjenjivanje recepta
                            Console.Write("Unesite naziv recepta za ocjenjivanje: ");
                            string nazivZaOcjenu = Console.ReadLine();

                            // Pronađi recept u knjizi recepata
                            var receptZaOcjenu = knjigaRecepataService.pretraziPoNazivu(knjigaRecepata, nazivZaOcjenu);
                            if (receptZaOcjenu != null)
                            {
                                Console.Write("Unesite ocjenu (1-5): ");
                                if (int.TryParse(Console.ReadLine(), out int ocjena) && ocjena >= 1 && ocjena <= 5)
                                {
                                    Console.Write("Unesite opis: ");
                                    string opis = Console.ReadLine();

                                    // Dodavanje ocjene receptu
                                    var novaOcjena = new Ocjena(receptZaOcjenu.Ocjene?.Count + 1 ?? 1, ocjena, opis);
                                    ocjenaService.dodajOcjenu(receptZaOcjenu, novaOcjena); // Dodavanje kroz servis

                                    Console.WriteLine("Ocjena dodana.");
                                }
                                else
                                {
                                    Console.WriteLine("Nevažeća ocjena.");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Recept nije pronađen.");
                            }
                            break;

                        
                        case 5:
                            // Dodavanje novog recepta
                            Console.Write("Unesite naziv recepta: ");
                            string noviNaziv = Console.ReadLine();

                            Console.Write("Unesite vrijeme pripreme (u minutama): ");
                            int vrijemePripreme = int.Parse(Console.ReadLine());

                            Console.Write("Unesite broj kalorija: ");
                            int kalorije = int.Parse(Console.ReadLine());

                            Console.Write("Unesite kategoriju (1. Slatko, 2. Slano): ");
                            var novaKategorija = Console.ReadLine() == "1" ? Kategorija.Slatko : Kategorija.Slano;

                            Console.Write("Unesite broj osoba: ");
                            int brojOsoba = int.Parse(Console.ReadLine());

                            // Dodavanje sastojaka receptu
                            Console.WriteLine("\nDostupni sastojci:");
                            for (int i = 0; i < sastojci.Count; i++)
                            {
                                Console.WriteLine($"{i + 1}. {sastojci[i].ImeSastojka} ({sastojci[i].MjernaJedinica})");
                            }

                            Console.WriteLine("\nUnesite brojeve sastojaka koje želite dodati (odvojeno zarezom, npr. 1,2,3): ");
                            var odabraniBrojevi = Console.ReadLine().Split(',').Select(s => int.Parse(s.Trim()) - 1).ToList();

                            List<KolSastojaka> noviSastojci = new List<KolSastojaka>();
                            foreach (var broj in odabraniBrojevi)
                            {
                                if (broj >= 0 && broj < sastojci.Count)
                                {
                                    Console.Write($"Unesite količinu za {sastojci[broj].ImeSastojka} ({sastojci[broj].MjernaJedinica}): ");
                                    double kolicina = double.Parse(Console.ReadLine());
                                    noviSastojci.Add(new KolSastojaka
                                    {
                                        Sastojak = sastojci[broj],
                                        Kolicina = kolicina
                                    });
                                }
                                else
                                {
                                    Console.WriteLine("Neispravan unos broja sastojka, preskače se.");
                                }
                            }

                            // Kreiranje recepta i dodavanje u knjigu recepata
                            var noviRecept = new Recept(noviNaziv, vrijemePripreme, kalorije, novaKategorija, noviSastojci, brojOsoba, new List<Ocjena>());
                            knjigaRecepataService.dodajRecept(knjigaRecepata, noviRecept);
                            Console.WriteLine("\nRecept dodan uspješno.");
                            break;

                        case 6:
                            // Dodavanje sastojka postojećem receptu
                            Console.Write("Unesite naziv recepta kojem želite dodati sastojak: ");
                            string nazivRecepta = Console.ReadLine();

                            // Pretraga recepta po nazivu
                            var receptZaIzmenu = knjigaRecepataService.pretraziPoNazivu(knjigaRecepata, nazivRecepta);
                            if (receptZaIzmenu != null)
                            {
                                Console.WriteLine("\nDostupni sastojci:");
                                for (int i = 0; i < sastojci.Count; i++)
                                {
                                    Console.WriteLine($"{i + 1}. {sastojci[i].ImeSastojka} ({sastojci[i].MjernaJedinica})");
                                }

                                Console.Write("Unesite broj sastojka koji želite dodati: ");
                                if (int.TryParse(Console.ReadLine(), out int izborSastojka) && izborSastojka > 0 && izborSastojka <= sastojci.Count)
                                {
                                    var odabraniSastojak = sastojci[izborSastojka - 1];

                                    Console.Write($"Unesite količinu za {odabraniSastojak.ImeSastojka} ({odabraniSastojak.MjernaJedinica}): ");
                                    if (double.TryParse(Console.ReadLine(), out double kolicina) && kolicina > 0)
                                    {
                                        // Dodavanje sastojka receptu
                                        var noviKolSastojaka = new KolSastojaka
                                        {
                                            Sastojak = odabraniSastojak,
                                            Kolicina = kolicina
                                        };

                                        receptZaIzmenu.Sastojci.Add(noviKolSastojaka);
                                        Console.WriteLine($"Sastojak '{odabraniSastojak.ImeSastojka}' uspešno dodat u recept '{receptZaIzmenu.Naziv}'.");
                                    }
                                    else
                                    {
                                        Console.WriteLine("Neispravna količina.");
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Neispravan izbor sastojka.");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Recept nije pronađen.");
                            }
                            break;

                        case 7:
                            // Preračunavanje sastojaka za različit broj osoba
                            Console.Write("Unesite naziv recepta za preračunavanje sastojaka: ");
                            string nazivReceptaPreracunavanje = Console.ReadLine();

                            // Pretraga recepta po nazivu
                            var receptZaPreracunavanje = knjigaRecepataService.pretraziPoNazivu(knjigaRecepata, nazivReceptaPreracunavanje);
                            if (receptZaPreracunavanje != null)
                            {
                                Console.Write($"Recept je predviđen za {receptZaPreracunavanje.BrojOsoba} osoba. Unesite novi broj osoba: ");
                                if (int.TryParse(Console.ReadLine(), out int noviBrojOsoba) && noviBrojOsoba > 0)
                                {
                                    // Preračunavanje sastojaka
                                    var prilagodjeniSastojci = knjigaRecepataService.PreracunajSastojke(receptZaPreracunavanje, noviBrojOsoba);

                                    Console.WriteLine($"\nSastojci prilagođeni za {noviBrojOsoba} osoba:");
                                    foreach (var sastojak in prilagodjeniSastojci)
                                    {
                                        Console.WriteLine($"- {sastojak.Sastojak.ImeSastojka}: {sastojak.Kolicina:F2} {sastojak.Sastojak.MjernaJedinica}");
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Neispravan unos broja osoba.");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Recept nije pronađen.");
                            }
                            break;

                        case 8:
                            // Dodavanje novog sastojka
                            Console.WriteLine("\nUnesite naziv novog sastojka: ");
                            string nazivSastojka = Console.ReadLine();

                            Console.WriteLine("Izaberite mjernu jedinicu za sastojak:");
                            Console.WriteLine("1. Kilogram");
                            Console.WriteLine("2. Litar");
                            Console.WriteLine("3. Komad");

                            int izbor = int.Parse(Console.ReadLine());
                            MjernaJedinica mjernaJedinica = 0;

                            // Odabir mjerne jedinice
                            switch (izbor)
                            {
                                case 1:
                                    mjernaJedinica = MjernaJedinica.Kilogram;
                                    break;
                                case 2:
                                    mjernaJedinica = MjernaJedinica.Litar;
                                    break;
                                case 3:
                                    mjernaJedinica = MjernaJedinica.Komad;
                                    break;
                                default:
                                    Console.WriteLine("Neispravan unos. Sastojak nije dodat.");
                                    break;
                            }

                            // Dodavanje novog sastojka u listu
                            var noviSastojak = new Sastojak(nazivSastojka, mjernaJedinica);
                            sastojci.Add(noviSastojak);

                            Console.WriteLine($"\nSastojak '{nazivSastojka}' dodat uspješno kao '{mjernaJedinica}'.");
                            break;
                        case 9:
                            // Pretraga recepata po maksimalnim kalorijama
                            Console.Write("Unesite maksimalan broj kalorija koje možete konzumirati: ");
                            if (int.TryParse(Console.ReadLine(), out int maxKalorije) && maxKalorije > 0)
                            {
                                Console.WriteLine("Da li želite slane recepte, slatke recepte ili oba? (1. Slano, 2. Slatko, 3. Oba)");
                                var izb = Console.ReadLine();

                                List<Recept> filtriraniRecepti = new List<Recept>();
                                switch (izb)
                                {
                                    case "1": // Slano
                                        filtriraniRecepti = knjigaRecepataService.FiltrirajPoKalorijama(knjigaRecepata, maxKalorije, Kategorija.Slano);
                                        break;

                                    case "2": // Slatko
                                        filtriraniRecepti = knjigaRecepataService.FiltrirajPoKalorijama(knjigaRecepata, maxKalorije, Kategorija.Slatko);
                                        break;

                                    case "3": // Oba
                                        filtriraniRecepti = knjigaRecepataService.FiltrirajPoKalorijama(knjigaRecepata, maxKalorije, null);
                                        break;

                                    default:
                                        Console.WriteLine("Neispravan izbor.");
                                        break;
                                }

                                // Prikaz rezultata
                                if (filtriraniRecepti != null && filtriraniRecepti.Count > 0)
                                {
                                    Console.WriteLine("\nPronađeni recepti:");
                                    foreach (var recept in filtriraniRecepti)
                                    {
                                        receptService.prikazi(recept);
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Nema recepata koji zadovoljavaju kriterijume.");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Neispravan unos za kalorije.");
                            }
                            break;

                        case 0:
                            // Izlaz iz programa
                            izlaz = true;
                            Console.WriteLine("Hvala što koristite Knjigu recepata. Doviđenja!");
                            break;


                        default:
                            Console.WriteLine("Nepoznata opcija. Pokušajte ponovo.");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Unijeli ste nevažeći unos.");
                }
            }
        }
    }
}