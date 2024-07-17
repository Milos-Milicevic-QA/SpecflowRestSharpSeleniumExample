using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;


namespace SymphonyAutomation.Pages
{
    public class CareersPage
    {
        private readonly IWebDriver _driver;
        private readonly WebDriverWait _wait;

        private readonly By jobTitleSelector = By.XPath("//div[contains(@class, 'currentOpenings--job-title')]");
        private readonly By jobLocationSelector = By.XPath("//div[contains(@class, 'currentOpenings--job-locationWrapper-name')]");

        public CareersPage(IWebDriver driver, WebDriverWait wait)
        {
            _driver = driver;
            _wait = wait;
        }

        public int GetOpenPositionsCount()
        {
            var jobPositions = _wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(jobTitleSelector));
            return jobPositions.Count;
        }

        public void GetJobTitlesAndLocations(string fileName)
        {
            var jobTitleElements = _wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(jobTitleSelector));
            var jobLocationElements = _wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(jobLocationSelector));

            Assert.AreEqual(jobTitleElements.Count, jobLocationElements.Count, "The number of job titles and locations do not match.");

            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);

            HashSet<string> existingJobs = new HashSet<string>();
            if (File.Exists(path))
            {
                var existingEntries = File.ReadAllLines(path);
                foreach (var entry in existingEntries)
                {
                    existingJobs.Add(entry);
                }
            }

            using (StreamWriter writer = new StreamWriter(path, append: true))
            {
                for (int i = 0; i < jobTitleElements.Count; i++)
                {
                    string title = jobTitleElements[i].Text;
                    string location = jobLocationElements[i].Text;
                    string jobEntry = $"{title}, {location}";

                    if (!existingJobs.Contains(jobEntry))
                    {
                        writer.WriteLine(jobEntry);
                        existingJobs.Add(jobEntry);
                    }
                }
            }

            Console.WriteLine($"File saved at: {path}");
        }
    }
}
