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

        public IWebElement password_input => _driverWait.Until(e => _driver.FindElement(By.CssSelector("input#password")));
        public IWebElement login_button => _driverWait.Until(e => _driver.FindElement(By.CssSelector("button#login-submit")));
        public IWebElement login_form => _driverWait.Until(e => _driver.FindElement(By.CssSelector("form#form-login")));

        public void InputPassword(string pass)
        {
            _driverWait.Until(rdy => password_input.Enabled && JSReady() && ElementResizingEnded(login_form));
            password_input.SendKeys(pass);
        }

        public void SubmitLogin()
        {
            login_button.Click();
        }
    }
}
