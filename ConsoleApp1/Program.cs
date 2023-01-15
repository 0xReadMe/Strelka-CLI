using System.Diagnostics;
using Strelka_DLL;

const string login = "+79153018803";
const string pass = "fomxif-7voFha-zutsep";
const string cardNumber = "03362261410";
Stopwatch stopwatch = new Stopwatch();

try
{
    stopwatch.Start();
    Parser strelka = new Parser();

    var b = strelka.GetBalance(cardNumber);
    Console.WriteLine($"Ваш баланс: {b}");

    //strelka.Authorization(login, pass);
    //Thread.Sleep(2000);
    //var balances = strelka.GetPersonalBalance();
    //var cardNames = strelka.GetPersonalCardNames();
    //foreach(var i in balances)
    //    Console.WriteLine("Balance:" + i);
    //foreach(var i in cardNames)
    //    Console.WriteLine("Card:" + i);
    //stopwatch.Stop();

    //Parser.KillProcesses();
    TimeSpan ts = stopwatch.Elapsed;

    string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
        ts.Hours, ts.Minutes, ts.Seconds,
        ts.Milliseconds / 10);
    Console.WriteLine(elapsedTime);
}
catch (Exception ex)
{
    Parser.KillProcesses();
    Console.WriteLine("Возникла непредвиденная ошибка:");
    Console.WriteLine(ex);
}