using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmirateHMBot.Services
{
    public static class EservicesMohreService
    {
        public static ChromeDriver Driver;

        public static async Task Authenticate()
        {
            var chromeDriverService = ChromeDriverService.CreateDefaultService();
            chromeDriverService.HideCommandPromptWindow = true;
            Driver = new ChromeDriver(chromeDriverService);
            Driver.Navigate().GoToUrl("https://eservices.mohre.gov.ae/enetwasal/login.aspx?lang=eng");
            Driver.FindElement(By.XPath("//input[@id='txtUserName']")).SendKeys("mkassem1979");
            Driver.FindElement(By.XPath("//input[@id='txtPassword']")).SendKeys("Abcd@1234");
            Driver.FindElement(By.XPath("//input[@id='cmdlogin']")).Click();

            Driver.SwitchTo().Frame(Driver.FindElementById("PgSecurity1"));
            Driver.FindElement(By.XPath("//input[@id='txtAnswer']")).SendKeys("ajman");
            Driver.FindElement(By.XPath("//input[@id='btnSubmit2']")).Click();
            Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(4);
            try
            {
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
            Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(0);
            await Task.Delay(3000);
        }

        public static async Task GetEmplyeesIds()
        {
            Driver.Navigate().GoToUrl("https://eservices.mohre.gov.ae/enetwasal/rptComEmpList.aspx?comno=948292");
            await Task.Delay(4000);
            
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
            Process.Start("x.html");
            var codes = doc.DocumentNode.SelectNodes("//td[@width='195']");
            foreach (var code in codes)
            {
                var c = code.SelectSingleNode("./text()").InnerText;
                Console.WriteLine(c);
            }
        }

    }
}
