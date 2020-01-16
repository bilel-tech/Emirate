using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmirateHMBot.Models
{
    static public class SaveFromPermitRenewEid
    {
        public static IWebDriver EidDriver;
        public static EID Eid;
        public static async void NaviagetToEIDAsync(IWebDriver Driver, EID eid)
        {
            EidDriver = Driver;
            //https://eform.emiratesid.ae/#1579105284625-17

            Eid = eid;
            saveRenenewEidStep1();
            do
            {
                try
                {
                    var nexPage = EidDriver.FindElement(By.XPath("//h1[@class='mendixFormTitle_title']")).Text;
                    if (nexPage.Contains("2"))
                        break;
                }
                catch (Exception)
                {
                    await Task.Delay(500);
                    continue;
                }

            } while (true);
            saveRenenewEidStep2();
            //return;
            do
            {
                try
                {
                    var nexPage = EidDriver.FindElement(By.XPath("//h1[@class='mendixFormTitle_title']")).Text;
                    if (nexPage.Contains("3"))
                        break;
                }
                catch (Exception)
                {
                    await Task.Delay(500);
                    continue;
                }

            } while (true);
            saveRenenewEidStep3();
            //return;
            do
            {
                try
                {
                    var nexPage = EidDriver.FindElement(By.XPath("//h1[@class='mendixFormTitle_title']")).Text;
                    if (nexPage.Contains("4"))
                        break;
                }
                catch (Exception)
                {
                    await Task.Delay(500);
                    continue;
                }

            } while (true);
            saveRenenewEidStep4();
            //return;
            do
            {
                try
                {
                    var nexPage = EidDriver.FindElement(By.XPath("//h1[@class='mendixFormTitle_title']")).Text;
                    if (nexPage.Contains("5"))
                        break;
                }
                catch (Exception)
                {
                    await Task.Delay(500);
                    continue;
                }

            } while (true);
            saveRenenewEidStep5();
            do
            {
                try
                {
                    var nexPage = EidDriver.FindElement(By.XPath("//h1[@class='mendixFormTitle_title']")).Text;
                    if (nexPage.Contains("6"))
                        break;
                }
                catch (Exception)
                {
                    await Task.Delay(500);
                    continue;
                }

            } while (true);
            saveRenenewEidStep6();
        }
        public static void saveRenenewEidStep1()
        {
            //EidDriver.Navigate().GoToUrl(@"C:\Users\MonsterComputer\Desktop\new\step1.html");
            EidDriver.FindElement(By.XPath("//label[text()='Applicant Class']/../following-sibling::td//select")).SendKeys("Resident / Expat");
        }
        public static void saveRenenewEidStep2()
        {
            //EidDriver.Navigate().GoToUrl(@"C:\Users\MonsterComputer\Desktop\ICA Registration System1.html");
            EidDriver.FindElement(By.XPath("//input[@id='dijit_form_ComboBox_3']")).SendKeys(Eid.Nationality);
            EidDriver.FindElement(By.XPath("//input[@name='eForm.Person/FirstNameEN']")).SendKeys(Eid.NameEnglish);
            EidDriver.FindElement(By.XPath("//select[@name='eForm.Person/Gender']")).SendKeys(Eid.Gender);
            EidDriver.FindElement(By.XPath("//input[@name='eForm.Person/MotherFirstNameEN'] ")).SendKeys(Eid.MotherNameArabic);
        }
        public static void saveRenenewEidStep3()
        {
            //EidDriver.Navigate().GoToUrl(@"C:\Users\MonsterComputer\Desktop\new\step3.html");
            EidDriver.FindElement(By.XPath("//input[@id='dijit_form_ComboBox_5']")).SendKeys(Eid.PlaceofBirth);
            EidDriver.FindElement(By.XPath("//label[text()='Place of Birth - English']/../following-sibling::td//input")).SendKeys("unspecified");
            EidDriver.FindElement(By.XPath("//input[@class='mendixFormDatePicker_dateInput']")).SendKeys(Eid.DateofBirth);
        }
        public static void saveRenenewEidStep4()
        {
            //EidDriver.Navigate().GoToUrl(@"C:\Users\MonsterComputer\Desktop\new\step4.html");
            EidDriver.FindElement(By.XPath("//input[@id='dijit_form_ComboBox_5']")).SendKeys(Eid.PlaceofBirth);
            EidDriver.FindElement(By.XPath("//label[text()='Place of Birth - English']/../following-sibling::td//input")).SendKeys("unspecified");
            EidDriver.FindElement(By.XPath("//input[@class='mendixFormDatePicker_dateInput']")).SendKeys(Eid.DateofBirth);
        }
        public static void saveRenenewEidStep5()
        {
            //EidDriver.Navigate().GoToUrl(@"C:\Users\MonsterComputer\Desktop\new\step5.html");
            EidDriver.FindElement(By.XPath("//input[@name='eForm.Passport/PassportNumber']")).SendKeys(Eid.PassportNumber);
            EidDriver.FindElement(By.XPath("//div[@srcattribute='PassportIssueDateGregorian']//input")).SendKeys(Eid.DateofIssuePassport);
            EidDriver.FindElement(By.XPath("//div[@srcattribute='PassportExpiryDate']//input")).SendKeys(Eid.DateofExpiryPassport);
            EidDriver.FindElement(By.XPath("//input[@srcattribute='PersonNumber']")).SendKeys(Eid.UID);
        }
        public static void saveRenenewEidStep6()
        {
            //EidDriver.Navigate().GoToUrl(@"C:\Users\MonsterComputer\Desktop\new\step6.html");
            EidDriver.FindElement(By.XPath("//input[@srcattribute='ContactMobilePhoneNumber']")).SendKeys(Eid.MobileNumber);
        }
    }
}
