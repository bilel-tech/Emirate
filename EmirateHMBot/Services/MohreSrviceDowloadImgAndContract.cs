using EmirateHMBot.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace EmirateHMBot.Services
{
    public static class MohreSrviceDowloadImgAndContract

    {
        public static async Task<string> DownloadImage(Employee employee)
        {
            string error = "";
            var client = new WebClient();

            try
            {
                using (client)
                {
                    client.DownloadFileAsync(new Uri("http://eservices.mohre.gov.ae/NewMolGateway/LabourCardPrint.aspx?cardNo=" + employee.PersonCode), $@"labord cards\{employee.PersonName}-{employee.PersonCode}.jpg");
                }
            }
            catch (Exception ex)
            {
                error = ex.ToString();
                return error;
            }
            return error;
        }
        public static async Task<string> DownloadContract(Employee Employee)
        {
            string error = "";
            var countryCode = Employee.PersonCode.Substring(0, 3);
            countryCode = int.Parse(countryCode) + "";

            var dateOfbirth = Employee.PersonCode.Substring(3, 6);
            if (dateOfbirth[dateOfbirth.Length - 2] != '0')
            {
                dateOfbirth = dateOfbirth[0] + "" + dateOfbirth[1] + "/" + dateOfbirth[2] + dateOfbirth[3] + "/" + "19" + dateOfbirth[dateOfbirth.Length - 2] + dateOfbirth[dateOfbirth.Length - 1];
                Console.WriteLine(dateOfbirth);
            }
            else
            {
                dateOfbirth = dateOfbirth[0] + "" + dateOfbirth[1] + "/" + dateOfbirth[2] + dateOfbirth[3] + "/" + "20" + dateOfbirth[dateOfbirth.Length - 2] + dateOfbirth[dateOfbirth.Length - 1];
                Console.WriteLine(dateOfbirth);
            }
            
            var format = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("__VIEWSTATE",Employee.__VIEWSTATE),
                new KeyValuePair<string, string>("__VIEWSTATEGENERATOR",Employee.__VIEWSTATEGENERATOR),
                new KeyValuePair<string, string>("__EVENTVALIDATION",Employee.__EVENTVALIDATION),
                new KeyValuePair<string, string>("type",Employee.TypePage),
                new KeyValuePair<string, string>("type",Employee.TypeNews),
                new KeyValuePair<string, string>("type",Employee.TypeEvents),
                new KeyValuePair<string, string>("type",Employee.TypeFaqs),
                new KeyValuePair<string, string>("type",Employee.TypeService),
                new KeyValuePair<string, string>("txtLabourCardNo",Employee.CardNbr),
                new KeyValuePair<string, string>("txtPersonCode",Employee.PersonCode),
                new KeyValuePair<string, string>("txtDOB",dateOfbirth),
                new KeyValuePair<string, string>("drpLabNat",countryCode),
                new KeyValuePair<string, string>("btnNext","Next"),
            };
            var response = await HttpCaller.PostFormData("https://eservices.mohre.gov.ae/enetwasal/employeeCredential.aspx?emprequestType=2", format);
            if (response.error != null)
            {
                return error;
            }
            using (var x = File.Create("labord contracts/"+ Employee.PersonName +"-"+ Employee.CardNbr+".pdf"))
            {
                await response.html.CopyToAsync(x);
            }
            return error;
        }
    }
}
