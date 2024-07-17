using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using TechTalk.SpecFlow;

namespace SymphonyAutomation
{
    [Binding]
    public sealed class Hooks
    {
        private IWebDriver _driver;
        private WebDriverWait _wait;

        [BeforeScenario]
        public void BeforeScenario(ScenarioContext scenarioContext)
        {
            if (scenarioContext.ScenarioInfo.Tags.Contains("ui"))
            {
                var options = new ChromeOptions();
                options.AddArgument("--disable-gpu");
                options.AddArgument("--no-sandbox");
                options.AddArgument("--disable-dev-shm-usage");
                options.AddArgument("--window-size=1920,1080");
                //options.AddArgument("--headless");


                _driver = new ChromeDriver(options);
                _driver.Manage().Window.Maximize();
                _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));

                
                scenarioContext.Set(_driver, "WebDriver");
                scenarioContext.Set(_wait, "WebDriverWait");
            }
        }

        [AfterScenario]
        public void AfterScenario(ScenarioContext scenarioContext)
        {
            if (scenarioContext.TryGetValue("WebDriver", out IWebDriver driver))
            {
                driver.Quit();
                driver.Dispose();
            }
        }
    }
}
