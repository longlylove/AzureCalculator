using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;

namespace Framework.Utilities
{
    public class Screenshot: Utils
    {
        private readonly IWebDriver _driver;

        public Screenshot(IWebDriver web)
        {
            _driver = web;
        }

        public void CaptureScreen(string filePath)
        {
            try
            {
                if (!(_driver is ITakesScreenshot takesScreenshot))
                    return;
                var screenshot = takesScreenshot.GetScreenshot();
                screenshot.SaveAsFile(filePath, ScreenshotImageFormat.Png);
                Log($"[NOTE] Screenshot created: {new Uri(filePath)}");
            }
            catch (Exception e)
            {
                Log("[ERROR] Error while taking screenshot:\n" + e);
            }
        }
    }
}
