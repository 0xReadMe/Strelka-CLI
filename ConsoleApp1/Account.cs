namespace ConsoleApp1
{
    class Account
    {
        string login;
        string password;

        public Account(string login, string password)
        {
            this.login = login ?? "";
            this.password = password ?? "";
        }

        /// <summary>
        /// Метод должен получить список пользователя карт стрелка 
        /// </summary>
        /// <returns>Список карт стрелка данного аккаунта</returns>
        public List<StrelkaCard> GetStrelkaCards()
        {
            //TODO
            return new List<StrelkaCard>() { new StrelkaCard("", StrelkaType.Default, 0) };
        }
    }
}