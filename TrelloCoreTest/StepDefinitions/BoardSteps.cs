using OpenQA.Selenium;
using TrelloCoreTest.PageObjects;
using TechTalk.SpecFlow;
using NUnit.Framework;
using TrelloCoreTest.Support;
using System.Linq;

namespace TrelloCoreTest.StepDefinitions
{
    [Binding, Parallelizable]
    public class BoardSteps
    {
        private BoardMainPage BoardMainPage => new BoardMainPage(driver);

        private IWebDriver driver;
        private ScenarioContext scenarioContext;

        public BoardSteps(IWebDriver driver, ScenarioContext scenarioContext)
        {
            this.driver = driver;
            this.scenarioContext = scenarioContext;
        }

        [When(@"I create a card in the first left hand side list")]
        public void WhenIOpenATrelloBoard()
        {
            scenarioContext.Add("UsedListName", BoardMainPage.GetFirstLeftHandSideListName()); // Save left hand side list name for verification at the end
            scenarioContext.Add("UsedCardName", DataDrivenTestHelper.Instance.CardToCreate); // Get the name set in the json data field into the scenario context

            BoardMainPage.AddCardToList(scenarioContext.Get<string>("UsedCardName"));
        }

        [Then(@"the new card is successfully created")]
        public void ThenTheNewCardIsSuccessfullyCreated()
        {
            var cardsDisplayed = BoardMainPage.GetAllCardTitlesInList(scenarioContext.Get<string>("UsedListName"));

            CollectionAssert.Contains(cardsDisplayed, scenarioContext.Get<string>("UsedCardName"), "Card is not within the list");
        }


    }
}


