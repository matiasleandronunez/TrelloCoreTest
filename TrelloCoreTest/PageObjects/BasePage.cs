﻿using System;
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
        protected IWebDriver driver;
        protected WebDriverWait driverWait;

        public BasePage(IWebDriver driver)
        {
            this.driver = driver;
            driverWait = new WebDriverWait(driver, new System.TimeSpan(0, 0, 0, 30, 0));
        }

        /// <summary>
        /// Validates the page displayed by the browser matches the Page Object element in use. If not overriden by a PO's particular implementation, it will only wait and check for the URI to match the Base URI of that particular page
        /// </summary>
        /// <returns>
        /// bool indicating page is loaded
        /// </returns>
        public virtual bool ValidatePage()
        {
            //if current URI without parameters matches base URI
            return driverWait.Until(url => driver.Url.Split('?')[0] == Url);
        }

        /// <summary>
        /// Returns pages name for reference. If not overriden returns Class name instead.
        /// </summary>
        /// <returns>
        /// Page object name as string
        /// </returns>
        public virtual string PageName()
        {
            return this.GetType().Name;
        }

        public virtual string Url
        {
            get
            {
                return EnvironmentConfig.Instance.BaseURL;
            }
        }

        //Static variable to be set once for classes inheriting from BasePage after original Open
        protected static long browserWidth { get; set; }
        
        protected void SetBrowserWidth()
        {
            try
            {
                IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                browserWidth = (long)js.ExecuteScript("return $('html > body').width()");
            }
            catch (WebDriverException)
            {
                browserWidth = driver.Manage().Window.Size.Width;
            }
        }

        public virtual void Open(string part = "")
        {
            if (string.IsNullOrEmpty(Url))
            {
                throw new ArgumentException("The main URL cannot be null or empty.");
            }

            //Method not implemented for some browsers, ignore maximize for those
            try
            {
                driver.Manage().Window.Maximize();
            }
            catch (NotImplementedException)
            {
            }
            catch (WebDriverException)
            {
            }

            driver.Navigate().GoToUrl(string.Concat(Url, part));

            SetBrowserWidth();
        }

        protected void WaitForAjax()
        {
            driverWait.Until(rdy => JSReady() && JQueryReady());
        }

        protected bool JSReady()
        {
            try
            {
                return (bool)(driver as IJavaScriptExecutor).ExecuteScript("return document.readyState == 'complete'");
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
                return (bool)(driver as IJavaScriptExecutor).ExecuteScript("return jQuery.active == 0");
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
            return driverWait.Until(r => {
                siz2 = e.Size;
                if (siz == siz2) { return true; } else { siz = siz2; return false; }
            });
        }
    }
}
