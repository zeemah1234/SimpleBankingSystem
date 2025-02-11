using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SimpleBankingApplication.Models;

namespace SimpleBankingApplication.Services
{
    public class PassBookService
    {
        private readonly string FilePath = Path.Combine(Directory.GetCurrentDirectory(),@"AccountData.Json");
        private readonly string FilePathPassBook = Path.Combine(Directory.GetCurrentDirectory(),@"PassBookData.Json");
        private List<Customer> Accounts = new List<Customer>();

        private List<Passbook> Passbooks = new List<Passbook>();

        private readonly string PassBookPath;

        public PassBookService(string userName)
        {
            PassBookPath = $"Passbooks/(userName)_passbook.txt";

            Directory.CreateDirectory("Passbooks");

            LoadDataFromJson();
            LoadDataFromJsonPassBook();

            using (StreamWriter writer = new StreamWriter(PassBookPath, true))
            {
                //writer.WriteLine(new string('-', 150));
                //writer.WriteLine("PASSBOOK".PadLeft(67));
                // writer.WriteLine(new string('-', 150));
                // writer.WriteLine("Date                 | Transaction Type | Amount | Balance          ");
                // writer.WriteLine(new string('-', 150));


                writer.WriteLine("==================================================================");
                writer.WriteLine("                      PASSBOOK                                    ");
                writer.WriteLine("==================================================================");
                writer.WriteLine("Date             | Transaction Type | Amount  |  Balance           ");
                writer.WriteLine("==================================================================");

            }
        }

        public void PrintPassBookData(string userName)
        {
            var account = Accounts.Find(x => x.Account.UserName == userName);
            if (account != null)
            {
                var data = Passbooks.Find(x => x.AccountId == account.Account.AccountId);
                if(data != null)
                {
                    using(StreamWriter writer = new StreamWriter(PassBookPath, true))
                    {
                        string date = data.Date.ToString("yyyy-MM-dd  HH:mm:ss");
                        writer.WriteLine($"{date,-18} | {data.TransactionType,-16} | {data.Amount,8:C} | {data.Balance,9:C}");
                    }
                    Console.WriteLine("Pass book printed successfully.");
                }
                else
                {
                    Console.WriteLine("No passbook data found with Id: "+ account.Account.AccountId);
                }
            }
            else
            {
                Console.WriteLine("No passbook data found with user name: " + userName);
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
                Console.WriteLine("Data loaded successfully.");

            }

        }

    }
}