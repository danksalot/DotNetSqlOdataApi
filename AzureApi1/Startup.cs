﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(AzureApi1.Startup))]

namespace AzureApi1
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {

        }
    }
}
