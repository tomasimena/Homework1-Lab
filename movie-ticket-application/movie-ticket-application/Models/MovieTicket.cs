using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace movie_ticket_application.Models
{
    public class MovieTicket
    {
        
        public int Id { get; set; }

        public int MovieId { get; set; }
        public Movie Movie { get; set; }

        public string HallName { get; set; }

        [DataType(DataType.Date)]
        public DateTime ExpirationDate { get; set; }

    }
}
