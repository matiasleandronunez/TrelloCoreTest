using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace TrelloCoreTest.PageObjects
{
    public class AtlassianSSOPage : BasePage
    {
        public AtlassianSSOPage(IWebDriver driver) : base(driver)
        {
        }

        public IWebElement passwordInput => driverWait.Until(e => driver.FindElement(By.CssSelector("input#password")));
        public IWebElement loginButton => driverWait.Until(e => driver.FindElement(By.CssSelector("button#login-submit")));
        public IWebElement loginForm => driverWait.Until(e => driver.FindElement(By.CssSelector("form#form-login")));

        public void InputPassword(string pass)
        {
            driverWait.Until(rdy => passwordInput.Enabled && JSReady() && ElementResizingEnded(loginForm));
            passwordInput.SendKeys(pass);
        }

        public void SubmitLogin()
        {
            loginButton.Click();
        }
    }
}
