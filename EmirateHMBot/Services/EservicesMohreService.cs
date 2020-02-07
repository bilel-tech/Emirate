using EmirateHMBot.Models;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text;
using System.Threading.Tasks.Dataflow;
using System.Reflection;
using Newtonsoft.Json;

namespace EmirateHMBot.Services
{
    public static class EservicesMohreService
    {
        //8KNAT0KH save code for payoneer
        public static HttpCaller httpCaller = new HttpCaller();
        public static ChromeDriver Driver;
        public static CompanyInfo companyInfo;
        public static List<Employee> employees;
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
            //var cookies = Driver.Manage().Cookies.AllCookies;
            //foreach (var cookie in cookies)
            //{
            //    allCookies = allCookies + cookie.Name + "=" + cookie.Value + ";";
            //}
            //allCookies.Remove(allCookies.LastIndexOf(";"));
            MessageBox.Show("login succes");

        }
        public static async Task<List<Employee>> GetEmplyeesInfo(string companyCode)
        {
            await Task.Delay(2000);
            Driver.Navigate().GoToUrl("https://eservices.mohre.gov.ae/enetwasal/arabic/rptComEmpList.aspx?comno=" + companyCode);//352128 151518 948292
                                                                                                                                 //Driver.Navigate().GoToUrl(@"C:\Users\MonsterComputer\Desktop\EmirateHMBot\EmirateHMBot\bin\Debug\x.html");
            employees = new List<Employee>();
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

            doc = new HtmlAgilityPack.HtmlDocument();

            var bytes = Encoding.UTF8.GetBytes(Driver.PageSource);
            var html = Encoding.UTF8.GetString(bytes);

            File.WriteAllText("x.html", html);
            var nbrOfEmployees = int.Parse(Driver.FindElement(By.XPath("//td[text()='مجموع عدد العمال']/following-sibling::td")).Text.Trim());
            if (nbrOfEmployees > 250)
            {
                var companyName = Driver.FindElement(By.XPath("//td[text()='إسم الشركة']/following-sibling::td")).Text.Trim();
                var companyCategory = Driver.FindElement(By.XPath("//td[text()='الفئة']/following-sibling::td")).Text.Trim();
                var dateOfImprimate = Driver.FindElement(By.XPath("//td[contains(text(),'طبعت في')]")).Text.Trim().Split(':');
                var date = dateOfImprimate[1];
                int index = 0;
                do
                {
                    doc.LoadHtml(Driver.PageSource);

                    var employeesNodes = doc.DocumentNode.SelectNodes("//tr[@height='20']/following-sibling::tr");
                    for (int i = 0; i < employeesNodes.Count; i++)
                    {
                        index++;
                        var name = employeesNodes[i].SelectSingleNode("./td[@width='250']/text()").InnerText;
                        var cardCode = employeesNodes[i].SelectSingleNode("./td[@width='195']/text()").InnerText;
                        var cardStatut = employeesNodes[i].SelectSingleNode("./td[@width='195']/text()[2]").InnerText;
                        var cardDate = employeesNodes[i].SelectSingleNode("./td[@width='195']/text()[3]").InnerText;
                        var personalCode = employeesNodes[i].SelectSingleNode("./td[@width='100']/text()").InnerText;
                        var job = employeesNodes[i].SelectSingleNode("./td[@width='150']/text()").InnerText;
                        var passportNbr = employeesNodes[i].SelectSingleNode("./td[@width='90']/text()").InnerText;
                        var passporCountry = employeesNodes[i].SelectSingleNode("./td[@width='90']/text()[2]").InnerText;

                        employees.Add(new Employee
                        {
                            PersonName = name,
                            CardNbr = cardCode,
                            PersonCode = personalCode,
                            Job = job,
                            PassportNumber = passportNbr,
                            PassportCountry = passporCountry,
                            CardStatut = cardStatut,
                            CardDate = cardDate
                        });
                    }
                    if (index == nbrOfEmployees)
                    {
                        break;
                    }

                    Driver.FindElement(By.XPath("//input[@value='بعد']")).Click();
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

                } while (true);

                //SaveBrutHtml(companyCategory, nbrOfEmployees, companyName, date, companyCode);
                return employees;
            }


            else
            {
                html = html/*.Replace("../include/stylesArb.css", Path.GetFullPath("stylesArb.css"))*/.Replace("../images/mollogo_small.jpg", Path.GetFullPath("mollogo_small.jpg"));
                doc.LoadHtml(html);
                var employeesNodes1 = doc.DocumentNode.SelectNodes("//tr[@height='20']/following-sibling::tr");
                //var namesAndCodes = new Dictionary<string, string>();

                for (int i = 0; i < employeesNodes1.Count; i++)
                {

                    var name = employeesNodes1[i].SelectSingleNode("./td[@width='250']/text()").InnerText;
                    var cardCode = employeesNodes1[i].SelectSingleNode("./td[@width='195']/text()").InnerText;
                    var personalCode = employeesNodes1[i].SelectSingleNode("./td[@width='100']/text()").InnerText;

                    employees.Add(new Employee { PersonName = name, CardNbr = cardCode, PersonCode = personalCode });
                }
                doc.DocumentNode.SelectSingleNode("//*[@id='ContentDiv']/table/tbody/tr/td/table[2]/tbody/tr/td[1]/table").Remove();
                doc.DocumentNode.SelectSingleNode("//*[@id='ContentDiv']/table/tbody/tr/td/table[2]/tbody/tr/td[1]").Remove();
                File.WriteAllText("x.html", doc.DocumentNode.OuterHtml);
            }

            return employees;
        }

        public static void SaveBrutHtml(List<Employee> employees, CompanyInfo companyInfo)
        {
            var newTr = false;
            var firstpageformat = File.ReadAllText("firstpageformat.txt").Replace("DateImp", companyInfo.DateOfImprimate).Replace("DateTimeImp", companyInfo.DateOfImprimate).Replace("category", companyInfo.CompanyCategory).Replace("companyCode", companyInfo.CompanyCode).Replace("nbrEmployees", companyInfo.NbrOfEmployees);
            //var secondeTab = "<table dir=\"rtl\" border=\"0\" cellpadding=\"2\" width=\"675\"><tbody><tr height=\"20\"><td width=\"30\" align=\"center\" class=\"head22\">رقم</td><td width=\"90\" align=\"center\" class=\"head22\">الرقم الشخصي</td><td width=\"275\" align=\"center\" class=\"head22\">اسم الشخص</td><td width=\"150\" align=\"center\" class=\"head22\">المسمى الوظيفي</td><td width=\"80\" align=\"center\" class=\"head22\">بيانات جواز السفر</td><td width=\"200\" align=\"center\" class=\"head22\">بيانات بطاقة العمل</td>";
            var employeeNodeFormat = File.ReadAllText("employeeNodeFormat.txt")/*.Replace("", "").Replace("", "").Replace("", "").Replace("", "").Replace("", "").Replace("", "").Replace("", "").Replace("", "")*/;
            var elements ="";
            StringBuilder nodes = new StringBuilder(firstpageformat);
            var index = 1;
            for (int i = 0; i < employees.Count; i++)
            {
                elements =  employeeNodeFormat.Replace("CardNbr", employees[i].CardNbr).Replace("PassportNumber", employees[i].PassportNumber).Replace("CardStatut", employees[i].CardStatut).Replace("job", employees[i].Job).Replace("employeeName", employees[i].PersonName).Replace("employeeCode", employees[i].PersonCode).Replace("rowNbr", (i+1)+"").Replace("PassportCountry", employees[i].PassportCountry).Replace("CardDate", employees[i].CardDate);
                nodes.Append(elements);
                if (newTr)
                {
                    index++;
                    if (index == 17)
                    {
                        //elements = elements + "</tr></tbody></table><h3></h3>";
                        //nodes = nodes + elements;
                        //elements = firstTab;
                        index = 0;
                    }

                }
                if ((i + 1) == 13)
                {
                    nodes.Append( $"<div style=\"position: absolute; left: 26.45px; top: 767.00px\" class=\"cls_002\"><span class=\"cls_002\"> </span><a >https://eservices.mohre.gov.ae/enetwasal/arabic/rptComEmpList.aspx?comno={companyInfo.CompanyCode}</a> </div>< div style = \"position:absolute;left:569.95px;top:767.00px\" class=\"cls_002\"><span class=\"cls_002\">1/15</span></div></div>");
                    //nodes = nodes + elements;
                    //newTr = true;
                    //elements = firstTab;
                    index = 0;
                }

            }
            //nodes = nodes + firstTab + elements;
            //doc.LoadHtml(File.ReadAllText("htmlTemplate.html"));
            //var htmlTemplateString = doc.DocumentNode.OuterHtml.Replace("30/01/2020 01:41:24", date);
            //htmlTemplateString = htmlTemplateString.Replace("bilel", nodes);
            //var bytes1 = Encoding.UTF8.GetBytes(htmlTemplateString);
            //htmlTemplateString = Encoding.UTF8.GetString(bytes1);
            //File.WriteAllText("x.html", htmlTemplateString);

        }

        public static async Task<List<RequiredCompany>> GetRequiredCompanies(List<string> companiesCode, int moreThen, int lessThen)
        {
            await Task.Delay(2000);
            Driver.Navigate().GoToUrl("https://eservices.mohre.gov.ae/enetwasal/home.aspx");
            var cookies = Driver.Manage().Cookies.AllCookies;
            foreach (var cookie in cookies)
            {
                allCookies = allCookies + cookie.Name + "=" + cookie.Value + ";";
            }
            allCookies.Remove(allCookies.LastIndexOf(";"));

            httpCaller.cookies = allCookies;

            var requiredCompanies = new List<RequiredCompany>();
            var tpl = new TransformBlock<string, (RequiredCompany companyEmployees, string error)>
              (async x => await GetcompanyEmployees(x).ConfigureAwait(false),
              new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = 20 });
            foreach (var companyCode in companiesCode)
                tpl.Post(companyCode);
            var listCompaniesStatut = new List<CompanyStatut>();
            foreach (var companyCode in companiesCode)
            {
                var response = await tpl.ReceiveAsync();
                if (response.error != null)
                    continue;
                if (response.companyEmployees.EmployeesNbr > moreThen && response.companyEmployees.EmployeesNbr < lessThen)
                {
                    requiredCompanies.Add(response.companyEmployees);
                }

            }
            Console.WriteLine(requiredCompanies.Count);
            return requiredCompanies;
        }

        private static async Task<(RequiredCompany companyEmployees, string error)> GetcompanyEmployees(string cmpanyCode)
        {
            var company = new RequiredCompany();
            var response = await httpCaller.GetDoc1("https://eservices.mohre.gov.ae/enetwasal/arabic/rptComEmpList.aspx?comno=" + cmpanyCode);
            if (response.error != null)
                return (null, response.error);
            company.CompanyCode = cmpanyCode;
            try
            {
                company.EmployeesNbr = int.Parse(response.doc.DocumentNode.SelectSingleNode("//td[text()='مجموع عدد العمال']/following-sibling::td").InnerText.Trim());
            }
            catch (Exception)
            {
                return (null, "invalid code");
            }
            return (company, null);
        }
    }
}
