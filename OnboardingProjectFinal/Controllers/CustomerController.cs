using Newtonsoft.Json;
using OnboardingProjectFinal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnboardingProjectFinal.Controllers
{
    public class CustomerController : Controller
    {
        OnboardingEntities db = new OnboardingEntities();
        // GET: Customer
        public ActionResult Index()
        {
            return View();
        }

        //select function 
        public JsonResult GetCustomerList()
        {
            List<CustomerViewModel> SaleList = db.Customers.Select(x => new CustomerViewModel
            {
                Id = x.Id,
                Name = x.Name,
                Address = x.Address

            }).ToList();
            return Json(SaleList, JsonRequestBehavior.AllowGet);
        }

        //Edit page select 
        public JsonResult GetCustomerRecordById(int CustomerId)
        {
            Customer model = db.Customers.Where(x => x.Id == CustomerId).SingleOrDefault();
            string value = string.Empty;
            value = JsonConvert.SerializeObject(model, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            return Json(value, JsonRequestBehavior.AllowGet);

        }

        ////insert and update data into database 
        //public JsonResult EditCustomerRecord(CustomerViewModel model)
        //{
        //    var result = false;
        //    try
        //    {
        //        //if id >0, update 
        //        if (model.Id > 0)
        //        {
        //            Customer cust = db.Customers.SingleOrDefault(x => x.Id == model.Id);

        //            cust.Name = model.Name;
        //            cust.Address = model.Address;

        //            db.SaveChanges();
        //            result = true;
        //        }
        //        else
        //        {
        //            //insert 

        //            //check if the product is already existing 
        //            Customer cust = new Customer();
        //            cust.Name = model.Name;
        //            cust.Address = model.Address;
        //            db.Customers.Add(cust);
        //            db.SaveChanges();
        //            result = true;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //    return Json(result, JsonRequestBehavior.AllowGet);
        //}

        //insert and update data into database 
        public JsonResult SaveDataInDatabase(CustomerViewModel model)
        {
            var result = false;
            try
            {
                //if id >0, update 
                if (model.Id > 0)
                {
                    Customer cust = db.Customers.SingleOrDefault(x => x.Id == model.Id);
                    ////check if the customer name is already existing  
                    Customer custvali = db.Customers.SingleOrDefault(x => x.Name == model.Name && x.Id != model.Id);
                    if (custvali == null)
                    {
                        cust.Name = model.Name;
                        cust.Address = model.Address;

                        db.SaveChanges();
                        result = true;

                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json("no", JsonRequestBehavior.AllowGet);
                    }
                      
                }
                else
                {
                    // insert

                    //check if the customer is already existing  
                    Customer custvali = db.Customers.SingleOrDefault(x => x.Name == model.Name);
                    if (custvali == null)
                    {
                        Customer cust = new Customer();
                        cust.Name = model.Name;
                        cust.Address = model.Address;

                        db.Customers.Add(cust);
                        db.SaveChanges();
                        result = true;

                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json("no", JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }

        //delete the data 
        public JsonResult DeleteCustomerRecord(int CustomerId)
        {
            bool result = false;
            Customer cust = db.Customers.SingleOrDefault(x => x.Id == CustomerId);
            ProductSold custsold = db.ProductSolds.FirstOrDefault(x => x.CustomerId == CustomerId);

            // check this customer data is existing and not be used in the productsold table  
            if (cust != null && custsold == null)
            {
                db.Customers.Remove(cust);
                db.SaveChanges();
                result = true;
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else if (custsold != null)
            {
                return Json("use", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(result, JsonRequestBehavior.AllowGet);
            }


        }
    }
}