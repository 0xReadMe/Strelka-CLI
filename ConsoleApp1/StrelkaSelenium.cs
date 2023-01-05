using Newtonsoft.Json;
using OpenQA.Selenium;
namespace ConsoleApp1
{
    public class StrelkaSelenium
    {
        #region Поля
        public byte codeException;
        public List<double> balances;
        public List<string> cardNames;
        #endregion

        #region Свойства
        public string CardNumber { get; set; }
        public IWebDriver Driver { get; set; }
        public Random Random { get; set; }
        #endregion

        #region Constructor
        // Contructor for create wedriver
        public StrelkaSelenium(string cardNumber)
        {
            CardNumber = cardNumber;

            var drivers = new Drivers();
            Driver = drivers.Driver;
            
            Random Random = new Random();
            this.Random = Random;
        }
        #endregion

        #region Methods
        // Method for get balance on strelkacard.ru
        public string GetBalance()
        {
            // Find input field for send card number
            IWebElement inputField = Driver.FindElement(By.Name("cardnum"));
            inputField.Click();
            inputField.SendKeys(CardNumber);

            // Find and click button for activate AngularJS script and load div-block with balance
            IWebElement buttonClick = FindCSS("body > div.snap-content > div > section.landing-check.card-background " +
                "> div.container.tile-container > div.tile-cols > div.tile-right > div.tile-row.ng-scope > div.tile-box.tile-link.tile-big " +
                "> form > div.tile-link.tile-box.tile-small.ng-pristine.ng-untouched.ng-valid.ng-isolate-scope > img");
            buttonClick.Click();
            // Delay for wait a full load AngularJS script
            Thread.Sleep(Random.Next(600, 1000));
            
            // Find a p-block with  balance and get them text
            var balance = FindXpath("//p[@class='ng-binding']").GetAttribute("textContent");

            // Check if Angular script is slow and he return "--" 
            if (balance == "--") {
                Driver.Navigate().Refresh();
                balance = GetBalance();
            }

            return balance; // Return card balance
        }

        // Method for autorization on strelkacard.ru
        public string Authorization(string login, string pwd) 
        {
            Driver.Navigate().GoToUrl(Driver.Url);
            // Find authorization button and click them
            FindCSS(".header-auth-in").Click();
            Thread.Sleep(500); // A little delay for load form

            // Find input field, click them and send "login" to form
            IWebElement inputLogin = FindCSS(".login > form:nth-child(3) > div:nth-child(1) > input:nth-child(2)");
            inputLogin.Click();
            inputLogin.SendKeys(login);

            // Find input field
            IWebElement inputPwd = FindCSS(".login > form:nth-child(3) > div:nth-child(2) > input:nth-child(2)");
            inputPwd.Click();
            inputPwd.SendKeys(pwd);

            // Click to checkbox "Запомнить меня"
            FindCSS(".login > form:nth-child(3) > div:nth-child(4) > div:nth-child(1) > label:nth-child(1) > input:nth-child(1)").Click();

            // Find login-button and click them
            FindCSS("form.ng-dirty > button:nth-child(5)").Click();
            Thread.Sleep(1000); // Wait for load page

            // If page title == main page title, we catch exception
            if (Driver.Title == "Стрелка") 
            {
                return ExceptionAccont(out codeException);
            }
            else{
                GetPersonalAccount(out balances, out cardNames);
                return "success";
            }
        }

        protected void GetPersonalAccount(out List<double> balances, out List<string> cardNames) 
        {
            // TODO: продумать архитектуру
            Thread.Sleep(1500);
            var mainPageBalance = FindCSS("._red > p:nth-child(1)").GetAttribute("textContent");
            var mainPageNameCard = FindCSS("li.cards-item:nth-child(1) > div:nth-child(1) > div:nth-child(1) > div:nth-child(1)").GetAttribute("textContent");
            var allBalances = Driver.FindElements(By.ClassName("count"));
            balances = new List<double>(); // out
            // TODO: исправить поиск тегов
            for (int i = 0; i < allBalances.Count; i++) {
                if (allBalances[i].FindElement(By.TagName("p")).TagName == "p")
                {
                    balances.Add(Convert.ToDouble(allBalances[i].GetAttribute("textContent")));
                }
            }
            var allNameCards = Driver.FindElements(By.ClassName("cards-item-title"));
            cardNames = new List<string>(); // out
            for (int i = 0; i < allNameCards.Count; i++)
            {
                if (allNameCards[i].GetAttribute("textContent") != "Баланс" && allNameCards[i].GetAttribute("textContent") != "Услуги") 
                {
                    cardNames.Add(allNameCards[i].GetAttribute("textContent"));
                }
            }
            
        }

        public void GetCookies() {
            string[] cookies = File.ReadAllText("ourCookie.txt").Trim().Split("; ");
            Driver.Manage().Cookies.DeleteAllCookies();
            foreach (string cookie in cookies)
            {
                Console.WriteLine(cookie);
                Driver.Manage().Cookies.AddCookie(new Cookie(cookie.Split('=')[0], cookie.Split('=')[1]));
            }
            Driver.Navigate().GoToUrl("https://lk.strelkacard.ru/cards");
            var findCookie = Driver.Manage().Cookies.AllCookies;
            var s = new List<string>();
            var b = new List<string>();
            foreach (var cook in findCookie) 
            {
                s.Add(cook.Value);
                
                string item = string.Join(Environment.NewLine, cook.Name);
                b.Add(item);

            }
            using (StreamWriter outF = new StreamWriter("findCookie.txt"))
            {
                for(int i = 0; i < s.Count; i++)
                    outF.WriteLine(b[i] + "=" + s[i]);
            }
            

        }
        #region UtilsMethods
        private IWebElement FindCSS(string SelectorCSS)
        {
            return Driver.FindElement(By.CssSelector(SelectorCSS));
        }
        private IWebElement FindXpath(string xpath)
        {
            return Driver.FindElement(By.XPath(xpath));
        }
        private IWebElement FindClass(string className)
        {
            return Driver.FindElement(By.ClassName(className));
        }
        public void CloseWeb()
        {
            Driver.Quit();
        }
        public static void KillProcesses() 
        {
            // names of processes, which need to kill
            List<string> name = new List<string> { "chromedriver", "geckodriver"};

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
        public string ExceptionAccont(out byte codeException) 
        {
            // TODO: ДОписать определение ошибок входа в аккаунт            
            try {
                // Brute defence exception (Превышено количество попыток входа)
                IWebElement exception = FindCSS("body > div.snap-content > div > header > div > div " +
                "> div.header-right > div:nth-child(1) > div.header-auth > div > form > div:nth-child(2) > div");
                if (exception.GetAttribute("textContent") == "Превышено максимальное число попыток. Попробуйте позднее")
                {
                    codeException = 0;
                    return "Error 0x00 (Brute defense exception)";
                }
                else
                {
                    exception = FindCSS("111");
                    codeException = 0;
                    return "";
                }
            }
            catch {
                try {
                    IWebElement exception = FindCSS("html.ng-scope body div.snap-content div.container-min-height header.header " +
                    "div.container div.header-box div.header-right div.header-right-row div.header-auth div.login.ng-scope.show " +
                    "form.ng-scope.ng-isolate-scope.ng-dirty.ng-valid-parse.ng-submitted.ng-invalid.ng-invalid-server div.form-group.has-error " +
                    "div.form-notice.ng-binding.ng-scope");
                    codeException = 1;
                    return "Error 0x01 (Login exception)";
                }
                catch {
                    
                    try {
                        IWebElement exception = FindCSS("body > div.snap-content > div > header > div > div > div.header-right " +
                            "> div:nth-child(1) > div.header-auth > div > form > div:nth-child(2) > div");
                        if (exception.GetAttribute("textContent") == "Неверное сочетание логина и пароля.")
                        {
                            codeException = 2;
                            return "Error 0x02 (Incorrect login or password)";
                        }
                        else {
                            exception = FindCSS("111");
                            codeException = 2;
                            return "";
                        }
                    }
                    catch (Exception e){
                        codeException = 255;
                        return e.Message;
                    }
                }
            
            }
        }

        #endregion
        #endregion
    }
}