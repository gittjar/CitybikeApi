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
              // GET: api/CitybikeTripsMay2021/AllStations
        [HttpGet("AllStations")]
        public async Task<ActionResult<IEnumerable<StationData>>> GetAllStations()
        {
            var departureCounts = await _context.BiketripsMay2021
                .GroupBy(t => t.Departure_station_name)
                .Select(g => new StationData { Station = g.Key, DepartureCount = g.Count(), ReturnCount = 0 })
                .ToListAsync();

            var returnCounts = await _context.BiketripsMay2021
                .GroupBy(t => t.Return_station_name)
                .Select(g => new StationData { Station = g.Key, DepartureCount = 0, ReturnCount = g.Count() })
                .ToListAsync();

            var combinedCounts = departureCounts
                .Concat(returnCounts)
                .GroupBy(x => x.Station)
                .Select(g => new StationData
                {
                    Station = g.Key,
                    DepartureCount = g.Sum(x => x.DepartureCount),
                    ReturnCount = g.Sum(x => x.ReturnCount)
                })
                .ToList();

            return Ok(combinedCounts);
        }

            // GET: api/CitybikeTripsMay2021/station/{stationId}
            [HttpGet("station/{stationId}")]
            public async Task<ActionResult<IEnumerable<BiketripsMay2021>>> GetTripsByStationId(int stationId)
            {
                if (_context.BiketripsMay2021 == null)
                {
                    return NotFound();
                }
            
                var trips = await _context.BiketripsMay2021
                    .Where(t => t.Departure_station_id == stationId || t.Return_station_id == stationId)
                    .ToListAsync();
            
                if (trips == null || trips.Count == 0)
                {
                    return NotFound();
                }
            
                return Ok(trips);
            }

        // GET: api/CitybikeTripsMay2021/count
        [HttpGet("count")]
        public async Task<ActionResult<object>> GetTripsCount(string search = "")
        {
            if (_context.BiketripsMay2021 == null)
            {
                return NotFound();
            }

            var query = _context.BiketripsMay2021.AsQueryable();

            // Apply search filter if provided
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(t => t.Departure_station_name.Contains(search) || 
                                        t.Return_station_name.Contains(search) ||
                                        t.Departure.ToString().Contains(search) || 
                                        t.Return.ToString().Contains(search));
            }

            var totalCount = await query.CountAsync();

            var result = new
            {
                TotalTrips = totalCount,
                SearchTerm = search,
                Timestamp = DateTime.UtcNow
            };

            return Ok(result);
        }

        // GET: api/CitybikeTripsMay2021/count/by-station/{stationId}
        [HttpGet("count/by-station/{stationId}")]
        public async Task<ActionResult<object>> GetTripsCountByStation(int stationId)
        {
            if (_context.BiketripsMay2021 == null)
            {
                return NotFound();
            }

            var departureCount = await _context.BiketripsMay2021
                .CountAsync(t => t.Departure_station_id == stationId);

            var returnCount = await _context.BiketripsMay2021
                .CountAsync(t => t.Return_station_id == stationId);

            var totalCount = await _context.BiketripsMay2021
                .CountAsync(t => t.Departure_station_id == stationId || t.Return_station_id == stationId);

            var stationName = await _context.BiketripsMay2021
                .Where(t => t.Departure_station_id == stationId || t.Return_station_id == stationId)
                .Select(t => t.Departure_station_id == stationId ? t.Departure_station_name : t.Return_station_name)
                .FirstOrDefaultAsync();

            var result = new
            {
                StationId = stationId,
                StationName = stationName,
                DepartureTripsCount = departureCount,
                ReturnTripsCount = returnCount,
                TotalTripsCount = totalCount,
                Timestamp = DateTime.UtcNow
            };

            return Ok(result);
        }

        // GET: api/CitybikeTripsMay2021/count/by-date-range
        [HttpGet("count/by-date-range")]
        public async Task<ActionResult<object>> GetTripsCountByDateRange(
            DateTime? startDate = null, 
            DateTime? endDate = null)
        {
            if (_context.BiketripsMay2021 == null)
            {
                return NotFound();
            }

            var query = _context.BiketripsMay2021.AsQueryable();

            if (startDate.HasValue)
            {
                query = query.Where(t => t.Departure >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                query = query.Where(t => t.Departure <= endDate.Value);
            }

            var totalCount = await query.CountAsync();

            var result = new
            {
                TotalTrips = totalCount,
                StartDate = startDate?.ToString("yyyy-MM-dd"),
                EndDate = endDate?.ToString("yyyy-MM-dd"),
                Timestamp = DateTime.UtcNow
            };

            return Ok(result);
        }

        // GET: api/CitybikeTripsMay2021/summary
        [HttpGet("summary")]
        public async Task<ActionResult<object>> GetTripsSummary()
        {
            if (_context.BiketripsMay2021 == null)
            {
                return NotFound();
            }

            var totalTrips = await _context.BiketripsMay2021.CountAsync();
            var avgDuration = await _context.BiketripsMay2021.AverageAsync(t => t.Duration_sec);
            var avgDistance = await _context.BiketripsMay2021.AverageAsync(t => t.Covered_distance_m);
            var maxDuration = await _context.BiketripsMay2021.MaxAsync(t => t.Duration_sec);
            var maxDistance = await _context.BiketripsMay2021.MaxAsync(t => t.Covered_distance_m);
            var minDuration = await _context.BiketripsMay2021.MinAsync(t => t.Duration_sec);
            var minDistance = await _context.BiketripsMay2021.MinAsync(t => t.Covered_distance_m);

            var firstTrip = await _context.BiketripsMay2021.MinAsync(t => t.Departure);
            var lastTrip = await _context.BiketripsMay2021.MaxAsync(t => t.Departure);

            var uniqueStations = await _context.BiketripsMay2021
                .Select(t => t.Departure_station_id)
                .Union(_context.BiketripsMay2021.Select(t => t.Return_station_id))
                .Distinct()
                .CountAsync();

            var result = new
            {
                TotalTrips = totalTrips,
                UniqueStations = uniqueStations,
                DateRange = new
                {
                    FirstTrip = firstTrip,
                    LastTrip = lastTrip
                },
                Duration = new
                {
                    AverageSeconds = Math.Round(avgDuration, 2),
                    AverageMinutes = Math.Round(avgDuration / 60, 2),
                    MaxSeconds = maxDuration,
                    MinSeconds = minDuration
                },
                Distance = new
                {
                    AverageMeters = Math.Round(avgDistance, 2),
                    AverageKilometers = Math.Round(avgDistance / 1000, 2),
                    MaxMeters = maxDistance,
                    MinMeters = minDistance
                },
                Timestamp = DateTime.UtcNow
            };

            return Ok(result);
        }
    }
}