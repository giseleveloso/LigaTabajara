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
    public class ComissoesController : Controller
    {
        private LigaTabajaraDbContext db = new LigaTabajaraDbContext();

        // GET: Comissoes
        public ActionResult Index(string nome, string cargo)
        {
            var comissaos = db.Comissaos.Include(c => c.Time);
            if (!string.IsNullOrEmpty(nome))
                comissaos = comissaos.Where(j => j.Nome.Contains(nome));

            if (!string.IsNullOrEmpty(cargo))
                comissaos = comissaos.Where(j => j.Cargo.ToString() == cargo);

            var cargos = Enum.GetNames(typeof(Cargo)).ToList();
            ViewBag.Cargos = new SelectList(cargos);

            return View(comissaos.ToList());
        }

        // GET: Comissoes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comissao comissao = db.Comissaos.Find(id);
            if (comissao == null)
            {
                return HttpNotFound();
            }
            return View(comissao);
        }

        // GET: Comissoes/Create
        public ActionResult Create()
        {
            ViewBag.TimeId = new SelectList(db.Times, "Id", "Nome");
            return View();
        }

        // POST: Comissoes/Create
        // Para se proteger de mais ataques, habilite as propriedades específicas às quais você quer se associar. Para 
        // obter mais detalhes, veja https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Nome,Cargo,DataNascimento,TimeId")] Comissao comissao)
        {
            // Verifica se o cargo já está atribuído ao time
            bool cargoExiste = db.Comissaos.Any(c => c.TimeId == comissao.TimeId && c.Cargo == comissao.Cargo);

            if (cargoExiste)
            {
                ModelState.AddModelError("Cargo", "Este cargo já está atribuído a um membro da comissão técnica deste time.");
            }

            if (ModelState.IsValid)
            {
                db.Comissaos.Add(comissao);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.TimeId = new SelectList(db.Times, "Id", "Nome", comissao.TimeId);
            return View(comissao);
        }


        // GET: Comissoes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comissao comissao = db.Comissaos.Find(id);
            if (comissao == null)
            {
                return HttpNotFound();
            }
            ViewBag.TimeId = new SelectList(db.Times, "Id", "Nome", comissao.TimeId);
            return View(comissao);
        }

        // POST: Comissoes/Edit/5
        // Para se proteger de mais ataques, habilite as propriedades específicas às quais você quer se associar. Para 
        // obter mais detalhes, veja https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nome,Cargo,DataNascimento,TimeId")] Comissao comissao)
        {
            bool cargoExiste = db.Comissaos.Any(c =>
                c.TimeId == comissao.TimeId &&
                c.Cargo == comissao.Cargo &&
                c.Id != comissao.Id);

            if (cargoExiste)
            {
                ModelState.AddModelError("Cargo", "Este cargo já está atribuído a um membro da comissão técnica deste time.");
            }

            if (ModelState.IsValid)
            {
                db.Entry(comissao).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.TimeId = new SelectList(db.Times, "Id", "Nome", comissao.TimeId);
            return View(comissao);
        }


        // GET: Comissoes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comissao comissao = db.Comissaos.Find(id);
            if (comissao == null)
            {
                return HttpNotFound();
            }
            return View(comissao);
        }

        // POST: Comissoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Comissao comissao = db.Comissaos.Find(id);
            db.Comissaos.Remove(comissao);
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
