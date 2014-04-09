using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eSocium.Domain.Abstract;
using eSocium.Domain.Entities;


namespace eSocium.Web.Controllers
{
    public class SurveyController : Controller
    {
        private ISurveyRepository repository;

        public SurveyController(ISurveyRepository repo)
        {
            repository = repo;
        }

        public ActionResult Index()
        {
            IEnumerable<Survey> Surveys = repository.Surveys
                .Where(s => s.CreatorName == User.Identity.Name);
            return View(Surveys);
        }

        public ActionResult Edit(int surveyID)
        {
            Survey survey = repository.Surveys
                .FirstOrDefault(p => p.SurveyID == surveyID);
            if (survey == null || survey.CreatorName != User.Identity.Name)
            {
                return HttpNotFound();
            }
            return View(survey);
        }

        [HttpPost]
        public ActionResult Edit(Survey survey)
        {
            if (ModelState.IsValid) {
                survey.LastModificationTime = DateTime.Now;
                repository.SaveSurvey(survey);
                TempData["message"] = string.Format("Survey {0} is successfully saved!", survey.Name);
                return RedirectToAction("Index");
            } else {
                // there is something wrong with the data values
                return View(survey);
            }
        }

        public ActionResult Create()
        {
            Survey survey = new Survey();
            survey.LastModificationTime = DateTime.Now;
            survey.CreationTime = DateTime.Now;
            survey.CreatorName = User.Identity.Name;
            return View("Edit", survey);
        }

        public ActionResult Delete(int surveyID)
        {
            Survey survey = repository.Surveys
                .FirstOrDefault(p => p.SurveyID == surveyID);
            if (survey == null || survey.CreatorName != User.Identity.Name)
            {
                return HttpNotFound();
            }
            survey = repository.DeleteSurvey(surveyID);
            TempData["message"] = string.Format("Survey {0} is successfully deleted!", survey.Name);
            return RedirectToAction("Index");
        }
    }
}
