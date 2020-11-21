using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using TrelloCoreTest.Support;

namespace TrelloCoreTest.PageObjects
{
    public class BasePage
    {
        protected IWebDriver _driver;
        protected WebDriverWait _driverWait;

        public BasePage(IWebDriver driver)
        {
            _driver = driver;
            _driverWait = new WebDriverWait(driver, new System.TimeSpan(0, 0, 0, 30, 0));
        }

        public virtual bool ValidatePage()
        {
            /// <summary>Validates the page displayed by the browser matches the Page Object element in use. If not overriden by a PO's particular implementation, it will only wait and check for the URI to match the Base URI of that particular page</summary>
            //if current URI without parameters matches base URI
            return _driverWait.Until(url => _driver.Url.Split('?')[0] == Url);
        }

        public virtual string PageName()
        {
            /// <summary>Returns pages name for reference. If not overriden returns Class name instead.</summary>
            return this.GetType().Name;
        }

        public virtual string Url
        {
            get
            {
                return EnvironmentConfig.Instance.Base_url;
            }
        }

        public virtual void Open(string part = "")
        {
            if (string.IsNullOrEmpty(Url))
            {
                throw new ArgumentException("The main URL cannot be null or empty.");
            }

            _driver.Manage().Window.Maximize();
            _driver.Navigate().GoToUrl(string.Concat(Url, part));
        }

        protected void WaitForAjax()
        {
            _driverWait.Until(rdy => JSReady() && JQueryReady());
        }

        protected bool JSReady()
        {
            try
            {
                return (bool)(_driver as IJavaScriptExecutor).ExecuteScript("return document.readyState == 'complete'");
            }
            catch (Exception)
            {
                return false;
            }
        }

        protected bool JQueryReady()
        {
            try
            {
                return (bool)(_driver as IJavaScriptExecutor).ExecuteScript("return jQuery.active == 0");
            }
            catch (Exception)
            {
                return false;
            }
        }

        protected bool ElementResizingEnded(IWebElement e)
        {
            var siz = e.Size;
            var siz2 = e.Size;
            return _driverWait.Until(r => {
                siz2 = e.Size;
                if (siz == siz2) { return true; } else { siz = siz2; return false; }
            });
        }
    }
}
