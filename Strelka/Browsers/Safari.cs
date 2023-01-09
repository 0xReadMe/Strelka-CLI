using OpenQA.Selenium.Safari;
using OpenQA.Selenium;


namespace Strelka_DLL
{
    class Safari : Browser
    {
        public Safari()
        {
        
        }
        public IWebDriver InitializeSafari()
        {
            SafariOptions options = new();

            SafariDriverService service = SafariDriverService.CreateDefaultService(Path + "\\driver");
            service.HideCommandPromptWindow = true;
            service.SuppressInitialDiagnosticInformation = true;

            return new SafariDriver(service, options) { Url = URL };
        }
    }
}
