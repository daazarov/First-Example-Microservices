using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Client.Mvc.Models;
using Client.Mvc.Services;

namespace Client.Mvc.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<string> Booking([FromServices]BookingService bookingService)
        {
            await bookingService.NewBooking();
            return "Ok";
        }
        public async Task<string> Pay([FromServices]BookingService bookingService)
        {
            await bookingService.PayInvoice();
            return "Ok";
        }
    }
}
