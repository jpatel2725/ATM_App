using System;
using System.Collections.Generic;

namespace ATMApplication
{
    // This class represents a bank account
    public class Account
    {
        public string AccountHolderName { get; private set; }
        public int AccountNumber { get; private set; }
        public float AnnualInterestRate { get; private set; }
        public float Balance { get; private set; }
        private List<string> transactions = new List<string>();

        // This sets up a new account with the given details
        public Account(int accountNumber, float initialBalance, float annualInterestRate, string accountHolderName)
        {
            AccountNumber = accountNumber;
            Balance = initialBalance;
            AnnualInterestRate = annualInterestRate;
            AccountHolderName = accountHolderName;
            transactions.Add($"Account created with initial balance: {initialBalance:C}");
        }

        // This method adds money to the account
        public void Deposit(float amount)
        {
            Balance += amount;
            transactions.Add($"Deposited: {amount:C}");
        }

        // This method takes money out of the account
        public void Withdraw(float amount)
        {
            if (amount > Balance)
            {
                Console.WriteLine("Insufficient funds.");
                return;
            }
            Balance -= amount;
            transactions.Add($"Withdrew: {amount:C}");
        }

        // This method shows all the transactions for the account
        public void DisplayTransactions()
        {
            Console.WriteLine("Transaction History:");
            foreach (var transaction in transactions)
            {
                Console.WriteLine(transaction);
            }
        }
    }

    // This class manages the bank and its accounts
    public class Bank
    {
        private List<Account> accounts = new List<Account>();

        // This sets up the bank with 10 default accounts
        public Bank()
        {
            for (int i = 0; i < 10; i++)
            {
                accounts.Add(new Account(100 + i, 100.0f, 3.0f, "Default User"));
            }
        }

        // This method finds an account by its number
        public Account GetAccount(int accountNumber)
        {
            return accounts.Find(account => account.AccountNumber == accountNumber);
        }

        // This method creates a new account with the given details
        public void OpenAccount(string clientName, int accountNumber, float initialBalance, float annualInterestRate)
        {
            if (accountNumber < 100 || accountNumber > 1000)
            {
                Console.WriteLine("Account number must be between 100 and 1000.");
                return;
            }

            if (annualInterestRate > 3.0f)
            {
                Console.WriteLine("Interest rate must be less than or equal to 3%.");
                return;
            }

            if (accounts.Exists(account => account.AccountNumber == accountNumber))
            {
                Console.WriteLine("Account number already exists.");
                return;
            }

            accounts.Add(new Account(accountNumber, initialBalance, annualInterestRate, clientName));
            Console.WriteLine("Account created successfully!");
        }

        // This method checks if an account number is unique
        public bool IsAccountNumberUnique(int accountNumber)
        {
            return !accounts.Exists(account => account.AccountNumber == accountNumber);
        }
    }

    // This class handles user interaction with the ATM
    public class AtmApplication
    {
        private Bank bank = new Bank();

        public void Run()
        {
            bool running = true;
            while (running)
            {
                DisplayMainMenu();
                int choice;
                if (!int.TryParse(Console.ReadLine(), out choice))
                {
                    Console.WriteLine("Invalid input. Please enter a number.");
                    continue;
                }

                switch (choice)
                {
                    case 1:
                        CreateAccount();
                        break;
                    case 2:
                        SelectAccount();
                        break;
                    case 3:
                        running = false;
                        break;
                    default:
                        Console.WriteLine("Invalid option. Try again.");
                        break;
                }
            }
        }

        // This shows the main menu
        private void DisplayMainMenu()
        {
            Console.WriteLine("ATM Main Menu:");
            Console.WriteLine("1. Create Account");
            Console.WriteLine("2. Select Account");
            Console.WriteLine("3. Exit");
        }

        // This lets the user create a new account
        private void CreateAccount()
        {
            Console.WriteLine("Enter Account Holder Name:");
            string accountHolderName = Console.ReadLine();
            if (string.IsNullOrEmpty(accountHolderName) || !IsValidName(accountHolderName))
            {
                Console.WriteLine("Invalid name. Please enter a valid name.");
                return;
            }

            Console.WriteLine("Enter Account Number (100-1000):");
            int accountNumber;
            if (!int.TryParse(Console.ReadLine(), out accountNumber))
            {
                Console.WriteLine("Invalid input. Please enter a valid account number.");
                return;
            }
            if (!bank.IsAccountNumberUnique(accountNumber))
            {
                Console.WriteLine("Account number already exists.");
                return;
            }
            if (accountNumber < 100 || accountNumber > 1000)
            {
                Console.WriteLine("Account number must be between 100 and 1000.");
                return;
            }

            Console.WriteLine("Enter Initial Balance:");
            float initialBalance;
            if (!float.TryParse(Console.ReadLine(), out initialBalance) || initialBalance < 0)
            {
                Console.WriteLine("Invalid input. Please enter a valid balance.");
                return;
            }

            Console.WriteLine("Enter Annual Interest Rate (max 3%):");
            float annualInterestRate;
            if (!float.TryParse(Console.ReadLine(), out annualInterestRate) || annualInterestRate > 3.0f)
            {
                Console.WriteLine("Invalid input. Interest rate must be less than or equal to 3%.");
                return;
            }

            bank.OpenAccount(accountHolderName, accountNumber, initialBalance, annualInterestRate);
        }

        // This lets the user select an existing account
        private void SelectAccount()
        {
            Console.WriteLine("Enter Account Number:");
            int accountNumber;
            if (!int.TryParse(Console.ReadLine(), out accountNumber))
            {
                Console.WriteLine("Invalid input. Please enter a valid account number.");
                return;
            }

            Account account = bank.GetAccount(accountNumber);
            if (account != null)
            {
                Console.WriteLine($"Welcome, {account.AccountHolderName}");
                bool accountMenuRunning = true;
                while (accountMenuRunning)
                {
                    DisplayAccountMenu();
                    int choice;
                    if (!int.TryParse(Console.ReadLine(), out choice))
                    {
                        Console.WriteLine("Invalid input. Please enter a number.");
                        continue;
                    }

                    switch (choice)
                    {
                        case 1:
                            Console.WriteLine($"Current Balance: {account.Balance:C}");
                            break;
                        case 2:
                            Console.WriteLine("Enter amount to deposit:");
                            float depositAmount;
                            if (!float.TryParse(Console.ReadLine(), out depositAmount) || depositAmount < 0)
                            {
                                Console.WriteLine("Invalid input. Please enter a valid amount.");
                                break;
                            }
                            account.Deposit(depositAmount);
                            Console.WriteLine("Deposit successful.");
                            break;
                        case 3:
                            Console.WriteLine("Enter amount to withdraw:");
                            float withdrawAmount;
                            if (!float.TryParse(Console.ReadLine(), out withdrawAmount) || withdrawAmount < 0)
                            {
                                Console.WriteLine("Invalid input. Please enter a valid amount.");
                                break;
                            }
                            account.Withdraw(withdrawAmount);
                            break;
                        case 4:
                            account.DisplayTransactions();
                            break;
                        case 5:
                            accountMenuRunning = false;
                            break;
                        default:
                            Console.WriteLine("Invalid option. Try again.");
                            break;
                    }
                }
            }
            else
            {
                Console.WriteLine("Account not found.");
            }
        }

        // This shows the account menu
        private void DisplayAccountMenu()
        {
            Console.WriteLine("Account Menu:");
            Console.WriteLine("1. Check Balance");
            Console.WriteLine("2. Deposit");
            Console.WriteLine("3. Withdraw");
            Console.WriteLine("4. Display Transactions");
            Console.WriteLine("5. Exit Account");
        }

        // This checks if the name entered is valid
        private bool IsValidName(string name)
        {
            foreach (char c in name)
            {
                if (!char.IsLetter(c) && !char.IsWhiteSpace(c))
                {
                    return false;
                }
            }
            return true;
        }
    }

    // The main program entry point
    class Program
    {
        static void Main(string[] args)
        {
            AtmApplication atmApp = new AtmApplication();
            atmApp.Run();
        }
    }
}
