using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;

namespace Strelka_DLL
{
    class Chrome : Browser
    {
        public Chrome() 
        {
            
        }

        public IWebDriver InitializeChrome()
        {
            // Set settings for Chrome WebDriver
            ChromeOptions options = new ChromeOptions();
            options.AddArguments(new List<string>() {
                "disable-gpu",
                "headless",
                "disable-infobars",
                "no-sandbox",
                "disable-images",
                "incognito",
                "disable-extensions",
                "ignore-certificate-errors"
                });

            ChromeDriverService service = ChromeDriverService.CreateDefaultService(ExecutePath + "\\driver");
            service.HideCommandPromptWindow = true;
            service.SuppressInitialDiagnosticInformation = true;
            return new ChromeDriver(service, options) { Url = URL };
        }
    }
}
