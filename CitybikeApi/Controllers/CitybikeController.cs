using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using CitybikeApi.Data;
using CitybikeApi.Models;
using static System.Net.Mime.MediaTypeNames;

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
        // with text --> https://localhost:7297/api/Stationbyname/rautatie
        [HttpGet("{text}")]
        public async Task<ActionResult<IEnumerable<Station>>> GetStations(string text)
        {
            if (_context.StationByName == null)
            {
                return NotFound();
            }
  
                return await _context.StationByName
                    .Where(s => s.Nimi.Contains(text))
                    .ToListAsync();      
            
           
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


        // GET: api/Stations/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Station>> GetStations(int id)
        {


            if (_context.StationById == null)
            {
                return NotFound();
            }

            return await _context.StationById.FindAsync(id);

        }

    }
}







