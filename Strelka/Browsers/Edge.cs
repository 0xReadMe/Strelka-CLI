﻿using OpenQA.Selenium;
using OpenQA.Selenium.Edge;

namespace Strelka_DLL
{
    class Edge : Browser
    {
        public Edge()
        {

        }

        public IWebDriver InitializeEdge()
        {
            // Set settings for Chrome WebDriver
            EdgeOptions options = new EdgeOptions();
            options.AddArguments(new List<string>() {

                });

            EdgeDriverService service = EdgeDriverService.CreateDefaultService(ExecutePath + "\\driver");
            service.HideCommandPromptWindow = true;
            service.SuppressInitialDiagnosticInformation = true;
            return new EdgeDriver(service, options) { Url = URL };
        }
    }
}
