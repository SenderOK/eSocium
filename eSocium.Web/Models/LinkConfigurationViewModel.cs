using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using eSocium.Domain.Entities;

namespace eSocium.Web.Models
{
    public class LinkConfigurationViewModel
    {
        public List<SelectLinkEditorViewModel> Links { get; set; }
        public LinkConfiguration LinkConfiguration { get; set; }
        
        public LinkConfigurationViewModel()
        {
            LinkConfiguration = new LinkConfiguration();
            Links = new List<SelectLinkEditorViewModel>();
        }        

        public LinkConfigurationViewModel(LinkConfiguration _LinkConfiguration)
        {
            LinkConfiguration = _LinkConfiguration;           
            Links = new List<SelectLinkEditorViewModel> 
            {
                new SelectLinkEditorViewModel() {Selected = false, Type = -1, Description = "Переход от краткого прилагательного к полному", Example = "исполнителен -> исполнительный"}, 
                new SelectLinkEditorViewModel() {Selected = false, Type = -2, Description = "Переход от компаратива прилагательного к начальной форме", Example = "исполнительнее -> исполнительный"},
                new SelectLinkEditorViewModel() {Selected = false, Type = -3, Description = "Переход от личной формы глагола к инфинитиву", Example = "исполняю -> исполнять"},
                new SelectLinkEditorViewModel() {Selected = false, Type = -5, Description = "Переход от отглагольного причастия к инфинитиву глагола", Example = "адаптированный -> адаптировать"},
                new SelectLinkEditorViewModel() {Selected = false, Type = -6, Description = "Переход от деепричастия к инфинитиву глагола", Example = "адаптируя -> адаптировать"},
                new SelectLinkEditorViewModel() {Selected = false, Type = -4, Description = "Переход от краткого причастия к полному", Example = "прошептан -> прошёптанный"},
                new SelectLinkEditorViewModel() {Selected = false, Type = -7, Description = "Переход от отчества к соответствующему имени собственному", Example = "Иванович -> Иван"},
                new SelectLinkEditorViewModel() {Selected = false, Type = -8, Description = "Переход от отчества женского рода к отчеству мужского рода", Example = "Петровна -> Петрович"},
                new SelectLinkEditorViewModel() {Selected = false, Type = -9, Description = "Переход от фамилии женского рода к фамилии мужского рода", Example = "Иванова -> Иванов"},
                new SelectLinkEditorViewModel() {Selected = false, Type = 10, Description = "Переход от фамилии мужского рода к форме множественного числа фамилии", Example = "Иванов -> Ивановы"},
                new SelectLinkEditorViewModel() {Selected = false, Type =-11, Description = "Переход от несовершенного вида глагола/причастия/деепричастия к совершенному", Example = "искать -> найти"},
                new SelectLinkEditorViewModel() {Selected = false, Type = 11, Description = "Переход от совершенного вида глагола/причастия/деепричастия к несовершенному", Example = "найти -> искать"},                
                new SelectLinkEditorViewModel() {Selected = false, Type =-13, Description = "Переход от мужского разговорного отчества к формальному", Example = "Иваныч -> Иванович"},
                new SelectLinkEditorViewModel() {Selected = false, Type =-14, Description = "Переход от женского разговорного отчества к формальному", Example = "Алексевна -> Алексеевна"},
                new SelectLinkEditorViewModel() {Selected = false, Type =-15, Description = "Переход от превосходной степени прилагательного с суффиксом -ейш- и приставкой наи- к форме без приставки", Example = "наиактивнейший -> активнейший"},
                new SelectLinkEditorViewModel() {Selected = false, Type =-17, Description = "Переход от превосходной степени прилагательного с суффиксом -айш- и приставкой наи- к форме без приставки", Example = "наивеличайший -> величайший"},

                new SelectLinkEditorViewModel() {Selected = false, Type =-12, Description = "Переход от превосходной формы прилагательного к начальной форме", Example = "сильнейший -> сильный"},
                //new SelectLinkEditorViewModel {Selected = false, Type =-16, Description = "Переход от превосходной формы прилагательного к начальной форме", Example = "исполнительнее -> исполнительный"},
                //new SelectLinkEditorViewModel {Selected = false, Type =-18, Description = "Переход от превосходной формы прилагательного к начальной форме", Example = "исполнительнее -> исполнительный"},
                //new SelectLinkEditorViewModel {Selected = false, Type =-19, Description = "Переход от превосходной формы прилагательного к начальной форме", Example = "исполнительнее -> исполнительный"},
                //new SelectLinkEditorViewModel {Selected = false, Type =-20, Description = "Переход от превосходной формы прилагательного к начальной форме", Example = "исполнительнее -> исполнительный"},

                new SelectLinkEditorViewModel() {Selected = false, Type =-21, Description = "Переход от прилагательного с приставкой не- к форме без приставки", Example = "неинтересный -> интересный"},
                new SelectLinkEditorViewModel() {Selected = false, Type =-22, Description = "Переход от прилагательного с приставкой без- к форме без приставки", Example = "безинтересный -> интересный"},
                new SelectLinkEditorViewModel() {Selected = false, Type =-23, Description = "Переход от существительного с приставкой не- к форме без приставки", Example = "недруг -> друг"},
                new SelectLinkEditorViewModel() {Selected = false, Type =-24, Description = "Переход от существительного с приставкой без- к форме без приставки", Example = "бездействие -> действие"}
            };

            if (_LinkConfiguration.Links.Trim() != "")
            {
                foreach (string link in _LinkConfiguration.Links.Trim().Split())
                {
                    int n = Convert.ToInt32(link);
                    foreach (SelectLinkEditorViewModel linkSelect in Links)
                    {
                        if (linkSelect.Type == n)
                        {
                            linkSelect.Selected = true;
                            break;
                        }
                    }
                }
            }
        }

        public List<int> CheckedLinks()
        {
            List<int> result = new List<int>();
            foreach (SelectLinkEditorViewModel l in Links)
            {
                if (l.Selected) 
                {
                    result.Add(l.Type);
                    if (l.Type == -12) 
                    {
                        // все ссылки для перехода от превосходной формы прилагательного к начальной форме
                        result.Add(-16);
                        result.Add(-18);
                        result.Add(-19);
                        result.Add(-20);
                    }
                }
            }
            return result;
        }
        
    }
}