using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vvs_knjiga_recepata_v2.Models;

namespace vvs_knjiga_recepata_v2.Services.OcjenaServices
{
    public interface IOcjenaService
    {
        public double prosjecnaOcjena(List<Ocjena> ocjene);
        public void dodajOcjenu(Recept recept, Ocjena ocjena);
    }
}
