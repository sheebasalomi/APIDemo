using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;

namespace WebApiEmployee
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            //https://localhost:44325/HtmlPage1.html ==> client application HtmlPage1

            EnableCorsAttribute cors = new EnableCorsAttribute("https://localhost:44325", "*", "*");
            config.EnableCors(cors);


            //--------Return only XML/JSON form web api sevice
            //config.Formatters.Remove(config.Formatters.XmlFormatter);// return only JSON 

            //config.Formatters.Remove(config.Formatters.JsonFormatter);// return only XMK 


            //--------JSONFORMATTER--------------
            // config.Formatters.JsonFormatter.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
            // config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

        }
    }
}
