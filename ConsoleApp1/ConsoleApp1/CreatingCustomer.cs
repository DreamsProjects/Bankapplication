using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class CreatingCustomer
    {
        public CreatingCustomer(int newID, int acc, List<Clients> list, List<Account> accountList)
        {
            Console.Write("Organisationsnummer: ");
            string orgNumb = Console.ReadLine().ToLower();

            Console.Write("Företagsnamn: ");
            string compName = Console.ReadLine().ToLower();

            Console.Write("Adress: ");
            string address = Console.ReadLine().ToLower();

            Console.Write("Stad: ");
            string city = Console.ReadLine().ToLower();

            Console.Write("Region: ");
            string region = Console.ReadLine().ToLower();

            Console.Write("Postadress: ");
            string mailaddress = Console.ReadLine().ToLower();

            Console.Write("Land: ");
            string country = Console.ReadLine().ToLower();

            Console.Write("Telefonnummer: ");
            string phoneNumb = Console.ReadLine().ToLower();

            Clients client = new Clients(newID, orgNumb, compName, address, city, region, mailaddress, country, phoneNumb);

            Account account = new Account(acc, newID, 0);

            Console.WriteLine("Ditt kundnummer är: " + newID);
            Console.WriteLine("Ditt konto nummer är: " + acc);

            list.Add(client);
            accountList.Add(account);
        }
    }
}
