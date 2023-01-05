using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Safari;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Chromium;

namespace ConsoleApp1
{
    public class Drivers
    {
        public IWebDriver Driver { get; set; }
        const string URL = "https://strelkacard.ru/";
        public static string Path => Environment.CurrentDirectory;

        public Drivers() 
        {
            // TODO: сделать проверки или отдельный метод executeBrowser
            try
            {
                // Init Chrome Driver;
                Driver = Chrome();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Возникли пробелемы с бразуером CHROME\n" +
                    $"{e}");
                try
                {
                    // Init Gecko Driver;
                    Driver = Mozilla();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Возникли проблемы с браузером FIREFOX\n" +
                        $"{ex}");
                    try
                    {
                        //TODO: IE
                        Driver = IE();
                    }
                    catch
                    {
                        try
                        {
                            //TODO: Edge
                            Driver = Edge();
                        }
                        catch
                        {
                            try
                            {
                                //TODO: Safari
                                Driver = Safari();
                            }
                            catch
                            {
                                Console.WriteLine(ex);
                                //try
                                //{
                                //    //TODO: Chromium
                                //    Driver = Chromium();
                                //}
                                //catch
                                //{

                                //}
                            }
                        }
                    }

                }
            }
        }
        public IWebDriver Chrome()
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
                }); //"headless"

            ChromeDriverService service = ChromeDriverService.CreateDefaultService(Path + "\\driver");
            service.HideCommandPromptWindow = true;
            service.SuppressInitialDiagnosticInformation = true;

            // Init Chrome WebDriver
            return new ChromeDriver(service, options) { Url = URL };
        }
        public IWebDriver Mozilla()
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
        public IWebDriver IE()
        {
            InternetExplorerOptions options = new InternetExplorerOptions();

            InternetExplorerDriverService service = InternetExplorerDriverService.CreateDefaultService(Path + "\\driver");
            service.HideCommandPromptWindow = true;
            service.SuppressInitialDiagnosticInformation = true;

            return new InternetExplorerDriver(service, options) { Url = URL };
        }
        public IWebDriver Edge()
        {
            EdgeOptions options = new();

            EdgeDriverService service = EdgeDriverService.CreateDefaultService(Path + "\\driver");
            service.HideCommandPromptWindow = true;
            service.SuppressInitialDiagnosticInformation = true;

            return new EdgeDriver(service, options) { Url = URL };
        }
        public IWebDriver Safari()
        {
            SafariOptions options = new();

            SafariDriverService service = SafariDriverService.CreateDefaultService(Path + "\\driver");
            service.HideCommandPromptWindow = true;
            service.SuppressInitialDiagnosticInformation = true;

            return new SafariDriver(service, options) { Url = URL };
        }
    }
}
