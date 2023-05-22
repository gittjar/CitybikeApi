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

    public class CitybikeTripsMay2021Controller : ControllerBase
	{
		private CitybiketripsMay2021DBContext _context;


		public CitybikeTripsMay2021Controller(CitybiketripsMay2021DBContext context)
		{
			_context = context;
		}

        // GET: api/CitybikeTripsMay2021
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BiketripsMay2021>>> GetBiketripsMay2021(int pageNumber = 1, int pageSize = 10)
        {
            if (_context.BiketripsMay2021 == null)
            {
                return NotFound();
            }

            var skip = (pageNumber - 1) * pageSize;
            var take = pageSize;

            var totalTrips = await _context.BiketripsMay2021.CountAsync();

            var biketripsmay2021 = await _context.BiketripsMay2021
                .OrderBy(c => c.Departure)
              //  .ThenBy(c => c.Return)
                .Distinct()
                .Skip(skip)
                .Take(take)
                .ToListAsync();

            var result = new
            {
                TotalItems = totalTrips,
                PageNumber = pageNumber,
                PageSize = pageSize,
                Data = biketripsmay2021
            };

            return Ok(result);



          //  return await _context.BiketripsMay2021.ToListAsync();

        }
        /*
        private bool BiketripsMay2021Exists(int id)
        {
            return (_context.BiketripsMay2021?.Any(e => e.Duration_sec == id)).GetValueOrDefault();
        }
        */



    }
}

