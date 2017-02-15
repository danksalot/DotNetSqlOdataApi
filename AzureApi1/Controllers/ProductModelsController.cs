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
    builder.EntitySet<ProductModel>("ProductModels");
    builder.EntitySet<ProductModelProductDescription>("ProductModelProductDescriptions"); 
    builder.EntitySet<Product>("Products"); 
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class ProductModelsController : ODataController
    {
        private AzureDBModel db = new AzureDBModel();

        // GET: odata/ProductModels
        [EnableQuery]
        public IQueryable<ProductModel> GetProductModels()
        {
            return db.ProductModels;
        }

        // GET: odata/ProductModels(5)
        [EnableQuery]
        public SingleResult<ProductModel> GetProductModel([FromODataUri] int key)
        {
            return SingleResult.Create(db.ProductModels.Where(productModel => productModel.ProductModelID == key));
        }

        // PUT: odata/ProductModels(5)
        public async Task<IHttpActionResult> Put([FromODataUri] int key, Delta<ProductModel> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ProductModel productModel = await db.ProductModels.FindAsync(key);
            if (productModel == null)
            {
                return NotFound();
            }

            patch.Put(productModel);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductModelExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(productModel);
        }

        // POST: odata/ProductModels
        public async Task<IHttpActionResult> Post(ProductModel productModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.ProductModels.Add(productModel);
            await db.SaveChangesAsync();

            return Created(productModel);
        }

        // PATCH: odata/ProductModels(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] int key, Delta<ProductModel> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ProductModel productModel = await db.ProductModels.FindAsync(key);
            if (productModel == null)
            {
                return NotFound();
            }

            patch.Patch(productModel);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductModelExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(productModel);
        }

        // DELETE: odata/ProductModels(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] int key)
        {
            ProductModel productModel = await db.ProductModels.FindAsync(key);
            if (productModel == null)
            {
                return NotFound();
            }

            db.ProductModels.Remove(productModel);
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/ProductModels(5)/ProductModelProductDescriptions
        [EnableQuery]
        public IQueryable<ProductModelProductDescription> GetProductModelProductDescriptions([FromODataUri] int key)
        {
            return db.ProductModels.Where(m => m.ProductModelID == key).SelectMany(m => m.ProductModelProductDescriptions);
        }

        // GET: odata/ProductModels(5)/Products
        [EnableQuery]
        public IQueryable<Product> GetProducts([FromODataUri] int key)
        {
            return db.ProductModels.Where(m => m.ProductModelID == key).SelectMany(m => m.Products);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProductModelExists(int key)
        {
            return db.ProductModels.Count(e => e.ProductModelID == key) > 0;
        }
    }
}
