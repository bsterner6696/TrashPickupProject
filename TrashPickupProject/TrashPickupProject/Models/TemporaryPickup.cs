using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TrashPickupProject.Models
{
    public class TemporaryPickup
    {

        [Key]
        public int Id { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string DayOfWeek { get; set; }
    }
}