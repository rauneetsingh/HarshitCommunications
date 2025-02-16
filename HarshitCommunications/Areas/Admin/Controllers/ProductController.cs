using HarshitCommunications.DataAccess.Repository.IRepository;
using HarshitCommunications.Models;
using HarshitCommunications.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace HarshitCommunications.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webhostingEnvironment;
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webhostingEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webhostingEnvironment = webhostingEnvironment;
        }

        public IActionResult Index()
        {
            List<Product> objProductList = _unitOfWork.Product.GetAll(includeProperties: "Category").ToList();

            return View(objProductList);
        }

        //private string CleanDescription(string description)
        //{
        //    if (string.IsNullOrWhiteSpace(description))
        //        return string.Empty;

        //    try
        //    {
        //        // Remove escape characters and trim spaces dynamically
        //        string cleaned = description
        //            .Replace("\\r\\n", " ") // Replace newline escape sequences
        //            .Replace("\r\n", " ")   // Replace actual newlines
        //            .Replace("\\", "")      // Remove unwanted backslashes
        //            .Trim();                // Trim leading and trailing spaces

        //        // Replace multiple spaces with a single space
        //        cleaned = System.Text.RegularExpressions.Regex.Replace(cleaned, @"\s+", " ");

        //        return cleaned;
        //    }
        //    catch
        //    {
        //        return description.Trim(); // Return trimmed version if anything fails
        //    }
        //}


        public IActionResult Upsert(int? id)
        {
            ProductVM productVM = new ProductVM()
            {
                CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                Product = new Product()
            };

            if (id == null || id == 0)
            {
                //create

                return View(productVM);
            }
            else
            {
                //update
                productVM.Product = _unitOfWork.Product.Get(u => u.Id == id);

                return View(productVM);
            }

        }

        [HttpPost]
        public IActionResult Upsert(ProductVM obj, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _webhostingEnvironment.WebRootPath;
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath = Path.Combine(wwwRootPath, @"Images\Products");

                    if (!string.IsNullOrEmpty(obj.Product.ImageUrl))
                    {
                        //delete the old image
                        var oldimagePath = Path.Combine(wwwRootPath, obj.Product.ImageUrl.TrimStart('\\'));

                        if (System.IO.File.Exists(oldimagePath))
                        {
                            System.IO.File.Delete(oldimagePath);
                        }
                    }

                    using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }

                    obj.Product.ImageUrl = @"\Images\Products\" + fileName;
                }

                if (obj.Product.Id == 0)
                {
                    _unitOfWork.Product.Add(obj.Product);
                    TempData["Success"] = "Product created successfully!";
                }
                else
                {
                    _unitOfWork.Product.Update(obj.Product);
                    TempData["Success"] = "Product updated successfully!";
                }

                _unitOfWork.Save();
                return RedirectToAction("Index");
            }
            return View();
        }

        //public IActionResult Edit(int? id)
        //{
        //    if (id == null || id == 0)
        //    {
        //        return NotFound();
        //    }

        //    var obj = _unitOfWork.Product.Get(u => u.Id == id);
        //    return View(obj);
        //}

        //[HttpPost]
        //public IActionResult Edit(Product obj)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _unitOfWork.Product.Update(obj);
        //        _unitOfWork.Save();
        //        TempData["success"] = "Product Updated successfully";
        //        return RedirectToAction("Index");
        //    }
        //    return View();
        //}

        //public IActionResult Delete(int? id)
        //{
        //    if (id == null || id == 0)
        //    {
        //        return NotFound();
        //    }

        //    var obj = _unitOfWork.Product.Get(u => u.Id == id);
        //    return View(obj);
        //}

        //[HttpPost, ActionName("Delete")]
        //public IActionResult DeletePOST(int? id)
        //{
        //    var obj = _unitOfWork.Product.Get(u => u.Id == id);
        //    if (obj == null)
        //    {
        //        return NotFound();
        //    }
        //    _unitOfWork.Product.Remove(obj);
        //    _unitOfWork.Save();
        //    TempData["success"] = "Product Deleted successfully";
        //    return RedirectToAction("Index");
        //}

        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            List<Product> objProductList = _unitOfWork.Product.GetAll(includeProperties: "Category").ToList();
            return Json(new { data = objProductList });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var productToDelete = _unitOfWork.Product.Get(u => u.Id == id);
            if (productToDelete == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            //delete the old image
            var oldimagePath = Path.Combine(_webhostingEnvironment.WebRootPath, productToDelete.ImageUrl.TrimStart('\\'));

            if (System.IO.File.Exists(oldimagePath))
            {
                System.IO.File.Delete(oldimagePath);
            }

            _unitOfWork.Product.Remove(productToDelete);
            _unitOfWork.Save();

            return Json(new { success = false, message = "Product Deleted Successfully!" });
        }

        #endregion
    }
}
