using System;
using System.Linq;
using System.Security.Claims;
using System.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using BlogDB.Core;
using The_Intern_MVC.Models;

namespace The_Intern_MVC.Controllers
{
    public class RegisterController : ControllerBase
    {
        private readonly IAuthorRepo _authorRepo;

        public RegisterController(IAuthorRepo authorRepo)
        {
            _authorRepo = authorRepo;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(string username, string passwordHash)
        {
            Author author = _authorRepo.GetAuthorByName(username);
            bool authorDoesNotAlreadyExist = (author == null);

            if (authorDoesNotAlreadyExist)
            {
                // register a new user in the database
                var isSuccessful = _authorRepo.TryRegisterAuthor(username, passwordHash, out var Author);

                if (isSuccessful)
                {
                    return RedirectToAction("Index", "Login");
                }
                else 
                {
                    TempData["message"] = "Something went horribly wrong when you tried to register... :(";
                }
            }
            else
            {
                TempData["message"] = "Username is not available!";
            }
            return View("~/Views/Register/Index");
        }
    }
}