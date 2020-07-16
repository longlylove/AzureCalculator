using System;
using AzureCalculator.Pages;
using Framework.Base;
using Framework.Models;
using Framework.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TechTalk.SpecFlow;

namespace AzureCalculator.Steps
{
    [Binding]
    public class AzureCalculatorOnBrowserSteps: SeleniumActionHelper
    {
        private readonly CalculatorPage _calculatorPage;
        private readonly Contexts _contexts;
        private string _resultText;

        public AzureCalculatorOnBrowserSteps(ScenarioContext scenarioContext, Contexts contexts) : base(contexts)
        {
            _contexts = contexts;
            _calculatorPage = new CalculatorPage(_contexts.Driver);
            
        }

        [Given(@"I am on the Azure Calculator site")]
        public void GivenIAmOnTheAzureCalculatorSite()
        {
            NavigateToUrl(Properties.Test.Default.AzureCalculatorUrl);
            new WaitFor(_contexts).PageToLoad("Simple Calculator");
        }
        
        [When(@"'(.*)' '(.*)' '(.*)' via calculator UI")]
        public void WhenViaCalculatorUi(string leftNumber, string doOperation, string rightNumber)
        {
            _resultText = _calculatorPage.DoCalculate(leftNumber, doOperation, rightNumber);
        }
        
        [Then(@"'(.*)' is returned")]
        public void ThenIsReturned(string resultNumber)
        {
            Assert.IsTrue(_resultText==resultNumber,
                $"Unexpected result. Expected: {resultNumber}. Actual: {_resultText}");
        }
    }
}
