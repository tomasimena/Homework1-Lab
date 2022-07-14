using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace movie_ticket_application.Models
{
    public class ShoppingCart
    {
        public int Id { get; set; }
        public string IdentityUserId { get; set; }

        public IdentityUser IdentityUser { get; set; }

        public List<CartItem> CartItems { get; set; }
    }
}
