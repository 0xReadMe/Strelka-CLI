using OpenQA.Selenium.Firefox;
using OpenQA.Selenium;

namespace Strelka_DLL
{
    class Mozilla : Browser
    {
        public Mozilla()
        {

        }
        public IWebDriver InitializeMozilla()
        {
            // Set settings for Gecko WebDriver
            FirefoxOptions options = new FirefoxOptions();
            options.AddArguments(new List<string>
            {
                // TODO: добавить настройки
            });

            FirefoxDriverService service = FirefoxDriverService.CreateDefaultService(Path + "\\driver");
            service.HideCommandPromptWindow = true;
            service.SuppressInitialDiagnosticInformation = true;

            // Init Gecko WebDriver
            return new FirefoxDriver(service, options) { Url = URL };
        }
    }
}
