using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RoleProject.Models;


    public class Dates_For_Car
    {
       
        public ICollection<DateTime> Start_Recive { get; set; }
        public ICollection<DateTime> End_Recive { get; set; }
        public ICollection<Client> Clients { get; set; }
    }
