using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Framework.Models;
using OpenQA.Selenium;

namespace Framework.Utilities
{
    public class WaitFor: Utils
    {
        private readonly IWebDriver _driver;
        public WaitFor(Contexts contexts) : base(contexts)
        {
            _driver = contexts.Driver;
        }

        public void PageToLoad(string pageTitle)
        {
            try
            {
                using (var sw = new StopwatchHelper())
                {
                    var exec = new JsExecutor(_driver);
                    var title = "";
                    while (sw.Elapsed.TotalMilliseconds <= TimeSpan.FromSeconds(10).TotalMilliseconds)
                    {
                        title = (string)exec.Execute("return document.title");
                        var readyState = (string)exec.Execute("return document.readyState");
                        if (!(title.ToLower().Contains(pageTitle.ToLower())))
                        {
                            Thread.Sleep(500);
                        }
                        else
                        {
                            if (readyState != "complete")
                            {
                                Thread.Sleep(500);
                            }
                            else if (readyState == "complete")
                            {
                                Thread.Sleep(300);
                                return;
                            }
                        }
                    }
                    if (title != pageTitle)
                    {
                        throw new Exception("Test is stopped due to targeted page is not return"
                                            + Environment.NewLine + "Expected: " + pageTitle
                                            + Environment.NewLine + "Actual: " + title);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Test Internal Errors. Stacktrace: "
                                    + Environment.NewLine + ex);
            }
        }
    }
}
