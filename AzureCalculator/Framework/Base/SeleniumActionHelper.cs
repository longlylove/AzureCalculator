using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.Models;
using Framework.Utilities;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace Framework.Base
{
    public class SeleniumActionHelper : Utils
    {
        private readonly IWebDriver _driver;

        public SeleniumActionHelper(IWebDriver driver)
        {
            _driver = driver;
        }

        public SeleniumActionHelper(Contexts contexts) : base(contexts)
        {
            _driver = contexts.Driver;
        }

        public void NavigateToUrl(string url)
        {
            Log("[NOTE] Navigating to: " + url);

            _driver.Navigate().GoToUrl(url);
        }

        public IWebElement FindElement(By locator, bool visibility = true)
        {
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(Properties.Test.Default.TimeoutInSecs));

            return wait.Until(visibility ? SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(locator) : 
                SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(locator));
        }

        public void WaitForElement(By locator)
        {
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(Properties.Test.Default.TimeoutInSecs));

            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(locator));
        }

        public bool IsElementPresent(By locator)
        {
            try
            {
                _driver.FindElement(locator);
                return true;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        public void EnterText(By locator, string text, bool clearText = true)
        {
            SendKeys(locator, text, clearText);
        }

        public void SendKeys(By locator, string text, bool clearText)
        {
            if (text != null)
            {
                var element = Click(locator);

                if (clearText)
                    element.Clear();
                element.SendKeys(text);
            }
        }

        public void SendKeysUsingJavascript(By locator, string text)
        {
            var element = FindElement(locator);

            var js = (IJavaScriptExecutor)_driver;

            js.ExecuteScript($"arguments[0].setAttribute('value', '{text}')", element);
        }

        public void ClickElementUsingJavascript(By locator)
        {
            var element = FindElement(locator);

            var js = (IJavaScriptExecutor)_driver;

            js.ExecuteScript("arguments[0].click();", element);
        }

        public IWebElement ClearText(By locator)
        {
            var element = FindElement(locator);

            element.Clear();

            return element;
        }

        public IWebElement Click(By locator)
        {
            var element = FindElement(locator);
            ScrollToView(element);
            try
            {
                element.Click();

            }
            catch (ElementClickInterceptedException)
            {
                element.Click();
            }
            catch (Exception e)
            {
                throw new Exception(e.ToString(), e.InnerException);
            }

            return element;
        }

        public IWebElement ScrollToView(IWebElement element)
        {
            var js = (IJavaScriptExecutor)_driver;

            js.ExecuteScript("arguments[0].scrollIntoView();", element);

            return element;
        }

        public string GetText(By locator)
        {
            var webElement = FindElement(locator);
            var text = "";
            try
            {
                text = webElement.Text;
                if (string.IsNullOrWhiteSpace(text))
                {
                    text = webElement.GetAttribute("textContent");
                }
            }
            catch (Exception e)
            {
                Log($"Error retrieving element text of {webElement.GetAttribute("Name")}. Error: {e.Message}");
            }

            return text;
        }

        public void SwitchToFrame(By locator)
        {
            _driver.SwitchTo().Frame(FindElement(locator));
        }

        public void SwitchToDefaultContent()
        {
            _driver.SwitchTo().DefaultContent();
        }

        public string GetElementAttribute(By locator, string attribute)
        {
            return FindElement(locator).GetAttribute(attribute);
        }
    }
}
