using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace SeleniumTest
{
    public class Tests
    {

        public static class LoginInfo
        {
            private static readonly string username = "Admin";

            private static readonly string password = "admin123";
            public static string Username { get => username; }

            public static string Password { get => password; }
        }

        public class LoginPage
        {
            public virtual string Url { get; set; }

            public virtual string WordtoFindOnPage0 { get; set; }

            public virtual string WordtoFindOnPage1 { get; set; }

            public virtual string WordtoFindOnPage2 { get; set; }

            public virtual string WordtoFindOnPageForcheck { get; set; }

            public LoginPage()
            {
                Url = @"https://opensource-demo.orangehrmlive.com/";
                WordtoFindOnPage0 = "txtUsername";
                WordtoFindOnPage1 = "txtPassword";
                WordtoFindOnPage2 = "btnLogin";
                WordtoFindOnPageForcheck = "welcome";
            }

        }

        public class SubPage : LoginPage
        {
            public override string Url { get; set; }

            public string shiftname = "Ivan";

            public override string WordtoFindOnPage0 { get; set; }

            public override string WordtoFindOnPage1 { get; set; }

            public override string WordtoFindOnPage2 { get; set; }

            public override string WordtoFindOnPageForcheck { get; set; }

            public SubPage()
            {
                Url = @"https://opensource-demo.orangehrmlive.com/index.php/admin/workShift";
                WordtoFindOnPage0 = "workShift_workHours_from";
                WordtoFindOnPage1 = "workShift_workHours_to";
                WordtoFindOnPage2 = "workShift_availableEmp";
                WordtoFindOnPageForcheck = "welcome";
            }
        }

        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void Test1()
        {
            IWebDriver driver = new ChromeDriver();
            LoginPage loginPageObj = new();
            driver.Navigate().GoToUrl(loginPageObj.Url);
            driver.FindElement(By.Id(loginPageObj.WordtoFindOnPage0)).SendKeys(LoginInfo.Username);
            driver.FindElement(By.Id(loginPageObj.WordtoFindOnPage1)).SendKeys(LoginInfo.Password);
            driver.FindElement(By.Id(loginPageObj.WordtoFindOnPage2)).Click();
            Assert.IsTrue(driver.FindElement(By.Id(loginPageObj.WordtoFindOnPageForcheck)).Displayed);


            SubPage subPageObj = new();
            driver.Navigate().GoToUrl(subPageObj.Url);
            driver.FindElement(By.Id("btnAdd")).Click();
            driver.FindElement(By.Id("workShift_name")).SendKeys(subPageObj.shiftname);
            foreach (IWebElement option in driver.FindElement(By.Id(subPageObj.WordtoFindOnPage0)).FindElements(By.TagName("option")))
            {
                if (option.Text == "06:00")
                {
                    option.Click();
                }
            }
            foreach (IWebElement option in driver.FindElement(By.Id(subPageObj.WordtoFindOnPage1)).FindElements(By.TagName("option")))
            {
                if (option.Text == "18:00")
                {
                    option.Click();
                }
            }
            foreach (IWebElement option in driver.FindElement(By.Id(subPageObj.WordtoFindOnPage2)).FindElements(By.TagName("option")))
            {
                option.Click();
            }
            driver.FindElement(By.Id("btnAssignEmployee")).Click();
            driver.FindElement(By.Id("btnSave")).Click();
            Assert.IsTrue(driver.FindElement(By.XPath($"//*[text()='{subPageObj.shiftname}']")).Displayed);

            int i = 1;
            int res = 1;
            bool state = true;
            string str = $"#resultTable > tbody > tr:nth-child(" + i + ") > td:nth-child(2) > a";
            while (state)
            {
                str = $"#resultTable > tbody > tr:nth-child(" + i + ") > td:nth-child(2) > a";
                string current = driver.FindElement(By.CssSelector(str)).Text;
                if (driver.FindElement(By.CssSelector(str)).Text == subPageObj.shiftname)
                {
                    driver.FindElement(By.CssSelector("#resultTable > tbody > tr:nth-child(" + i + ") > td:nth-child(1)")).Click();
                    state = false;
                    res = i;
                    driver.FindElement(By.Id("btnDelete")).Click();
                    driver.FindElement(By.Id("dialogDeleteBtn")).Click();
                }
                i++;
            }
            Assert.IsTrue(res == i-1);
        }
    }
}