using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RoleProject.Models;

namespace RoleProject.Controllers
{
    [Authorize(Roles ="Admin")]
    public class AgincesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();



        [AllowAnonymous]
        // GET: Aginces
        public ActionResult List_Of_All()
        {
            return View(db.Agince.ToList());
        }

        public ActionResult sorting()
        {
            return PartialView("_Sorting_Agince_Partial");
        }
        [AllowAnonymous]
        public ActionResult Go_sorting(int? num)
        {


            if (num == null)
            {

                return View("List_Of_All", db.Agince.ToList());


            }
            else

            {

                var agince = new List<Agince>();
                switch (num)
                {

                    case 1:
                        agince = db.Agince.OrderBy(e => e.name).ToList();
                        break;
                    case 2:
                        agince = db.Agince.OrderBy(e => e.city).ToList();
                        break;
                    case 3:
                        agince = db.Agince.OrderBy(e => e.street).ToList();
                        break;


                }



                return View("List_Of_All", agince);
            }
        }


        public ActionResult Search(string searchItem)
        {



            return PartialView("_Search_Aginces_Partial");
        }
        [AllowAnonymous]
     
        public ActionResult Go_Search(string searchItem)
        {

            var c = db.Users.FirstOrDefault(v => v.Id == searchItem);
            if (c == null) {
                return View("SearchError");// go to error page

            }
            else
            return View("Details", c);
        }



        [AllowAnonymous]
        // GET: Aginces/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Agince agince = db.Agince.Find(id);
            if (agince == null)
            {
                return HttpNotFound();
            }
            return View(agince);
        }

        //// GET: Aginces/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: Aginces/Create

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create( Agince agince)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        /*Add photo to Data Base*/
        //        string filename = Path.GetFileNameWithoutExtension(agince.photo_path.FileName);
        //        string Extintion = Path.GetExtension(agince.photo_path.FileName);
        //        filename = filename + DateTime.Now.ToString("yymmssfff") + Extintion;
        //        agince.photo_Agince = filename;
        //        filename = Path.Combine(Server.MapPath("~/images/"), filename);
        //        agince.photo_path.SaveAs(filename);
        //        //----------------------------//

        //        db.Agince.Add(agince);
        //        db.SaveChanges();
        //        return RedirectToAction("List_Of_All");
        //    }

        //    return View(agince);
        //}
        [AllowAnonymous]

        // GET: Aginces/complete_data
        public ActionResult complete_data(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Agince agince = db.Agince.Find(id);
            if (agince == null)
            {
                return HttpNotFound();
            }
            return View(agince);
        }

        // POST: Aginces/complete_data

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult complete_data(String id, Agince agince)
        {

            try
            {
                var newAgince = db.Agince.FirstOrDefault(agince_ => agince_.Agince_ID == id);

                if (agince.photo_path != null)
                {
                   
                    /*Add photo to Data Base*/
                    string filename = Path.GetFileNameWithoutExtension(agince.photo_path.FileName);
                    string Extintion = Path.GetExtension(agince.photo_path.FileName);
                    filename = filename + DateTime.Now.ToString("yymmssfff") + Extintion;
                    newAgince.photo_Agince = filename;
                    filename = Path.Combine(Server.MapPath("~/images/"), filename);
                    agince.photo_path.SaveAs(filename);
                    //----------------------------/
                }
                db.SaveChanges();
                return RedirectToAction("login","Account");
            }
            catch
            {
                return View("complete_data");
            }
        }

        [AllowAnonymous]
        // GET: Aginces/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Agince agince = db.Agince.Find(id);
            if (agince == null)
            {
                return HttpNotFound();
            }
            return View(agince);
        }

        // POST: Aginces/Edit/5
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(String id, Agince agince)
        {

            try
            {
                var newAgince = db.Agince.FirstOrDefault(agince_ => agince_.Agince_ID == id);

                if (agince.photo_path != null)
                {
                    newAgince.name = agince.name;
                    newAgince.phone_number = agince.phone_number;
                    /*Add photo to Data Base*/
                    string filename = Path.GetFileNameWithoutExtension(agince.photo_path.FileName);
                    string Extintion = Path.GetExtension(agince.photo_path.FileName);
                    filename = filename + DateTime.Now.ToString("yymmssfff") + Extintion;
                    newAgince.photo_Agince = filename;
                    filename = Path.Combine(Server.MapPath("~/images/"), filename);
                    agince.photo_path.SaveAs(filename);
                    //----------------------------/
                }
                db.SaveChanges();
                return RedirectToAction("List_Of_All");
            }
            catch
            {
                return View("Edit");
            }
        }

        // GET: Aginces/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Agince agince = db.Agince.Find(id);
            if (agince == null)
            {
                return HttpNotFound();
            }
            return View(agince);
        }

        // POST: Aginces/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Agince agince = db.Agince.Find(id);
            db.Agince.Remove(agince);
            db.SaveChanges();
            return RedirectToAction("List_Of_All");
        }

      
    }
}
