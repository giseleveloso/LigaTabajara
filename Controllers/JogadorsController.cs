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
    public class JogadorsController : Controller
    {
        private LigaTabajaraDbContext db = new LigaTabajaraDbContext();

        // GET: Jogadors
        public ActionResult Index(string nome, string posicao, string pePreferido)
        {
            var jogadores = db.Jogadors.Include(j => j.Time);

            if (!string.IsNullOrEmpty(nome))
                jogadores = jogadores.Where(j => j.Nome.Contains(nome));

            if (!string.IsNullOrEmpty(posicao))
                jogadores = jogadores.Where(j => j.Posicao.ToString() == posicao);

            if (!string.IsNullOrEmpty(pePreferido))
                jogadores = jogadores.Where(j => j.PePreferido.ToString() == pePreferido);

            // Garantir que mesmo sem jogadores, as listas não sejam nulas
            var posicoes = db.Jogadors.Select(j => j.Posicao.ToString()).Distinct().ToList();
            var pes = db.Jogadors.Select(j => j.PePreferido.ToString()).Distinct().ToList();

            ViewBag.Posicoes = posicoes.Any()
                ? posicoes
                : new List<string> { "Goleiro", "Zagueiro", "Lateral", "Meio-Campo", "Atacante" };

            ViewBag.Pes = pes.Any()
                ? pes
                : new List<string> { "Destro", "Canhoto", "Ambidestro" };


            return View(jogadores.ToList());
        }


        // GET: Jogadors/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Jogador jogador = db.Jogadors.Include(p => p.Time).FirstOrDefault(p => p.Id == id);
            if (jogador == null)
            {
                return HttpNotFound();
            }
            return View(jogador);
        }

        // GET: Jogadors/Create
        public ActionResult Create()
        {
            ViewBag.TimeId = new SelectList(db.Times, "Id", "Nome");
            return View();
        }

        // POST: Jogadors/Create
        // Para se proteger de mais ataques, habilite as propriedades específicas às quais você quer se associar. Para 
        // obter mais detalhes, veja https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Nome,DataNascimento,Nacionalidade,Posicao,NumCamisa,Altura,Peso,PePreferido,TimeId,Gols")] Jogador jogador)
        {
            if (ModelState.IsValid)
            {
                db.Jogadors.Add(jogador);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.TimeId = new SelectList(db.Times, "Id", "Nome", jogador.TimeId);
            return View(jogador);
        }

        public JsonResult PorTime(int timeId)
        {
            var jogadores = db.Jogadors
                              .Where(j => j.TimeId == timeId)
                              .Select(j => new { j.Id, j.Nome })
                              .ToList();
            return Json(jogadores, JsonRequestBehavior.AllowGet);
        }


        // GET: Jogadors/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Jogador jogador = db.Jogadors.Find(id);
            if (jogador == null)
            {
                return HttpNotFound();
            }
            ViewBag.TimeId = new SelectList(db.Times, "Id", "Nome", jogador.TimeId);
            return View(jogador);
        }

        // POST: Jogadors/Edit/5
        // Para se proteger de mais ataques, habilite as propriedades específicas às quais você quer se associar. Para 
        // obter mais detalhes, veja https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nome,DataNascimento,Nacionalidade,Posicao,NumCamisa,Altura,Peso,PePreferido,TimeId,Gols")] Jogador jogador)
        {
            if (ModelState.IsValid)
            {
                db.Entry(jogador).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TimeId = new SelectList(db.Times, "Id", "Nome", jogador.TimeId);
            return View(jogador);
        }

        // GET: Jogadors/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Jogador jogador = db.Jogadors.Include(j => j.Time).FirstOrDefault(j => j.Id == id);
            if (jogador == null)
            {
                return HttpNotFound();
            }
            return View(jogador);
        }

        // POST: Jogadors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Jogador jogador = db.Jogadors.Find(id);
            db.Jogadors.Remove(jogador);
            db.SaveChanges();
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
