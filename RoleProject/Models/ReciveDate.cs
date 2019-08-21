using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RoleProject.Models
{
    public class ReciveDate
    {
        public int id { get; set; }
        [Display(Name = "Start Recive Date")]
        public DateTime Start_Recive_Date { get; set; }
        [Display(Name = "End Recive Date")]
        public DateTime End_Recive_Date { get; set; }
        public double Total_Cost { get; set; }
        public Client client { get; set; }
        public Car cars { get; set; }
    }
}