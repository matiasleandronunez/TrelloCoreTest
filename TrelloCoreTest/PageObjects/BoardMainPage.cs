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

        public IReadOnlyCollection<IWebElement> allListsDiv => driverWait.Until(e => driver.FindElements(By.CssSelector("div.js-list-content")));
        public IWebElement boardCanvasDiv => driverWait.Until(e => driver.FindElement(By.CssSelector("div.board-canvas")));

        public string GetFirstLeftHandSideListName()
        {
            return GetFirstLeftHandSideList()
                .FindElement(By.CssSelector("textarea.list-header-name"))
                .Text;
        }

        public IWebElement GetFirstLeftHandSideList()
        {
            driverWait.Until(listsLoaded => allListsDiv.Count != 0);

            return allListsDiv
                .First();
        }

        public IWebElement GetListByName(string lName)
        {
            return driverWait.Until(list => boardCanvasDiv.FindElement(By.XPath($".//textarea[text()='{lName}']/ancestor::div[contains(@class,'js-list-content')]")));
        }

        ///<summary>
        ///Adds a card to a list. If no list is specified it'll use the first left hand side list displayed in the board
        ///</summary>
        ///<param name="cardTitle">Card title to add</param>
        ///<param name="listName">Optional, the list name where to add the card</param>
        ///<returns></returns>
        public void AddCardToList(string cardTitle, string listName = null)
        {
            var listWrapper = !String.IsNullOrEmpty(listName) ? GetListByName(listName) : GetFirstLeftHandSideList();

            listWrapper
                    .FindElement(By.CssSelector("a.open-card-composer"))
                    .Click();

            driverWait.Until(inputRdy => listWrapper.FindElement(By.CssSelector("textarea.list-card-composer-textarea")))
                .SendKeys(cardTitle);

            driverWait.Until(button => listWrapper.FindElement(By.CssSelector("input.js-add-card")))
                .Click();
        }

        ///<summary>
        ///Return a list of all card titles displayed within a list
        ///</summary>
        ///<param name="listName">List name from which to retrieve cards</param>
        ///<returns>Card names within the list as string's list</returns>
        public List<string> GetAllCardTitlesInList(string listName)
        {
            var cards = driverWait.Until(cs => GetListByName(listName).FindElements(By.CssSelector("a.list-card")));

            return cards.Select(cTitle => cTitle.FindElement(By.CssSelector("span.list-card-title")).Text)
                .ToList<string>();
        }
    }
}
