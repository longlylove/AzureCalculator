using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.Base;
using Framework.Utilities;
using OpenQA.Selenium;

namespace AzureCalculator.Pages
{
    public class CalculatorPage: SeleniumActionHelper
    {
        private readonly IWebDriver _driver;
        private readonly JsExecutor _jsExecutor;

        public CalculatorPage(IWebDriver driver) : base(driver)
        {
            _driver = driver;
            _jsExecutor = new JsExecutor(_driver);
        }

        private readonly By _leftNumber = By.Id("leftNumber");
        private readonly By _rightNumber = By.Id("rightNumber");
        private readonly By _operatorSelector = By.Id("operator");
        private readonly By _addOperator = By.XPath("//*[@id='operator']/option[@value='+']");
        private readonly By _subtractOperator = By.XPath("//*[@id='operator']/option[@value='-']");
        private readonly By _multiplyOperator = By.XPath("//*[@id='operator']/option[@value='*']");
        private readonly By _divideOperator = By.XPath("//*[@id='operator']/option[@value='/']");
        private readonly By _resultNumber = By.XPath("//input[@class='result']");
        private readonly By _calculateButton = By.Id("calculate");
        private readonly By _iframeCalc = By.XPath("//div/iframe");

        public string DoCalculate(string leftNumber, string doOperation, string rightNumber)
        {
            leftNumber = !string.IsNullOrWhiteSpace(leftNumber) ? leftNumber : "0";
            rightNumber = !string.IsNullOrWhiteSpace(rightNumber) ? rightNumber : "0";
            doOperation = !string.IsNullOrWhiteSpace(doOperation) ? doOperation : "adding";

            EnterText(_leftNumber,leftNumber);
            EnterText(_rightNumber, rightNumber);
            Click(_operatorSelector);
            WaitInMilliseconds(500);
            switch (doOperation)
            {
                case "adding":
                    Click(_addOperator);
                    break;
                case "subtracting":
                    Click(_subtractOperator);
                    break;
                case "multiplying":
                    Click(_multiplyOperator);
                    break;
                case "dividing":
                    Click(_divideOperator);
                    break;
                default:
                    break;
            }
            SwitchToFrame(_iframeCalc);
            Click(_calculateButton);

            SwitchToDefaultContent();
            var resultText = "";
            using (var s = new StopwatchHelper())
            {
                while (s.Elapsed.TotalMilliseconds <= TimeSpan.FromSeconds(7).TotalMilliseconds)
                {
                    resultText = GetElementAttribute(_resultNumber,"value");
                    if (resultText == "")
                        WaitInMilliseconds(200);
                    else
                        break;
                }
            }

            return resultText;
        }
    }
}
