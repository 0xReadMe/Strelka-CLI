using OpenQA.Selenium;

namespace ConsoleApp1
{
    internal abstract class Browser
    {
        protected const string URL = "https://strelkacard.ru/";
        public static string Path => Environment.CurrentDirectory;


        
    }
}
