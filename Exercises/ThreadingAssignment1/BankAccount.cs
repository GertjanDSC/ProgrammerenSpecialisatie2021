namespace ThreadingExample
{
    public class BankAccount
    {
        private Object acctLock = new();
        public double Balance { get; set; }
        public string Name { get; set; }

        public BankAccount(string name, double balance)
        {
            Name = name;
            Balance = balance;
        }

        public double Withdraw(double amt)
        {
            if ((Balance - amt) < 0)
            {
                Console.WriteLine($"Sorry ${Balance} in Account");
                return Balance;
            }

            lock (acctLock)
            {
                if (Balance >= amt)
                {
                    Console.WriteLine("Removed {0} and {1} left in Account",
                    amt, (Balance - amt));
                    Balance -= amt;
                }

                return Balance;

            }
        }

        // You can only point at methods
        // without arguments and that return 
        // nothing
        public void IssueWithdraw()
        {
            Withdraw(1);
        }
    }
}