using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vvs_knjiga_recepata_v2.Exceptions;
using vvs_knjiga_recepata_v2.Models;

namespace vvs_knjiga_recepata_v2
{
    public class Recept
    {
        public string Naziv { get; set; }
        public int VrijemePripreme { get; set; }
        public int Kalorije { get; set; }
        public Kategorija Kategorija { get; set; }
        public List<KolSastojaka> Sastojci { get; set; } = new List<KolSastojaka>();
        public int BrojOsoba { get; set; }
        public List<Ocjena> Ocjene { get; set; } = new List<Ocjena>();
        public Recept(string naziv, int vrijemePripreme, int kalorije, Kategorija kategorija, List<KolSastojaka> sastojci,int brojOsoba,List<Ocjena> ocjene)
        {
            if (string.IsNullOrWhiteSpace(naziv)) throw new NevalidanUnosException("Naziv recepta ne može biti prazan.");
            if (vrijemePripreme <= 0) throw new NevalidanUnosException("Vrijeme pripreme mora biti veće od 0.");
            if (kalorije < 0) throw new NevalidanUnosException("Kalorije ne mogu biti negativne.");
            if (brojOsoba <= 0) throw new NevalidanUnosException("Broj osoba mora biti veći od 0.");
            if (sastojci == null || sastojci.Count == 0)
                throw new NevalidanUnosException("Recept mora imati barem jedan sastojak.");

            Naziv = naziv;
            VrijemePripreme = vrijemePripreme;
            Kalorije = kalorije;
            Kategorija = kategorija;
            Sastojci = sastojci;
            BrojOsoba = brojOsoba;
            Ocjene = ocjene;

        }

    }
}