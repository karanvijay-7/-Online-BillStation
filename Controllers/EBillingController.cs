using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EBilling.Models;
using EBilling.Repository;

namespace EBilling.Controllers
{
    public class EBillingController : Controller
    {
        // GET: EBilling
        public ActionResult Index()
        {
            Data data = new Data();
            var list = data.GetAllDetail();

            return View(list);
        }
        public ActionResult create() 
        {
            return View();
        }
        [HttpPost]

        public ActionResult create(BillDetail details)
        {
            Data data = new Data();
            data.saveBillingDetails(details);
            ModelState.Clear();
            return View();
        }
        
        public ActionResult createItem(Items item) 
        {
            return PartialView("_CreateItem",item);
        }
        public ActionResult ViewBill(int Id) 
        {
            Data data = new Data();
            var details = data.GetDetail(Id);  
            return View(details);
        }
    }
}