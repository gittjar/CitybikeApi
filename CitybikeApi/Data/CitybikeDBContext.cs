﻿using System;
using CitybikeApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CitybikeApi.Data
{
	public class CitybikeDBContext : DbContext
	{
        public CitybikeDBContext(DbContextOptions<CitybikeDBContext> options)
        : base(options)
        {
        }

        public DbSet<CitybikeApi.Models.Station> Station { get; set; } = default!;
        public DbSet<CitybikeApi.Models.Station> StationByName { get; set; } = default!;
        public DbSet<CitybikeApi.Models.Station> StationById { get; set; } = default!;
    }
}


