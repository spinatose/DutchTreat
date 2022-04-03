using DutchTreat.Data;
using DutchTreat.Services;
using DutchTreat.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DutchTreat.Controllers
{
    public class AppController: Controller
    {
        private readonly IMailService _mailService;
        private readonly IDutchAdapter _adapter;

        public AppController(IMailService mailService, IDutchAdapter adapter)
        {
            _mailService = mailService;
            _adapter = adapter;
        }

        public IActionResult About() => View(); 

        public IActionResult Index() => View(_adapter.GetAllProducts());

        [HttpGet("contact")]
        public IActionResult Contact() {
            //throw new InvalidProgramException("bad boy!!!!!");
            return View(); 
        }

        [HttpPost("contact")]
        public IActionResult Contact(ContactViewModel model)
        {
            if (ModelState.IsValid)
            {
                // send the email
                _mailService.SendMessage("scot.pfuntner@radpartners.com", model.Subject, $"From: {model.Name} - {model.Email}, Message: {model.Message}");
                ViewBag.UserMessage = "Mail Sent Successfully!";
                ModelState.Clear();
            }
   
            return View();
        }

        public IActionResult Shop() => View(_adapter.GetAllProducts());
    }
}
