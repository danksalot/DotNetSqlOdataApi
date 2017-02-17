﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;
using System.Web.Http.OData.Builder;
using System.Web.Http.OData.Extensions;
using AzureApi1.Models;

namespace AzureApi1
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // OData Web API routes
            ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
            builder.EntitySet<Address>("Addresses");
            builder.EntitySet<Customer>("Customers");
            builder.EntitySet<CustomerAddress>("CustomerAddresses");
            builder.EntitySet<SalesOrderDetail>("SalesOrderDetails");
            builder.EntitySet<SalesOrderHeader>("SalesOrderHeaders");
            builder.EntitySet<Product>("Products");
            builder.EntitySet<ProductModel>("ProductModels");
            builder.EntitySet<ProductCategory>("ProductCategories");
            builder.EntitySet<ProductDescription>("ProductDescriptions");
            builder.EntitySet<ProductModelProductDescription>("ProductModelProductDescriptions");
            config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
        }
    }
}
