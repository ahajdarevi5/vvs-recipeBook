using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vvs_knjiga_recepata_v2.Models
{
    public class Ocjena
    {
        public int id { get; set; }
        public int ocjena { get; set; }
        public string Opis { get; set; }

        public Ocjena(int id, int ocjena, string opis)
        {
            this.id = id;
            this.ocjena = ocjena;  
            this.Opis = opis;      
        }
    }

}
