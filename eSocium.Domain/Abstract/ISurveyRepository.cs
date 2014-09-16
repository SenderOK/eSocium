using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using eSocium.Domain.Entities;

namespace eSocium.Domain.Abstract
{
    public interface ISurveyRepository
    {
        IQueryable<Survey> Surveys { get; }
        IQueryable<Question> Questions { get; }
        IQueryable<Answer> Answers { get; }
        IQueryable<LinkConfiguration> LinkConfigurations { get; }

        void SaveSurvey(Survey survey);
        Survey DeleteSurvey(int surveyID);

        Question SaveQuestion(Question question);
        Question DeleteQuestion(int questioID);

        Answer SaveAnswer(Answer answer);

        void SaveLinkConfiguration(LinkConfiguration LinkConfiguration);
        LinkConfiguration DeleteLinkConfiguration(int LinkConfigurationID);

        Lemma SaveLemma(Lemma lemma);
    }
}
