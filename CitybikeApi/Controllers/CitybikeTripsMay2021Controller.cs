﻿using System;
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
        public async Task<ActionResult<IEnumerable<BiketripsMay2021>>> GetBiketripsMay2021(int pageNumber = 1, int pageSize = 500)
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
                .ThenBy(c => c.Return)
                .Distinct() // take only 1 item
                .Skip(skip)
                .Take(take)
                .ToListAsync();

            var totalPages = totalTrips / 500; // trips per page = 500

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

        //  return await _context.BiketripsMay2021.ToListAsync();

    }
        /*
        private bool BiketripsMay2021Exists(int id)
        {
            return (_context.BiketripsMay2021?.Any(e => e.Duration_sec == id)).GetValueOrDefault();
        }
        */

        [Route("api/[controller]")]
        [ApiController]

        public class CitybikeTripsController : ControllerBase
        {
            private CitybiketripsMay2021DBContext _context;


            public CitybikeTripsController(CitybiketripsMay2021DBContext context)
            {
                _context = context;
            }

            // GET: api/CitybikeTrips
            [HttpGet]
            public async Task<ActionResult<IEnumerable<BiketripsMay2021>>> GetBiketrips()
            {
                if (_context.Biketrips == null)
                {
                    return NotFound();
                }


                return await _context.Biketrips.ToListAsync();



            }
        }
    
}

