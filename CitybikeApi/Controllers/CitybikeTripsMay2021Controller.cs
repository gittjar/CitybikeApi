using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CitybikeApi.Data;
using CitybikeApi.Models;

namespace CitybikeApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CitybikeTripsMay2021Controller : ControllerBase
    {
        private readonly CitybiketripsMay2021DBContext _context;

        public CitybikeTripsMay2021Controller(CitybiketripsMay2021DBContext context)
        {
            _context = context;
        }

        // GET: api/CitybikeTripsMay2021
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BiketripsMay2021>>> GetBiketripsMay2021(
            int pageNumber = 1, 
            int pageSize = 500, 
            string sortBy = "departure", 
            string sortOrder = "asc", 
            string search = "")
        {
            if (_context.BiketripsMay2021 == null)
            {
                return NotFound();
            }

            var query = _context.BiketripsMay2021.AsQueryable();

            // Apply search filter
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(t => t.Departure.ToString().Contains(search) || t.Return.ToString().Contains(search));
            }

            // Apply sorting
            switch (sortBy.ToLower())
            {
                case "departure":
                    query = sortOrder.ToLower() == "desc" ? query.OrderByDescending(t => t.Departure) : query.OrderBy(t => t.Departure);
                    break;
                case "return":
                    query = sortOrder.ToLower() == "desc" ? query.OrderByDescending(t => t.Return) : query.OrderBy(t => t.Return);
                    break;
                case "duration":
                    query = sortOrder.ToLower() == "desc" ? query.OrderByDescending(t => t.Duration_sec) : query.OrderBy(t => t.Duration_sec);
                    break;
                case "distance":
                    query = sortOrder.ToLower() == "desc" ? query.OrderByDescending(t => t.Covered_distance_m) : query.OrderBy(t => t.Covered_distance_m);
                    break;
                default:
                    query = query.OrderBy(t => t.Departure);
                    break;
            }

            var totalTrips = await query.CountAsync();

            var skip = (pageNumber - 1) * pageSize;
            var take = pageSize;

            var biketripsmay2021 = await query
                .Skip(skip)
                .Take(take)
                .ToListAsync();

            var totalPages = (int)Math.Ceiling(totalTrips / (double)pageSize);

            var result = new
            {
                TotalItems = totalTrips,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalPages = totalPages,
                Data = biketripsmay2021
            };

            return Ok(result);
        }

        // GET: api/CitybikeTripsMay2021/TopDepartureStations
        [HttpGet("TopDepartureStations")]
        public async Task<ActionResult<IEnumerable<object>>> GetTopDepartureStations()
        {
            var topDepartureStations = await _context.BiketripsMay2021
                .GroupBy(t => t.Departure_station_name)
                .Select(g => new { Station = g.Key, Count = g.Count() })
                .OrderByDescending(g => g.Count)
                .Take(10)
                .ToListAsync();

            return Ok(topDepartureStations);
        }

        // GET: api/CitybikeTripsMay2021/TopReturnStations
        [HttpGet("TopReturnStations")]
        public async Task<ActionResult<IEnumerable<object>>> GetTopReturnStations()
        {
            var topReturnStations = await _context.BiketripsMay2021
                .GroupBy(t => t.Return_station_name)
                .Select(g => new { Station = g.Key, Count = g.Count() })
                .OrderByDescending(g => g.Count)
                .Take(10)
                .ToListAsync();

            return Ok(topReturnStations);
        }
    }
}