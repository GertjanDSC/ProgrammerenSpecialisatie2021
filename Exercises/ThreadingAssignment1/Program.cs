namespace ThreadingExample
{
    // With threads you can execute multiple pieces of code that share resources
    // and data without corrupting it

    // You can't guarantee when a thread executes. You also must lock resources
    // until a thread is done with them or you could corrupt them

    class Program
    {
        static void Main(string[] args)
        {
            SimpleExample(args);
            SleepExample(args);
            LockExample(args);
            DataToThreadsExample(args);
        }

        // ----- Simple Thread Example -----
        static void SimpleExample(string[] args)
        {
            // Create a thread and start it
            Thread t = new(Print1);
            t.Start();

            // This code will run randomly:
            for (int i = 0; i < 1000; i++)
            {
                Console.Write(0);
            }
        }

        static void Print1()
        {
            for (int i = 0; i < 1000; i++)
            {
                Console.Write(1);
            }
        }
        
        // With sleep() the thread is suspended for the designated amount of time
        static void SleepExample(string[] args)
        {
            int num = 1;
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine(num);

                // Pause for 1 second
                Thread.Sleep(1000);

                num++;
            }
            Console.WriteLine("Thread Ends");
        }

        // ----- Lock Example -----
        // lock keeps other threads from entering 
        // a statement block until another thread
        // leaves
        static void LockExample(string[] args)
        {
            BankAccount account = new("KBC", 10);
            Thread[] threads = new Thread[15];

            // CurrentThread gets you the current executing thread
            Thread.CurrentThread.Name = "main";

            // Create 15 threads that will call for IssueWithdraw to execute
            for (int i = 0; i < 15; i++)
            {
                // You can only point at methods without arguments that return nothing
                Thread t = new(new ThreadStart(account.IssueWithdraw));
                t.Name = i.ToString();
                threads[i] = t;
            }

            // Have threads try to execute
            for (int i = 0; i < 15; i++)
            {
                // Check if thread has started
                Console.WriteLine("Thread {0} Alive : {1}", threads[i].Name, threads[i].IsAlive);
                // Start thread
                threads[i].Start();
                // Check if thread has started
                Console.WriteLine("Thread {0} Alive : {1}", threads[i].Name, threads[i].IsAlive);
            }

            // Get thread priority (Normal Default).
            // Also Lowest, BelowNormal, AboveNormal and Highest
            // Changing priority doesn't guarantee the highest precedence though.
            // It is best to not mess with this!
            Console.WriteLine("Current Priority : {0}", Thread.CurrentThread.Priority);
            Console.WriteLine("Thread {0} Ending", Thread.CurrentThread.Name);
        }

        // ----- Passing Data to Threads -----
        // You can pass arguments to a thread using a lambda expression

        static void DataToThreadsExample(string[] args)
        {
            Thread t = new(() => CountTo(10));
            t.Start();

            // You can use multiline lambdas
            new Thread(() =>
            {
                CountTo(5);
                CountTo(6);
            }).Start();
        }

        static void CountTo(int maxNum)
        {
            for (int i = 0; i <= maxNum; i++)
            {
                Console.WriteLine(i);
            }
        }
    }
}
