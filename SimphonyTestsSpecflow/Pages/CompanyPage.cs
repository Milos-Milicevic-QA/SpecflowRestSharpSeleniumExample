using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;


namespace SymphonyAutomation.Pages
{
    public class CompanyPage
    {
        private readonly IWebDriver _driver;
        private readonly WebDriverWait _wait;

       
        private readonly By hqSelector = By.XPath("//ul[@class='pageMetaDetails--list']//span[contains(text(),'San Francisco')]");
        private readonly By foundedYearSelector = By.XPath("//span[normalize-space()='2007']");
        private readonly By consultingOfficesSelector = By.XPath("//ul[@class='pageMetaDetails--list']//span");
        private readonly By engineeringHubsSelector = By.XPath("//ul[@class='pageMetaDetails--list']//span[contains(text(),'Sarajevo') or contains(text(),'Skopje') or contains(text(),'Belgrade') or contains(text(),'Novi Sad') or contains(text(),'Banja Luka') or contains(text(),'Nis') or contains(text(),'Santo Domingo')]");
        private readonly By clientsSelector = By.XPath("//ul[@class='pageMetaDetails--list']//span[contains(text(),'300')]");

        public CompanyPage(IWebDriver driver, WebDriverWait wait)
        {
            _driver = driver;
            _wait = wait;
        }

        public string GetHQ()
        {
            return _wait.Until(ExpectedConditions.ElementIsVisible(hqSelector)).Text;
        }

        public string GetFoundedYear()
        {
            return _wait.Until(ExpectedConditions.ElementIsVisible(foundedYearSelector)).Text;
        }

        public List<string> GetConsultingOffices()
        {
            var locationElements = _wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(consultingOfficesSelector));
            return locationElements.Select(element => element.Text).ToList();
        }

        public List<string> GetEngineeringHubs()
        {
            var elements = _wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(engineeringHubsSelector));
            return elements.Select(e => e.Text).ToList();
        }

        public string GetClients()
        {
            return _wait.Until(ExpectedConditions.ElementIsVisible(clientsSelector)).Text;
        }
    }
}
