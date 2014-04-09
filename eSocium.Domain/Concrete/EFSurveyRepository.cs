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

        public Answer SaveAnswer(Answer answer)
        {
            Answer dbEntry;
            if (answer.AnswerID == 0)
            {
                // Create workflow
                // Fully formed surveys must come there (except for [key] = SurveyID)
                dbEntry = context.Answers.Add(answer);
            }
            else
            {
                dbEntry = context.Answers.Find(answer.AnswerID);
                if (dbEntry != null)
                {
                    dbEntry.Text = answer.Text;
                }
            }
            context.SaveChanges();

            SaveQuestion(answer.Question); // consistency!
            return dbEntry;

        }
    }
}