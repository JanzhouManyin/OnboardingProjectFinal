using Newtonsoft.Json;
using OnboardingProjectFinal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnboardingProjectFinal.Controllers
{
    public class ProductController : Controller
    {
        OnboardingEntities db = new OnboardingEntities();
        // GET: Product
        public ActionResult Index()
        {
            return View();
        }

        //select function 
        public JsonResult GetProductList()
        {

            List<ProductViewModel> ProductList = db.Products.Select(x => new ProductViewModel
            {
                Id = x.Id,
                Name = x.Name,
                Price = x.Price
            }).ToList();
            return Json(ProductList, JsonRequestBehavior.AllowGet);
        }

        //Edit page select 
        public JsonResult GetProductRecordById(int ProductId)
        {
            Product model = db.Products.Where(x => x.Id == ProductId).SingleOrDefault();
            string value = string.Empty;
            value = JsonConvert.SerializeObject(model, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            return Json(value, JsonRequestBehavior.AllowGet);

        }

        //insert and update data into database 
        public JsonResult SaveDataInDatabase(ProductViewModel model)
        {
            var result = false;
            try
            {
                //if id >0, update 
                if (model.Id > 0)
                {
                    Product pro = db.Products.SingleOrDefault(x => x.Id == model.Id);
                    //check if the product name is already existing  
                    Product provali = db.Products.SingleOrDefault(x => x.Name == model.Name && x.Id != model.Id);
                    if (provali == null)
                    {
                        pro.Name = model.Name;
                        pro.Price = model.Price;

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
                    //insert 

                    //check if the product is already existing  
                    Product provali = db.Products.SingleOrDefault(x => x.Name == model.Name);
                    if (provali == null)
                    {
                        Product pro = new Product();
                        pro.Name = model.Name;
                        pro.Price = model.Price;

                        db.Products.Add(pro);
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
        public JsonResult DeleteProductRecord(int ProductId)
        {
            bool result = false;
            Product pro = db.Products.SingleOrDefault(x => x.Id == ProductId);
            ProductSold prosold = db.ProductSolds.FirstOrDefault(x => x.ProductId == ProductId);

            // check this product data is existing and not be used in the productsold table  
            if (pro != null && prosold == null)
            {
                db.Products.Remove(pro);
                db.SaveChanges();
                result = true;
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else if (prosold != null)
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