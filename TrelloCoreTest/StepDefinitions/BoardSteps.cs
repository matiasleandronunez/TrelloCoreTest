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
            // Save left hand side list name for verification at the end
            scenarioContext.Add("UsedListName", BoardMainPage.GetFirstLeftHandSideListName());

            /* Get the name set in the json data field into the scenario context and appends randomized name to allow validating
               different scenarios when there's parallel execution of the same test. */
            scenarioContext.Add("UsedCardName", $"{DataDrivenTestHelper.Instance.CardToCreate}{GenericUtils.RandomString()}"); 

            BoardMainPage.AddCardToList(scenarioContext.Get<string>("UsedCardName"));
        }

        [Then(@"the new card is successfully created")]
        public void ThenTheNewCardIsSuccessfullyCreated()
        {
            var cardsDisplayed = BoardMainPage.GetAllCardTitlesInList(scenarioContext.Get<string>("UsedListName"));

            var cardUsed = scenarioContext.Get<string>("UsedCardName");

            CollectionAssert.Contains(cardsDisplayed, cardUsed, $"Card {cardUsed} is not within the list");
        }


    }
}


