﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaApp.Models
{
    public class Movie
    {
        public int Id { get; set; }

        [Required(ErrorMessage ="Name Cannot Be Null or Empty ")]
        public string Name { get; set; }
        public string Description { get; set; }
        public string Language { get; set; }
        public string Duration { get; set; }
        public DateTime PlayingDate { get; set; }
        public DateTime PlayingTime { get; set; }
        public double TicketPrice { get; set; }
        public double Rating { get; set; }
        public string Genre { get; set; }
        public string TrailorUrl { get; set; }
        public string ImageUrl { get; set; }

        [NotMapped]  //This means when running add-migration ...this column should not be include in the DB
        public IFormFile Image { get; set; }

        public ICollection<Reservation> Reservations { get; set; }

    }


}
