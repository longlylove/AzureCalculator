using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using Framework.Models;
using Framework.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TechTalk.SpecFlow;

namespace Framework.Base
{
    public class ReportGenerator: Utils
    {
        public ReportGenerator(Contexts contexts) : base(contexts)
        {
        }

        public static ExtentV3HtmlReporter InitializeReport(string reportName)
        {
            var reportsDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Reports");

            if (!Directory.Exists(reportsDirectory))
            {
                Directory.CreateDirectory(reportsDirectory);
            }

            var reportsFilename = $"{reportName}_{DateTime.Now:yyyyMMdd_HHmmss}_{Properties.Test.Default.AzureCalculatorPath}.html";

            var reportsPath = Path.Combine(reportsDirectory, reportsFilename);

            var v3HtmlReporter = new ExtentV3HtmlReporter(reportsPath);

            v3HtmlReporter.LoadConfig(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "reports_config.xml"));

            return v3HtmlReporter;
        }

        public static void CreateStepNode(
            ReportLogs report, List<string> logs,
            FeatureContext featureContext, TestContext testContext, 
            ScenarioContext scenarioContext, Contexts contexts)
        {
            if (report.Features == null)
            {
                report.Features = new List<FeatureNode>();
            }

            var feature = report.Features.Find(f => f.Name == featureContext.FeatureInfo.Title);

            if (feature == null)
            {
                feature = new FeatureNode
                {
                    Name = featureContext.FeatureInfo.Title
                };

                report.Features.Add(feature);
            }

            if (feature.Scenarios == null)
            {
                feature.Scenarios = new List<ScenarioNode>();
            }

            var scenario = feature.Scenarios.Find(s => s.Name == testContext.TestName);

            if (scenario == null)
            {
                scenario = new ScenarioNode
                {
                    Name = testContext.TestName,
                    Tags = scenarioContext.ScenarioInfo.Tags
                };

                feature.Scenarios.Add(scenario);
            }

            var keyword = scenarioContext.StepContext.StepInfo.StepInstance.Keyword;
            var step = scenarioContext.StepContext.StepInfo.StepInstance.Text;

            if (string.IsNullOrEmpty(keyword))
            {
                keyword = scenarioContext.StepContext.StepInfo.StepInstance.StepDefinitionKeyword.ToString();

                logs.Add($"{keyword} {step}");
                logs.AddRange(contexts.Logs);
            }
            else
            {
                logs = logs?.Select(x => "&emsp;" + x.Replace("<br />", "<br />&emsp;")).ToList();

                logs.AddRange(contexts.Logs);

                if (scenario.Steps == null)
                {
                    scenario.Steps = new List<StepNode>();
                }

                if (scenarioContext.TestError == null)
                {
                    scenario.Steps.Add(new StepNode
                    {
                        Keyword = keyword,
                        Name = step,
                        Pass = true,
                        Logs = new List<string>(logs),
                        ErrorMessage = string.Empty,
                        Screenshot = string.Empty
                    });
                }
                else
                {
                    scenario.Steps.Add(new StepNode
                    {
                        Keyword = keyword,
                        Name = step,
                        Pass = scenarioContext.TestError == null,
                        Logs = new List<string>(logs),
                        ErrorMessage = scenarioContext.TestError.Message,
                        
                        Screenshot = report.Name.Contains("API")? string.Empty : contexts.Screenshot
                    });
                }
            }
        }
    }
}
