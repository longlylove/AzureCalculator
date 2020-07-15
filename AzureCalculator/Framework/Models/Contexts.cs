using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;

namespace Framework.Models
{
    public class Contexts
    {
        public IWebDriver Driver;
        public List<string> Logs;
        public string Screenshot;
    }
}
