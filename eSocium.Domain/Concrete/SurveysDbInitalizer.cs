using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Data.Entity;
using eSocium.Domain.Entities;

namespace eSocium.Domain.Concrete
{
    public class SurveysDbInitalizer : DropCreateDatabaseIfModelChanges<SurveysContext>
    {
        protected override void Seed(SurveysContext context)
        {
            context.Surveys.Add(new Survey 
            { 
                Name = "Test Survey",
                CreatorName = "Nikita",
                CreationTime = DateTime.Now,
                LastModificationTime = DateTime.Now 
            });

            //context.Surveys.Add(new Survey
            //{
            //    Name = "Another Test Survey",
            //    CreatorName = "Nikita",
            //    CreationTime = DateTime.Now,
            //    LastModificationTime = DateTime.Now
            //});
            base.Seed(context);
        }
    }
}
