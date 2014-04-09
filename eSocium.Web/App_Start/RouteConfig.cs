using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace eSocium.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Answer",
                "Answer/Index/Question{QuestionID}",
                 new { controller = "Answer", action = "Index", QuestionID = 0 }
            );

            routes.MapRoute(
                "Question1",
                "Question/Edit/Question{QuestionID}",
                 new { controller = "Question", action = "Edit", QuestionID = 0 }
            );

            routes.MapRoute(
                "Question2",
                "Question/{action}/Survey{SurveyID}",
                 new { controller = "Question", action = "Index", SurveyID = 0 }
            );

            routes.MapRoute(
                "Survey",
                "Survey/Edit/Survey{SurveyID}",
                new { controller = "Survey", action = "Edit", SurveyID = 0 }
            );

            // what about Survey/Delete ???

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}",
                defaults: new { controller = "Home", action = "Index"}
            );

        }
    }
}