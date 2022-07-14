using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace movie_ticket_application.Models
{
    public class Movie
    {
        
        public int Id { get; set; }
        public string Title { get; set; }

        public decimal Price { get; set; }
        public string Genre { get; set; }

        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }

        public List<MovieTicket> MovieTickets { get; set; }


    }
}
