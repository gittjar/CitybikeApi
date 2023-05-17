using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace CitybikeApi.Models
{
    public class Station
    {
        public int FID { get; set; }
        public int ID { get; set; }
        public string? Nimi { get; set; }
        public string? Namn { get; set; }
        public string? Name { get; set; }
        public string? Osoite { get; set; }
        public string? Adress { get; set; }
        public string? Kaupunki { get; set; }
        public string? Stad { get; set; }
        public string? Operaattor { get; set; }
        public int Kapasiteet { get; set; }
        public decimal x { get; set; }
        public decimal y { get; set; }

    }

}

