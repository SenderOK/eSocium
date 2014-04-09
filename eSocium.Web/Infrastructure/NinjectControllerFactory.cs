﻿using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing; 
using Ninject;
using eSocium.Domain.Concrete;
using eSocium.Domain.Abstract;

namespace eSocium.Web.Infrastructure
{
    public class NinjectControllerFactory : DefaultControllerFactory
    {
        private IKernel ninjectKernel;

        public NinjectControllerFactory()
        {
            ninjectKernel = new StandardKernel();
            AddBindings();
        }

        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
 	        return controllerType == null 
                ? null
                :(IController)ninjectKernel.Get(controllerType);
        }

        private void AddBindings()
        {
            ninjectKernel.Bind<ISurveyRepository>().To<EFSurveyRepository>();
        }
    }
}