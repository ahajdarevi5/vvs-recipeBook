using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vvs_knjiga_recepata_v2.Models;
using vvs_knjiga_recepata_v2.Services.SastojakServices;

namespace vvs_knjiga_recepata_v2.Services.ReceptServices
{
    public class ReceptService : IReceptService
    {
        //Ostavljanje ocjene za recept
        public void ocijeni(Recept recept)
        {
            int ocjena = 0;

            Console.WriteLine("Unesite ocjenu za odabrani recept (1-5): ");
            string unos = Console.ReadLine();

            if (!int.TryParse(unos, out ocjena) || ocjena < 1 || ocjena > 5)
            {
                throw new Exception("Pogresan unos");
            }

            Console.WriteLine("Unesite komentar: ");
            string komentar = Console.ReadLine();

            recept.Ocjene.Add(new Ocjena(recept.Ocjene.Count, ocjena, komentar));
        }

        //ISPIS RECEPTA, ispis prosječne ocjene
        public void prikazi(Recept recept)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("Recept: " + recept.Naziv);
            sb.AppendLine("Kategorija: " + recept.Kategorija);
            sb.AppendLine("Kalorije: " + recept.Kalorije);
            sb.AppendLine("Vrijeme pripreme: " + recept.VrijemePripreme + " minuta");
            sb.AppendLine("Broj osoba: " + recept.BrojOsoba);
            sb.AppendLine("\nSastojci:");

            foreach (var sastojak in recept.Sastojci)
            {
                sb.AppendLine($"- {sastojak.Sastojak.ImeSastojka}: {sastojak.Kolicina} {sastojak.Sastojak.MjernaJedinica}");
            }
            if (recept.Ocjene.Count > 0)
            {
                double prosjecnaOcjena = izracunajProsjecnuOcjenu(recept.Ocjene);
                sb.AppendLine("\nOcjene:");
                foreach (var ocjena in recept.Ocjene)
                {
                    sb.AppendLine($"Ocjena {ocjena.ocjena}: {ocjena.Opis}");
                }
                sb.AppendLine($"Prosječna ocjena: {prosjecnaOcjena:F2}"); 
            }
            else
            {
                sb.AppendLine("\nNema ocjena.");
            }

            Console.WriteLine(sb.ToString());
            Console.WriteLine(".............................");
        }
        public double izracunajProsjecnuOcjenu(List<Ocjena> ocjene)
        {
            if (ocjene.Count == 0)
                return 0;

            double suma = 0;
            foreach (var ocjena in ocjene)
            {
                suma += ocjena.ocjena;
            }

            return suma / ocjene.Count;
        }


    }
}
