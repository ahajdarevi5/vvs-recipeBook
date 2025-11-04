using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vvs_knjiga_recepata_v2.Services.ReceptServices;

namespace vvs_knjiga_recepata_v2.Services.KnjigaRecepataServices
{
    public interface IKnjigaRecepataService
    {
        public List<Recept> DohvatiSveRecepte(KnjigaRecepata knjigaRecepata);
        public void dodajRecept(KnjigaRecepata knjigaRecepata, Recept recept);
        public void ispisiKnjiguRecepata(KnjigaRecepata knjigaRecepata, ReceptService receptService);
        public void IzbrisiRecept(KnjigaRecepata knjigaRecepata, Recept recept);
        public Recept pretraziPoNazivu(KnjigaRecepata knjigaRecepata, String naziv);
        public List<Recept> SortirajRecepte(
                   KnjigaRecepata knjigaRecepata,
                   string kategorija,
                   string kriterijum,
                   List<string>? unetiSastojci = null,
                   (int min, int max)? vremenskiOpseg = null,
                   int? maksimalneKalorije = null);
       public List<KolSastojaka> PreracunajSastojke(Recept recept, int brojOsoba);
       public List<Recept> FiltrirajPoKalorijama(KnjigaRecepata knjigaRecepata, int maxKalorije, Kategorija? kategorija);
    }
}
