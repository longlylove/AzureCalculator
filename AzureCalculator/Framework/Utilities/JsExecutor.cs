using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;

namespace Framework.Utilities
{
    public class JsExecutor: Utils
    {
        private static IWebDriver _driver;
        private static IJavaScriptExecutor _jsExecutor;

        public JsExecutor(IWebDriver driver)
        {
            _driver = driver;
            _jsExecutor = _driver as IJavaScriptExecutor;
        }

        public object Execute(string js, params object[] args)
        {
            object returnValue;
            using (new StopwatchHelper())
            {
                if (_jsExecutor == null)
                {
                    throw new Exception("JavaScriptExecutor instance was not initialized");
                }
                returnValue = _jsExecutor.ExecuteScript(js, args);
                try
                {
                    var pageTitle = _driver.Title;
                }
                catch (InvalidOperationException e)
                {
                    throw new InvalidOperationException($"Exception in WebDriver while doing integrity check after executing script: {js} with args [{args}]", e);
                }
            }
            if (returnValue is string s)
            {
                return s;
            }
            return returnValue;
        }
    }
}
