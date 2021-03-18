using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using System.Web.Http.OData;
using System.Web.Http.OData.Routing;
using WebApiExercise01.Data;
using WebApiExercise01.Models;

namespace WebApiExercise01.Controllers
{
    /*
    The WebApiConfig class may require additional changes to add a route for this controller. Merge these statements into the Register method of the WebApiConfig class as applicable. Note that OData URLs are case sensitive.

    using System.Web.Http.OData.Builder;
    using System.Web.Http.OData.Extensions;
    using WebApiExercise01.Models;
    ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
    builder.EntitySet<Players>("Players");
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class PlayersController : ODataController
    {
        private PlayerDbContext db = new PlayerDbContext();

        // GET: odata/Players
        [EnableQuery]
        public IQueryable<Players> GetPlayers()
        {
            return db.Players;
        }

        // GET: odata/Players(5)
        [EnableQuery]
        public SingleResult<Players> GetPlayers([FromODataUri] int key)
        {
            return SingleResult.Create(db.Players.Where(players => players.PId == key));
        }

        // PUT: odata/Players(5)
        public IHttpActionResult Put([FromODataUri] int key, Delta<Players> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Players players = db.Players.Find(key);
            if (players == null)
            {
                return NotFound();
            }

            patch.Put(players);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PlayersExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(players);
        }

        // POST: odata/Players
        public IHttpActionResult Post(Players players)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Players.Add(players);
            db.SaveChanges();

            return Created(players);
        }

        // PATCH: odata/Players(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public IHttpActionResult Patch([FromODataUri] int key, Delta<Players> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Players players = db.Players.Find(key);
            if (players == null)
            {
                return NotFound();
            }

            patch.Patch(players);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PlayersExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(players);
        }

        // DELETE: odata/Players(5)
        public IHttpActionResult Delete([FromODataUri] int key)
        {
            Players players = db.Players.Find(key);
            if (players == null)
            {
                return NotFound();
            }

            db.Players.Remove(players);
            db.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PlayersExists(int key)
        {
            return db.Players.Count(e => e.PId == key) > 0;
        }
    }
}
