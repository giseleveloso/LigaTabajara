using LigaTabajara.Data;
using LigaTabajara.Models;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace LigaTabajara.Controllers
{
    public class HomeController : Controller
    {
        private LigaTabajaraDbContext db = new LigaTabajaraDbContext();

        public ActionResult Index()
        {
            var times = db.Times
                          .Include("Jogadores")
                          .Include("ComissaoTecnica")
                          .ToList();

            var timesAptos = times.Where(IsTimeApto1).ToList();
            var statusLiga = (times.Count == 20 && timesAptos.Count == 20) ? Status.Apto : Status.Inapto;

            ViewBag.StatusLiga = statusLiga;
            return View(times.Select(t => new TimeStatusViewModel
            {
                Time = t,
                Status = IsTimeApto1(t) ? Status.Apto : Status.Inapto
            }).ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var time = db.Times
                .Include(t => t.Jogadores)
                .Include(t => t.ComissaoTecnica)
                .FirstOrDefault(t => t.Id == id);

            if (time == null)
                return HttpNotFound();

            ViewBag.IsApto = IsTimeApto(time);
            return View(time);
        }

        private bool IsTimeApto(Time time)
        {
            if (string.IsNullOrWhiteSpace(time.Nome) || string.IsNullOrWhiteSpace(time.Estado) ||
                string.IsNullOrWhiteSpace(time.Cidade) || string.IsNullOrWhiteSpace(time.Estadio) ||
                string.IsNullOrWhiteSpace(time.CorPrimaria) || string.IsNullOrWhiteSpace(time.CorSecundaria))
                return false;

            if (time.Jogadores == null || time.Jogadores.Count < 30)
                return false;

            if (time.ComissaoTecnica == null || time.ComissaoTecnica.Count < 5)
                return false;

            return time.ComissaoTecnica.Select(c => c.Cargo).Distinct().Count() == time.ComissaoTecnica.Count;
        }


        private bool IsTimeApto1(Time time)
        {
            if (string.IsNullOrWhiteSpace(time.Nome) || time.Jogadores == null || time.ComissaoTecnica == null)
                return false;

            if (time.Jogadores.Count < 30 || time.ComissaoTecnica.Count < 5)
                return false;

            return time.ComissaoTecnica.Select(c => c.Cargo).Distinct().Count() == time.ComissaoTecnica.Count;
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }

    public class TimeStatusViewModel
    {
        public Time Time { get; set; }
        public Status Status { get; set; }
    }
}