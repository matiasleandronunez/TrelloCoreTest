using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace TrelloCoreTest.PageObjects
{
    public class LoginPage : BasePage
    {
        public LoginPage(IWebDriver driver) : base(driver)
        {
        }

        public IWebElement email_input => _driverWait.Until(e => _driver.FindElement(By.CssSelector("input#user")));
        public IWebElement password_input => _driverWait.Until(e => _driver.FindElement(By.CssSelector("input#password")));
        public IWebElement login_button => _driverWait.Until(e => _driver.FindElement(By.CssSelector("input#login")));
        public IWebElement email_pass_div => _driverWait.Until(e => _driver.FindElement(By.CssSelector("div.login-password-container-email")));


        public override void Open(string part = "/login")
        {
            base.Open(part);
        }

        public void InputEmail(string email)
        {
            email_input.SendKeys(email);
        }

        public void InputPassword(string pass)
        {
            _driverWait.Until(div_resized => ElementResizingEnded(email_pass_div));
            password_input.SendKeys(pass);
        }

        public void SubmitLogin()
        {
            login_button.Click();
        }
    }
}
