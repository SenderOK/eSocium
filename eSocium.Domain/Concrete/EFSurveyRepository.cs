using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using eSocium.Domain.Abstract;
using eSocium.Domain.Entities;

namespace eSocium.Domain.Concrete
{
    public class EFSurveyRepository : ISurveyRepository
    {
        private SurveysContext context = new SurveysContext();

        public IQueryable<Survey> Surveys
        {
            get { return context.Surveys; }
        }

        public IQueryable<Question> Questions
        {
            get { return context.Questions; }
        }

        public IQueryable<Answer> Answers
        {
            get { return context.Answers; }
        }

        public IQueryable<LinkConfiguration> LinkConfigurations
        {
            get { return context.LinkConfigurations; }
        }

        public IQueryable<Lemma> Lemmas
        {
            get { return context.Lemmas; }
        }

        public void SaveSurvey(Survey survey)
        {
            if (survey.SurveyID == 0)
            {
                // Create workflow
                // Fully formed surveys must come there (except for [key] = SurveyID)
                context.Surveys.Add(survey);
            }
            else
            {
                // Edit Workflow
                // Only several fields interest us here
                Survey dbEntry = context.Surveys.Find(survey.SurveyID);
                if (dbEntry != null)
                {
                    dbEntry.Name = survey.Name;
                    dbEntry.LastModificationTime = survey.LastModificationTime;
                    dbEntry.Description = survey.Description;
                    dbEntry.SurveyRegionDescription = survey.SurveyRegionDescription;
                    dbEntry.SurveyTimeDescription = survey.SurveyTimeDescription;
                }
            }
            context.SaveChanges();
        }

        public Survey DeleteSurvey(int surveyID)
        {
            Survey dbEntry = context.Surveys.Find(surveyID);
            if (dbEntry != null)
            {
                context.Surveys.Remove(dbEntry);
                context.SaveChanges();
            }
            return dbEntry;
        }

        // We assume that question.Survey is known here
        public Question SaveQuestion(Question question)
        {
            Question dbEntry;
            if (question.QuestionID == 0)
            {
                // Create workflow
                // Fully formed surveys must come there (except for [key] = SurveyID)
                dbEntry = context.Questions.Add(question);
            }
            else
            {
                dbEntry = context.Questions.Find(question.QuestionID);
                if (dbEntry != null)
                {
                    dbEntry.Wording = question.Wording;
                    dbEntry.LastModificationTime = question.LastModificationTime;
                    dbEntry.Label = question.Label;
                    dbEntry.AdditionalInfo = question.AdditionalInfo;
                }
            }
            context.SaveChanges();

            question.Survey.LastModificationTime = question.LastModificationTime;
            SaveSurvey(question.Survey); // consistency!
            return dbEntry;
        }

        // Fully formed questions must come there
        public Question DeleteQuestion(int questionID)
        {
            Question dbEntry = context.Questions.Find(questionID);

            if (dbEntry != null)
            {
                dbEntry.Survey.LastModificationTime = DateTime.Now;
                SaveSurvey(dbEntry.Survey);
                // consistency!

                context.Questions.Remove(dbEntry);
                context.SaveChanges();
            }
            return dbEntry;
        }
        //!
        public void SaveAnswers(List<Answer> answers)
        {
            if (answers == null || answers.Count == 0)
            {
                return;
            }

            /*
            foreach(Answer ans in answers)
            {
                context.Answers.Add(ans);                     
            }                       
            context.SaveChanges();
            */

            // using SqlBulkCopy technology
            System.Data.DataTable table = new System.Data.DataTable();
            table.Columns.Add("Text", typeof(string));
            table.Columns.Add("UserRespondentID", typeof(int));
            table.Columns.Add("QuestionID", typeof(int));
            table.Columns.Add("RespondentID", typeof(int));

            for (int i = 0; i < answers.Count; ++i)
                table.Rows.Add(new object[] {
                            answers[i].Text,
                            answers[i].UserRespondentID,
                            answers[i].QuestionID,
                            answers[i].RespondentID
                });

            string connString = context.Database.Connection.ConnectionString;            

            using (System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(connString))
            {
                conn.Open();
                using (System.Data.SqlClient.SqlTransaction tran = conn.BeginTransaction())
                {
                    using (System.Data.SqlClient.SqlBulkCopy bulkCopy =
                            new System.Data.SqlClient.SqlBulkCopy(conn, System.Data.SqlClient.SqlBulkCopyOptions.Default, tran))
                    {
                        bulkCopy.DestinationTableName = "Answer";
                        bulkCopy.ColumnMappings.Add("Text", "Text");
                        bulkCopy.ColumnMappings.Add("UserRespondentID", "UserRespondentID");
                        bulkCopy.ColumnMappings.Add("QuestionID", "QuestionID");
                        bulkCopy.ColumnMappings.Add("RespondentID", "RespondentID");
                        bulkCopy.WriteToServer(table);
                    }
                    tran.Commit();
                }
            }
            context.Dispose();
            context = new SurveysContext();

            SaveQuestion(answers.First().Question); // consistency! 
        }

        public void SaveLinkConfiguration(LinkConfiguration LinkConfiguration)
        {
            if (LinkConfiguration.LinkConfigurationID == 0)
            {
                // Create workflow
                // Fully formed LinkConfigurations must come there (except for [key] = LinkConfigurationID)
                context.LinkConfigurations.Add(LinkConfiguration);
            }
            else
            {
                // Edit Workflow
                // Only several fields interest us here
                LinkConfiguration dbEntry = context.LinkConfigurations.Find(LinkConfiguration.LinkConfigurationID);
                if (dbEntry != null)
                {
                    dbEntry.Name = LinkConfiguration.Name;
                    dbEntry.LastModificationTime = LinkConfiguration.LastModificationTime;
                    dbEntry.Description = LinkConfiguration.Description;
                    dbEntry.Links = LinkConfiguration.Links;
                }
            }
            context.SaveChanges();
        }

        public LinkConfiguration DeleteLinkConfiguration(int LinkConfigurationID)
        {
            LinkConfiguration dbEntry = context.LinkConfigurations.Find(LinkConfigurationID);
            if (dbEntry != null)
            {
                context.LinkConfigurations.Remove(dbEntry);
                context.SaveChanges();
            }
            return dbEntry;
        }

        public void SaveLemmas(List<Lemma> lemmas)
        {
            if (lemmas == null || lemmas.Count == 0)
            {
                return;
            }
            /* Failed to use standard EF bulk insertion
             *
            context.Dispose();
            context = new SurveysContext();
            context.Configuration.AutoDetectChangesEnabled = false;
            context.Configuration.ValidateOnSaveEnabled = false;            
            for (int i = 0; i < lemmas.Count; ++i)
            {                                               
                context.Lemmas.Add(lemmas[i]);
            }
            context.ChangeTracker.DetectChanges();
            context.SaveChanges();
            */

            // using SqlBulkCopy technology

            System.Data.DataTable table = new System.Data.DataTable();
            table.Columns.Add("OpenCorporaLemma", typeof(int));
            table.Columns.Add("Word", typeof(string));
            table.Columns.Add("NormalizedWord", typeof(string));
            table.Columns.Add("AnswerID", typeof(int));
            table.Columns.Add("LinkConfigurationID", typeof(int));            

            for (int i = 0; i < lemmas.Count; ++i)
                table.Rows.Add(new object[] {
                            lemmas[i].OpenCorporaLemma,
                            lemmas[i].Word,
                            lemmas[i].NormalizedWord,
                            lemmas[i].AnswerID,
                            lemmas[i].LinkConfigurationID
                });

            string connString = context.Database.Connection.ConnectionString;            

            using (System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(connString))
            {
                conn.Open();
                using (System.Data.SqlClient.SqlTransaction tran = conn.BeginTransaction())
                {
                    using (System.Data.SqlClient.SqlBulkCopy bulkCopy =
                            new System.Data.SqlClient.SqlBulkCopy(conn, System.Data.SqlClient.SqlBulkCopyOptions.Default, tran))
                    {
                        bulkCopy.DestinationTableName = "Lemma";
                        bulkCopy.ColumnMappings.Add("OpenCorporaLemma", "OpenCorporaLemma");
                        bulkCopy.ColumnMappings.Add("Word", "Word");
                        bulkCopy.ColumnMappings.Add("NormalizedWord", "NormalizedWord");
                        bulkCopy.ColumnMappings.Add("AnswerID", "AnswerID");
                        bulkCopy.ColumnMappings.Add("LinkConfigurationID", "LinkConfigurationID");                        
                        bulkCopy.WriteToServer(table);
                    }
                    tran.Commit();
                }
            }

            context.Dispose();
            context = new SurveysContext();
        }

    }
}