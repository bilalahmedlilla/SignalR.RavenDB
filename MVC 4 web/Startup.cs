﻿using Owin;
using Microsoft.Owin;
using Microsoft.AspNet.SignalR;
using SignalR.RavenDB;
using System;

[assembly: OwinStartup(typeof(MVC4Web.Startup))]

namespace MVC4Web
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // Any connection or hub wire up and configuration should go here
            GlobalHost.DependencyResolver.UseRaven(new RavenScaleoutConfiguration("solutionEngine", "http://localhost:8090")
            {
                //Expiration = TimeSpan.FromMinutes(10)
            });

            app.MapSignalR();
        }
    }
}