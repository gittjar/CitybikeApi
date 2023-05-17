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
        public async Task<ActionResult<IEnumerable<Station>>> GetStations()
        {
            if (_context.Station == null)
            {
                return NotFound();
            }
            return await _context.Station.ToListAsync();
        }


          private bool StationExists(int id)
           {
               return (_context.Station?.Any(e => e.FID == id)).GetValueOrDefault();
           }
    }
}


