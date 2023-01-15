﻿using Microsoft.Win32;
using OpenQA.Selenium;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Strelka_DLL
{
    public class Parser
    {
        public IWebDriver Browser { get; set; }
        private Chrome _chrome = new Chrome();
        private Mozilla _mozilla = new Mozilla();
        private InternetExplorer _IE = new InternetExplorer();
        private Edge _edge = new Edge();
        private Safari _safari = new Safari();

        bool isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
        bool isOSX = RuntimeInformation.IsOSPlatform(OSPlatform.OSX);
        bool isLinux = RuntimeInformation.IsOSPlatform(OSPlatform.Linux);

        public Parser()
        {
            if (isWindows)
            {
                foreach (Browser browser in GetBrowsers()) 
                {
                    Console.WriteLine($"{browser.Name}: \n\tPath: {browser.Path} \n\tVersion: {browser.Version}");
                    Browser = browser.Name switch
                    {
                        "Google Chrome" => _chrome.InitializeChrome(),
                        "Mozilla Firefox" => _mozilla.InitializeMozilla(),
                        "LibreWolf" => _mozilla.InitializeMozilla(),
                        "Safari" => _safari.InitializeSafari(),
                        "Edge" => _edge.InitializeEdge(),
                        "Internet Explorer" => _IE.InitializeIE()
                    };
                    break;
                }



            }
            if (isOSX)
            {
                // TODO: конструкция свич с выбором браузера на основе парса браузеров
                try
                {
                    Browser = _chrome.InitializeChrome();
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Возникли пробелемы с бразуером CHROME\n {e}");
                    try
                    {
                        Browser = _mozilla.InitializeMozilla();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Возникли проблемы с браузером FIREFOX\n {ex}");
                        try
                        {
                            Browser = _IE.InitializeIE();
                        }
                        catch
                        {
                            try
                            {
                                Browser = _edge.InitializeEdge();
                            }
                            catch
                            {
                                try
                                {
                                    Browser = _safari.InitializeSafari();
                                }
                                catch
                                {
                                    Console.WriteLine(ex);
                                }
                            }
                        }

                    }
                }
            }
            if (isLinux)
            {
                // TODO: конструкция свич с выбором браузера на основе парса браузеров
                try
                {
                    Browser = _chrome.InitializeChrome();
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Возникли пробелемы с бразуером CHROME\n {e}");
                    try
                    {
                        Browser = _mozilla.InitializeMozilla();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Возникли проблемы с браузером FIREFOX\n {ex}");
                        try
                        {
                            Browser = _IE.InitializeIE();
                        }
                        catch
                        {
                            try
                            {
                                Browser = _edge.InitializeEdge();
                            }
                            catch
                            {
                                try
                                {
                                    Browser = _safari.InitializeSafari();
                                }
                                catch
                                {
                                    Console.WriteLine(ex);
                                }
                            }
                        }

                    }
                }
            }
            
        }

        #region Methods
        /// <summary>
        /// Method for get balance on strelkacard.ru.
        /// This method do not requires account authorization
        /// </summary>
        /// <param name="cardNumber">Card number</param>
        /// <returns>Balance of strelka card</returns>
        public string GetBalance(string cardNumber)
        {
            Browser.Navigate().GoToUrl(Browser.Url);
            IWebElement inputField = Browser.FindElement(By.Name("cardnum")); // Find input field for send card number
            inputField.Click();
            inputField.SendKeys(cardNumber);

            // Find and click button for activate AngularJS script and load div-block with balance
            IWebElement buttonClick = FindCSS("body > div.snap-content > div > section.landing-check.card-background > div.container.tile-container > div.tile-cols > div.tile-right > " +
                "div.tile-row.ng-scope > div.tile-box.tile-link.tile-big > form > div.tile-link.tile-box.tile-small.ng-pristine.ng-untouched.ng-valid.ng-isolate-scope > img"); 
            buttonClick.Click();
            Thread.Sleep(1000); // Delay for wait a full load AngularJS script

            var balance = FindXpath("//p[@class='ng-binding']").GetAttribute("textContent"); // Find a p-block with  balance and get them text

            // Check if Angular script is slowly then our program and he return "--" 
            if (balance == "--")
            {
                Browser.Navigate().Refresh();
                balance = GetBalance(cardNumber);
            }
            return balance; // Return card balance
        }

        /// <summary>
        /// Method for autorization on strelkacard.ru
        /// </summary>
        /// <param name="login">Your login from strelkacard.ru</param>
        /// <param name="pwd">Your password from strelkacard.ru</param>
        public void Authorization(string login, string pwd)
        {
            Browser.Navigate().GoToUrl(Browser.Url);// Find authorization button and click them
            FindCSS(".header-auth-in").Click();
            Thread.Sleep(500); // A little delay for load form

            IWebElement inputLogin = FindCSS(".login > form:nth-child(3) > div:nth-child(1) > input:nth-child(2)"); // Find input field, click them and send "login" to form
            inputLogin.Click();
            inputLogin.SendKeys(login);

            IWebElement inputPwd = FindCSS(".login > form:nth-child(3) > div:nth-child(2) > input:nth-child(2)"); // Find input field
            inputPwd.Click();
            inputPwd.SendKeys(pwd);
            
            FindCSS(".login > form:nth-child(3) > div:nth-child(4) > div:nth-child(1) > label:nth-child(1) > input:nth-child(1)").Click(); // Click to checkbox "Запомнить меня"

            // Find login-button and click them
            FindCSS("form.ng-dirty > button:nth-child(5)").Click();
            Thread.Sleep(2000); // Wait for load page
        }

        /// <summary>
        /// Get balances from personal account on strelkacard.ru. This method requires account authorization
        /// </summary>
        /// <returns>All card balances from your personal account</returns>
        public List<double> GetPersonalBalance() 
        {
            var allBalances = Browser.FindElements(By.ClassName("count"));
            List<double> balances = new List<double>();
            for (int i = 0; i < allBalances.Count; i++)
            {
                if (allBalances[i].FindElement(By.TagName("p")).TagName == "p")
                    balances.Add(Convert.ToDouble(allBalances[i].GetAttribute("textContent")));
            }
            return balances;
        }

        /// <summary>
        /// Get card names from personal account on strelkacard.ru. This method requires account authorization
        /// </summary>
        /// <returns>All card names from your personal account</returns>
        public List<string> GetPersonalCardNames() 
        {
            var allNameCards = Browser.FindElements(By.ClassName("cards-item-title"));
            List<string> cardNames = new List<string>();
            for (int i = 0; i < allNameCards.Count; i++)
            {
                if (allNameCards[i].GetAttribute("textContent") != "Баланс" && allNameCards[i].GetAttribute("textContent") != "Услуги")
                    cardNames.Add(allNameCards[i].GetAttribute("textContent"));
            }
            return cardNames;
        }

        /// <summary>
        /// Get type of cards from personal account on strelkacard.ru. This method requires account authorization
        /// </summary>
        public void GetPersonalTypeCards() 
        {
        
        }


        /// <summary>
        /// Get the price of the next ride. This method requires account authorization
        /// </summary>
        public void GetPersonalPriceForNextRide() 
        {
        
        }

        /// <summary>
        /// Get discount validity period. This method requires account authorization
        /// </summary>
        public void GetDiscountValidityPeriod() 
        {
        
        }

        /// <summary>
        /// Get mail from personal account on strelkacard.ru. This method requires account authorization
        /// </summary>
        public void GetPersonalAccountMail() 
        {
        
        }

        /// <summary>
        /// Get the count of rides to go to the next discount level. This method requires account authorization
        /// </summary>
        public void GetCountRidesForNextLevel() 
        {
        
        }

        //public void GetCookies()
        //{
        //    string[] cookies = File.ReadAllText("ourCookie.txt").Trim().Split("; ");
        //    Browser.Manage().Cookies.DeleteAllCookies();
        //    foreach (string cookie in cookies)
        //    {
        //        Console.WriteLine(cookie);
        //        Browser.Manage().Cookies.AddCookie(new Cookie(cookie.Split('=')[0], cookie.Split('=')[1]));
        //    }
        //    Browser.Navigate().GoToUrl("https://lk.strelkacard.ru/cards");
        //    var findCookie = Browser.Manage().Cookies.AllCookies;
        //    var s = new List<string>();
        //    var b = new List<string>();
        //    foreach (var cook in findCookie)
        //    {
        //        s.Add(cook.Value);

        //        string item = string.Join(Environment.NewLine, cook.Name);
        //        b.Add(item);

        //    }
        //    using (StreamWriter outF = new StreamWriter("findCookie.txt"))
        //    {
        //        for (int i = 0; i < s.Count; i++)
        //            outF.WriteLine(b[i] + "=" + s[i]);
        //    }


        //}
        #endregion

        #region UtilsMethods

        // Methods to simplify code reading
        private IWebElement FindCSS(string SelectorCSS)
        {
            return Browser.FindElement(By.CssSelector(SelectorCSS));
        }
        private IWebElement FindXpath(string xpath)
        {
            return Browser.FindElement(By.XPath(xpath));
        }
        private IWebElement FindClass(string className)
        {
            return Browser.FindElement(By.ClassName(className));
        }

        /// <summary>
        /// Get all browsers and their version, path, name on PC
        /// </summary>
        /// <returns>List of all browsers on PC</returns>
        private static List<Browser> GetBrowsers()
        {
            RegistryKey browserKeys = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\WOW6432Node\Clients\StartMenuInternet"); //on 64bit the browsers are in a different location
            if (browserKeys == null)
                browserKeys = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Clients\StartMenuInternet");

            string[] browserNames = browserKeys.GetSubKeyNames(); var browsers = new List<Browser>();

            for (int i = 0; i < browserNames.Length; i++)
            {
                Browser browser = new Browser();

                RegistryKey browserKey = browserKeys.OpenSubKey(browserNames[i]);
                browser.Name = (string)browserKey.GetValue(null);

                RegistryKey browserKeyPath = browserKey.OpenSubKey(@"shell\open\command");
                browser.Path = StripQuotes(browserKeyPath.GetValue(null).ToString());


                //RegistryKey browserIconPath = browserKey.OpenSubKey(@"DefaultIcon");
                //browser.IconPath = browserIconPath.GetValue(null).ToString().StripQuotes();

                browsers.Add(browser);
                if (browser.Path != null)
                    browser.Version = FileVersionInfo.GetVersionInfo(browser.Path).FileVersion;
                else
                    browser.Version = "unknown";
            }
            return browsers;
        }

        /// <summary>
        /// Static method for kill our browser processes
        /// </summary>
        public static void KillProcesses()
        {
            List<string> name = new List<string> { "chromedriver", "geckodriver" }; // Names of processes, which need to kill
            Process[] process = Process.GetProcesses(); // Get all processes in system
            foreach (Process processName in process) // Go through each process
            {
                foreach (string s in name)
                {
                    if (processName.ProcessName.ToLower() == s.ToLower()) // find a process to kill
                        processName.Kill(); // Kill them
                }
            }
        }

        /// <summary>
        /// If string begins and ends with quotes, they are removed
        /// </summary>
        /// <param name="s">Your string with quotes</param>
        /// <returns>String without quotes</returns>
        public static string StripQuotes(string s)
        {
            if (s.EndsWith("\"") && s.StartsWith("\""))
                return s.Substring(1, s.Length - 2);
            else
                return s;
        }
        #endregion
    }
}