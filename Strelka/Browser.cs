using Microsoft.Win32;
using OpenQA.Selenium;
using System.Diagnostics;

namespace Strelka_DLL
{
    class Browser
    {
        protected const string URL = "https://strelkacard.ru/";
        protected static string ExecutePath => Environment.CurrentDirectory;
        public string? Name { get; set; }
        public string? Path { get; set; }
        public string? Version { get; set; }
        //public string? IconPath { get; set; }
    }
}
