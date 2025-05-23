﻿using System;
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
    public class ClassificacaosController : Controller
    {
        private LigaTabajaraDbContext db = new LigaTabajaraDbContext();

        // GET: Classificacaos
        public ActionResult Index()
        {
            var classificacoes = db.Classificacaos
                                   .Include(c => c.Time)
                                   .OrderByDescending(c => c.Pontos)
                                   .ThenByDescending(c => c.SaldoGols)
                                   .ThenByDescending(c => c.GolsPro)
                                   .ToList();
            return View(classificacoes);
        }

        // GET: Classificacaos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Classificacao classificacao = db.Classificacaos.Find(id);
            if (classificacao == null)
            {
                return HttpNotFound();
            }
            return View(classificacao);
        }

        // GET: Classificacaos/Create
        public ActionResult Create()
        {
            ViewBag.TimeId = new SelectList(db.Times, "Id", "Nome");
            return View();
        }

        // POST: Classificacaos/Create
        // Para se proteger de mais ataques, habilite as propriedades específicas às quais você quer se associar. Para 
        // obter mais detalhes, veja https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,TimeId,Pontos,Vitorias,Empates,Derrotas,GolsPro,GolsContra,SaldoGols")] Classificacao classificacao)
        {
            if (ModelState.IsValid)
            {
                db.Classificacaos.Add(classificacao);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.TimeId = new SelectList(db.Times, "Id", "Nome", classificacao.TimeId);
            return View(classificacao);
        }

        // GET: Classificacaos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Classificacao classificacao = db.Classificacaos.Find(id);
            if (classificacao == null)
            {
                return HttpNotFound();
            }
            ViewBag.TimeId = new SelectList(db.Times, "Id", "Nome", classificacao.TimeId);
            return View(classificacao);
        }

        // POST: Classificacaos/Edit/5
        // Para se proteger de mais ataques, habilite as propriedades específicas às quais você quer se associar. Para 
        // obter mais detalhes, veja https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,TimeId,Pontos,Vitorias,Empates,Derrotas,GolsPro,GolsContra,SaldoGols")] Classificacao classificacao)
        {
            if (ModelState.IsValid)
            {
                db.Entry(classificacao).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TimeId = new SelectList(db.Times, "Id", "Nome", classificacao.TimeId);
            return View(classificacao);
        }

        // GET: Classificacaos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Classificacao classificacao = db.Classificacaos.Find(id);
            if (classificacao == null)
            {
                return HttpNotFound();
            }
            return View(classificacao);
        }


        public ActionResult Artilharia()
        {
            var artilheiros = db.Gols
                .GroupBy(g => g.Jogador)
                .Select(g => new ArtilheiroViewModel
                {
                    JogadorId = g.Key.Id,
                    Nome = g.Key.Nome,
                    Time = g.Key.Time.Nome,
                    Gols = g.Count()
                })
                .OrderByDescending(a => a.Gols)
                .ThenBy(a => a.Nome)
                .ToList();

            return View(artilheiros);
        }


        // POST: Classificacaos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Classificacao classificacao = db.Classificacaos.Find(id);
            db.Classificacaos.Remove(classificacao);
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
