using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using System.Web.Http.OData;
using System.Web.Http.OData.Routing;
using AzureApi1.Models;

namespace AzureApi1.Controllers
{
    /*
    The WebApiConfig class may require additional changes to add a route for this controller. Merge these statements into the Register method of the WebApiConfig class as applicable. Note that OData URLs are case sensitive.

    using System.Web.Http.OData.Builder;
    using System.Web.Http.OData.Extensions;
    using AzureApi1.Models;
    ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
    builder.EntitySet<ProductDescription>("ProductDescriptions");
    builder.EntitySet<ProductModelProductDescription>("ProductModelProductDescriptions"); 
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class ProductDescriptionsController : ODataController
    {
        private AzureDBModel db = new AzureDBModel();

        // GET: odata/ProductDescriptions
        [EnableQuery]
        public IQueryable<ProductDescription> GetProductDescriptions()
        {
            return db.ProductDescriptions;
        }

        // GET: odata/ProductDescriptions(5)
        [EnableQuery]
        public SingleResult<ProductDescription> GetProductDescription([FromODataUri] int key)
        {
            return SingleResult.Create(db.ProductDescriptions.Where(productDescription => productDescription.ProductDescriptionID == key));
        }

        // PUT: odata/ProductDescriptions(5)
        public async Task<IHttpActionResult> Put([FromODataUri] int key, Delta<ProductDescription> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ProductDescription productDescription = await db.ProductDescriptions.FindAsync(key);
            if (productDescription == null)
            {
                return NotFound();
            }

            patch.Put(productDescription);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductDescriptionExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(productDescription);
        }

        // POST: odata/ProductDescriptions
        public async Task<IHttpActionResult> Post(ProductDescription productDescription)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.ProductDescriptions.Add(productDescription);
            await db.SaveChangesAsync();

            return Created(productDescription);
        }

        // PATCH: odata/ProductDescriptions(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] int key, Delta<ProductDescription> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ProductDescription productDescription = await db.ProductDescriptions.FindAsync(key);
            if (productDescription == null)
            {
                return NotFound();
            }

            patch.Patch(productDescription);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductDescriptionExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(productDescription);
        }

        // DELETE: odata/ProductDescriptions(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] int key)
        {
            ProductDescription productDescription = await db.ProductDescriptions.FindAsync(key);
            if (productDescription == null)
            {
                return NotFound();
            }

            db.ProductDescriptions.Remove(productDescription);
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/ProductDescriptions(5)/ProductModelProductDescriptions
        [EnableQuery]
        public IQueryable<ProductModelProductDescription> GetProductModelProductDescriptions([FromODataUri] int key)
        {
            return db.ProductDescriptions.Where(m => m.ProductDescriptionID == key).SelectMany(m => m.ProductModelProductDescriptions);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProductDescriptionExists(int key)
        {
            return db.ProductDescriptions.Count(e => e.ProductDescriptionID == key) > 0;
        }
    }
}
