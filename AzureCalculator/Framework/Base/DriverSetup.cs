using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.Models;
using Framework.Utilities;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;

namespace Framework.Base
{
    public class DriverSetup: Utils
    {
        public static IWebDriver SetupBrowser(string browser)
        {
            BrowserFlavours flavour;
            switch (browser.ToLower())
            {
                case "firefox":
                case "ff":
                    flavour = BrowserFlavours.Firefox;
                    break;
                case "internetexplorer":
                case "internet explorer":
                case "internet_explorer":
                case "ie":
                    flavour = BrowserFlavours.InternetExplorer;
                    break;
                case "headlesschrome":
                case "headless chrome":
                case "headless_chrome":
                case "hc":
                    flavour = BrowserFlavours.HeadlessChrome;
                    break;
                default:
                    flavour = BrowserFlavours.Chrome;
                    break;
            }

            return ConfigureBrowser(flavour);
        }

        private static IWebDriver ConfigureBrowser(BrowserFlavours flavour)
        {
            IWebDriver driver = null;
            switch (flavour)
            {
                case BrowserFlavours.Chrome:
                    var chromeOptions = new ChromeOptions();

                    chromeOptions.AddArgument("--no-sandbox");
                    chromeOptions.AddArgument("--start-maximized");
                    chromeOptions.AddArgument("--ignore-certificate-errors");
                    chromeOptions.AddArgument("disable-infobars");
                    chromeOptions.AddArgument("--disable-extensions");
                    chromeOptions.AddArgument("--allow-insecure-localhost");
                    chromeOptions.AddArgument("--disable-dev-shm-usage");
                    chromeOptions.AddArgument("--disable-gpu");
                    chromeOptions.AddArgument(
                        "--user-agent=Mozilla/5.0 (iPad; CPU OS 6_0 like Mac OS X) AppleWebKit/536.26 (KHTML, like Gecko) Version/6.0 Mobile/10A5355d Safari/8536.25");

                    driver = new ChromeDriver(BuildDirectory, chromeOptions);

                    break;
                case BrowserFlavours.HeadlessChrome:
                    var headlessChromeOptions = new ChromeOptions();

                    headlessChromeOptions.AddArgument("--no-sandbox");
                    headlessChromeOptions.AddArgument("--headless");
                    headlessChromeOptions.AddArgument("--window-size=1920,1080");
                    headlessChromeOptions.AddArgument("--ignore-certificate-errors");
                    headlessChromeOptions.AddArgument("disable-infobars");
                    headlessChromeOptions.AddArgument("--disable-extensions");
                    headlessChromeOptions.AddArgument("--allow-insecure-localhost");
                    headlessChromeOptions.AddArgument("--disable-gpu");
                    headlessChromeOptions.AddArgument(
                        "--user-agent=Mozilla/5.0 (iPad; CPU OS 6_0 like Mac OS X) AppleWebKit/536.26 (KHTML, like Gecko) Version/6.0 Mobile/10A5355d Safari/8536.25");

                    driver = new ChromeDriver(BuildDirectory, headlessChromeOptions);

                    break;
                case BrowserFlavours.Firefox:
                    var profile = new FirefoxProfile();
                    profile.SetPreference("general.useragent.override",
                        "Mozilla/5.0 (iPad; CPU OS 6_0 like Mac OS X) AppleWebKit/536.26 (KHTML, like Gecko) Version/6.0 Mobile/10A5355d Safari/8536.25");
                    var firefoxOptions = new FirefoxOptions
                    {
                        AcceptInsecureCertificates = true,
                        Profile = profile
                    };

                    driver = new FirefoxDriver(BuildDirectory, firefoxOptions);
                    driver.Manage().Window.Maximize();

                    break;
                case BrowserFlavours.InternetExplorer:
                    var internetExplorerOptions = new InternetExplorerOptions
                    {
                        IntroduceInstabilityByIgnoringProtectedModeSettings = true,
                        RequireWindowFocus = true,
                        EnsureCleanSession = true,
                        AcceptInsecureCertificates = false,
                    };

                    driver = new InternetExplorerDriver(BuildDirectory, internetExplorerOptions,
                        TimeSpan.FromMinutes(3));
                    //driver.Manage().Timeouts().PageLoad.Add(TimeSpan.FromSeconds(30));
                    driver.Manage().Window.Maximize();

                    break;
                default:
                    break;
            }

            return driver;
        }

        public static void DisposeDriverProcesses()
        {
            var processesToCheck = new List<string>
            {
                "chromedriver",
                "iedriverserver",
                "geckodriver",
                "webdriver"
            };

            var processes = Process.GetProcesses();

            foreach (var process in processes)
            {
                try
                {
                    var shouldKill = processesToCheck
                        .Any(processName => process.ProcessName.ToLower().Contains(processName));
                    if (!shouldKill)
                        continue;
                    process.Kill();
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e);
                }
            }
        }
    }
}
