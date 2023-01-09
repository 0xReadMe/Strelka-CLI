using OpenQA.Selenium;

namespace Strelka_DLL
{
    abstract class Browser
    {
        protected const string URL = "https://strelkacard.ru/";
        public static string Path => Environment.CurrentDirectory;        
    }
}
