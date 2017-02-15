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
    builder.EntitySet<ProductModelProductDescription>("ProductModelProductDescriptions");
    builder.EntitySet<ProductDescription>("ProductDescriptions"); 
    builder.EntitySet<ProductModel>("ProductModels"); 
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class ProductModelProductDescriptionsController : ODataController
    {
        private AzureDBModel db = new AzureDBModel();

        // GET: odata/ProductModelProductDescriptions
        [EnableQuery]
        public IQueryable<ProductModelProductDescription> GetProductModelProductDescriptions()
        {
            return db.ProductModelProductDescriptions;
        }

        // GET: odata/ProductModelProductDescriptions(5)
        [EnableQuery]
        public SingleResult<ProductModelProductDescription> GetProductModelProductDescription([FromODataUri] int key)
        {
            return SingleResult.Create(db.ProductModelProductDescriptions.Where(productModelProductDescription => productModelProductDescription.ProductModelID == key));
        }

        // PUT: odata/ProductModelProductDescriptions(5)
        public async Task<IHttpActionResult> Put([FromODataUri] int key, Delta<ProductModelProductDescription> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ProductModelProductDescription productModelProductDescription = await db.ProductModelProductDescriptions.FindAsync(key);
            if (productModelProductDescription == null)
            {
                return NotFound();
            }

            patch.Put(productModelProductDescription);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductModelProductDescriptionExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(productModelProductDescription);
        }

        // POST: odata/ProductModelProductDescriptions
        public async Task<IHttpActionResult> Post(ProductModelProductDescription productModelProductDescription)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.ProductModelProductDescriptions.Add(productModelProductDescription);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ProductModelProductDescriptionExists(productModelProductDescription.ProductModelID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Created(productModelProductDescription);
        }

        // PATCH: odata/ProductModelProductDescriptions(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] int key, Delta<ProductModelProductDescription> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ProductModelProductDescription productModelProductDescription = await db.ProductModelProductDescriptions.FindAsync(key);
            if (productModelProductDescription == null)
            {
                return NotFound();
            }

            patch.Patch(productModelProductDescription);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductModelProductDescriptionExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(productModelProductDescription);
        }

        // DELETE: odata/ProductModelProductDescriptions(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] int key)
        {
            ProductModelProductDescription productModelProductDescription = await db.ProductModelProductDescriptions.FindAsync(key);
            if (productModelProductDescription == null)
            {
                return NotFound();
            }

            db.ProductModelProductDescriptions.Remove(productModelProductDescription);
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/ProductModelProductDescriptions(5)/ProductDescription
        [EnableQuery]
        public SingleResult<ProductDescription> GetProductDescription([FromODataUri] int key)
        {
            return SingleResult.Create(db.ProductModelProductDescriptions.Where(m => m.ProductModelID == key).Select(m => m.ProductDescription));
        }

        // GET: odata/ProductModelProductDescriptions(5)/ProductModel
        [EnableQuery]
        public SingleResult<ProductModel> GetProductModel([FromODataUri] int key)
        {
            return SingleResult.Create(db.ProductModelProductDescriptions.Where(m => m.ProductModelID == key).Select(m => m.ProductModel));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProductModelProductDescriptionExists(int key)
        {
            return db.ProductModelProductDescriptions.Count(e => e.ProductModelID == key) > 0;
        }
    }
}
