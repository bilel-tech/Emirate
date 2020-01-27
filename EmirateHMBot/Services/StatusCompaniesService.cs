using CsvHelper;
using EmirateHMBot.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using System.Windows.Forms;

namespace EmirateHMBot.Services
{
    public static class StatusCompaniesService
    {
        public static HttpCaller HttpCaller = new HttpCaller();
        public static async Task GetCompaniesStaus()
        {
            var companies = new List<string>();
            try
            {
                using (var reader = new StreamReader("Comapnies code.csv"))
                {
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        var values = line.Split(';');
                       
                        companies.Add(values[0]);
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("No such file \"Comapnies code.csv file\" or it is not closed");
                return;
            }

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
            using (var writer = new StreamWriter("companies statut.csv"))
            using (var csv = new CsvWriter(writer))
            {
                csv.WriteRecords(listCompaniesStatut);
            }
        }

        private static async Task<(CompanyStatut companyStatut, string error)> GetStatut(string companyCode)
        {
            var companyStatut = new CompanyStatut();
            var response = await HttpCaller.PostJson("https://www.mohre.gov.ae/services/AjaxHandler.asmx/LoadServiceResult", "{\"languageId\":\"1\",\"languageCode\":\"en-GB\",\"keywords\":\"" + companyCode + "\",\"method\":\"CI\"}");
            if (response.error != null)
                return (null, response.error);
            var Object = JObject.Parse(response.json);
            companyStatut.CompanyName = (string)Object.SelectToken("d.CompanyName");
            companyStatut.CompanyCode = (string)Object.SelectToken("d.CompanyNumber");
            companyStatut.CompanyStatus = (string)Object.SelectToken("d.CompanyStatus");
            return (companyStatut, null);
        }
    }
}
