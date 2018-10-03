using Newtonsoft.Json;
using OnboardingProjectFinal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnboardingProjectFinal.Controllers
{
    public class SaleController : Controller
    {
        OnboardingEntities db = new OnboardingEntities();
        // GET: Sale
        public ActionResult Index()
        {
            List<Product> ProList = db.Products.ToList();
            ViewBag.ListOfProducts = new SelectList(ProList, "Id", "Name");

            List<Customer> CustList = db.Customers.ToList();
            ViewBag.ListOfCustomers = new SelectList(CustList, "Id", "Name");

            List<Store> StoreList = db.Stores.ToList();
            ViewBag.ListOfStores = new SelectList(StoreList, "Id", "Name");
            return View();
        }

        //select function 
        public JsonResult GetSaleList()
        {
            List<SaleViewModel> SaleList = db.ProductSolds.Select(x => new SaleViewModel
            {
                Id = x.Id,
                ProductName = x.Product.Name,
                CustomerName = x.Customer.Name,
                StoreName = x.Store.Name,
                DataSold = x.DataSold
            }).ToList();
            return Json(SaleList, JsonRequestBehavior.AllowGet);
        }

        //Edit page select 
        public JsonResult GetSaleRecordById(int SaleId)
        {
            ProductSold model = db.ProductSolds.Where(x => x.Id == SaleId).SingleOrDefault();
            string value = string.Empty;
            value = JsonConvert.SerializeObject(model, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            return Json(value, JsonRequestBehavior.AllowGet);

        }
        //insert and update data into database 
        public JsonResult SaveDataInDatabase(SaleViewModel model)
        {
            var result = false;
            try
            {
                //check if the input is null, return "re"
                if (model.ProductId == null || model.CustomerId == null || model.StoreId == null || model.DataSold == null)
                {
                    return Json("re", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    //if id >0, update 
                    if (model.Id > 0)
                    {
                        ProductSold prosold = db.ProductSolds.SingleOrDefault(x => x.Id == model.Id);

                        prosold.ProductId = model.ProductId;
                        prosold.CustomerId = model.CustomerId;
                        prosold.StoreId = model.StoreId;
                        prosold.DataSold = model.DataSold;
                        db.SaveChanges();
                        result = true;
                    }
                    else
                    {
                        // insert

                        //check if the customer is already existing  
                        ProductSold prosold = new ProductSold();
                        prosold.ProductId = model.ProductId;
                        prosold.CustomerId = model.CustomerId;
                        prosold.StoreId = model.StoreId;
                        prosold.DataSold = model.DataSold;
                        db.ProductSolds.Add(prosold);
                        db.SaveChanges();
                        result = true;
                    }
                }
                return Json(result, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                throw ex;
            }


        }
        //delete the data 
        public JsonResult DeleteSaleRecord(int SaleId)
        {
            bool result = false;
            ProductSold prosold = db.ProductSolds.SingleOrDefault(x => x.Id == SaleId);
            if (prosold != null)
            {
                db.ProductSolds.Remove(prosold);
                db.SaveChanges();
                result = true;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}