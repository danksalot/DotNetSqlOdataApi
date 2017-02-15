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
    builder.EntitySet<SalesOrderHeader>("SalesOrderHeaders");
    builder.EntitySet<Address>("Addresses"); 
    builder.EntitySet<Customer>("Customers"); 
    builder.EntitySet<SalesOrderDetail>("SalesOrderDetails"); 
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class SalesOrderHeadersController : ODataController
    {
        private AzureDBModel db = new AzureDBModel();

        // GET: odata/SalesOrderHeaders
        [EnableQuery]
        public IQueryable<SalesOrderHeader> GetSalesOrderHeaders()
        {
            return db.SalesOrderHeaders;
        }

        // GET: odata/SalesOrderHeaders(5)
        [EnableQuery]
        public SingleResult<SalesOrderHeader> GetSalesOrderHeader([FromODataUri] int key)
        {
            return SingleResult.Create(db.SalesOrderHeaders.Where(salesOrderHeader => salesOrderHeader.SalesOrderID == key));
        }

        // PUT: odata/SalesOrderHeaders(5)
        public async Task<IHttpActionResult> Put([FromODataUri] int key, Delta<SalesOrderHeader> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            SalesOrderHeader salesOrderHeader = await db.SalesOrderHeaders.FindAsync(key);
            if (salesOrderHeader == null)
            {
                return NotFound();
            }

            patch.Put(salesOrderHeader);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SalesOrderHeaderExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(salesOrderHeader);
        }

        // POST: odata/SalesOrderHeaders
        public async Task<IHttpActionResult> Post(SalesOrderHeader salesOrderHeader)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.SalesOrderHeaders.Add(salesOrderHeader);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (SalesOrderHeaderExists(salesOrderHeader.SalesOrderID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Created(salesOrderHeader);
        }

        // PATCH: odata/SalesOrderHeaders(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] int key, Delta<SalesOrderHeader> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            SalesOrderHeader salesOrderHeader = await db.SalesOrderHeaders.FindAsync(key);
            if (salesOrderHeader == null)
            {
                return NotFound();
            }

            patch.Patch(salesOrderHeader);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SalesOrderHeaderExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(salesOrderHeader);
        }

        // DELETE: odata/SalesOrderHeaders(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] int key)
        {
            SalesOrderHeader salesOrderHeader = await db.SalesOrderHeaders.FindAsync(key);
            if (salesOrderHeader == null)
            {
                return NotFound();
            }

            db.SalesOrderHeaders.Remove(salesOrderHeader);
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/SalesOrderHeaders(5)/Address
        [EnableQuery]
        public SingleResult<Address> GetAddress([FromODataUri] int key)
        {
            return SingleResult.Create(db.SalesOrderHeaders.Where(m => m.SalesOrderID == key).Select(m => m.Address));
        }

        // GET: odata/SalesOrderHeaders(5)/Address1
        [EnableQuery]
        public SingleResult<Address> GetAddress1([FromODataUri] int key)
        {
            return SingleResult.Create(db.SalesOrderHeaders.Where(m => m.SalesOrderID == key).Select(m => m.Address1));
        }

        // GET: odata/SalesOrderHeaders(5)/Customer
        [EnableQuery]
        public SingleResult<Customer> GetCustomer([FromODataUri] int key)
        {
            return SingleResult.Create(db.SalesOrderHeaders.Where(m => m.SalesOrderID == key).Select(m => m.Customer));
        }

        // GET: odata/SalesOrderHeaders(5)/SalesOrderDetails
        [EnableQuery]
        public IQueryable<SalesOrderDetail> GetSalesOrderDetails([FromODataUri] int key)
        {
            return db.SalesOrderHeaders.Where(m => m.SalesOrderID == key).SelectMany(m => m.SalesOrderDetails);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SalesOrderHeaderExists(int key)
        {
            return db.SalesOrderHeaders.Count(e => e.SalesOrderID == key) > 0;
        }
    }
}
