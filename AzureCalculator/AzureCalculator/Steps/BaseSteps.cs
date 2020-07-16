using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AventStack.ExtentReports;
using AventStack.ExtentReports.Gherkin.Model;
using AventStack.ExtentReports.Reporter;
using Framework.Base;
using Framework.Models;
using Framework.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Tracing;

namespace AzureCalculator.Steps
{
    [Binding]
    public class BaseSteps: SelectSettings
    {
        private readonly ScenarioContext _scenarioContext;
        private readonly FeatureContext _featureContext;
        private readonly TestContext _testContext;
        private readonly Contexts _contexts;
        private static ExtentReports _extent;
        private static ExtentV3HtmlReporter _v3HtmlReporter;
        private static List<string> _logs;
        private static ReportLogs _report;
        private static string _runDirectory;

        public BaseSteps(
            Contexts contexts, TestContext testContext,
            ScenarioContext scenarioContext, FeatureContext featureContext) :
            base(contexts, testContext)
        {
            _contexts = contexts;
            _testContext = testContext;
            _scenarioContext = scenarioContext;
            _featureContext = featureContext;
        }

        #region Before

        [BeforeTestRun(Order = 1)]
        public static void GetGenerateReportSettingFromEnvironment()
        {
            var utils = new Utils();
            var generateReport = utils.GetAppSettings("GenerateReport");
            Properties.Test.Default.GenerateReport =
                !string.IsNullOrWhiteSpace(generateReport)
                    ? bool.Parse(generateReport)
                    : Properties.Test.Default.GenerateReport;
        }

        [BeforeTestRun(Order = 2)]
        public static void ReportSetup()
        {
            if (Properties.Test.Default.GenerateReport)
            {
                _report = new ReportLogs
                {
                    Name = "AzureCalculator_UI"
                };
                _runDirectory = $"AzureCalculator_UI_{DateTime.Now:yyyyMMdd_HHmmss}";
                _v3HtmlReporter = ReportGenerator.InitializeReport("AzureCalculator_UI",_runDirectory);
                _v3HtmlReporter.Start();
                _extent = new ExtentReports();
                _extent.AttachReporter(_v3HtmlReporter);
            }
        }

        [BeforeScenario(Order = 1)]
        public void GetConfigurationFromRunSettings()
        {
            Properties.Test.Default.AzureCalculatorUrl =
                GetNewSettingsIfExists("AzureCalculatorUrl", out var value)
                    ? value.ToUpper()
                    : Properties.Test.Default.AzureCalculatorUrl;
        }

        [BeforeScenario(Order = 2)]
        public void InitializeDriver()
        {
            Properties.Test.Default.Browser =
                GetNewSettingsIfExists("Browser", out var value)
                    ? value.ToUpper()
                    : Properties.Test.Default.Browser;
            _contexts.Driver = DriverSetup.SetupBrowser(Properties.Test.Default.Browser);
        }

        [BeforeStep(Order = 0)]
        public void AddLogsArray()
        {
            if (Properties.Test.Default.GenerateReport)
            {
                _logs = new List<string>();
                _contexts.Logs = new List<string>();
            }
        }

        [BeforeTestRun(Order = 0)]
        [AfterTestRun(Order = 1)]
        public static void DriverTearDown()
        {
            DriverSetup.DisposeDriverProcesses();
        }
        #endregion

        #region After
        [AfterStep(Order = 1)]
        public void CaptureScreenshotOnFailure()
        {
            if (_scenarioContext.TestError != null)
            {
                try
                {
                    Console.Out.WriteLine(_scenarioContext.TestError);

                    if (!Directory.Exists(ReportsDirectory)) Directory.CreateDirectory(ReportsDirectory);

                    var fileName = $"{_scenarioContext.ScenarioInfo.Title.ToIdentifier()}_{DateTime.Now:yyyyMMdd_HHmmss}.png";
                    var filePath = Path.Combine(ReportsDirectory, 
                        $"{_runDirectory}\\{fileName}");
                    var screenshot = new Screenshot(_contexts.Driver);

                    screenshot.CaptureScreen(filePath);

                    _contexts.Screenshot = filePath;
                }
                catch (Exception e)
                {
                    Console.Out.WriteLine($"[ERROR] Failed to get screenshot of the page.\n" + e);
                }
            }
        }

        [AfterStep(Order = 2)]
        public void LogRunStep()
        {
            if (Properties.Test.Default.GenerateReport)
            {
                ReportGenerator.CreateStepNode(_report, _logs,
                    _featureContext, _testContext, _scenarioContext, _contexts);
            }
        }

        [AfterScenario(Order = 1)]
        public void CloseDriver()
        {
            try
            {
                _contexts.Driver.Quit();
            }
            catch
            {
                _contexts.Driver.Quit();
            }
            finally
            {
                _contexts.Driver?.Quit();
            }
        }

        [AfterTestRun(Order = 2)]
        public static void GenerateTestReport()
        {
            if (Properties.Test.Default.GenerateReport)
            {
                _v3HtmlReporter.Stop();
                _extent.AddSystemInfo("Azure Calculator page",
                    $"{Properties.Test.Default.AzureCalculatorUrl})");
                foreach (var feature in _report.Features)
                {
                    var featureNode = _extent.CreateTest<Feature>(feature.Name);

                    foreach (var scenario in feature.Scenarios)
                    {
                        var scenarioNode = featureNode.CreateNode<Scenario>(scenario.Name);

                        foreach (var tag in scenario.Tags)
                        {
                            scenarioNode.AssignCategory(tag);
                        }

                        foreach (var step in scenario.Steps)
                        {
                            var gherkinKeyword = new GherkinKeyword(step.Keyword.Trim());

                            var stepNode = scenarioNode.CreateNode(gherkinKeyword, step.Keyword + step.Name);

                            foreach (var log in step.Logs)
                            {
                                stepNode.Info(log);
                            }

                            if (!step.Pass && !_report.Name.Contains("API"))
                            {
                                stepNode.Fail(step.ErrorMessage).AddScreenCaptureFromPath(step.Screenshot);
                            }
                        }
                    }
                }

                _extent.Flush();
            }
        }

        #endregion
    }
}
