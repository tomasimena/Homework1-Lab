using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace movie_ticket_application.Models
{
    public class CartItem
    {

        public int Id { get; set; }

        public int Quantity { get; set; }

        public int MovieTicketId { get; set; }

        public MovieTicket MovieTicket { get; set; }

        public int ShoppingCartId { get; set; }

        public ShoppingCart ShoppingCart {get; set;}


    }
}
