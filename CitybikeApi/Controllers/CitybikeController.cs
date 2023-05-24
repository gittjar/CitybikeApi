using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using CitybikeApi.Data;
using CitybikeApi.Models;

namespace CitybikeApi.Controllers

{

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
        public async Task<ActionResult<IEnumerable<Station>>> GetStations(int pageNumber = 1, int pageSize = 30)
        {
            if (_context.Station == null)
            {
                return NotFound();
            }

            var skip = (pageNumber - 1) * pageSize;
            var take = pageSize;


            var totalStations = await _context.Station.CountAsync();

            var citybikestations = await _context.Station
                .OrderBy(c => c.Name)
                .ThenBy(c => c.Osoite)
                .Distinct() // take only 1 item
                .Skip(skip)
                .Take(take)
                .ToListAsync();

            var totalPages = totalStations / 30; // 1 sivu = 30 asemaa

            var result = new
            {
                TotalItems = totalStations,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalPages = totalPages,
                Data = citybikestations,

            };

            return Ok(result);



            // return await _context.Station.ToListAsync();
        }

          private bool StationExists(int id)
           {
               return (_context.Station?.Any(e => e.FID == id)).GetValueOrDefault();
           }
    }
}


