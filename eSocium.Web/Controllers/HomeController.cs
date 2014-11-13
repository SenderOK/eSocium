using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
//#
using eSocium.Domain.Entities;
using eSocium.Domain.Abstract;
//#

namespace eSocium.Web.Controllers
{
    public class HomeController : Controller
    {
        //#
        /*
        private ISurveyRepository repository;

        public HomeController(ISurveyRepository repo)
        {
            repository = repo;
        }
        */
        //#
        public ActionResult Index()
        {
            //ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";
            
            // didn't know where to put it
            //#
            TextNormalizer.Normalizer.Initalize();
            /*
            int QuestionID = 1;
            int LinkConfigurationID = 1;
            IEnumerable<Answer> answers = repository.Answers
                .Where(a => a.QuestionID == QuestionID);
            Answer answer = answers.First();
            // got the answers

            eSocium.Domain.Entities.LinkConfiguration cfg = repository.LinkConfigurations
                .FirstOrDefault(c => c.LinkConfigurationID == LinkConfigurationID);
            // got the configuration

            eSocium.Domain.Entities.Lemma l = new eSocium.Domain.Entities.Lemma
            {
                AnswerID = answer.AnswerID,
                LinkConfigurationID = LinkConfigurationID,
                OpenCorporaLemma = 239017,
                Answer = answer,
                LinkConfiguration = cfg
            };
            repository.SaveLemma(l);   
            */
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
