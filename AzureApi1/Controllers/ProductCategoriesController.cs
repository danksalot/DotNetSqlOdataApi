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
    builder.EntitySet<ProductCategory>("ProductCategories");
    builder.EntitySet<Product>("Products"); 
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class ProductCategoriesController : ODataController
    {
        private AzureDBModel db = new AzureDBModel();

        // GET: odata/ProductCategories
        [EnableQuery]
        public IQueryable<ProductCategory> GetProductCategories()
        {
            return db.ProductCategories;
        }

        // GET: odata/ProductCategories(5)
        [EnableQuery]
        public SingleResult<ProductCategory> GetProductCategory([FromODataUri] int key)
        {
            return SingleResult.Create(db.ProductCategories.Where(productCategory => productCategory.ProductCategoryID == key));
        }

        // PUT: odata/ProductCategories(5)
        public async Task<IHttpActionResult> Put([FromODataUri] int key, Delta<ProductCategory> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ProductCategory productCategory = await db.ProductCategories.FindAsync(key);
            if (productCategory == null)
            {
                return NotFound();
            }

            patch.Put(productCategory);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductCategoryExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(productCategory);
        }

        // POST: odata/ProductCategories
        public async Task<IHttpActionResult> Post(ProductCategory productCategory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.ProductCategories.Add(productCategory);
            await db.SaveChangesAsync();

            return Created(productCategory);
        }

        // PATCH: odata/ProductCategories(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] int key, Delta<ProductCategory> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ProductCategory productCategory = await db.ProductCategories.FindAsync(key);
            if (productCategory == null)
            {
                return NotFound();
            }

            patch.Patch(productCategory);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductCategoryExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(productCategory);
        }

        // DELETE: odata/ProductCategories(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] int key)
        {
            ProductCategory productCategory = await db.ProductCategories.FindAsync(key);
            if (productCategory == null)
            {
                return NotFound();
            }

            db.ProductCategories.Remove(productCategory);
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/ProductCategories(5)/ProductCategory1
        [EnableQuery]
        public IQueryable<ProductCategory> GetProductCategory1([FromODataUri] int key)
        {
            return db.ProductCategories.Where(m => m.ProductCategoryID == key).SelectMany(m => m.ProductCategory1);
        }

        // GET: odata/ProductCategories(5)/ProductCategory2
        [EnableQuery]
        public SingleResult<ProductCategory> GetProductCategory2([FromODataUri] int key)
        {
            return SingleResult.Create(db.ProductCategories.Where(m => m.ProductCategoryID == key).Select(m => m.ProductCategory2));
        }

        // GET: odata/ProductCategories(5)/Products
        [EnableQuery]
        public IQueryable<Product> GetProducts([FromODataUri] int key)
        {
            return db.ProductCategories.Where(m => m.ProductCategoryID == key).SelectMany(m => m.Products);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProductCategoryExists(int key)
        {
            return db.ProductCategories.Count(e => e.ProductCategoryID == key) > 0;
        }
    }
}
