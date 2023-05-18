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
        public async Task<ActionResult<IEnumerable<BiketripsMay2021>>> GetBiketripsMay2021()
        {
            if (_context.BiketripsMay2021 == null)
            {
                return NotFound();
            }
            return await _context.BiketripsMay2021.ToListAsync();

        }
        /*
        private bool BiketripsMay2021Exists(int id)
        {
            return (_context.BiketripsMay2021?.Any(e => e.Duration_sec == id)).GetValueOrDefault();
        }
        */



    }
}
