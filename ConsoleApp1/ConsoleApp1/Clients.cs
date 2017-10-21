using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Globalization;

namespace ConsoleApp1
{
    public class Clients
    {
        public int CustomNumber { get; set; }
        public string OrganisationNumber { get; set; }
        public string CompanyName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string MailingAddress { get; set; }
        public string Country { get; set; }
        public string PhoneNumber { get; set; }
        public List<Clients> List = new List<Clients>();
        public List<Account> accountList = new List<Account>();

        public Clients(int custName, string orgNumb, string compName, string address, string city, string reg, string mailAdd, string country, string phone)
        {
            CustomNumber = custName;
            OrganisationNumber = orgNumb;
            CompanyName = compName;
            Address = address;
            City = city;
            Region = reg;
            MailingAddress = mailAdd;
            Country = country;
            PhoneNumber = phone;
        }

        public override string ToString()
        {
            return CustomNumber + ";" + OrganisationNumber + ";" + CompanyName + ";" + Address +
                ";" + City + ";" + Region + ";" + MailingAddress + ";" + Country + ";" + PhoneNumber;
        }

        public bool Reading() //För att läsa Kunder
        {
            using (var reader = new StreamReader("bankdata.txt")) //Textfil  läsas in
            {
                decimal sum = 0;
                int numberOfClients = Convert.ToInt32(reader.ReadLine());

                for (int i = 0; i < numberOfClients; i++)
                {
                    string information = reader.ReadLine();

                    string[] toArrays = information.Split(';');

                    Clients client = new Clients(Convert.ToInt32(toArrays[0]),
                    toArrays[1], toArrays[2], toArrays[3], toArrays[4],
                    toArrays[5], toArrays[6], toArrays[7], toArrays[8]);

                    List.Add(client);
                }

                Console.WriteLine("Antal kunder: " + numberOfClients);

                int numberOfAccount = Convert.ToInt32(reader.ReadLine());

                for (int i = 0; i < numberOfAccount; i++)
                {
                    string hello = reader.ReadLine();
                    string[] toArrays = hello.Split(';');

                    Account account = new Account(Convert.ToInt32(toArrays[0]),
                    Convert.ToInt32(toArrays[1]),
                    Convert.ToDecimal(toArrays[2], CultureInfo.InvariantCulture));

                    accountList.Add(account);

                    sum += Convert.ToDecimal(toArrays[2], CultureInfo.InvariantCulture);
                }

                Console.WriteLine("Antal konton: " + numberOfAccount);
                Console.WriteLine("Totala Saldot: " + sum);
            }
            return true;
        }
    }

    public class Account
    {
        public int AccountNumber { get; set; }
        public int CustomNumber { get; set; }
        public decimal Balance { get; set; }

        public Account(int accNumb, int custNumb, decimal balance)
        {
            AccountNumber = accNumb;
            CustomNumber = custNumb;
            Balance = balance;
        }

        public override string ToString()
        {
            return AccountNumber + ";" + CustomNumber + ";" + Balance.ToString(CultureInfo.InvariantCulture);
        }
    }
}
