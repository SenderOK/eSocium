using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eSocium.Domain.Abstract;
using eSocium.Domain.Entities;
using eSocium.Web.Models;
using eSocium.Web.Models.Concrete;

namespace eSocium.Web.Controllers
{
    public class QuestionController : Controller
    {
        private ISurveyRepository repository;

        public QuestionController(ISurveyRepository repo)
        {
            repository = repo;
        }
        public ActionResult Index(int SurveyID)
        {
            Survey survey = repository.Surveys
                .FirstOrDefault(s => s.SurveyID == SurveyID);

            if (survey == null || survey.CreatorName != User.Identity.Name)
            {
                return HttpNotFound();
            }

            return View(new QuestionsViewModel
            {
                Questions = survey.Questions,
                SurveyID = SurveyID,
                SurveyName = survey.Name,
            });
        }

        public ActionResult Edit(int questionID)
        {
            Question question = repository.Questions
                .FirstOrDefault(p => p.QuestionID == questionID);
            if (question == null || question.Survey.CreatorName != User.Identity.Name)
            {
                return HttpNotFound();
            }
            return View(question);
        }

        [HttpPost]
        public ActionResult Edit(Question question)
        {
            if (ModelState.IsValid)
            {
                question.LastModificationTime = DateTime.Now;
                // the complex property of Survey is lost
                // restore it using known SurveyID
                question.Survey = repository.Surveys
                    .FirstOrDefault(s => s.SurveyID == question.SurveyID);
                repository.SaveQuestion(question);
                TempData["message"] = string.Format("Question is successfully saved!");
                return RedirectToAction("Index", new { question.SurveyID });
            }
            else
            {
                // there is something wrong with the data values
                return View(question);
            }
        }

        public ActionResult Delete(int questionID)
        {
            Question question = repository.Questions
                .FirstOrDefault(p => p.QuestionID == questionID);
            if (question == null || question.Survey.CreatorName != User.Identity.Name)
            {
                return HttpNotFound();
            }
            int? surveyID = question.SurveyID;
            question = repository.DeleteQuestion(questionID);
            TempData["message"] = string.Format("Question is successfully deleted!");
            return RedirectToAction("Index", new { surveyID });
        }

        public ActionResult Create(int SurveyID)
        {
            Survey survey = repository.Surveys
                .FirstOrDefault(s => s.SurveyID == SurveyID);

            if (survey == null || survey.CreatorName != User.Identity.Name)
            {
                return HttpNotFound();
            }

            return View(new GetXlsViewModel { 
                SurveyID = SurveyID,
                sheetNumber = 1,
            });
        }

        [HttpPost]
        public ActionResult Create(GetXlsViewModel data, 
                                   HttpPostedFileBase xlsFile)
        {
            if (ModelState.IsValid)
            {
                RespondentAnswerTable RAT = null;
                try 
                {
                    RAT = new RespondentAnswerTable(
                        Methods.GetWorksheetFromHttpFile(xlsFile, data.sheetNumber), 
                        data.hasHeader);
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("xlsFile", e.Message);
                    if (!ModelState.IsValid)
                    {
                        return View(data);
                    }
                }

                Survey survey = repository.Surveys
                    .FirstOrDefault(s => s.SurveyID == data.SurveyID);

                for (int i = 0; i < RAT.answers.Length; ++i)
                {
                    Question question = new Question()
                    {
                        CreationTime = DateTime.Now,
                        LastModificationTime = DateTime.Now,
                        Label = "",
                        AdditionalInfo = "",
                        Survey = survey,
                        SurveyID = data.SurveyID
                    };
                    if (!data.hasHeader)
                    {
                        question.Wording = "AUTO WORDING";
                    }
                    else
                    {
                        question.Wording = RAT.questionLabels[i];
                    }
                    question = repository.SaveQuestion(question);
                    // the question is created, now let's add info about answers
                    // it would be nice to create respondents here

                    List<Answer> answers = new List<Answer>();
                    foreach (var resp_answ in RAT.answers[i])
                    {
                        Answer answer = new Answer()
                        {
                            Text = resp_answ.Value,
                            UserRespondentID = resp_answ.Key,
                            RespondentID = null,
                            Question = question,
                            QuestionID = question.QuestionID
                        };
                        answers.Add(answer);
                    }
                    repository.SaveAnswers(answers);
                    if (RAT.answers.Length == 1 && !data.hasHeader)
                    {
                        return RedirectToAction("Edit", new { question.QuestionID });
                    }
                }
                return RedirectToAction("Index", new { data.SurveyID });
            }
            else
            {
                return View(data);
            }
        }

        public ActionResult Analyze(int questionID)
        {
            return View();            
        }

    }
}