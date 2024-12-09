using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace CitybikeApi.Models
{
public class StationData
{
    public required string Station { get; set; }

    public int DepartureCount { get; set; }

    public int ReturnCount { get; set; }
}
}