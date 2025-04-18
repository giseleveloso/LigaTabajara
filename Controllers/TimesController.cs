using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LigaTabajara.Data;
using LigaTabajara.Models;

namespace LigaTabajara.Controllers
{
    public class TimesController : Controller
    {
        private LigaTabajaraDbContext db = new LigaTabajaraDbContext();

        // GET: Times
        public ActionResult Index()
        {
            return View(db.Times.ToList());
        }

        // GET: Times/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var time = db.Times
                 .Include(t => t.Jogadores)
                 .Include(t => t.ComissaoTecnica)
                 .FirstOrDefault(t => t.Id == id);

            if (time == null)
            {
                return HttpNotFound();
            }
            return View(time);
        }

        // GET: Times/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Times/Create
        // Para se proteger de mais ataques, habilite as propriedades específicas às quais você quer se associar. Para 
        // obter mais detalhes, veja https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Nome,Cidade,Estado,AnoFundacao,Estadio,CapacidadeEstadio,CorPrimaria,CorSecundaria,Status")] Time time)
        {
            if (ModelState.IsValid)
            {
                db.Times.Add(time);
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }

            return View(time);
        }

        // GET: Times/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Time time = db.Times.Find(id);
            if (time == null)
            {
                return HttpNotFound();
            }
            return View(time);
        }

        // POST: Times/Edit/5
        // Para se proteger de mais ataques, habilite as propriedades específicas às quais você quer se associar. Para 
        // obter mais detalhes, veja https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nome,Cidade,Estado,AnoFundacao,Estadio,CapacidadeEstadio,CorPrimaria,CorSecundaria,Status")] Time time)
        {
            if (ModelState.IsValid)
            {
                db.Entry(time).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            return View(time);
        }

        // GET: Times/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Time time = db.Times.Find(id);
            if (time == null)
            {
                return HttpNotFound();
            }

            // Verificar se existem referências
            bool temPartidas = db.Partidas.Any(p => p.TimeCasaId == id || p.TimeVisitanteId == id);
            bool temJogadores = db.Jogadors.Any(j => j.TimeId == id);
            bool temComissao = db.Comissaos.Any(c => c.TimeId == id);

            ViewBag.TemReferencias = temPartidas || temJogadores || temComissao;
            ViewBag.TemPartidas = temPartidas;
            ViewBag.TemJogadores = temJogadores;
            ViewBag.TemComissao = temComissao;

            return View(time);
        }

        // POST: Times/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            // Verificar se existem referências
            bool temPartidas = db.Partidas.Any(p => p.TimeCasaId == id || p.TimeVisitanteId == id);
            bool temJogadores = db.Jogadors.Any(j => j.TimeId == id);
            bool temComissao = db.Comissaos.Any(c => c.TimeId == id);

            if (temPartidas || temJogadores || temComissao)
            {
                // Tem referências, retorna para a view com mensagem de erro
                Time time = db.Times.Find(id);
                ViewBag.TemReferencias = true;
                ViewBag.TemPartidas = temPartidas;
                ViewBag.TemJogadores = temJogadores;
                ViewBag.TemComissao = temComissao;
                ViewBag.ErrorMessage = "Não é possível excluir este time porque existem registros dependentes.";
                return View(time);
            }

            // Não tem referências, pode excluir
            Time timeToDelete = db.Times.Find(id);
            db.Times.Remove(timeToDelete);
            db.SaveChanges();

            TempData["SuccessMessage"] = "Time excluído com sucesso!";
            return RedirectToAction("Index");
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
