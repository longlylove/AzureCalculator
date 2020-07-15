using System;
using System.Collections.Generic;
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

namespace AzureCalculatorAPI.Base
{
    [Binding]
    public class SetupHooks: SelectSettings
    {
        private readonly ScenarioContext _scenarioContext;
        private readonly FeatureContext _featureContext;
        private readonly TestContext _testContext;
        private readonly Contexts _contexts;
        private static ExtentReports _extent;
        private static ExtentV3HtmlReporter _v3HtmlReporter;
        private static List<string> _logs;
        private static ReportLogs _report;

        public SetupHooks(
            Contexts contexts, TestContext testContext,
            ScenarioContext scenarioContext, FeatureContext featureContext) : 
            base(contexts, testContext)
        {
            _contexts = contexts;
            _testContext = testContext;
            _scenarioContext = scenarioContext;
            _featureContext = featureContext;
        }

        [BeforeTestRun(Order = 0)]
        public static void GetGenerateReportSettingFromEnvironment()
        {
            var utils = new Utils();
            var generateReport = utils.GetAppSettings("GenerateReport");
            Properties.Test.Default.GenerateReport =
                string.IsNullOrWhiteSpace(generateReport) || bool.Parse(generateReport);
        }

        [BeforeScenario(Order = 1)]
        public void GetConfigurationFromRunSettings()
        {
            string value;

            #region Test.settings
            Properties.Test.Default.AzureCalculatorHost =
                GetNewSettingsIfExists("AzureCalculatorHost", out value)
                    ? value.ToUpper()
                    : Properties.Test.Default.AzureCalculatorHost;

            Properties.Test.Default.AzureCalculatorPath =
                GetNewSettingsIfExists("AzureCalculatorPath", out value)
                    ? value.ToUpper()
                    : Properties.Test.Default.AzureCalculatorPath;
            #endregion
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

        [BeforeTestRun(Order = 1)]
        public static void ReportSetup()
        {
            if (Properties.Test.Default.GenerateReport)
            {
                _report = new ReportLogs
                {
                    Name = "AzureCalculator_API"
                };
                _v3HtmlReporter = ReportGenerator.InitializeReport("AzureCalculator_API");
                _v3HtmlReporter.Start();
                _extent = new ExtentReports();
                _extent.AttachReporter(_v3HtmlReporter);
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

        [AfterTestRun(Order = 2)]
        public static void GenerateTestReport()
        {
            if (Properties.Test.Default.GenerateReport)
            {
                _v3HtmlReporter.Stop();
                _extent.AddSystemInfo("Azure Calculator API endpoint",
                    $"{Properties.Test.Default.AzureCalculatorHost} ({Properties.Test.Default.AzureCalculatorPath})");
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
    }
}
