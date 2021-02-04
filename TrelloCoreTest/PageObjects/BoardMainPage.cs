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
    public class BoardMainPage : BasePage
    {
        public BoardMainPage(IWebDriver driver) : base(driver)
        {
        }

        public IReadOnlyCollection<IWebElement> all_lists_div => driverWait.Until(e => driver.FindElements(By.CssSelector("div.js-list-content")));
        public IWebElement board_canvas_div => driverWait.Until(e => driver.FindElement(By.CssSelector("div.board-canvas")));

        public string GetFirstLeftHandSideListName()
        {
            return GetFirstLeftHandSideList()
                .FindElement(By.CssSelector("textarea.list-header-name"))
                .Text;
        }

        public IWebElement GetFirstLeftHandSideList()
        {
            driverWait.Until(lists_loaded => all_lists_div.Count != 0);

            return all_lists_div
                .First();
        }

        public IWebElement GetListByName(string lname)
        {
            return driverWait.Until(list => board_canvas_div.FindElement(By.XPath($".//textarea[text()='{lname}']/ancestor::div[contains(@class,'js-list-content')]")));
        }

        public void AddCardToList(string card_title, string list_name = null)
        {
            /// Adds a card to a list. If no list is specified it'll use the first left hand side list displayed in the board
            /// 
            IWebElement list_wrapper = !String.IsNullOrEmpty(list_name) ? GetListByName(list_name) : GetFirstLeftHandSideList();

            list_wrapper
                    .FindElement(By.CssSelector("a.open-card-composer"))
                    .Click();

            driverWait.Until(input_rdy => list_wrapper.FindElement(By.CssSelector("textarea.list-card-composer-textarea")))
                .SendKeys(card_title);

            driverWait.Until(button => list_wrapper.FindElement(By.CssSelector("input.js-add-card")))
                .Click();
        }

        public List<string> GetAllCardTitlesInList(string list_name)
        {
            /// Return a list of all card titles displayed within a list
            /// 
            var cards = driverWait.Until(cs => GetListByName(list_name).FindElements(By.CssSelector("a.list-card")));

            return cards.Select(c_title => c_title.FindElement(By.CssSelector("span.list-card-title")).Text)
                .ToList<string>();
        }
    }
}
