using Newtonsoft.Json;
using OnboardingProjectFinal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnboardingProjectFinal.Controllers
{
    public class StoreController : Controller
    {
        OnboardingEntities db = new OnboardingEntities();
        // GET: Store
        public ActionResult Index()
        {
            return View();
        }

        //select function 
        public JsonResult GetStoreList()
        {
            List<StoreViewModel> SaleList = db.Stores.Select(x => new StoreViewModel
            {
                Id = x.Id,
                Name = x.Name,
                Address = x.Address

            }).ToList();
            return Json(SaleList, JsonRequestBehavior.AllowGet);
        }

        //Edit page select 
        public JsonResult GetStoreRecordById(int StoreId)
        {
            Store model = db.Stores.Where(x => x.Id == StoreId).SingleOrDefault();
            string value = string.Empty;
            value = JsonConvert.SerializeObject(model, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            return Json(value, JsonRequestBehavior.AllowGet);

        }

        ////insert and update data into database 
        //public JsonResult EditStoreRecord(StoreViewModel model)
        //{
        //    var result = false;
        //    try
        //    {
        //        //if id >0, update 
        //        if (model.Id > 0)
        //        {
        //            Store cust = db.Stores.SingleOrDefault(x => x.Id == model.Id);

        //            cust.Name = model.Name;
        //            cust.Address = model.Address;

        //            db.SaveChanges();
        //            result = true;
        //        }
        //        else
        //        {
        //            //insert 

        //            //check if the product is already existing 
        //            Store cust = new Store();
        //            cust.Name = model.Name;
        //            cust.Address = model.Address;
        //            db.Stores.Add(cust);
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
        public JsonResult SaveDataInDatabase(StoreViewModel model)
        {
            var result = false;
            try
            {
                //if id >0, update 
                if (model.Id > 0)
                {
                    Store cust = db.Stores.SingleOrDefault(x => x.Id == model.Id);
                    ////check if the Store name is already existing  
                    Store storevali = db.Stores.SingleOrDefault(x => x.Name == model.Name && x.Id != model.Id);
                    if (storevali == null)
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

                    //check if the Store is already existing  
                    Store custvali = db.Stores.SingleOrDefault(x => x.Name == model.Name);
                    if (custvali == null)
                    {
                        Store cust = new Store();
                        cust.Name = model.Name;
                        cust.Address = model.Address;

                        db.Stores.Add(cust);
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
        public JsonResult DeleteStoreRecord(int StoreId)
        {
            bool result = false;
            Store cust = db.Stores.SingleOrDefault(x => x.Id == StoreId);
            ProductSold custsold = db.ProductSolds.FirstOrDefault(x => x.StoreId == StoreId);

            // check this Store data is existing and not be used in the productsold table  
            if (cust != null && custsold == null)
            {
                db.Stores.Remove(cust);
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