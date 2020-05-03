using JWT.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace JWT.Controllers
{
    [Authorize]
    public class ReservationController : Controller
    {
        [HttpGet]
        [Authorize(Roles = "admin")]
        public IEnumerable<Reservation> Extract() => new List<Reservation> {
            new Reservation { Id = 1, Name = "Александр", StartLocation = "Сан-Франциско", EndLocation = "Уолтем" },
            new Reservation { Id = 2, Name = "Анастасия", StartLocation = "Сан-Диего",     EndLocation = "Редмонд" },
            new Reservation { Id = 3, Name = "Дмитрий",   StartLocation = "Нью-Йорк",      EndLocation = "Лас-Вегас" },
            new Reservation { Id = 4, Name = "Диана",     StartLocation = "Майами",        EndLocation = "Бостон" },
            new Reservation { Id = 5, Name = "Юрий",      StartLocation = "Портленд",      EndLocation = "Лос-Анджелес" }
        };
    }
}
