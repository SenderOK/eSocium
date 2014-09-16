using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eSocium.Domain.Abstract;
using eSocium.Domain.Entities;
using eSocium.Web.Models;
using TextNormalizer;
using TextTokenizer;
using TextTagger;

namespace eSocium.Web.Controllers
{
    public class AnalyzeController : Controller
    {
        private ISurveyRepository repository;

        private void Normalize(int QuestionID, int LinkConfigurationID)
        {            
            IEnumerable<Answer> answers = repository.Answers
                .Where(a => a.QuestionID == QuestionID);
            // got the answers
            
            eSocium.Domain.Entities.LinkConfiguration cfg = repository.LinkConfigurations
                .FirstOrDefault(c => c.LinkConfigurationID == LinkConfigurationID);
            // got the configuration

            LinkConfigurationViewModel tmp = new LinkConfigurationViewModel(cfg);            
            TextNormalizer.LinkConfiguration configuration = new TextNormalizer.LinkConfiguration();
            configuration.setConf(tmp.CheckedLinks());
            // prepared the configuration

            foreach (Answer answer in answers)
            {
                // process each answer
                string[] Sentences = SentenceTokenizer.tokenize(answer.Text);
                foreach (string sentence in Sentences)
                {
                    List<Form> forms = Tagger.tagSentence(sentence);
                    foreach (Form form in forms)
                    {       
                        int lemma = form.parentLemma.id;
                        if (form.isUnknown())
                        {                            
                            System.Diagnostics.Debug.WriteLine(form.word);
                        }
                        else
                        {
                            lemma = Normalizer.aggregate(Normalizer.getLemmaById(lemma), configuration).id;
                        }

                        eSocium.Domain.Entities.Lemma l = new eSocium.Domain.Entities.Lemma
                        {
                            AnswerID = answer.AnswerID,
                            LinkConfigurationID = LinkConfigurationID,
                            OpenCorporaLemma = lemma
                        };

                        repository.SaveLemma(l);
                        
                    }
                }
            }

        }

        public AnalyzeController(ISurveyRepository repo)
        {
            repository = repo;
        }

        public ActionResult Index(int QuestionID)
        {
            NormalizerViewModel LinkConfigurations = new NormalizerViewModel
               {
                   Configurations = repository.LinkConfigurations
                   .Where(s => s.CreatorName == User.Identity.Name),
                   SelectedQuestionId = QuestionID
               };
            if (LinkConfigurations.Configurations.Count() == 0)
            {
                TempData["Message"] = "You have no link configurations. You should first create the link configuration";
                return RedirectToAction("Index", "LinkConfiguration");
            }
            return View(LinkConfigurations);            
        }

        [HttpPost]
        public ActionResult SelectMethod(NormalizerViewModel m)
        {
            Normalize(m.SelectedQuestionId, m.SelectedConfigurationId);
            return View();
        }
    }
}
