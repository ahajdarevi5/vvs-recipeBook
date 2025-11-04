using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vvs_knjiga_recepata_v2
{
    public class Sastojak
    {
        public string ImeSastojka { get; set; }
        public MjernaJedinica MjernaJedinica { get; set; }

        public Sastojak(string imeSastojka, MjernaJedinica mjernaJedinica)
        {
            ImeSastojka = imeSastojka;
            MjernaJedinica = mjernaJedinica;
        }
    }
}
