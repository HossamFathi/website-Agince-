using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using RoleProject.Models;
using RoleProject.View_Model;

namespace RoleProject.Controllers
{
    

    public class CarsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Cars
       [AllowAnonymous]
        public ActionResult List_Of_All()
        {
            var cars = new List<Car>();

            if (User.IsInRole("Agince"))
            {
                string id = User.Identity.GetUserId();
                //var agince = new List<Agince>();
                //agince = db.Agince.FirstOrDefault(w => w.Agince_ID == id).Collection_Of_Car;
                cars = (from w in db.Cars
                       where w.Agince_Of_Car.Agince_ID == id
                       select w).ToList();

            }
            else
            {
                

                cars = db.Cars.ToList();
             
            }

            return View(cars);

        }

        //List<int> numbers = new List<int>();

        [AllowAnonymous]
        public ActionResult filter()
        {
         
            return PartialView("_filter_Car", db.Car_properties.ToList());
        }
        [AllowAnonymous]
        public ActionResult go_filter(String prop_name)
        {

            //int prop_id = Convert.ToInt32((from w in db.Car_properties
            //                               where w.proprity_Name == prop_name
            //                               select w.id));


            int prop_id = db.Car_properties.FirstOrDefault(e => e.proprity_Name == prop_name).id;


            var carsIds = (from e in db.Car_And_Properites
                           where e.id == prop_id
                           select e.Car_Id).ToList();

            List<Car> cars = new List<Car>();
            foreach (var item in carsIds)
            {
                Car res = db.Cars.FirstOrDefault(e => e.Car_Id == item);
                cars.Add(res);

            }

            return View("List_Of_All", cars);


        }


        // search
        [AllowAnonymous]
        public ActionResult Search(/*string searchItem*/) {

  

            return PartialView("_Search_Car_Partial");
        }
        [AllowAnonymous]

        public ActionResult Go_Search(string searchItem)/*, int? num)*/
        {
            //int Chassis_No = Convert.ToInt32(searchItem);
            List<Car> cars = db.Cars.Where(x => x.Type_Of_Car.Contains(searchItem) || x.Car_Brand.Contains(searchItem) || x.Car_Model.Contains(searchItem) /*|| x.Chassis_No.Contains(searchItem)*/).ToList();
            return View("List_Of_All", cars);
            #region OLDcODE
            /*
             var cars = new List<Car>();
             var car2 = new Car();
             if (num == 1)
             {
                 cars = (from n in db.Cars
                         where n.Type_Of_Car.Contains(searchItem)
                         select n).ToList();
                 return View("List_Of_All", cars);
             }

             else if (num == 2)
             {
                 cars = (from n in db.Cars
                         where n.Car_Model.Contains(searchItem)
                         select n).ToList();
                 return View("List_Of_All", cars);
             }

             else if (num == 3)
             {
                 cars = (from n in db.Cars
                         where n.Car_Brand.Contains(searchItem)
                         select n).ToList();
                 return View("List_Of_All", cars);
             }

             else if (num == 4)
             {
                 int Chassis_No = Convert.ToInt32(searchItem);
                 car2 = (db.Cars.FirstOrDefault(e => e.Chassis_No == Chassis_No));
                 return View("List_Of_All", car2);
             }
            else
             {
                 return View("Car_Not_Found");


             }
            */
            #endregion
        }




        //sorting
        [AllowAnonymous]

        public ActionResult sorting()
        {
            return PartialView("_Sorting_Partial");
        }
        [AllowAnonymous]

        public ActionResult Go_sorting(int? num)
        {


            if (num == null) {

                return View("List_Of_All", db.Cars.ToList());


            }
            else

            {

                var cars = new List<Car>();
                switch (num) {

                    case 1:
                        cars = db.Cars.OrderBy(e => e.price_in_day).ToList();
                        break;
                    case 2:
                        cars = db.Cars.OrderBy(e => e.Car_Brand).ToList();
                        break;
                    case 3:
                        cars = db.Cars.OrderBy(e => e.Car_Model).ToList();
                        break;


                }



                return View("List_Of_All", cars);
            }

        }

      

        // GET: Cars/Details/5
        [AllowAnonymous]

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Car car = db.Cars.FirstOrDefault(Car_ => Car_.Car_Id == id);
            cars_and_properties cars_And_Properties = new cars_and_properties() ;
            cars_And_Properties.Property_Name = 
                (from car_ in db.Car_And_Properites
                where(car_.Cars.Car_Id == car.Car_Id)
                select(
                car_.properties.proprity_Name        
                       )).ToList();

            Dates_For_Car Date = new Dates_For_Car();
            Date.Start_Recive = (from Dates in db.ReciveDates
                                 where (Dates.cars.Car_Id == car.Car_Id)
                                 select
                                 (
                                    Dates.Start_Recive_Date
                                     )).ToList();
            Date.End_Recive = (from Dates in db.ReciveDates
                                 where (Dates.cars.Car_Id == car.Car_Id)
                                 select
                                 (
                                    Dates.End_Recive_Date
                                     )).ToList();
            Date.Clients = (from Dates in db.ReciveDates
                                 where (Dates.cars.Car_Id == car.Car_Id)
                                 select
                                 (
                                    Dates.client
                                     )).ToList();
            Agince agince = (from ca in db.Cars
                             from ag in db.Agince
                             where (ag.Agince_ID == ca.Agince_Of_Car.Agince_ID)
                             select (ag)).FirstOrDefault();

            ViewBag.Agince = agince;
            ViewBag.list_of_Recived_Date = Date;
            ViewBag.list_of_properties = cars_And_Properties;
            
            if (car == null)
            {
                return HttpNotFound();
            }
            return View(car);
        }



        // GET: Cars/Create
        //[Authorize(Roles = "Agince")]
        [Authorize(Roles = "Agince")]
        public ActionResult Create()
        {
         
            return View();
        }

        // POST: Cars/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Agince")]

        public ActionResult Create(Car car)
        {
            try
            {
                string Agince_ID = User.Identity.GetUserId();
                Agince agince = db.Agince.FirstOrDefault(agince_ => agince_.Agince_ID == Agince_ID);
                if (car.photo_path != null)
                {
                    string filename = Path.GetFileNameWithoutExtension(car.photo_path.FileName);
                    string Extintion = Path.GetExtension(car.photo_path.FileName);
                    filename = filename + DateTime.Now.ToString("yymmssfff") + Extintion;
                    car.photo_Car = filename;
                    filename = Path.Combine(Server.MapPath("~/images/"), filename);
                    car.photo_path.SaveAs(filename);
                }
                car.Agince_Of_Car = agince;
                agince.Collection_Of_Car.Add(car);
                db.Cars.Add(car);
                if (agince.Agince_ID !=  null)
                {
                    car.Agince_Of_Car = agince;
                }
                
                db.SaveChanges();
                return RedirectToAction("List_Of_All", "Car_properties", car);
            }
            catch
            {
                return View(car);
            }
        }
        [Authorize(Roles = "Agince")]
        public ActionResult Add_properity(int? id)
        {
            Car car;
           
                car = TempData["car_called"] as Car;
          
                var properity = db.Car_properties.FirstOrDefault(prop => prop.id == id);
            Car_And_Properites car_And_Properites = new Car_And_Properites();
            car_And_Properites.Car_Id = car.Car_Id;
            car_And_Properites.properties = properity;
            db.Car_And_Properites.Add(car_And_Properites);

            db.SaveChanges();
            
            return RedirectToAction("List_Of_all","Car_properties",car);
        }


        [Authorize(Roles = "Agince")]

        // GET: Cars/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Car car = db.Cars.Find(id);
            if (car == null)
            {
                return HttpNotFound();
            }
            return View(car);
        }
        // POST: Cars/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Agince")]
        public ActionResult Edit(int? id,Car car)
        {

            try
            {
                //return car to edit
                var Car_Edititing = db.Cars.FirstOrDefault(car_ => car_.Car_Id == id);
                // update data in car
                Car_Edititing.Car_Brand = car.Car_Brand;
                Car_Edititing.Car_Model = car.Car_Model;
                Car_Edititing.Chassis_No = car.Chassis_No;
                Car_Edititing.price_in_day= car.price_in_day;
               


                if (car.photo_path != null)
                {
                    //to update photo-------------------------------------------------
                    string filename = Path.GetFileNameWithoutExtension(car.photo_path.FileName);
                    string Extintion = Path.GetExtension(car.photo_path.FileName);
                    filename = filename + DateTime.Now.ToString("yymmssfff") + Extintion;
                    Car_Edititing.photo_Car = filename;
                    filename = Path.Combine(Server.MapPath("~/images/"), filename);
                    car.photo_path.SaveAs(filename);
                    //-----------------------------------------------------------------
                }
                
                // save update
                db.SaveChanges();
                return RedirectToAction("List_Of_All", "Car_properties", Car_Edititing);
            }
            catch
            {

                return View(car);
            }
        }
        [Authorize(Roles = "Agince")]
        // GET: Cars/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Car car = db.Cars.Find(id);
            if (car == null)
            {
                return HttpNotFound();
            }
            return View(car);
        }
        // POST: Cars/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Agince")]
        public ActionResult DeleteConfirmed(int id)
        {
            Car car = db.Cars.Find(id);
            db.Cars.Remove(car);
            db.SaveChanges();
            return RedirectToAction("List_Of_All");
        }
        [Authorize(Roles ="Client")]
        public ActionResult Recive(int? id)
        {
            var Cars_Recived = db.Cars.FirstOrDefault(car => car.Car_Id == id);
            if (Cars_Recived != null)
            {
                
                
                    return View(Cars_Recived);// display View of recive
     
                 
                
            }
            else
            {
                return View("Car_Not_Found"); // dispaly Cars is Not Fount ;

            }
        }
        // POST: Cars/Edit/5


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Client,Agince")]
        public ActionResult Recive(int? Car_Id, DateTime Start_Recive_Date, DateTime End_Recive_Date)
        {
            
                /*check by Cars number*/
                var Cars_Recived = db.Cars.FirstOrDefault(car => car.Car_Id == Car_Id);
                ReciveDate reciveDate = new ReciveDate();
                reciveDate.Start_Recive_Date = Start_Recive_Date;
                reciveDate.End_Recive_Date = End_Recive_Date;
                reciveDate.cars = Cars_Recived;
                Dates_For_Car Date = new Dates_For_Car();
                Date.Start_Recive = (from Dates in db.ReciveDates
                                     where (Dates.cars.Car_Id == Cars_Recived.Car_Id)
                                     select
                                     (
                                        Dates.Start_Recive_Date
                                         )).ToList();
                Date.End_Recive = (from Dates in db.ReciveDates
                                     where (Dates.cars.Car_Id == Cars_Recived.Car_Id)
                                     select
                                     (
                                        Dates.End_Recive_Date
                                         )).ToList();



                foreach (var start in Date.Start_Recive)
                {
                    foreach (var end in Date.End_Recive)
                    {
                        if ((Start_Recive_Date >= start) && (End_Recive_Date <= end))
                        {
                            return View("Recive_Alredy_Done", Cars_Recived);//display Cars is Elredy recirved}
                        }
                        else
                        {
                            continue;
                        }
                    }

                }

              
                string client_id = User.Identity.GetUserId();
                Client CLIENT = db.Client.FirstOrDefault(client_ => client_.Client_ID == client_id);
                reciveDate.client = CLIENT;
                Calc_price(reciveDate);
                Cars_Recived.reciveDates.Add(reciveDate);
                CLIENT.Booked_Car.Add(reciveDate);
                db.SaveChanges();
                ViewBag.startDate = Start_Recive_Date;
                ViewBag.endDate = End_Recive_Date;
                ViewBag.client = CLIENT.Name;


                return View("Displa_Reciving", Cars_Recived);// display confirm for client recived done
            
           


        }
        [Authorize(Roles = "Client,Agince")]
        public ActionResult DetailsCancel(int? id) {
            ViewBag.CarID = id;
            return View();
        }
        [Authorize(Roles = "Client,Agince")]

        public ActionResult CancelReciveFirst(int? id, DateTime Start_Recive_Date)
        {
            var Cars_Recived = db.Cars.FirstOrDefault(car => car.Car_Id == id);
            if (Cars_Recived != null)
            {
                string client_id = User.Identity.GetUserId();
                Client CLIENT = db.Client.FirstOrDefault(client_ => client_.Client_ID == client_id);
                ReciveDate reciveDate = db.ReciveDates.Where(ReciveDate => ReciveDate.Start_Recive_Date == Start_Recive_Date && ReciveDate.client.Client_ID == CLIENT.Client_ID).FirstOrDefault();
                
                if (reciveDate != null)
                {
                    ViewBag.ReciveDate = reciveDate;
                    return View("CancelRecive",Cars_Recived);// display View of recive
                }
            else
                {
                    return View("Recive_Not_Done_Yet", Cars_Recived);//display Cars is not recirved yet
                }
            }
            else
            {
                return View("Car_Not_Found"); // dispaly Cars is Not Fount 
            }
        }

        // POST: Cars/CancelRecive/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Client,Agince")]

        public ActionResult CancelRecive(ReciveDate reciveDate)
        {
            try
            {
                reciveDate = TempData["ReciveDate"] as ReciveDate ;
                ReciveDate Rea_reciveDate = (from reciveDate_ in db.ReciveDates
                                            where (reciveDate_.id == reciveDate.id)
                                            select (
                                            reciveDate_
                                            )).FirstOrDefault();
                Car Cars_Recived = (from reciveDate_ in db.ReciveDates
                                   where (reciveDate_.id == reciveDate.id)
                                   select (
                                   reciveDate_.cars
                                   )).FirstOrDefault();
                //Car Cars_Recived = db.Cars.FirstOrDefault(car => car.Car_Id == reciveDate.cars.Car_Id);

                    if (Rea_reciveDate != null)
                    {

                        if (Rea_reciveDate.Start_Recive_Date <= DateTime.Now)
                        {
                        Rea_reciveDate.End_Recive_Date = DateTime.Now;
                            Calc_price(Rea_reciveDate);
                        
                            db.SaveChanges();
                            return View("bill", Cars_Recived);
                        }
                        else
                        {
                        Rea_reciveDate.Start_Recive_Date = DateTime.Now;
                        Rea_reciveDate.End_Recive_Date = DateTime.Now;
                        db.SaveChanges();
                            return RedirectToAction("List_of_all");//display Cancel is Done with no cost 

                        }
                    }
                    else
                    {
                        return View("Recive_Not_Done_Yet", Cars_Recived);//display Cars is not recirved yet
                    }


            }
            catch
            {
                return View("Car_Not_Found"); // dispaly Cars is Not Fount 
            }
            }


        [Authorize(Roles = "Client,Agince")]

        public void Calc_price(ReciveDate reciveDate)
        {
            int days_is_recived = reciveDate.End_Recive_Date.Subtract(reciveDate.Start_Recive_Date).Days;
            reciveDate.Total_Cost = days_is_recived * reciveDate.cars.price_in_day;

        }
    }
}