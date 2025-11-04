using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vvs_knjiga_recepata_v2.Services.ReceptServices;

namespace vvs_knjiga_recepata_v2.Services.SastojakServices
{
    public class SastojakService : ISastojakService
    {
        public void prikaziSastojak(Sastojak sastojak)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Sastojak: " + sastojak.ImeSastojka);
            sb.AppendLine("Mjerna jedinica: " + sastojak.MjernaJedinica);
        }
    }
}
