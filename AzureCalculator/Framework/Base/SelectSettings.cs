using Framework.Models;
using Framework.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TechTalk.SpecFlow;

namespace Framework.Base
{
    [Binding]
    public class SelectSettings : Utils
    {
        private Contexts _contexts;
        private TestContext _testContext;

        public SelectSettings(Contexts contexts, TestContext testContext)
        {
            _contexts = contexts;
            _testContext = testContext;
        }

        public bool GetNewSettingsIfExists(string key, out string value)
        {
            value = string.Empty;

            var settingObj = _testContext.Properties[key];

            if (settingObj == null)
                return false;
            value = settingObj.ToString();
            return true;
        }
    }
}
