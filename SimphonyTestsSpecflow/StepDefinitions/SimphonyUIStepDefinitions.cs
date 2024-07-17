using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using TechTalk.SpecFlow;
using NUnit.Framework;
using SymphonyAutomation.Pages;
using SeleniumExtras.WaitHelpers;

namespace SimphonyTestsSpecflow.StepDefinitions
{
    [Binding]
    public sealed class SimphnyUIStepDefinitions
    {
        private IWebDriver _driver;
        private WebDriverWait _wait;
        private HomePage _homePage;
        private CompanyPage _companyPage;
        private CareersPage _careersPage;

        public SimphnyUIStepDefinitions()
        {
            _driver = ScenarioContext.Current.Get<IWebDriver>("WebDriver");
            _wait = ScenarioContext.Current.Get<WebDriverWait>("WebDriverWait");
            _homePage = new HomePage(_driver, _wait);
            _companyPage = new CompanyPage(_driver, _wait);
            _careersPage = new CareersPage(_driver, _wait);
        }

        [Given(@"I have launched the browser and maximized the window")]
        public void GivenIHaveLaunchedTheBrowserAndMaximizedTheWindow()
        {
            // The browser is already launched and maximized in the BeforeScenario hook
        }

        [When(@"I navigate to ""(.*)""")]
        public void WhenINavigateTo(string url)
        {
            _driver.Navigate().GoToUrl(url);
        }

        [Then(@"the URL should be ""(.*)""")]
        public void ThenTheURLShouldBe(string expectedUrl)
        {
            Assert.AreEqual(expectedUrl, _driver.Url);
        }

        [Given(@"I am on the Symphony homepage")]
        public void GivenIAmOnTheSymphonyHomepage()
        {
            _driver.Navigate().GoToUrl("https://symphony.is");
        }

        [When(@"I click on ""(.*)"" and select ""(.*)""")]
        public void WhenIClickOnAndSelect(string menu, string submenu)
        {
            _homePage.ClickOnMenuItem(menu);
            _homePage.SelectCompany();
            _wait.Until(ExpectedConditions.UrlContains("about-us/company"));
        }

        [When(@"I click on Careers menu item")]
        public void WhenIClickOnMenuItem()
        {
            _homePage.ClickOnCareers();
        }

        [Then(@"the HQ should be ""(.*)""")]
        public void ThenTheHQShouldBe(string expectedHQ)
        {
            var hq = _companyPage.GetHQ();
            Assert.AreEqual(expectedHQ, hq);
        }

        [Then(@"the Founded year should be ""(.*)""")]
        public void ThenTheFoundedYearShouldBe(string expectedFounded)
        {
            var foundedYear = _companyPage.GetFoundedYear();
            Assert.AreEqual(expectedFounded, foundedYear);
        }

        [Then(@"the Consulting Offices should include the following locations")]
        public void ThenTheConsultingOfficesShouldIncludeTheFollowingLocations(Table table)
        {
            var expectedLocations = table.Rows.Select(row => row["Location"]).ToList();
            var actualLocations = _companyPage.GetConsultingOffices();
            foreach (var expectedLocation in expectedLocations)
            {
                Assert.IsTrue(actualLocations.Contains(expectedLocation), $"Location '{expectedLocation}' was not found.");
            }
        }

        [Then(@"the Engineering Hubs should include the following locations")]
        public void ThenTheEngineeringHubsShouldIncludeTheFollowingLocations(Table table)
        {
            var expectedLocations = table.Rows.Select(row => row["Location"]).ToList();
            var actualLocations = _companyPage.GetEngineeringHubs();
            foreach (var expectedLocation in expectedLocations)
            {
                Assert.IsTrue(actualLocations.Contains(expectedLocation), $"Location '{expectedLocation}' was not found.");
            }
        }

        [Then(@"the Clients should include the following count")]
        public void ThenTheClientsShouldIncludeTheFollowingCount(Table table)
        {
            var expectedCount = table.Rows.Select(row => row["Count"]).First();
            var actualCount = _companyPage.GetClients();
            Assert.AreEqual(expectedCount, actualCount, $"Expected client count '{expectedCount}', but found '{actualCount}'.");
        }

        [Then(@"the number of open positions should be (\d+)")]
        public void ThenTheNumberOfOpenPositionsShouldBe(int expectedCount)
        {
            int actualCount = _careersPage.GetOpenPositionsCount();
            Assert.AreEqual(expectedCount, actualCount, $"Expected {expectedCount} job listings, but found {actualCount}.");
        }

        [Then(@"I save all job titles and locations to a file named ""(.*)""")]
        public void ThenISaveAllJobTitlesAndLocationsToAFileNamed(string fileName)
        {
            _careersPage.GetJobTitlesAndLocations(fileName);
        }
    }
}
