using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomationChallenge
{
    class Program
    {
        static void Main(string[] args)
        {
            DriverTester runTest = new DriverTester();
            runTest.DriverTest();
        }
    }

    class DriverTester
    {
        public void DriverTest()
        {
            //Chrome and Firefox confirmed working, Internet Explorer has an issue.
            IWebDriver driver = new ChromeDriver();
            //IWebDriver driver = new FirefoxDriver();
            //Internet Explorer takes forever to run through step 6 so lets not use it.
            //IWebDriver driver = new InternetExplorerDriver();

            //Setup a max timespan to wait for to be used in steps seven, eight, and nine.
            TimeSpan span = new TimeSpan(0, 0, 0, 30, 0);
            WebDriverWait wait = new WebDriverWait(driver, span);

            driver.Url = "http://hpadevtest.azurewebsites.net/";

            //Method to get the current step displayed by the web page. Used after each step to write to console and confirm each step completes.
            //Could be used to check if next step should be run.
            string GetStepCounter()
            {
                var stepCompleteCounter = driver.FindElement(By.Id("stepNum")).Text;
                return stepCompleteCounter;
            }

            //Create an alert method to simplify code further on.
            void AcceptAlert()
            {
                driver.SwitchTo().Alert().Accept();
            }

            //STEP ONE
            //Test try/catch if the element can't be found.
            try
            {
                //Click on the box to complete the first step.
                driver.FindElement(By.Id("Box1")).Click();
                //Accept the alert
                AcceptAlert();
                Console.WriteLine($"Step {GetStepCounter()} completed.");
            }
            catch (NoSuchElementException)
            {
                Console.WriteLine("Step one failed");
                throw;
            }

            //STEP TWO
            //Find the box for step two and click on it, then press the tab key
            IWebElement stepTwo = driver.FindElement(By.Id("Box3"));
            stepTwo.Click();
            stepTwo.SendKeys(Keys.Tab);
            //Accept the alert
            AcceptAlert();
            Console.WriteLine($"Step {GetStepCounter()} completed.");

            //STEP THREE
            //Set the optionVal to stepThreeVal then use that to determine which option to click by using the value field of input.
            var stepThreeVal = driver.FindElement(By.Id("optionVal")).Text;
            driver.FindElement(By.XPath($"//input[@value={stepThreeVal}]")).Click();
            //Accept the alert
            AcceptAlert();
            Console.WriteLine($"Step {GetStepCounter()} completed.");

            //STEP FOUR
            //Set stepFourVar to the value of selectionVal then find the dropdown and select the correct one.
            var stepFourVar = driver.FindElement(By.Id("selectionVal")).Text;
            IWebElement dropdown = driver.FindElement(By.XPath("/html/body/div[3]/center/div/div[2]/p/select"));
            var dropdownSelect = new SelectElement(dropdown);
            dropdownSelect.SelectByValue($"{stepFourVar}");
            //Accept the alert
            AcceptAlert();
            Console.WriteLine($"Step {GetStepCounter()} completed.");

            //STEP FIVE
            //Get the table, then make a list of the rows the table contains.
            IWebElement stepFiveTable = driver.FindElement(By.Id("FormTable"));
            List<IWebElement> rows = stepFiveTable.FindElements(By.XPath("id('FormTable')/tbody/tr/td")).ToList();

            //Tricky part because the IDs of the td share the same name.
            //Had to understand tables and how they function in order to iterate through the table and dive down into the input field to get the placeholder text.
            for (int i = 1; i < rows.Count(); i++)
            {
                string innerPlaceholderVar = driver.FindElement(By.XPath($"id('FormTable')/tbody/tr[{i}]/td/input")).GetAttribute("placeholder");
                IWebElement insertPlaceholder = driver.FindElement(By.XPath($"id('FormTable')/tbody/tr[{i}]/td/input"));
                insertPlaceholder.SendKeys(innerPlaceholderVar);
            }

            //Hit the submit button to submit the placeholder text in the inputs.
            driver.FindElement(By.XPath("/html/body/div[5]/center/div/div/div[1]/center/table/tbody/tr[10]/td/center/button")).Click();
            //Accept the alert
            AcceptAlert();
            Console.WriteLine($"Step {GetStepCounter()} completed.");

            //STEP SIX
            //Capture the result of step five.
            var stepFiveResult = driver.FindElement(By.Id("formResult")).Text;

            //Capture the line number to know where to put the step five result.
            var stepSixLineNumber = driver.FindElement(By.Id("lineNum")).Text;

            //Get the table row from stepSixLineNumber and the second table data, then clear the input field and insert the stepFiveResult, then hit enter.
            IWebElement insertCapture = driver.FindElement(By.XPath($"id('inputTable')/tbody/tr[{stepSixLineNumber}]/td[2]/input"));
            insertCapture.Clear();
            insertCapture.SendKeys(stepFiveResult);
            insertCapture.SendKeys(Keys.Enter);
            //Accept the alert
            AcceptAlert();
            Console.WriteLine($"Step {GetStepCounter()} completed.");

            //STEP SEVEN/EIGHT/NINE
            //Since steps seven, eight, and nine are all doing the same thing condense into a loop.
            int n = 7;
            while (n < 10)
            {
                driver.FindElement(By.Id($"BoxParagraph{n}")).Click();
                wait.Until(ExpectedConditions.AlertIsPresent());
                //Accept the alert
                AcceptAlert();
                Console.WriteLine($"Step {GetStepCounter()} completed.");
                n++;
            }

            //STEP TEN
            driver.FindElement(By.Id("BoxParagraph10")).Click();
            //Accept the alert
            AcceptAlert();
            Console.WriteLine($"Step {GetStepCounter()} completed.");

            //driver.Close();
        }
    }
}
