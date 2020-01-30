using EmirateHMBot.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace EmirateHMBot.Services
{
    public static class StatusCompaniesService
    {
        public static HttpCaller HttpCaller = new HttpCaller();
        public static async Task<List<CompanyStatut>> GetCompaniesStaus(List<string> companies)
        {


            var tpl = new TransformBlock<string, (CompanyStatut companyStatut, string error)>
              (async x => await GetStatut(x).ConfigureAwait(false),
              new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = 20 });
            foreach (var company in companies)
                tpl.Post(company);
            var listCompaniesStatut = new List<CompanyStatut>();
            foreach (var company in companies)
            {
                var response = await tpl.ReceiveAsync().ConfigureAwait(false);
                if (response.error != null)
                    continue;
                if (response.companyStatut.CompanyStatus == "PRIVATE")
                    continue;
                listCompaniesStatut.Add(response.companyStatut);
            }
            return listCompaniesStatut;
        }

        private static async Task<(CompanyStatut companyStatut, string error)> GetStatut(string companyCode)
        {
            var companyStatut = new CompanyStatut();
            var response = await HttpCaller.PostJson("https://www.mohre.gov.ae/services/AjaxHandler.asmx/LoadServiceResult", "{\"languageId\":\"1\",\"languageCode\":\"en-GB\",\"keywords\":\"" + companyCode + "\",\"method\":\"CI\"}");
            if (response.error != null)
                return (null, response.error);
            var Object = new JObject();
            try
            {
                Object = JObject.Parse(response.json);
            }
            catch (Exception)
            {

                return (null,"invalid company code");
            }
            companyStatut.CompanyName = (string)Object.SelectToken("d.CompanyName");
            companyStatut.CompanyCode = (string)Object.SelectToken("d.CompanyNumber");
            companyStatut.CompanyStatus = (string)Object.SelectToken("d.CompanyStatus");
            return (companyStatut, null);
        }
    }
}
