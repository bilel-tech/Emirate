using EmirateHMBot.Models;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Web;
using System.Web.UI;
using iTextSharp.awt.geom;

namespace EmirateHMBot.Services
{
    public static class EservicesMohreService
    {

        public static ChromeDriver Driver;
        public static List<Employee> employees = new List<Employee>();
        static string allCookies;
        private static object HtmlWorker;

        public static async Task Authenticate()
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

            Driver.FindElement(By.XPath("//input[@id='txtUserName']")).SendKeys("mkassem1979");
            Driver.FindElement(By.XPath("//input[@id='txtPassword']")).SendKeys("Abcd@1234");
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
            //var cookies = Driver.Manage().Cookies.AllCookies;
            //foreach (var cookie in cookies)
            //{
            //    allCookies = allCookies + cookie.Name + "=" + cookie.Value+";";
            //}
            //allCookies.Remove(allCookies.LastIndexOf(";"));
            MessageBox.Show("login succes");

        }
        public static async Task<List<Employee>> GetEmplyeesInfo()
        {
            await Task.Delay(2000);
            Driver.Navigate().GoToUrl("https://eservices.mohre.gov.ae/enetwasal/rptComEmpList.aspx?comno=948292");/*352128 151518 948292*/
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
            catch (Exception)
            {
            }
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(Driver.PageSource);
            doc.Save("x.html");

            //WebClient client = new WebClient();
            //client.Headers.Add(HttpRequestHeader.Cookie, "cookies:" + allCookies);
            //using (client)
            //{
            //    client.DownloadFile("https://eservices.mohre.gov.ae/enetwasal/images/mollogo_small.jpg", "images/mollogo_small.jpg");
            //}
            Document document = new Document(PageSize.A4.Rotate(), 10f, 10f, 100f, 0f);
            PdfWriter.GetInstance(document, new FileStream("MySamplePDF.pdf", FileMode.Create));
            document.Open();
            HTMLWorker hw =new HTMLWorker(document);
            hw.Parse(new StringReader(Driver.PageSource));
            document.Close();


            //Process.Start("x.html");
            var nodesCodes = doc.DocumentNode.SelectNodes("//td[@width='195']");
            var nodesNames = doc.DocumentNode.SelectNodes("//td[@width='250']");
            var nodesNamesAndCodes = doc.DocumentNode.SelectNodes("//tr[@height='20']/following-sibling::tr");
            //var namesAndCodes = new Dictionary<string, string>();

            for (int i = 0; i < nodesNamesAndCodes.Count; i++)
            {

                var name = nodesNamesAndCodes[i].SelectSingleNode("./td[@width='250']/text()").InnerText;
                var cardCode = nodesNamesAndCodes[i].SelectSingleNode("./td[@width='195']/text()").InnerText;
                var personalCode = nodesNamesAndCodes[i].SelectSingleNode("./td[@width='95']/text()").InnerText;

                employees.Add(new Employee { PersonName = name, CardNbr = cardCode, PersonCode = personalCode });
            }

            return employees;
        }

    }
}
