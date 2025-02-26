using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

class Program
{
    static Dictionary<int, int> accounts = new Dictionary<int, int>(); // Dictionary to store account IDs and balances
    static readonly object logLock = new object(); // Lock for thread-safe logging
    static readonly string logFilePath = "transactions.log"; // File path for logging transactions

    static void Main(string[] args)
    {
        InitializeAccounts();
        Console.WriteLine("Welcome to the Multi-Threaded Banking System!");

        while (true)
        {
            Console.WriteLine("\nMenu:");
            Console.WriteLine("1. View Account Balances");
            Console.WriteLine("2. Perform Transfer");
            Console.WriteLine("3. Simulate High-Concurrency Transfers");
            Console.WriteLine("4. Exit");
            Console.Write("Select an option: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    ViewBalances();
                    break;
                case "2":
                    PerformTransfer();
                    break;
                case "3":
                    SimulateHighConcurrency();
                    break;
                case "4":
                    Console.WriteLine("Exiting the system. Goodbye!");
                    return;
                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    break;
            }
        }
    }

    static void InitializeAccounts()
    {
        // Initialize accounts with some balances
        accounts[1] = 1000; // Account 1
        accounts[2] = 1000; // Account 2
        accounts[3] = 2000; // Account 3
    }

    static void ViewBalances()
    {
        Console.WriteLine("\nAccount Balances:");
        foreach (var account in accounts)
        {
            Console.WriteLine($"Account {account.Key}: ${account.Value}");
        }
    }

    static void PerformTransfer()
    {
        Console.Write("\nEnter source account ID: ");
        int sourceAccount = int.Parse(Console.ReadLine());
        Console.Write("Enter destination account ID: ");
        int destinationAccount = int.Parse(Console.ReadLine());
        Console.Write("Enter transfer amount: ");
        int amount = int.Parse(Console.ReadLine());

        if (!accounts.ContainsKey(sourceAccount) || !accounts.ContainsKey(destinationAccount))
        {
            Console.WriteLine("Invalid account ID(s). Please try again.");
            return;
        }

        Thread transferThread = new Thread(() => Transfer(sourceAccount, destinationAccount, amount));
        transferThread.Start();
        transferThread.Join(); // Wait for the transfer to complete
    }

    static void SimulateHighConcurrency()
    {
        Console.Write("\nEnter the number of concurrent transfers to simulate: ");
        int numTransfers = int.Parse(Console.ReadLine());

        Thread[] threads = new Thread[numTransfers];
        Random random = new Random();

        for (int i = 0; i < numTransfers; i++)
        {
            int sourceAccount = random.Next(1, 4); // Random source account (1, 2, or 3)
            int destinationAccount = random.Next(1, 4); // Random destination account (1, 2, or 3)
            int amount = random.Next(100, 500); // Random amount between 100 and 500

            threads[i] = new Thread(() => Transfer(sourceAccount, destinationAccount, amount));
            threads[i].Start();
        }

        // Wait for all threads to finish
        foreach (Thread thread in threads)
        {
            thread.Join();
        }

        Console.WriteLine("High-concurrency simulation complete.");
    }

    static void Transfer(int sourceAccount, int destinationAccount, int amount)
    {
        // Determine lock order to prevent deadlocks
        object firstLock = sourceAccount < destinationAccount ? accounts[sourceAccount] : accounts[destinationAccount];
        object secondLock = sourceAccount < destinationAccount ? accounts[destinationAccount] : accounts[sourceAccount];

        lock (firstLock)
        {
            lock (secondLock)
            {
                if (accounts[sourceAccount] >= amount)
                {
                    // Perform the transfer
                    accounts[sourceAccount] -= amount;
                    accounts[destinationAccount] += amount;

                    string logMessage = $"Transfer successful: ${amount} from Account {sourceAccount} to Account {destinationAccount}. " +
                                         $"New Balances: Account {sourceAccount}: ${accounts[sourceAccount]}, Account {destinationAccount}: ${accounts[destinationAccount]}";
                    Console.WriteLine(logMessage);
                    LogTransaction(logMessage);
                }
                else
                {
                    string logMessage = $"Transfer failed: Insufficient funds in Account {sourceAccount} for ${amount} transfer.";
                    Console.WriteLine(logMessage);
                    LogTransaction(logMessage);
                }
            }
        }
    }

    static void LogTransaction(string message)
    {
        lock (logLock) // Ensure thread-safe logging
        {
            using (StreamWriter writer = new StreamWriter(logFilePath, true))
            {
                writer.WriteLine($"{DateTime.Now}: {message}");
            }
        }
    }
}