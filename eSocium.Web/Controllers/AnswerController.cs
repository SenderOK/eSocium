using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eSocium.Domain.Abstract;
using eSocium.Domain.Entities;
using eSocium.Web.Models;

namespace eSocium.Web.Controllers
{
    public class AnswerController : Controller
    {
        private ISurveyRepository repository;

        public AnswerController(ISurveyRepository repo)
        {
            repository = repo;
        }

        public ActionResult Index(int questionID)
        {
            Question question = repository.Questions
                .FirstOrDefault(q => q.QuestionID == questionID);

            if (question == null || question.Survey.CreatorName != User.Identity.Name)
            {
                return HttpNotFound();
            }

            return View(new AnswersViewModel {
                SurveyID = question.Survey.SurveyID,
                QuestionID = questionID,
                QuestionWording = question.Wording,
                Answers = question.Answers
            });
        }
    }
}
