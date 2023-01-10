using OpenQA.Selenium.IE;
using OpenQA.Selenium;

namespace Strelka_DLL
{
    class InternetExplorer : Browser
    {
        public InternetExplorer()
        {

        }
        public IWebDriver InitializeIE()
        {
            InternetExplorerOptions options = new InternetExplorerOptions();
            //options.AddAdditionalInternetExplorerOption();

            InternetExplorerDriverService service = InternetExplorerDriverService.CreateDefaultService(Path + "\\driver");
            service.HideCommandPromptWindow = true;
            service.SuppressInitialDiagnosticInformation = true;

            return new InternetExplorerDriver(service, options) { Url = URL };
        }
    }
}
