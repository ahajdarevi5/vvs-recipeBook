using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vvs_knjiga_recepata_v2.Services.ReceptServices;
using vvs_knjiga_recepata_v2.Services.OcjenaServices;
using vvs_knjiga_recepata_v2.Exceptions;
using vvs_knjiga_recepata_v2.Models;

namespace vvs_knjiga_recepata_v2.Services.KnjigaRecepataServices
{
    public class KnjigaRecepataService : IKnjigaRecepataService
    {
        private readonly ReceptService _receptService;
        private readonly OcjenaService _ocjenaService;

        public KnjigaRecepataService(ReceptService receptService, OcjenaService ocjenaService)
        {
            this._receptService = receptService;
            this._ocjenaService = ocjenaService;
        }

        //DodajRecept
        public void dodajRecept(KnjigaRecepata knjigaRecepata, Recept recept)
        {
            knjigaRecepata.recepti.Add(recept);
        }
        
        //IzbrisiRecept
        public void IzbrisiRecept(KnjigaRecepata knjigaRecepata, Recept recept)
        {
            knjigaRecepata.recepti.Remove(recept);
        }
        //Ispisuje knjigu recepata, SVE RECEPTE
        public void ispisiKnjiguRecepata(KnjigaRecepata knjigaRecepata, ReceptService receptService)
        {
            var rec = knjigaRecepata.recepti;
            foreach (var recept in rec)
            {
                receptService.prikazi(recept);
            }
        }

        //Vraća sve recepte iz knjige
        public List<Recept> DohvatiSveRecepte(KnjigaRecepata knjigaRecepata)
        {
            return knjigaRecepata.recepti;
        }

        //Sortiraj recepte po kriterijima
        public List<Recept> SortirajRecepte(
                KnjigaRecepata knjigaRecepata,
                string kategorija,
                string kriterijum,
                List<string>? unetiSastojci = null,
                (int min, int max)? vremenskiOpseg = null,
                int? maksimalneKalorije = null)
        {
            
            if (knjigaRecepata.Recepti == null || !knjigaRecepata.Recepti.Any())
                throw new ArgumentException("Knjiga recepata ne sadrži recepte.");

            //PO KATEGORIJI SLANO SLATKO
            var filtriraniRecepti = knjigaRecepata.Recepti
                .Where(r =>
                    (kategorija.ToLower() == "slatko" && r.Kategorija == Kategorija.Slatko) ||
                    (kategorija.ToLower() == "slano" && r.Kategorija == Kategorija.Slano))
                .ToList();

            //Ako nema recepata za zadatu kategoriju
            if (!filtriraniRecepti.Any())
                return new List<Recept>();

            //SORTIRANJE PO DRUGOM KRITERIJU
            switch (kriterijum.ToLower())
            {
                case "vrijeme":
                    if (vremenskiOpseg.HasValue)
                    {
                        filtriraniRecepti = filtriraniRecepti
                            .Where(r => r.VrijemePripreme >= vremenskiOpseg.Value.min &&
                                        r.VrijemePripreme <= vremenskiOpseg.Value.max)
                            .OrderBy(r => r.VrijemePripreme)
                            .ToList();
                    }
                    else
                    {
                        Console.WriteLine("Vremenski opseg nije unet.");
                        return new List<Recept>();
                    }
                    break;

                case "kalorije":
                    if (maksimalneKalorije.HasValue)
                    {
                        filtriraniRecepti = filtriraniRecepti
                            .Where(r => r.Kalorije <= maksimalneKalorije.Value)
                            .OrderBy(r => r.Kalorije)
                            .ToList();
                    }
                    else
                    {
                        Console.WriteLine("Maksimalne kalorije nisu unete.");
                        return new List<Recept>();
                    }
                    break;

                case "sastojci":
                    if (unetiSastojci != null && unetiSastojci.Any())
                    {
                        filtriraniRecepti = filtriraniRecepti
                            .Where(recept =>
                            {
                                var sastojciRecepta = recept.Sastojci
                                    .Select(s => s.Sastojak.ImeSastojka.ToLower())
                                    .ToList();

                                int brojPoklapanja = unetiSastojci
                                    .Count(sastojak => sastojciRecepta.Contains(sastojak.ToLower()));

                                return brojPoklapanja >= (unetiSastojci.Count / 2.0);
                            })
                            .OrderByDescending(recept =>
                            {
                                var sastojciRecepta = recept.Sastojci
                                    .Select(s => s.Sastojak.ImeSastojka.ToLower())
                                    .ToList();

                                int brojPoklapanja = unetiSastojci
                                    .Count(sastojak => sastojciRecepta.Contains(sastojak.ToLower()));

                                return (double)brojPoklapanja / sastojciRecepta.Count;
                            })
                            .ToList();
                    }
                    else
                    {
                        Console.WriteLine("Nema unetih sastojaka za pretragu.");
                        return new List<Recept>();
                    }
                    break;

                default:
                    throw new ArgumentException($"Nepoznat kriterijum za sortiranje: {kriterijum}");
            }

            return filtriraniRecepti;
        }

        //Pretrazi recept po nazivu
        public Recept pretraziPoNazivu(KnjigaRecepata knjigaRecepata, string naziv)
        {
            return knjigaRecepata.Recepti.FirstOrDefault(r => r.Naziv.Equals(naziv, StringComparison.OrdinalIgnoreCase));
        }

        //Racuna koliko je sastojaka potrebno u odnosu na broj osoba
        public List<KolSastojaka> PreracunajSastojke(Recept recept, int brojOsoba)
        {
            if (brojOsoba<=0)
                throw new InvalidOperationException("Broj osoba ne može biti 0 ili negativna vrijednost.");

            double faktor = (double)brojOsoba / recept.BrojOsoba;

            return recept.Sastojci.Select(s => new KolSastojaka
            {
                Sastojak = s.Sastojak,
                Kolicina = s.Kolicina * faktor
            }).ToList();
        }

        //Filtrira recepte po kalorijama
        public List<Recept> FiltrirajPoKalorijama(KnjigaRecepata knjigaRecepata, int maxKalorije, Kategorija? kategorija)
        {
            if (maxKalorije < 0)
            {
                throw new NevalidanUnosException("Broj kalorija ne može biti negativan.");
            }

            return knjigaRecepata.Recepti
                .Where(r => r.Kalorije <= maxKalorije && (kategorija == null || r.Kategorija == kategorija))
                .ToList();
        }
    }
}

