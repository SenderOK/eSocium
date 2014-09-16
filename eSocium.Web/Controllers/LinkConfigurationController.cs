using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eSocium.Domain.Abstract;
using eSocium.Domain.Entities;
using eSocium.Web.Models;
using System.Text;

namespace eSocium.Web.Controllers
{
    public class LinkConfigurationController : Controller
    {
        private ISurveyRepository repository;

        public LinkConfigurationController(ISurveyRepository repo)
        {
            repository = repo;
        }

        //
        // GET: /LinkConfiguration/

        public ActionResult Index()
        {
            IEnumerable<LinkConfiguration> LinkConfigurations = repository.LinkConfigurations
                .Where(c => c.CreatorName == User.Identity.Name);
            return View(LinkConfigurations);
        }

        public ActionResult Edit(int LinkConfigurationID)
        {
            LinkConfiguration configuration = repository.LinkConfigurations
                .FirstOrDefault(c => c.LinkConfigurationID == LinkConfigurationID);
            if (configuration == null || configuration.CreatorName != User.Identity.Name)
            {
                return HttpNotFound();
            }
            return View(new LinkConfigurationViewModel(configuration));
        }

        [HttpPost]
        public ActionResult Edit(LinkConfigurationViewModel m)
        {
            TextNormalizer.LinkConfiguration c = new TextNormalizer.LinkConfiguration();
            List<int> linkset = m.CheckedLinks();
            if (!c.setConf(linkset))
            {
                // invalid linkset
                TempData["message"] = string.Format("Invalid Link Configuration");
                return View(m);
            }

            if (ModelState.IsValid) {
                m.LinkConfiguration.LastModificationTime = DateTime.Now;

                // make a string from a linkset
                StringBuilder builder = new StringBuilder();
                foreach (int n in linkset)
                {                    
                    builder.Append(n).Append(" ");
                }                
                m.LinkConfiguration.Links = builder.ToString().Trim();
                repository.SaveLinkConfiguration(m.LinkConfiguration);
                TempData["message"] = string.Format("Link configuration {0} is successfully saved!", m.LinkConfiguration.Name);
                return RedirectToAction("Index");
            } else {
                // there is something wrong with the data values
                return View(m);
            }
        }

        public ActionResult Create()
        {
            LinkConfiguration LinkConfiguration = new LinkConfiguration();
            LinkConfiguration.LastModificationTime = DateTime.Now;
            LinkConfiguration.CreationTime = DateTime.Now;
            LinkConfiguration.CreatorName = User.Identity.Name;
            LinkConfiguration.Links = "";
            return View("Edit", new LinkConfigurationViewModel(LinkConfiguration));
        }

        public ActionResult Delete(int LinkConfigurationID)
        {
            LinkConfiguration LinkConfiguration = repository.LinkConfigurations
                .FirstOrDefault(c => c.LinkConfigurationID == LinkConfigurationID);
            if (LinkConfiguration == null || LinkConfiguration.CreatorName != User.Identity.Name)
            {
                return HttpNotFound();
            }
            LinkConfiguration = repository.DeleteLinkConfiguration(LinkConfigurationID);
            TempData["message"] = string.Format("Link configuration {0} is successfully deleted!", LinkConfiguration.Name);
            return RedirectToAction("Index");
        }
    }    
}
