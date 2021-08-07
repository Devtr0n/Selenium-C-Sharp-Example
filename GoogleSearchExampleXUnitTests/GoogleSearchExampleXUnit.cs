using FluentAssertions;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.IO;
using System.Reflection;
using Xunit;

namespace SeleniumExample
{
    public class GoogleSearchExampleXUnit
    {
        [Fact]
        public void SearchForSeleniumHQXUnit()
        {
            //***************************************************************************************************
            // Arrange
            //***************************************************************************************************
            // find the assembly folder with Chrome Driver (chromedriver.exe)
            var browserDriverPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            // set Chrome to start maximized
            new ChromeOptions().AddArguments("--start-maximized");

            // navigate to Google via Chrome browser
            using var chromeDriver = new ChromeDriver(browserDriverPath, new ChromeOptions());
            chromeDriver.Navigate().GoToUrl("http://google.com");

            // create new wait timer and set it to 1 minute
            var wait = new WebDriverWait(chromeDriver, new TimeSpan(0, 0, 1, 0));

            // wait until an element on the page with the name q is visible (Google names their search box 'q')
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Name("q")));

            // locate the Google Search Box element
            var googleSearchBox = chromeDriver.FindElement(By.Name("q"));

            //***************************************************************************************************
            // Act
            //***************************************************************************************************

            // clear search field & enter "Selenium HQ" as text into the Google search engine
            googleSearchBox.Clear();
            googleSearchBox.SendKeys("xUnit");

            // pause until the Google Search button is visible
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Name("btnK")));

            // find "Google Search" button and click it
            var searchButton = chromeDriver.FindElement(By.Name("btnK"));
            searchButton.Click();

            // wait until search results stats appear which confirms that the search finished
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id("result-stats")));

            // find 'result-stats' element
            var resultStats = chromeDriver.FindElement(By.Id("result-stats"));

            //***************************************************************************************************
            // Assert
            //***************************************************************************************************

            // verify the 'result-stats' element contain the words 'About', 'results' and 'seconds'
            resultStats.Text.Should().Contain("About");
            resultStats.Text.Should().Contain("results");
            resultStats.Text.Should().Contain("seconds");

            // Find an emphasized search result in the list
            var results = chromeDriver.FindElement(By.TagName("em"));

            // ensure that the result text contains Selenium
            results.Text.Should().Contain("xUnit");

            // close Chrome browser
            chromeDriver.Close();
        }
    }
}
