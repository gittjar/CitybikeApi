using System;
namespace CitybikeApi.Models
{
	public class Biketrips2021_5
	{

		public DateTime Departure { get; set; }
		public DateTime Return { get; set; }
		public int Departure_station_id { get; set; }
		public string? Departure_station_name { get; set; }
		public int Return_station_id { get; set; }
		public string? Return_station_name { get; set; }
		public int Covered_distance_m { get; set; } // change int to SQL!
		public int Duration_sec { get; set; }

	}
}

