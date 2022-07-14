using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace movie_ticket_application.Models
{
    public class Order
    {

        public int Id { get; set; }

        public string IdentityUserId { get; set; }

        public IdentityUser IdentityUser { get; set; }

        public List<CartItem> CartItems { get; set; }

        public decimal TotalPrice { get; set; }

        public DateTime CreatedOn { get; set; }

    }
}
