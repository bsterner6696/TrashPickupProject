using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TrashPickupProject.Models
{
    public class Customer
    {

        [Key]
        public int Id { get; set; }

        public int Zipcode { get; set; }
        public string StreetAddress { get; set; }
        public string Name { get; set; }
        public string DayOfPickup { get; set; }
        public double Balance { get; set; }

        [ForeignKey("ApplicationUser")]
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}