using Eventures.App.Models;
using Eventures.Data;
using Eventures.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Eventures.App.Controllers
{
    [Authorize]
    public class EventsController : Controller
    {
        private readonly ApplicationDbContext context;
        public EventsController(ApplicationDbContext context)
        {
            this.context = context;
        }
        public IActionResult All(string searchString)
        {
            List<EventAllViewModel> events = context.Events
            .Select(e=> new EventAllViewModel
            {
                Id = e.Id,
              Name = e.Name,
              Place = e.Place,
              Start = e.Start.ToString("dd-MMM-yyyy HH:mm", CultureInfo.InvariantCulture),
              End = e.End.ToString("dd-MMM-yyyy HH:mm", CultureInfo.InvariantCulture),
              Owner = e.Owner.UserName
            })
            .ToList();
            if (!String.IsNullOrEmpty(searchString))
            {
                events = events.Where(s => s.Place.Contains(searchString)).ToList();
            }
            return this.View(events);
           }
        public IActionResult Create()
        {
            return this.View();
        }
        [HttpPost]
        public IActionResult Create(EventCreateBindingModel bindingModel)
        {
            if (this.ModelState.IsValid)
            {
                string currentUserId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                Event eventForDb = new Event
                {
                   Name = bindingModel.Name,
                   Place = bindingModel.Place,
                   Start = bindingModel.Start,
                   End = bindingModel.End,
                   TotalTickets = bindingModel.TotalTickets,
                   PricePerTicket = bindingModel.PricePerTicket,
                   OwnerId = currentUserId
                };
                context.Events.Add(eventForDb);
                context.SaveChanges();
                return this. RedirectToAction("All");
            }
                return this.View();
         }
    public IActionResult Index()
        {
            return View();
        }
    }
}
