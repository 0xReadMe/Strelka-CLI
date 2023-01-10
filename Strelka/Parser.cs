using OpenQA.Selenium;


namespace Strelka_DLL
{
    public class Parser
    {
        public IWebDriver Browser { get; set; }

        public Parser()
        {
            try
            {
                Chrome chrome = new Chrome();
                Browser = chrome.InitializeChrome();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Возникли пробелемы с бразуером CHROME\n" +
                    $"{e}");
                try
                {
                    // Init Gecko Driver;
                    Mozilla mozilla = new Mozilla();
                    Browser = mozilla.InitializeMozilla();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Возникли проблемы с браузером FIREFOX\n" +
                        $"{ex}");
                    try
                    {
                        //TODO: IE
                        InternetExplorer IE = new InternetExplorer();
                        Browser = IE.InitializeIE();
                    }
                    catch
                    {
                        try
                        {
                            //TODO: Edge
                            Edge edge = new Edge();
                            Browser = edge.InitializeEdge();
                        }
                        catch
                        {
                            try
                            {
                                //TODO: Safari
                                Safari safari = new Safari();
                                Browser = safari.InitializeSafari();
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

        #region Methods
        // Method for get balance on strelkacard.ru
        public string GetBalance(string cardNumber)
        {
            // Find input field for send card number
            IWebElement inputField = Browser.FindElement(By.Name("cardnum"));
            inputField.Click();
            inputField.SendKeys(cardNumber);

            // Find and click button for activate AngularJS script and load div-block with balance
            IWebElement buttonClick = FindCSS("body > div.snap-content > div > section.landing-check.card-background " +
                "> div.container.tile-container > div.tile-cols > div.tile-right > div.tile-row.ng-scope > div.tile-box.tile-link.tile-big " +
                "> form > div.tile-link.tile-box.tile-small.ng-pristine.ng-untouched.ng-valid.ng-isolate-scope > img");
            buttonClick.Click();
            // Delay for wait a full load AngularJS script
            Thread.Sleep(1000);

            // Find a p-block with  balance and get them text
            var balance = FindXpath("//p[@class='ng-binding']").GetAttribute("textContent");

            // Check if Angular script is slow and he return "--" 
            if (balance == "--")
            {
                Browser.Navigate().Refresh();
                balance = GetBalance(cardNumber);
            }

            return balance; // Return card balance
        }

        // Method for autorization on strelkacard.ru
        //public string Authorization(string login, string pwd)
        //{
        //    Browser.Navigate().GoToUrl(Browser.Url);
        //    // Find authorization button and click them
        //    FindCSS(".header-auth-in").Click();
        //    Thread.Sleep(500); // A little delay for load form

        //    // Find input field, click them and send "login" to form
        //    IWebElement inputLogin = FindCSS(".login > form:nth-child(3) > div:nth-child(1) > input:nth-child(2)");
        //    inputLogin.Click();
        //    inputLogin.SendKeys(login);

        //    // Find input field
        //    IWebElement inputPwd = FindCSS(".login > form:nth-child(3) > div:nth-child(2) > input:nth-child(2)");
        //    inputPwd.Click();
        //    inputPwd.SendKeys(pwd);

        //    // Click to checkbox "Запомнить меня"
        //    FindCSS(".login > form:nth-child(3) > div:nth-child(4) > div:nth-child(1) > label:nth-child(1) > input:nth-child(1)").Click();

        //    // Find login-button and click them
        //    FindCSS("form.ng-dirty > button:nth-child(5)").Click();
        //    Thread.Sleep(1000); // Wait for load page

        //    // If page title == main page title, we catch exception
        //    // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        //    // Подключить обработку ошибки
        //    //if (Browser.Title == "Стрелка")
        //    //{
        //    //    return ExceptionAccont(out codeException);
        //    //}
        //    //else
        //    //{
        //    //    GetPersonalAccount(out balances, out cardNames);
        //    //    return "success";
        //    //}
        //}

        //protected void GetPersonalAccount(out List<double> balances, out List<string> cardNames)
        //{
        //    Thread.Sleep(1500);
        //    var mainPageBalance = FindCSS("._red > p:nth-child(1)").GetAttribute("textContent");
        //    var mainPageNameCard = FindCSS("li.cards-item:nth-child(1) > div:nth-child(1) > div:nth-child(1) > div:nth-child(1)").GetAttribute("textContent");
        //    var allBalances = Browser.FindElements(By.ClassName("count"));
        //    balances = new List<double>(); // out
        //    // TODO: исправить поиск тегов
        //    for (int i = 0; i < allBalances.Count; i++)
        //    {
        //        if (allBalances[i].FindElement(By.TagName("p")).TagName == "p")
        //        {
        //            balances.Add(Convert.ToDouble(allBalances[i].GetAttribute("textContent")));
        //        }
        //    }
        //    var allNameCards = Browser.FindElements(By.ClassName("cards-item-title"));
        //    cardNames = new List<string>(); // out
        //    for (int i = 0; i < allNameCards.Count; i++)
        //    {
        //        if (allNameCards[i].GetAttribute("textContent") != "Баланс" && allNameCards[i].GetAttribute("textContent") != "Услуги")
        //        {
        //            cardNames.Add(allNameCards[i].GetAttribute("textContent"));
        //        }
        //    }

        //}

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
        public void CloseWeb()
        {
            Browser.Quit();
        }
        public static void KillProcesses()
        {
            // names of processes, which need to kill
            List<string> name = new List<string> { "chromedriver", "geckodriver" };

            // get all processes in system
            System.Diagnostics.Process[] process = System.Diagnostics.Process.GetProcesses();

            // go through each process
            foreach (System.Diagnostics.Process processName in process)
            {
                foreach (string s in name)
                {
                    if (processName.ProcessName.ToLower() == s.ToLower())
                    // find a process to kill
                    {
                        processName.Kill(); // kill them :)

                        // debugging
                        //Console.WriteLine($"Process {s.ToLower()} KILL");
                    }
                }
            }
        }
        #endregion
    }
}
