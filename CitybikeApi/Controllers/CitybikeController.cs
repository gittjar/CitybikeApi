﻿using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using CitybikeApi.Data;
using CitybikeApi.Models;

namespace CitybikeApi.Controllers

{
    // API Route Stations by name
    [Route("api/[controller]")]
    [ApiController]
    public class StationsByNameController : ControllerBase
    {
        private readonly CitybikeDBContext _context;

        public StationsByNameController(CitybikeDBContext context)
        {
            _context = context;
        }

        // GET: api/StationsByName
        // Text search works for example = https://localhost:7297/api/StationsByName?text=rautatie
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Station>>> GetStations(string text)
        {
            if (_context.StationByName == null)
            {
                return NotFound();
            }

            if (!string.IsNullOrEmpty(text))
            {
                return await _context.StationByName
                    .Where(s => s.Nimi.Contains(text))
                    .ToListAsync();
            }

            return await _context.StationByName.ToListAsync();

            // return await _context.Station.ToListAsync();
        }
    }
        // ALL STATIONS
        [Route("api/[controller]")]
        [ApiController]
        public class StationsController : ControllerBase
        {
            private readonly CitybikeDBContext _context;

            public StationsController(CitybikeDBContext context)
            {
                _context = context;
            }

            // GET: api/Stations
            [HttpGet]
            public async Task<ActionResult<IEnumerable<Station>>> GetStations()
            {
                if (_context.StationByName == null)
                {
                    return NotFound();
                }
                return await _context.StationByName.ToListAsync();

            }

            private bool StationExists(int id)
            {
                return (_context.Station?.Any(e => e.FID == id)).GetValueOrDefault();
            }
        }
    }



