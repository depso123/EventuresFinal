using Eventures.App.Models;
using Eventures.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Eventures.App.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext context;

        public OrdersController(ApplicationDbContext context)
        {
            this.context = context;
        }
        [HttpPost]

        public IActionResult Create(OrderCreateBindingModel bindingModel)
        {
            if (this.ModelState.IsValid)
            {
                string currentUserId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                var user = this.context.Users.SingleOrDefault(u => u.Id == currentUserId);
                var ev = this.context.Users.SingleOrDefault(u => u.Id == currentUserId);
                this.context.Events.SingleOrDefault(e => e.Id == bindingModel.EventId);
                if (user == null || ev == null || ev.TotalTickets < bindingModel.TicketsCount)
                {

                    return this.RedirectToAction("All", "Events");
                }




            }
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
