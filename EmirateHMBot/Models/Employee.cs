using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmirateHMBot.Models
{
    public class Employee
    {
        public string PersonName { get; set; }
        public string PersonCode { get; set; }
        public string CardNbr { get; set; }
        public string Job { get; set; }
        public string PassportNumber { get; set; }
        public string PassportCountry { get; set; }
        public string CardStatut { get; set; }
        public string CardDate { get; set; }
        public string __VIEWSTATE { get; set; }
        public string __VIEWSTATEGENERATOR { get; set; }
        public string __EVENTVALIDATION { get; set; }
        public string TypePage { get; set; } = "pages";
        public string TypeNews { get; set; } = "news";
        public string TypeEvents { get; set; } = "events";
        public string TypeFaqs { get; set; } = "faqs";
        public string TypeService { get; set; } = "service";
        public string btnNext { get; set; } = "Next";
    }
}
