using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DbContext;
using DbModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Models;
using Models.DTO;
using Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AppTravelWApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AttractionController : Controller
    {
        IAttractionService _service;
        ILogger<AttractionController> _logger;

        //GET: api/attraction/seed?count={count}
        [HttpGet()]
        [ActionName("Seed")]
        [ProducesResponseType(200, Type = typeof(int))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> Seed()
        {
            try
            {
                // Generate 50 users, 100 addresses, 1000 attractions, and random comments
                int cnt = await _service.Seed();
                return Ok(cnt);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException?.Message);
            }
        }

        //GET: api/attraction/removeseed
        [HttpGet()]
        [ActionName("RemoveSeed")]
        [ProducesResponseType(200, Type = typeof(string))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> RemoveSeed()
        {
            try
            {
                int _count = await _service.RemoveSeed();
                return Ok(_count);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException?.Message);
            }

        }

        [HttpGet("filter")]
        public async Task<IActionResult> FilterAttractions(
                [FromQuery(Name = "cityName")] string cityName = null,
                [FromQuery(Name = "countryName")] string countryName = null,
                [FromQuery(Name = "category")] string category = null,
                [FromQuery(Name = "description")] string description = null,
                [FromQuery(Name = "attractionName")] string attractionName = null)
        {
            var filteredAttractions = await _service.FilterAttractions(cityName, category, description, attractionName, countryName);

            if (filteredAttractions == null || filteredAttractions.Count == 0)
            {
                return NotFound("No matching attraction found");
            }

            return Ok(filteredAttractions);
        }

        public AttractionController(IAttractionService service, ILogger<AttractionController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet("attractions-with-no-comments")]
        public async Task<IActionResult> GetAttractionsWithNoComments()
        {
            try
            {
                var attractionsWithNoComments = await _service.GetAttractionsWithNoCommentsAsync();
                return Ok(attractionsWithNoComments);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAttractionById(Guid id)
        {
            try
            {
                var attraction = await _service.GetAttractionByIdAsync(id);
                return Ok(attraction);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetAllUsersAsync(
    [FromQuery] int pageSize = 10, // Default page size is 10
    [FromQuery] int pageNumber = 1) // Default page number is 1
        {
            try
            {
                // Calculate the number of items to skip based on page size and number
                int skip = (pageNumber - 1) * pageSize;

                var users = await _service.GetAllUsersAsync(pageSize, skip);
                return Ok(users);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException?.Message);
            }
        }

    }
}

