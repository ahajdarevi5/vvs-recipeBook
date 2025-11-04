using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vvs_knjiga_recepata_v2.Models;

namespace vvs_knjiga_recepata_v2.Services.OcjenaServices
{
    public class OcjenaService : IOcjenaService
    {
        public double prosjecnaOcjena(List<Ocjena> ocjene)
        {
            double suma = 0;
            for (int i = 0; i < ocjene.Count; i++)
            {
                suma += ocjene[i].ocjena;
            }

            if (ocjene.Count > 0)
            {
                suma/=ocjene.Count;
            }

            return suma;
        }
        public void dodajOcjenu(Recept recept, Ocjena ocjena)
        {
            if (recept.Ocjene == null)
            {
                recept.Ocjene = new List<Ocjena>();
            }
            recept.Ocjene.Add(ocjena);
        }


    }
}
