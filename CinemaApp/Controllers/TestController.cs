using CinemaApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
       //NB: setting this piece of class makes it avaible through out this controller...
        private static List<Movie> movies = new List<Movie>
        {
            new Movie(){Id = 0, Name="Mission Impossible", Language="English"},
            new Movie(){Id = 1, Name="Batman", Language="French" },
        };

        [HttpGet]
        public IEnumerable<Movie> Get()
        {
            return movies;
        }

        [HttpPost]
        public void Post([FromBody] Movie movie)
        {
            movies.Add(movie);
        }

        [HttpPut("id")]
        public void Put(int id, [FromBody] Movie movie)
        {
            movies[id] = movie;
        }

        [HttpDelete("id")]
        public void Delete(int id)
        {
            movies.RemoveAt(id);
        }












    }
}
