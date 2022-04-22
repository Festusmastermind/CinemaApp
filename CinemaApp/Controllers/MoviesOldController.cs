using CinemaApp.Data;
using CinemaApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CinemaApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesOldController : ControllerBase
    {
        private CinemaDbContext _dbContext;

        public MoviesOldController(CinemaDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: api/<MoviesController>
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_dbContext.Movies);
        }

        // GET api/<MoviesController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var movie =  _dbContext.Movies.Find(id);
            if(movie == null)
            {
                return NotFound("No record found against this Id");
            }
            return Ok(movie);
        }

        [HttpGet("[action]/{id}")] //i.e. api/movies/Test/2
        public int Test(int id)
        {
            return id;
        }
        // POST api/<MoviesController>
        /*[HttpPost]
        public IActionResult Post([FromBody] Movie movieObj)
        {
            _dbContext.Movies.Add(movieObj);
            _dbContext.SaveChanges(); // this save the data into the database...
            return StatusCode(StatusCodes.Status201Created);
        }
*/
        //because we are sending image along from a Form....then FromForm is used instead of FromBody 
        [HttpPost]
        public IActionResult Post([FromForm] Movie movieObj)
        {
            var guid = Guid.NewGuid();
            var filePath = Path.Combine("wwwroot/Images", guid + ".jpg"); //append the jpg format ..irrespective of the fileType coming.
            if(movieObj != null)
            {
                var fileStream = new FileStream(filePath, FileMode.Create); //create a stream and open the file Content 
                movieObj.Image.CopyTo(fileStream); //copy the image into the path specified
            }
            movieObj.ImageUrl = filePath.Remove(0, 14);
            _dbContext.Movies.Add(movieObj);
            _dbContext.SaveChanges();
            return StatusCode(StatusCodes.Status201Created); 
        }

        // PUT api/<MoviesController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromForm] Movie movieObj)
        {
            var movie = _dbContext.Movies.Find(id);
            if(movie == null)
            {
                return NotFound("No record was found against this Id");
            }
            else
            {
                var guid = Guid.NewGuid();
                var filePath = Path.Combine("wwwroot/Images", guid + ".jpg");
                if(movieObj.Image != null)
                {
                    var fileStream = new FileStream(filePath, FileMode.Create);
                    movieObj.Image.CopyTo(fileStream);
                    movie.ImageUrl = filePath.Remove(0, 14);
                }
                movie.Name = movieObj.Name;
                movie.Language = movieObj.Language;
                movie.Rating = movieObj.Rating;
                _dbContext.SaveChanges();
                return Ok("Record Updated Succefully");
            }
            
        }

        // DELETE api/<MoviesController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
           var movie =  _dbContext.Movies.Find(id);
            if (movie == null)
            {
                return NotFound("No record found against this Id");
            }
            else
            {
                _dbContext.Remove(movie);
                _dbContext.SaveChanges();
                return Ok("Movie Deleted Successfully");
            }
          
        }







    }
}
