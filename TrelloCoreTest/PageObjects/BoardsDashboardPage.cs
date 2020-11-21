using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using TrelloCoreTest.Support;

namespace TrelloCoreTest.PageObjects
{
    public class BoardsDashboardPage : BasePage
    {
        public BoardsDashboardPage(IWebDriver driver) : base(driver)
        {
        }

        public IWebElement all_boards_div => _driverWait.Until(e => _driver.FindElement(By.CssSelector("div.content-all-boards")));

        public override void Open(string part = "/boards")
        {
            base.Open($"/{EnvironmentConfig.Instance.Username}/{part}");
        }

        public void ClickOnBoard(string board_name)
        {
            _driverWait.Until(board => all_boards_div.FindElements(By.CssSelector($"a.board-tile[href$='/{Regex.Replace(board_name, @"\s", "-").ToLower()}']")))
                .First()
                .Click();
        }
    }
}
