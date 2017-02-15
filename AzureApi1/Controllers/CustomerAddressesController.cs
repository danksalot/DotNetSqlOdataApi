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
    builder.EntitySet<CustomerAddress>("CustomerAddresses");
    builder.EntitySet<Address>("Addresses"); 
    builder.EntitySet<Customer>("Customers"); 
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class CustomerAddressesController : ODataController
    {
        private AzureDBModel db = new AzureDBModel();

        // GET: odata/CustomerAddresses
        [EnableQuery]
        public IQueryable<CustomerAddress> GetCustomerAddresses()
        {
            return db.CustomerAddresses;
        }

        // GET: odata/CustomerAddresses(5)
        [EnableQuery]
        public SingleResult<CustomerAddress> GetCustomerAddress([FromODataUri] int key)
        {
            return SingleResult.Create(db.CustomerAddresses.Where(customerAddress => customerAddress.CustomerID == key));
        }

        // PUT: odata/CustomerAddresses(5)
        public async Task<IHttpActionResult> Put([FromODataUri] int key, Delta<CustomerAddress> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            CustomerAddress customerAddress = await db.CustomerAddresses.FindAsync(key);
            if (customerAddress == null)
            {
                return NotFound();
            }

            patch.Put(customerAddress);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerAddressExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(customerAddress);
        }

        // POST: odata/CustomerAddresses
        public async Task<IHttpActionResult> Post(CustomerAddress customerAddress)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.CustomerAddresses.Add(customerAddress);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (CustomerAddressExists(customerAddress.CustomerID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Created(customerAddress);
        }

        // PATCH: odata/CustomerAddresses(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] int key, Delta<CustomerAddress> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            CustomerAddress customerAddress = await db.CustomerAddresses.FindAsync(key);
            if (customerAddress == null)
            {
                return NotFound();
            }

            patch.Patch(customerAddress);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerAddressExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(customerAddress);
        }

        // DELETE: odata/CustomerAddresses(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] int key)
        {
            CustomerAddress customerAddress = await db.CustomerAddresses.FindAsync(key);
            if (customerAddress == null)
            {
                return NotFound();
            }

            db.CustomerAddresses.Remove(customerAddress);
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/CustomerAddresses(5)/Address
        [EnableQuery]
        public SingleResult<Address> GetAddress([FromODataUri] int key)
        {
            return SingleResult.Create(db.CustomerAddresses.Where(m => m.CustomerID == key).Select(m => m.Address));
        }

        // GET: odata/CustomerAddresses(5)/Customer
        [EnableQuery]
        public SingleResult<Customer> GetCustomer([FromODataUri] int key)
        {
            return SingleResult.Create(db.CustomerAddresses.Where(m => m.CustomerID == key).Select(m => m.Customer));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CustomerAddressExists(int key)
        {
            return db.CustomerAddresses.Count(e => e.CustomerID == key) > 0;
        }
    }
}
