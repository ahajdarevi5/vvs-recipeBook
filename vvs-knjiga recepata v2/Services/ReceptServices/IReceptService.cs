using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vvs_knjiga_recepata_v2.Models;

namespace vvs_knjiga_recepata_v2.Services.ReceptServices
{
    public interface IReceptService
    {
        public void ocijeni(Recept recept);
        public void prikazi(Recept recept);
        public double izracunajProsjecnuOcjenu(List<Ocjena> ocjene);

    }
}
