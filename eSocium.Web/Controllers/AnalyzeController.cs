using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eSocium.Domain.Abstract;
using eSocium.Domain.Entities;
using eSocium.Web.Models;
using eSocium.Web.Models.Concrete;
using TextNormalizer;
using TextTokenizer;
using TextTagger;

namespace eSocium.Web.Controllers
{
    public class AnalyzeController : Controller
    {
        private ISurveyRepository repository;

        private ResultViewModel Normalize(int QuestionID, int LinkConfigurationID)
        {            
            var answers = repository.Answers
                .Where(a => a.QuestionID == QuestionID).ToList();
            // got the answers                        
            
            eSocium.Domain.Entities.LinkConfiguration cfg = repository.LinkConfigurations
                .FirstOrDefault(c => c.LinkConfigurationID == LinkConfigurationID);
            // got the configuration

            LinkConfigurationViewModel tmp = new LinkConfigurationViewModel(cfg);            
            TextNormalizer.LinkConfiguration configuration = new TextNormalizer.LinkConfiguration();
            configuration.setConf(tmp.CheckedLinks());
            // prepared the configuration
            
            List<eSocium.Domain.Entities.Lemma> AllLemmas = 
                repository.Lemmas
                .Where(l => (l.LinkConfigurationID == LinkConfigurationID && l.Answer.QuestionID == QuestionID))
                .ToList();
            bool doNormalization = AllLemmas.Count == 0;

            ResultViewModel result = new ResultViewModel();
            result.UnknownWords.Add("");            

            Dictionary<string, int> LemmaCount = new Dictionary<string, int>();

            if (doNormalization)
            {
                // answers to this queston were not normalized according to given configuration
                foreach (Answer answer in answers)
                {
                    // process each answer
                    string[] Sentences = SentenceTokenizer.tokenize(answer.Text + ".");
                    // we have to add . not to lose the last sentence
                    foreach (string sentence in Sentences)
                    {
                        List<Form> forms = Tagger.tagSentence(sentence);
                        foreach (Form form in forms)
                        {
                            int lemmaID = form.parentLemma.id;
                            string NormalizedWord = "-1";
                            if (form.isUnknown() || form.isNumber())
                            {
                                result.UnknownWords.Add(form.word);
                            }
                            else
                            {
                                TextNormalizer.Lemma lemma = Normalizer.aggregate(Normalizer.getLemmaById(lemmaID), configuration);
                                lemmaID = lemma.id;
                                NormalizedWord = lemma.firstForm;

                                if (LemmaCount.ContainsKey(lemma.firstForm))
                                {
                                    LemmaCount[lemma.firstForm] += 1;
                                }
                                else
                                {
                                    LemmaCount.Add(lemma.firstForm, 1);
                                }
                                // counted the word
                            }

                            eSocium.Domain.Entities.Lemma l = new eSocium.Domain.Entities.Lemma
                            {
                                AnswerID = answer.AnswerID,
                                LinkConfigurationID = LinkConfigurationID,
                                OpenCorporaLemma = lemmaID,
                                Word = form.word,
                                NormalizedWord = NormalizedWord,
                                Answer = answer,
                                LinkConfiguration = cfg
                            };
                            AllLemmas.Add(l);
                        }
                    }
                }
                repository.SaveLemmas(AllLemmas);
            }
            else
            {
                // answers to this queston were already normalized according to given configuration
                // just calculating the statistics
                foreach(var l in AllLemmas)
                {
                    if (l.OpenCorporaLemma != -1)
                    {
                        //TextNormalizer.Lemma lemma = Normalizer.getLemmaById(l.OpenCorporaLemma);

                        if (LemmaCount.ContainsKey(l.NormalizedWord))
                        {
                            LemmaCount[l.NormalizedWord] += 1;
                        }
                        else
                        {
                            LemmaCount.Add(l.NormalizedWord, 1);
                        }
                        // counted the word
                    }
                    else
                    {
                        // the word is unknown
                        result.UnknownWords.Add(l.Word);
                    }
                }
            }
            // looking for the most frequent words
            result.MostCommonWords =
                    (from entry in LemmaCount
                     orderby entry.Value descending
                     select entry).Take(30).ToList();
            
            // time to find clustering

            // calculating number of lemmas
            HashSet<int> s = new HashSet<int>();
            foreach(var l in AllLemmas)
            {
                if (l.OpenCorporaLemma != -1)
                {
                    s.Add(l.OpenCorporaLemma);
                }
            }

            // preparing the data
            double[][] data = new double[answers.Count][];
            for(int i = 0; i < answers.Count; ++i)
            {
                data[i] = new double[s.Count];
            }
            Dictionary<int, int> LemmaIDNum = new Dictionary<int, int>();
            Dictionary<int, int> AnswerIDNum = new Dictionary<int, int>();
            Answer[] AnswerNumID = new Answer[answers.Count];
            Domain.Entities.Lemma[] LemmaNumID = new Domain.Entities.Lemma[s.Count];
            
            int ansCnt = 0, lemmaCnt = 0;

            foreach(var l in AllLemmas)
            {
                if (l.OpenCorporaLemma != -1)
                {
                    int currAns, currLemma;                    
                    if (!AnswerIDNum.ContainsKey(l.Answer.AnswerID))
                    {
                        AnswerNumID[ansCnt] = l.Answer;
                        currAns = ansCnt;                        
                        AnswerIDNum.Add(l.Answer.AnswerID, ansCnt++);
                    } 
                    else 
                    {
                        currAns = AnswerIDNum[l.Answer.AnswerID];
                    }

                    if (!LemmaIDNum.ContainsKey(l.OpenCorporaLemma))
                    {
                        LemmaNumID[lemmaCnt] = l;
                        currLemma = lemmaCnt;
                        LemmaIDNum.Add(l.OpenCorporaLemma, lemmaCnt++);
                    } 
                    else
                    {
                        currLemma = LemmaIDNum[l.OpenCorporaLemma];
                    }
                    ++data[currAns][currLemma];

                }
            } 
           
            // the data is ready, running KMeans
            int nClusters = 8;
            var res = KMeans.Cluster(data, nClusters, 50, null);                        

            // finally preparing the result            
            List<KeyValuePair<string, double>>[] TypicalWords = 
                new List<KeyValuePair<string, double>>[nClusters];
            List<string>[] Clustering = new List<string>[nClusters];
            for (int i = 0; i < nClusters; ++i )
            {
                Clustering[i] = new List<string>();
                TypicalWords[i] = new List<KeyValuePair<string, double>>();
            }
            for (int k = 0; k < nClusters; ++k)
            {
                for (int i = 0; i < res.ObjIds[k].Length; ++i)
                {
                    Clustering[k].Add(AnswerNumID[res.ObjIds[k][i]].Text);
                }

                // time to find most frequent words in cluster                
                TypicalWords[k] = (from x in Enumerable.Range(0, res.Means[k].Length)
                   .Select(i => new { val = res.Means[k][i], text = LemmaNumID[i].NormalizedWord }) 
                   where x.val > 0 orderby x.val descending select new KeyValuePair<string, double>(x.text, x.val))
                   .Take(5).ToList();                 
            }
            result.Clustering = Clustering.ToList();
            result.TypicalWords = TypicalWords.ToList();
            
            return result;            
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
            return View(Normalize(m.SelectedQuestionId, m.SelectedConfigurationId));
        }
    }
}
