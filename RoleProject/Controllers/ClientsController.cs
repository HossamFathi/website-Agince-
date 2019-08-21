using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RoleProject.View_Model;

using RoleProject.Models;

namespace RoleProject.Controllers
{

    //[Authorize(Roles = "Admin")]
    public class ClientsController : Controller
    {

      
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Clinets
       

        public ActionResult List_Of_All()
        {
            return View(db.Client.ToList());
        }

        //Search

        public ActionResult Search(string searchItem)
        {

            return PartialView("_Search_Client_Partial");
        }
  

        public ActionResult Go_Search(string searchItem)
        {

            var c = db.Client.FirstOrDefault(v => v.Client_ID == searchItem);

            if (c == null)
            {
                return View("SearchError");// go to error page

            }
            else
                return View("Details", c);
        }



        public ActionResult sorting()
        {
            return PartialView("_Sorting_clinet_Partial");
        }

        public ActionResult Go_sorting(int? num)
        {


            if (num == null)
            {

                return View("List_Of_All", db.Client.ToList());


            }
            else

            {

                var clinet = new List<Client>();
                switch (num)
                {

                    case 1:
                        clinet = db.Client.OrderBy(e => e.age).ToList();
                        break;
                    case 2:
                        clinet = db.Client.OrderBy(e => e.city).ToList();
                        break;
                    case 3:
                        clinet = db.Client.OrderBy(e => e.street).ToList();
                        break;
                    case 4:
                        clinet = db.Client.OrderBy(e => e.Name).ToList();
                        break;
                    case 5:
                        clinet = db.Client.OrderBy(e => e.date_of_licience_expiry).ToList();
                        break;

                }



                return View("List_Of_All", clinet);
            }
        }
        [AllowAnonymous]
        // GET: Clinets/Details/5
        public ActionResult Details(String id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Recived_Cars_for_Client recived_Cars_For_Client = new Recived_Cars_for_Client();
            Client clinet = db.Client.FirstOrDefault(client_ => client_.Client_ID == id);
          
            //recived_Cars_For_Client.cars = db.Cars.Where(car => car.CLIENT.Client_ID == clinet.Client_ID).ToList();
            ViewBag.recived_Cars_For_Client = recived_Cars_For_Client.cars;
            if (clinet == null)
            {
                return HttpNotFound();
            }
            return View(clinet);
        }

        // GET: Clinets/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: Clinets/Create

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create(Client client)
        //{
        //    if (ModelState.IsValid)
        //    {

        //        /*Add photo to Data Base*/
        //        string filename = Path.GetFileNameWithoutExtension(client.photo_path.FileName);
        //        string Extintion = Path.GetExtension(client.photo_path.FileName);
        //        filename = filename + DateTime.Now.ToString("yymmssfff") + Extintion;
        //        client.photo_Client = filename;
        //        filename = Path.Combine(Server.MapPath("~/images/"), filename);
        //        client.photo_path.SaveAs(filename);
        //        //----------------------------//
        //        db.Client.Add(client);
        //        db.SaveChanges();
        //        return RedirectToAction("List_Of_All");
        //    }

        //    return View(client);
        //}
        [AllowAnonymous]

        // GET: Clinets/Edit/5
        public ActionResult Edit(string id)
        {
            return View(db.Client.Find(id));
        }

        // POST: Clinets/Edit/5
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string Client_ID, Client client)
        {
            try
            {
                var newClient = db.Client.FirstOrDefault(client_ => client_.Client_ID == Client_ID);

                if (client.photo_path != null)
                {

                    /*Add photo to Data Base*/
                    string filename = Path.GetFileNameWithoutExtension(client.photo_path.FileName);
                    string Extintion = Path.GetExtension(client.photo_path.FileName);
                    filename = filename + DateTime.Now.ToString("yymmssfff") + Extintion;
                    newClient.photo_Client = filename;
                    filename = Path.Combine(Server.MapPath("~/images/"), filename);
                    client.photo_path.SaveAs(filename);
                    //----------------------------/
                }
                newClient.age = client.age;
                newClient.city = client.city;
                newClient.date_of_licience_expiry = client.date_of_licience_expiry;
                newClient.Name = client.Name;
                newClient.number_of_licience = client.number_of_licience;
                newClient.street = client.street;
                newClient.phone_Number = client.phone_Number;
               
                db.SaveChanges();
              
               return RedirectToAction("List_Of_All", "Cars"); 

                
            }
            catch
            {
                return View(client);
            }
        }


        [AllowAnonymous]
        // GET: Clients/complete_data
        public ActionResult complete_data(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Client clinet = db.Client.Find(id);
            if (clinet == null)
            {
                return HttpNotFound();
            }
            return View(clinet);
        }

        // Post : Clients/complete_data
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult complete_data(String id, Client clinet)
        {

            try
            {
                var newclinet = db.Client.FirstOrDefault(clinet_ => clinet_.Client_ID == id);

                if (clinet.photo_path != null)
                {

                    /*Add photo to Data Base*/
                    string filename = Path.GetFileNameWithoutExtension(clinet.photo_path.FileName);
                    string Extintion = Path.GetExtension(clinet.photo_path.FileName);
                    filename = filename + DateTime.Now.ToString("yymmssfff") + Extintion;
                    newclinet.photo_Client = filename;
                    filename = Path.Combine(Server.MapPath("~/images/"), filename);
                    clinet.photo_path.SaveAs(filename);
                    //----------------------------/
                }
                newclinet.age = clinet.age;
                newclinet.number_of_licience = clinet.number_of_licience;
                newclinet.date_of_licience_expiry = clinet.date_of_licience_expiry;
                
                db.SaveChanges();
                return RedirectToAction("List_of_all","Cars");
            }
            catch
            {
                return View("complete_data");
            }
        }



        // GET: Clinets/Delete/5
        public ActionResult Delete(String id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Client clinet = db.Client.Find(id);
            if (clinet == null)
            {
                return HttpNotFound();
            }
            return View(clinet);
        }

        // POST: Clinets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(String id)
        {
            Client clinet = db.Client.Find(id);
            db.Client.Remove(clinet);
            db.SaveChanges();
            return RedirectToAction("List_Of_All");
        }

        //public ActionResult Recive(string Client_ID, int Car_Id, DateTime Start_Book_Date, DateTime End_Book_Date)
        //{

            
        //    Client client = db.Client.FirstOrDefault(client_ => client_.Client_ID == Client_ID);

        //    Car Car_Booked = db.Cars.FirstOrDefault(Car_ => Car_.Car_Id == Car_Id);
        //    if (Car_Booked.Is_reseved == true)
        //    {
        //        return View("Recive_Alredy_Done");
        //    }

        //    if (Car_Booked is Car)
        //    {
        //        Car_Booked.CLIENT = client;
        //        client.Booked_Car.Add(Car_Booked);
        //        db.SaveChanges();
        //        return RedirectToAction("TO_Recive", "Cars", new { Car_Id, Start_Book_Date, End_Book_Date });//go to Book Car
        //    }
        //    else
        //        return RedirectToAction("List_Of_all", "Cars");

        //}

        

    }
}


