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
            //Wait Until initial resize of login box to send input
            driverWait.Until(rdy => {
                    try
                    {
                        return passwordInput.Enabled && JSReady() && ElementResizingEnded(loginForm);
                    }
                    catch (StaleElementReferenceException)
                    {
                    /* In very low latency networks, and because both Trello and Atlassian SSO pages have the same password box id.
                    WebDriver may confuse the previous input with the new page's input, hence triggering a Stale Element Reference Exception.
                    To avoid this from disrupting the test, the exception is caught and lambda returns false; forcing calling FindElement once again
                    after the old element is cleared from the DOM and the new password input is present*/
                    return false;
                    }
                });
            
            passwordInput.SendKeys(pass);
        }

        public void SubmitLogin()
        {
            loginButton.Click();
        }
    }
}
