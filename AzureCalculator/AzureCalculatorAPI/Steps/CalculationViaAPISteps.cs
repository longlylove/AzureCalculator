using System;
using System.Collections.Generic;
using System.Net;
using AzureCalculatorAPI.Base;
using AzureCalculatorAPI.Models;
using Framework.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using RestSharp;
using TechTalk.SpecFlow;

namespace AzureCalculatorAPI.Steps
{
    [Binding]
    public class CalculationViaApiSteps
    {
        private static readonly Utils Utils = new Utils();
        private static IRestResponse _response;
        private static readonly RequestHelper RequestHelper = new RequestHelper();

        [When(@"'(.*)' '(.*)' '(.*)' via API")]
        public void WhenViaApi(string leftNumber, string doOperation, string rightNumber)
        {
            var operation = "";
            switch (doOperation)
            {
                case "adding":
                    operation = "+";
                    break;
                case "subtracting":
                    operation = "-";
                    break;
                case "multiplying":
                    operation = "*";
                    break;
                case "dividing":
                    operation = "/";
                    break;
                default:
                    break;
            }

            var calcPayload = new CalculationFormula
            {
                LeftNumber = decimal.Parse(leftNumber),
                RightNumber = decimal.Parse(rightNumber),
                Operator = operation
            };
            
            var headers = new Dictionary<string, string>
            {
                {"Accept", "*/*"},
                {"Host", $"{Properties.Test.Default.AzureCalculatorHost}" },
                {"Origin", $"https://{Properties.Test.Default.AzureCalculatorHost}" },
                {"Referer", $"https://{Properties.Test.Default.AzureCalculatorHost}/" },
                {"Sec-Fetch-Dest", "empty" },
                {"Sec-Fetch-Mode", "cors" },
                {"Sec-Fetch-Site", "cross-site" },
                {"x-functions-key", Utils.GetAppSettings("CalcApiKey")}
            };


            _response = RequestHelper.ExecuteJson(
                Properties.Test.Default.AzureCalculatorHost,
                Properties.Test.Default.AzureCalculatorPath,
                Method.POST, true,
                headers,
                null,
                calcPayload
            );
        }

        [Then(@"'(.*)' is returned with http code '(.*)'")]
        public void ThenIsReturnedWithHttpCode(string resultNumber, int httpCode)
        {
            Console.WriteLine($"Response code: {(int)_response.StatusCode} - {_response.StatusCode}");
            Assert.IsTrue((int)_response.StatusCode == httpCode,$"Unexpected response code. Expected: {httpCode}. Actual: {(int)_response.StatusCode}");

            if (_response.Content!=null)
                Console.WriteLine($"Response content: {_response.Content}");
            if (resultNumber == "N/A")
                return;
            var content = JsonConvert.DeserializeObject<CalculationResult>(_response.Content);
            Assert.IsTrue(decimal.Parse(content.Value) == decimal.Parse(resultNumber), $"Unexpected calculation results. Expected: {resultNumber}. Actual: {content.Value}");
        }
    }
}
