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
using Newtonsoft.Json;

namespace LigaTabajara.Controllers
{
    public class PartidasController : Controller
    {
        private LigaTabajaraDbContext db = new LigaTabajaraDbContext();

        // GET: Partidas
        public ActionResult Index()
        {
            var partidas = db.Partidas.Include(p => p.TimeCasa).Include(p => p.TimeVisitante);
            return View(partidas.ToList());
        }

        // GET: Partidas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Partida partida = db.Partidas.Find(id);
            if (partida == null)
            {
                return HttpNotFound();
            }
            return View(partida);
        }

        // GET: Partidas/Create
        public ActionResult Create()
        {
            ViewBag.TimeCasaId = new SelectList(db.Times, "Id", "Nome");
            ViewBag.TimeVisitanteId = new SelectList(db.Times, "Id", "Nome");
            return View();
        }

        // POST: Partidas/Create
        // Para se proteger de mais ataques, habilite as propriedades específicas às quais você quer se associar. Para 
        // obter mais detalhes, veja https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(string GolsCasaJson, string GolsVisitanteJson,[Bind(Include = "Id,Rodada,DataHora,TimeCasaId,TimeVisitanteId,GolsTimeCasa,GolsTimeVisitante")] Partida partida)
        {
            if (partida.TimeCasaId == partida.TimeVisitanteId)
            {
                ModelState.AddModelError("", "Um time não pode jogar contra ele mesmo.");
            }

           var partidasExistentes = db.Partidas
            .Where(p =>
                (p.TimeCasaId == partida.TimeCasaId && p.TimeVisitanteId == partida.TimeVisitanteId) ||
                (p.TimeCasaId == partida.TimeVisitanteId && p.TimeVisitanteId == partida.TimeCasaId))
                .ToList();

            if (partidasExistentes.Count >= 2)
            {
                ModelState.AddModelError("", "Esses dois times já se enfrentaram duas vezes.");
            }

            if (ModelState.IsValid)
            {
                db.Partidas.Add(partida);
                db.SaveChanges();

                // Desserializa os gols
                var golsCasa = JsonConvert.DeserializeObject<List<Gol>>(GolsCasaJson);
                var golsVisitante = JsonConvert.DeserializeObject<List<Gol>>(GolsVisitanteJson);

                // Adiciona os gols ao banco, associando à partida
                foreach (var gol in golsCasa)
                {
                    gol.PartidaId = partida.Id;
                    db.Gols.Add(gol);
                }

                foreach (var gol in golsVisitante)
                {
                    gol.PartidaId = partida.Id;
                    db.Gols.Add(gol);
                }

                db.SaveChanges();


                if (partida.GolsTimeCasa>=0 && partida.GolsTimeVisitante>=0)
                {
                    AtualizarClassificacao(partida);
                }

                return RedirectToAction("Index");
            }

            ViewBag.TimeCasaId = new SelectList(db.Times, "Id", "Nome", partida.TimeCasaId);
            ViewBag.TimeVisitanteId = new SelectList(db.Times, "Id", "Nome", partida.TimeVisitanteId);
            return View(partida);
        }

        [HttpGet]
        public JsonResult ObterProximaRodada(int timeId)
        {
            // Conta quantas partidas o time já participou (como casa ou visitante)
            int partidasJogadas = db.Partidas
                .Count(p => p.TimeCasaId == timeId || p.TimeVisitanteId == timeId);

            int proximaRodada = partidasJogadas + 1;

            return Json(proximaRodada, JsonRequestBehavior.AllowGet);
        }


        // GET: Partidas/RegistrarResultado/5
        public ActionResult RegistrarResultado(int id)
        {
            var partida = db.Partidas.Include(p => p.TimeCasa)
                                     .Include(p => p.TimeVisitante)
                                     .FirstOrDefault(p => p.Id == id);

            if (partida == null) return HttpNotFound();
            return View(partida);
        }

        // POST: Partidas/RegistrarResultado/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RegistrarResultado(int id, int golsCasa, int golsVisitante)
        {
            var partida = db.Partidas.Find(id);
            if (partida == null) return HttpNotFound();

            partida.GolsTimeCasa = golsCasa;
            partida.GolsTimeVisitante = golsVisitante;
            db.SaveChanges();

            AtualizarClassificacao(partida);

            return RedirectToAction("Index");
        }

        private void AtualizarClassificacao(Partida partida)
        {
            var timeCasa = db.Classificacaos.FirstOrDefault(c => c.TimeId == partida.TimeCasaId);
            var timeVisitante = db.Classificacaos.FirstOrDefault(c => c.TimeId == partida.TimeVisitanteId);

            if (timeCasa == null)
            {
                timeCasa = new Classificacao { TimeId = partida.TimeCasaId };
                db.Classificacaos.Add(timeCasa);
            }
            if (timeVisitante == null)
            {
                timeVisitante = new Classificacao { TimeId = partida.TimeVisitanteId };
                db.Classificacaos.Add(timeVisitante);
            }

            timeCasa.GolsPro += partida.GolsTimeCasa;
            timeCasa.GolsContra += partida.GolsTimeVisitante;

            timeVisitante.GolsPro += partida.GolsTimeVisitante;
            timeVisitante.GolsContra += partida.GolsTimeCasa;

            timeCasa.SaldoGols = timeCasa.GolsPro - timeCasa.GolsContra;
            timeVisitante.SaldoGols = timeVisitante.GolsPro - timeVisitante.GolsContra;

            if (partida.GolsTimeCasa > partida.GolsTimeVisitante)
            {
                timeCasa.Vitorias++;
                timeCasa.Pontos += 3;
                timeVisitante.Derrotas++;
            }
            else if (partida.GolsTimeCasa < partida.GolsTimeVisitante)
            {
                timeVisitante.Vitorias++;
                timeVisitante.Pontos += 3;
                timeCasa.Derrotas++;
            }
            else
            {
                timeCasa.Empates++;
                timeVisitante.Empates++;
                timeCasa.Pontos += 1;
                timeVisitante.Pontos += 1;
            }

            db.SaveChanges();
        }


        public ActionResult RegistrarGol(int partidaId)
        {
            ViewBag.JogadorId = new SelectList(db.Jogadors, "Id", "Nome");
            ViewBag.PartidaId = partidaId;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RegistrarGol([Bind(Include = "PartidaId,JogadorId,Minuto")] Gol gol)
        {
            if (ModelState.IsValid)
            {
                db.Gols.Add(gol);

                var jogador = db.Jogadors.Find(gol.JogadorId);
                if (jogador != null)
                {
                    jogador.Gols++;
                }

                db.SaveChanges();
                return RedirectToAction("Details", new { id = gol.PartidaId });
            }
            return View(gol);
        }



        // GET: Partidas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Partida partida = db.Partidas.Find(id);
            if (partida == null)
            {
                return HttpNotFound();
            }
            ViewBag.TimeCasaId = new SelectList(db.Times, "Id", "Nome", partida.TimeCasaId);
            ViewBag.TimeVisitanteId = new SelectList(db.Times, "Id", "Nome", partida.TimeVisitanteId);
            return View(partida);
        }

        // POST: Partidas/Edit/5
        // Para se proteger de mais ataques, habilite as propriedades específicas às quais você quer se associar. Para 
        // obter mais detalhes, veja https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Rodada,DataHora,TimeCasaId,TimeVisitanteId,GolsTimeCasa,GolsTimeVisitante")] Partida partida)
        {
            if (ModelState.IsValid)
            {
                db.Entry(partida).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TimeCasaId = new SelectList(db.Times, "Id", "Nome", partida.TimeCasaId);
            ViewBag.TimeVisitanteId = new SelectList(db.Times, "Id", "Nome", partida.TimeVisitanteId);
            return View(partida);
        }

        // GET: Partidas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Partida partida = db.Partidas.Find(id);
            if (partida == null)
            {
                return HttpNotFound();
            }
            return View(partida);
        }

        // POST: Partidas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Partida partida = db.Partidas.Find(id);
            db.Partidas.Remove(partida);
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
