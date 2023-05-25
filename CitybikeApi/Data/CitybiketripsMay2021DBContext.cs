using System;
using Microsoft.EntityFrameworkCore;

namespace CitybikeApi.Data
{
	public class CitybiketripsMay2021DBContext : DbContext
	{
        public CitybiketripsMay2021DBContext(DbContextOptions<CitybiketripsMay2021DBContext> options)
        : base(options)
        {
        }

        public DbSet<CitybikeApi.Models.BiketripsMay2021> BiketripsMay2021 { get; set; } = default!;
        public DbSet<CitybikeApi.Models.BiketripsMay2021> Biketrips { get; set; } = default!;

    }

}


