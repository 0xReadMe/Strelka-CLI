namespace ConsoleApp1
{
    struct StrelkaCard
    {
        string number { get; }
        StrelkaType type { get; }
        double balance { get; }
        bool validated { get; }

        public StrelkaCard(string number, StrelkaType type, double balance)
        {
            this.number = number;
            this.type = type;
            this.balance = balance;
        }
    }

    enum StrelkaType
    {
        Default,
        Benefit,
        PupilCity,
        PupilVillage
    }
}