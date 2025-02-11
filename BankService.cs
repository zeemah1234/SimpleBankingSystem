using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SimpleBankingApplication.Models;

namespace SimpleBankingApplication.Services
{
    public class BankService
    {
        private readonly string FilePath = Path.Combine(Directory.GetCurrentDirectory(),@"AccountData.Json");
        private readonly string FilePathPassBook = Path.Combine(Directory.GetCurrentDirectory(),@"PassBookData.Json");
        private List<Customer> Accounts = new List<Customer>();
        private int newId = 1;
        public List<Passbook>? Passbooks { get; private set; }
        public decimal Balance { get; private set; }

        public BankService()
        {
            LoadDataFromJson();
            LoadDataFromJsonPassBook();
        }

        public void DepositAmount(string userName, decimal amount)
        {
            var account = Accounts.FirstOrDefault(x => x.Account.UserName == userName);
            if(account != null)
            {
                account.Account.Balance += amount;
                Balance += amount;
                var balance = account.Account.Balance;

                var passbookData = new Passbook
                {
                    PassbookId =  newId++,
                    AccountId = (int)account.Account.AccountId,
                    Date = DateTime.UtcNow,
                    TransactionType = "Deposit",
                    Amount = amount,
                    Balance = balance

                };
                Passbooks.Add(passbookData);
                Console.WriteLine("Amount deposited successfully.");
                SaveDataFromJson();
                SaveDataFromJsonPassBook();
            }

        }

        public void WithdrawAmount(string userName, decimal amount)
        {
            var account = Accounts.FirstOrDefault(x => x.Account.UserName == userName);
            if(account != null)
            {
                account.Account.Balance -= amount;
                Balance += amount;
                var balance = account.Account.Balance;

                var passbookData = new Passbook
                {
                    PassbookId =  newId++,
                    AccountId = (int)account.Account.AccountId,
                    Date = DateTime.UtcNow,
                    TransactionType = "Withdraw",
                    Amount = amount,
                    Balance = balance

                };
                Passbooks.Add(passbookData);
                Console.WriteLine("Amount withdraw successfully.");
                SaveDataFromJson();
                SaveDataFromJsonPassBook();
            }
            else
            {
                Console.WriteLine("No data found with user name: " + userName);
            }
        }

        public void CheckBalance(string userName)
        {
            var account = Accounts.Find(x => x.Account.UserName == userName);
            if(account != null)
            {
                Console.WriteLine($"Available balance is: {account.Account.Balance}");
            }
        }

        private void SaveDataFromJson()
        {
            var json = JsonConvert.SerializeObject(Accounts , Formatting.Indented);
            File.WriteAllText(FilePath, json);
            Console.WriteLine("Data saved successfully.");
        }

        private void SaveDataFromJsonPassBook()
        {
            var json = JsonConvert.SerializeObject(Accounts , Formatting.Indented);
            File.WriteAllText(FilePathPassBook, json);
            Console.WriteLine("Data saved successfully.");
        }

        private void LoadDataFromJson()
        {
            if (File.Exists(FilePath))
            {
                var json = File.ReadAllText(FilePath);
                Accounts = JsonConvert.DeserializeObject<List<Customer>>(json);
                Console.WriteLine("Data loaded successfully.");

            }

        }
        private void LoadDataFromJsonPassBook()
        {
            if (File.Exists( FilePathPassBook))
            {
                var json = File.ReadAllText( FilePathPassBook);
                Passbooks = JsonConvert.DeserializeObject<List<Passbook>>(json);
                if(Passbooks.Count != 0)
                {
                    newId = Passbooks[Passbooks.Count - 1].PassbookId + 1;
                }
                Console.WriteLine("Data loaded successfully.");

            }

        }

    }
}