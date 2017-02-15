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
    builder.EntitySet<Address>("Addresses");
    builder.EntitySet<CustomerAddress>("CustomerAddresses"); 
    builder.EntitySet<SalesOrderHeader>("SalesOrderHeaders"); 
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class AddressesController : ODataController
    {
        private AzureDBModel db = new AzureDBModel();

        // GET: odata/Addresses
        [EnableQuery]
        public IQueryable<Address> GetAddresses()
        {
            return db.Addresses;
        }

        // GET: odata/Addresses(5)
        [EnableQuery]
        public SingleResult<Address> GetAddress([FromODataUri] int key)
        {
            return SingleResult.Create(db.Addresses.Where(address => address.AddressID == key));
        }

        // PUT: odata/Addresses(5)
        public async Task<IHttpActionResult> Put([FromODataUri] int key, Delta<Address> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Address address = await db.Addresses.FindAsync(key);
            if (address == null)
            {
                return NotFound();
            }

            patch.Put(address);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AddressExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(address);
        }

        // POST: odata/Addresses
        public async Task<IHttpActionResult> Post(Address address)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Addresses.Add(address);
            await db.SaveChangesAsync();

            return Created(address);
        }

        // PATCH: odata/Addresses(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] int key, Delta<Address> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Address address = await db.Addresses.FindAsync(key);
            if (address == null)
            {
                return NotFound();
            }

            patch.Patch(address);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AddressExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(address);
        }

        // DELETE: odata/Addresses(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] int key)
        {
            Address address = await db.Addresses.FindAsync(key);
            if (address == null)
            {
                return NotFound();
            }

            db.Addresses.Remove(address);
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/Addresses(5)/CustomerAddresses
        [EnableQuery]
        public IQueryable<CustomerAddress> GetCustomerAddresses([FromODataUri] int key)
        {
            return db.Addresses.Where(m => m.AddressID == key).SelectMany(m => m.CustomerAddresses);
        }

        // GET: odata/Addresses(5)/SalesOrderHeaders
        [EnableQuery]
        public IQueryable<SalesOrderHeader> GetSalesOrderHeaders([FromODataUri] int key)
        {
            return db.Addresses.Where(m => m.AddressID == key).SelectMany(m => m.SalesOrderHeaders);
        }

        // GET: odata/Addresses(5)/SalesOrderHeaders1
        [EnableQuery]
        public IQueryable<SalesOrderHeader> GetSalesOrderHeaders1([FromODataUri] int key)
        {
            return db.Addresses.Where(m => m.AddressID == key).SelectMany(m => m.SalesOrderHeaders1);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AddressExists(int key)
        {
            return db.Addresses.Count(e => e.AddressID == key) > 0;
        }
    }
}
