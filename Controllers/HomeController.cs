using JWT.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace JWT.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index() => View();

        [HttpPost]
        public IActionResult Show([FromBody]List<Reservation> rList) =>
            PartialView("_PartialReservation", rList);
    }
}
