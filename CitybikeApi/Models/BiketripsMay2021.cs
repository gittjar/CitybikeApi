using System;
using Microsoft.EntityFrameworkCore;

namespace CitybikeApi.Models
{
	// Keyless model for API
    [Keyless]
    public class BiketripsMay2021
	{
		public DateTime Departure { get; set; }
		public DateTime Return { get; set; }
		public int Departure_station_id { get; set; }
		public string? Departure_station_name { get; set; }
		public int Return_station_id { get; set; }
		public string? Return_station_name { get; set; }
		public decimal Covered_distance_m { get; set; }
		public int Duration_sec { get; set; }

	}
}

