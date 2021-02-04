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

        public IWebElement emailInput => driverWait.Until(e => driver.FindElement(By.CssSelector("input#user")));
        public IWebElement passwordInput => driverWait.Until(e => driver.FindElement(By.CssSelector("input#password")));
        public IWebElement loginButton => driverWait.Until(e => driver.FindElement(By.CssSelector("input#login")));
        public IWebElement emailPassDiv => driverWait.Until(e => driver.FindElement(By.CssSelector("div.login-password-container-email")));


        public override void Open(string part = "/login")
        {
            base.Open(part);
        }

        public void InputEmail(string email)
        {
            emailInput.SendKeys(email);
        }

        public void InputPassword(string pass)
        {
            driverWait.Until(divResized => ElementResizingEnded(emailPassDiv));
            passwordInput.SendKeys(pass);
        }

        public void SubmitLogin()
        {
            //Changed waiting strategy for Trello login box
            WaitForAjax();
            driverWait.Until(divResized => ElementResizingEnded(emailPassDiv) && loginButton.Enabled && !passwordInput.Displayed);
            loginButton.Click();
        }
    }
}
