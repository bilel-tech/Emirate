using EmirateHMBot.Models;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text;

namespace EmirateHMBot.Services
{
    public static class EservicesMohreService
    {

        public static ChromeDriver Driver;
        public static List<Employee> employees ;
        public static HtmlAgilityPack.HtmlDocument doc;
        static string allCookies;
        private static object HtmlWorker;

        public static async Task Authenticate(string userN, string passW)
        {
            var chromeDriverService = ChromeDriverService.CreateDefaultService();
            var chromeOptions = new ChromeOptions();
            //nairconcord@gmail.com
            //Concord@20702
            chromeDriverService.HideCommandPromptWindow = true;
            chromeOptions.AddArguments("headless");
            Driver = new ChromeDriver(chromeDriverService, chromeOptions);
            //return;
            Driver.Navigate().GoToUrl("https://eservices.mohre.gov.ae/enetwasal/login.aspx?lang=eng");
            Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);

            Driver.FindElement(By.XPath("//input[@id='txtUserName']")).SendKeys(userN);//mkassem1979
            Driver.FindElement(By.XPath("//input[@id='txtPassword']")).SendKeys(passW);//Abcd@1234
            Driver.FindElement(By.XPath("//input[@id='cmdlogin']")).Click();

            Driver.SwitchTo().Frame(Driver.FindElementById("PgSecurity1"));
            Driver.FindElement(By.XPath("//input[@id='txtAnswer']")).SendKeys("ajman");
            Driver.FindElement(By.XPath("//input[@id='btnSubmit2']")).Click();
            try
            {
                await Task.Delay(2000);
                //we see if the answer is not the right one and prompt for the answer again, in that case we use the second answer we have
                var errorMessage = Driver?.FindElement(By.XPath("//span[@id='lblMsg2']"))?.Text ?? "";
                Driver.FindElement(By.XPath("//input[@id='txtAnswer']")).Clear();
                Driver.FindElement(By.XPath("//input[@id='txtAnswer']")).SendKeys("emarat");
                Driver.FindElement(By.XPath("//input[@id='btnSubmit2']")).Click();

            }
            catch (Exception)
            {
                Console.WriteLine("the first answer worked");
            }
            var cookies = Driver.Manage().Cookies.AllCookies;
            foreach (var cookie in cookies)
            {
                allCookies = allCookies + cookie.Name + "=" + cookie.Value + ";";
            }
            allCookies.Remove(allCookies.LastIndexOf(";"));
            MessageBox.Show("login succes");

        }
        public static async Task<List<Employee>> GetEmplyeesInfo(string companyCode)
        {
            await Task.Delay(2000);
            Driver.Navigate().GoToUrl("https://eservices.mohre.gov.ae/enetwasal/arabic/rptComEmpList.aspx?comno="+ companyCode);//352128 151518 948292
            //Driver.Navigate().GoToUrl(@"C:\Users\MonsterComputer\Desktop\EmirateHMBot\EmirateHMBot\bin\Debug\x.html");
            do
            {
                try
                {
                    Driver.FindElement(By.XPath("//tr[@height='20']/following-sibling::tr"));
                    break;
                }
                catch (Exception)
                {

                    await Task.Delay(500);
                }
            } while (true);
            try
            {
                Driver.SwitchTo().Alert().Accept();
                Driver.SwitchTo().DefaultContent();
                var xxx = Driver.FindElementById("btnNext").Text;
                Console.WriteLine("hello: " + xxx);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            //WebClient client = new WebClient();
            //client.Headers.Add(HttpRequestHeader.Cookie, "cookies:" + allCookies);
            //using (client)
            //{
            //    client.DownloadFile("https://eservices.mohre.gov.ae/enetwasal/images/mollogo_small.jpg", "images/mollogo_small.jpg");
            //}

            doc = new HtmlAgilityPack.HtmlDocument();
            var arabic = Encoding.UTF8;
            var bytes = arabic.GetBytes(Driver.PageSource);
            var html = arabic.GetString(bytes).Replace("../images/", "").Replace("../include/", "");
            File.WriteAllText("x.html", html);
            //var html = File.ReadAllText("x.html");
            doc.LoadHtml(html);

            employees = new List<Employee>();
            var nodesNamesAndCodes = doc.DocumentNode.SelectNodes("//tr[@height='20']/following-sibling::tr");
            //var namesAndCodes = new Dictionary<string, string>();

            for (int i = 0; i < nodesNamesAndCodes.Count; i++)
            {

                var name = nodesNamesAndCodes[i].SelectSingleNode("./td[@width='250']/text()").InnerText;
                var cardCode = nodesNamesAndCodes[i].SelectSingleNode("./td[@width='195']/text()").InnerText;
                var personalCode = nodesNamesAndCodes[i].SelectSingleNode("./td[@width='100']/text()").InnerText;

                employees.Add(new Employee { PersonName = name, CardNbr = cardCode, PersonCode = personalCode });
            }

            return employees;
        }

    }
}
