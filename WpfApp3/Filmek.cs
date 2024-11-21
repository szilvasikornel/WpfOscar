using System;

namespace WpfApp3
{
    public class Filmek
    {
        public string Azon { get; set; }
        public string Cim { get; set; }
        public int Ev { get; set; }
        public int JelolesSzam { get; set; }
        public int DijSzam { get; set; }
        public string Kategoria { get; set; } 

        public Filmek(string sor)
        {
            var r = sor.Split('\t');

            Azon = r[0];
            Cim = r[1];
            Ev = int.Parse(r[2]);
            JelolesSzam = int.Parse(r[3]);
            DijSzam = int.Parse(r[4]);
            Kategoria = r.Length >= 5 ? r[5] : string.Empty; 
        }
    }
}
