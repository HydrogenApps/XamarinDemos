using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(XplatChat.Service.Endpoint.Startup))]


namespace XplatChat.Service.Endpoint
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // Any connection or hub wire up and configuration should go here
            GlobalHost.Configuration.DisconnectTimeout = TimeSpan.FromSeconds(6);
            //GlobalHost.Configuration.ConnectionTimeout = TimeSpan.FromSeconds(5);

            app.MapSignalR();


        }
    }
}