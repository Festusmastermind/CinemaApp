using CinemaApp.Data;
using CinemaApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private CinemaDbContext _dbContext;
        public MoviesController(CinemaDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        //NB: Authorize Requires A bearer Token and the role(if applicable) to be able to access the endpoints ...

        [Authorize]
        [HttpGet("[action]")]
        public IActionResult AllMovies(string sort, int? pageNumber, int? pageSize)
        {
            //sort is optional ...i.e. it can be null
            //setting defaults for the page Number and pageSize
            var currentPageNumber = pageNumber ?? 1;
            var currentPageSize = pageSize ?? 5; //this gives the first 5 records from the the Db

            var movies = from movie in _dbContext.Movies
                         select new
                         {
                             Id = movie.Id,
                             Name = movie.Name,
                             Duration = movie.Duration,
                             Language = movie.Language,
                             Rating = movie.Rating,
                             Genre = movie.Genre,
                             ImageUrl = movie.ImageUrl
                         };
            //adding sorting feature i.e. either ascending or descending with a specific column in the Db table.\
            //adding paging as well...skip first records(no) and take next records(no)
            switch (sort)
            {
                case "desc":
                    return Ok(movies.Skip((currentPageNumber - 1) * currentPageSize).Take(currentPageSize).OrderByDescending(e => e.Rating));
                case "asc":
                    return Ok(movies.Skip((currentPageNumber - 1) * currentPageSize).Take(currentPageSize).OrderBy(e => e.Rating));
                default:
                    return Ok(movies.Skip((currentPageNumber -1) * currentPageSize).Take(currentPageSize));
            }

        }
        //GET api/movies/moviedetails?id=1  its either this approach when calling the api or the one below...
        //api/movies/moviedetails/1
        [Authorize]
        [HttpGet("[action]/{id}")]
        public IActionResult MovieDetail(int id)
        {
            var movie = _dbContext.Movies.Find(id);
            if(movie == null)
            {
                return NotFound("Record not found");
            }
            return Ok(movie);
        }

        [Authorize]
        [HttpGet("[action]")]  //api/movies/findmovies?movieName=xxxxx...
        public IActionResult FindMovies(string movieName)
        {
            var movies = from movie in _dbContext.Movies
                         where movie.Name.StartsWith(movieName)
                         select new
                         {
                             Id = movie.Id,
                             Name = movie.Name,
                             ImageUrl = movie.ImageUrl
                         };
            return Ok(movies);
        }

        [Authorize (Roles ="Admin")]
        [HttpPost]
        public IActionResult Post([FromForm] Movie movieObj)
        {
            var guid = Guid.NewGuid();
            var filePath = Path.Combine("wwwroot/Images", guid + ".jpg"); //append the jpg format ..irrespective of the fileType coming.
            if (movieObj != null)
            {
                var fileStream = new FileStream(filePath, FileMode.Create); //create a stream and open the file Content 
                movieObj.Image.CopyTo(fileStream); //copy the image into the path specified
            }
            //movieObj.ImageUrl = movieObj.Image.FileName; //to store the Image name only..
            movieObj.ImageUrl = filePath.Remove(0, 14);
            _dbContext.Movies.Add(movieObj);
            _dbContext.SaveChanges();
            return StatusCode(StatusCodes.Status201Created);
        }

        // PUT api/<MoviesController>/5
        [Authorize(Roles ="Admin")]
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromForm] Movie movieObj)
        {
            var movie = _dbContext.Movies.Find(id);
            if (movie == null)
            {
                return NotFound("No record was found against this Id");
            }
            else
            {
                var guid = Guid.NewGuid();
                var filePath = Path.Combine("wwwroot/Images", guid + ".jpg");
                if (movieObj.Image != null)
                {
                    var fileStream = new FileStream(filePath, FileMode.Create);
                    movieObj.Image.CopyTo(fileStream);
                    movie.ImageUrl = filePath.Remove(0, 14);
                  //  movie.ImageUrl = movieObj.Image.FileName; //Get the name of the image only..
                }
                movie.Name = movieObj.Name;
                movie.Description = movieObj.Description;
                movie.Language = movieObj.Language;
                movie.Duration = movieObj.Duration;
                movie.PlayingDate = movieObj.PlayingDate;
                movie.PlayingTime = movieObj.PlayingTime;
                movie.TicketPrice = movieObj.TicketPrice;
                movie.Rating = movieObj.Rating;
                movie.Genre = movieObj.Genre;
                movie.TrailorUrl = movieObj.TrailorUrl;
                _dbContext.SaveChanges();
                return Ok("Record Updated Succefully");
            }

        }

        // DELETE api/<MoviesController>/5
        [Authorize(Roles ="Admin")]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var movie = _dbContext.Movies.Find(id);
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
