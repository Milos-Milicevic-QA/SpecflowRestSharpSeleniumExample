using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace SymphonyAutomation.Pages
{
    public class HomePage
    {
        private readonly IWebDriver _driver;
        private readonly WebDriverWait _wait;

        
        private readonly By aboutUsLink = By.LinkText("About Us");
        private readonly By careersLink = By.XPath("//span[normalize-space()='Careers']");
        private readonly By companyLink = By.XPath("//a[normalize-space()='Company']");
        private readonly By currentOpeningsLink = By.XPath("//a[normalize-space()='Current Openings']");

        public HomePage(IWebDriver driver, WebDriverWait wait)
        {
            _driver = driver;
            _wait = wait;
        }

        public void ClickOnAboutUs()
        {
            var aboutUsElement = _wait.Until(ExpectedConditions.ElementToBeClickable(aboutUsLink));
            aboutUsElement.Click();
        }
        public void ClickOnCareers()
        {
            var aboutUsElement = _wait.Until(ExpectedConditions.ElementToBeClickable(careersLink));
            aboutUsElement.Click();
            _wait.Until(ExpectedConditions.UrlContains("Careers"));
        }

        public void SelectCompany()
        {
            var companyElement = _wait.Until(ExpectedConditions.ElementToBeClickable(companyLink));
            companyElement.Click();
            _wait.Until(ExpectedConditions.UrlContains("company"));
        }


        public void ClickOnMenuItem(string menu)
        {
            _driver.FindElement(By.LinkText(menu)).Click();
           
        }

        public void NavigateToCurrentOpenings()
        {
            var currentOpeningsElement = _wait.Until(ExpectedConditions.ElementToBeClickable(currentOpeningsLink));
            currentOpeningsElement.Click();
        }
    }
}
