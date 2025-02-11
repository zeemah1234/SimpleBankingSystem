using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SimpleBankingApplication.Models;

namespace SimpleBankingApplication.Services
{
    public class AccountService
    {
         private readonly string FilePath = Path.Combine(Directory.GetCurrentDirectory(),@"AccountData.Json");

        private List<Customer> customers = new List<Customer>();

        private int nextIdForCustomer = 1;
        private int nextIdAccount = 1;

        public AccountService()
        {
            LoadDataFromJson();
        }


        public void CreateNewCustomer(string firstName, string lastName,  DateTime dob, string address, string phoneNumber, string gender, string accountType)
        {
            string generatedUsername = GenerateUserName(firstName);
            string generatedPassword = GeneratePassword();

            var newCustomer = new Customer
            {
                CustomerId = nextIdForCustomer++,
                FirstName = firstName,
                LastName = lastName,
                DOB = dob, 
                Address = address,
                PhoneNumber = phoneNumber,
                Gender = gender,
                AccountType = accountType,
                Account = GenerateAccountDetails(accountType, firstName)
            };
            Console.WriteLine("Wait, account details are generating in progress .......");
            customers.Add(newCustomer);
            Console.WriteLine("Registered successfully");
            Console.WriteLine("Username: " + generatedUsername);
            Console.WriteLine("Password: " + generatedPassword);
            Console.WriteLine("Data saved successfully");

        }
        private string GenerateUserName(string name)
        {
            return name.ToLower() + "_user";
        }
        private string GeneratePassword()
        {
            return "P@ssw0rd123";
        }

        public bool GetCustomer(string userName, string password)
        {
            var customer = customers.FirstOrDefault(x => x.Account.UserName == userName && x.Account.Password == password);
            if (customer != null)
            {
                return true;
            }
            else 
            {
                Console.WriteLine("Customer not found, please try again");
                return false;
            }
        }

        private Account GenerateAccountDetails(string accType, string firstName)
        {
            var accountDetails = new Account
            {
                AccountId = nextIdAccount++,
                Balance = GetMinimumBalance(accType),
                UserName = GenerateUserName(firstName),
                Password = GeneratePassword(),
                AccountNumber = GenerateAccountNumber()
            };
            return accountDetails;
        }

        private string GenerateAccountNumber()
        {
            return "ACCC" + "1001" + new Random().Next(10000, 99999).ToString();
        }
        private decimal GetMinimumBalance(string accType)
        {
            return 1000m;
        }
        private void SaveDataFromJsonPassBook()
        {
            var json = JsonConvert.SerializeObject(customers, Formatting.Indented);
            File.WriteAllText(FilePath, json);
            Console.WriteLine("Data saved successfully.");
        }

        private void LoadDataFromJson()
        {
            if (File.Exists(FilePath))
            {
                var json = File.ReadAllText(FilePath);
                customers = JsonConvert.DeserializeObject<List<Customer>>(json);
                if(customers.Count !=0)
                {
                    nextIdForCustomer = customers[customers.Count - 1].CustomerId + 1;
                }
                Console.WriteLine("Data loaded successfully.");

            }
        }
    }
}