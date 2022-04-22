using CinemaApp.Data;
using CinemaApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationsController : ControllerBase
    {

        private CinemaDbContext _dbContext;

        public ReservationsController(CinemaDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [Authorize]
        [HttpPost("[action]")] //api/reservations/bookmovie
        public IActionResult BookMovie(Reservation reservationObj)
        {
            //pass the current dateTime from backend 
            reservationObj.ReservationTime = DateTime.Now;
            _dbContext.Reservations.Add(reservationObj);
            _dbContext.SaveChanges();
            return StatusCode(StatusCodes.Status201Created);
        }

        [Authorize (Roles ="Admin")]
        [HttpGet("[action]")] //api/reservations/getReservations
        public IActionResult GetReservations()
        {
            var reservations = from reservation in _dbContext.Reservations
                               join customer in _dbContext.Users on reservation.UserId equals customer.Id
                               join movie in _dbContext.Movies on reservation.MovieId equals movie.Id
                               select new
                               {
                                   Id = reservation.Id,
                                   ReservationTime = reservation.ReservationTime,
                                   CustomerName = customer.Name,
                                   MovieName = movie.Name
                               };
            return Ok(reservations);
        }

        [Authorize(Roles ="Admin")]
        [HttpGet("{id}")] //api/reservations/2
        public IActionResult GetReservationDetail(int id)
        {
            //  var getReservation =  _dbContext.Reservations.FirstOrDefault(e => e.Id == id);
            var getReservation = (from reservation in _dbContext.Reservations
                                  join customer in _dbContext.Users on reservation.UserId equals customer.Id
                                  join movie in _dbContext.Movies on reservation.MovieId equals movie.Id
                                  where reservation.Id == id
                                  select new
                                  {
                                      Id = reservation.Id,
                                      ReservationTime = reservation.ReservationTime,
                                      CustomerName = customer.Name,
                                      MovieName = movie.Name,
                                      Email = customer.Email,
                                      Qty = reservation.Qty, 
                                      Price = reservation.Price,
                                      Phone = reservation.Phone,
                                      PlayingDate = movie.PlayingDate,
                                      PlayingTime = movie.PlayingTime
                                  }).FirstOrDefault() ;  //its paranthezied so as to return it as an Object i.e. {}
            if (getReservation == null)
            {
                return NotFound();
            }
            return Ok(getReservation);     
        }

        //api/reservations/1
        [Authorize(Roles ="Admin")]
        [HttpDelete("{id}")] 
        public IActionResult DeleteReservation(int id)
        {
            var reservation = _dbContext.Reservations.Find(id);
            if (reservation == null)
            {
                return NotFound();
            }
            else
            {
                _dbContext.Reservations.Remove(reservation);
                _dbContext.SaveChanges();
                return Ok("Record Deleted Successfully");
            }
           
        }







    }

}
