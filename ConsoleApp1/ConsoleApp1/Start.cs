using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Globalization;

namespace ConsoleApp1
{
    public class Start
    {
        public Start()
        {
            Console.WriteLine("***************************************");
            Console.WriteLine("** VÄLKOMMEN TILL EMMAS BANKAPP 2017 **");
            Console.WriteLine("***************************************");

            Clients client = new Clients(1, "", "", "", "", "", "", "", ""); //Bara för att få fram clients functionen reading

            client.Reading(); //Ska få fram informationen från Reading i client

            Console.WriteLine("\n** Huvudmeny **");
            Console.WriteLine("0) Spara och avsluta");
            Console.WriteLine("1) Sök kund");
            Console.WriteLine("2) Visa kundbild");
            Console.WriteLine("3) Skapa kund");
            Console.WriteLine("4) Ta bort kund");
            Console.WriteLine("5) Skapa konto");
            Console.WriteLine("6) Ta bort konto");
            Console.WriteLine("7) Insättning");
            Console.WriteLine("8) Uttag");
            Console.WriteLine("9) Överföring");
            while (true)
            {
                try
                {
                    Console.Write("> ");
                    int input = int.Parse(Console.ReadLine());

                    if (input == 0)
                    {
                        string dateFileName = String.Format(DateTime.Now.ToString("yyyyMMdd-HHmm", new CultureInfo("sv-SE"))) + ".txt";

                        using (var write = new StreamWriter(dateFileName))
                        {
                            var amountOfClients = client.List.Count();

                            write.WriteLine(amountOfClients);//Skriver ut antal kunder till textfilen

                            foreach (var item in client.List)
                            {
                                write.WriteLine(item);
                            }

                            Console.WriteLine("Antal kunder: " + amountOfClients);

                            var amountOfAccounts = client.accountList.Count();
                            write.WriteLine(amountOfAccounts);

                            foreach (var item in client.accountList)
                            {
                                write.WriteLine(item);
                            }

                            Console.WriteLine("Antal konton: " + amountOfAccounts);

                            var money = client.accountList.Sum(x => x.Balance);

                            Console.WriteLine("Totala saldot: " + money);
                        }

                        Console.ReadLine();
                        Environment.Exit(0);
                    }

                    if (input == 1)
                    {
                        Console.WriteLine("*Sök kund*");
                        Console.Write("Namn/stad: ");
                        string cName = Console.ReadLine().ToLower();

                        var test = client.List.FindAll(x => x.CompanyName.ToLower().Contains(cName.ToLower()) || x.City.ToLower().Contains(cName));

                        foreach (var item in test)
                        {
                            Console.WriteLine(item.CustomNumber + ":" + item.CompanyName);
                        }
                    }

                    if (input == 2)
                    {
                        decimal sum = 0;

                        Console.WriteLine("*Visa kundbild*");
                        Console.Write("Kundnummer/Kontonummer:");
                        int clientNumber = int.Parse(Console.ReadLine().ToLower());

                        var cust = client.accountList.Any(x => x.CustomNumber == clientNumber || x.AccountNumber == clientNumber);

                        if (cust)
                        {
                            var changeFromAccountList = client.accountList.Where(x => x.AccountNumber == clientNumber).FirstOrDefault();
                            if (changeFromAccountList != null) clientNumber = changeFromAccountList.CustomNumber; // Tar fram kundnumret från kontot

                            var companies = client.List.Where(x => x.CustomNumber == clientNumber).FirstOrDefault();

                            var orgNum = companies.OrganisationNumber;
                            var clientNumb = companies.CompanyName;
                            var address = companies.Address;
                            var mailAdd = companies.MailingAddress;

                            Console.WriteLine("\nKundnummer: " + clientNumber);
                            Console.WriteLine("Organisationsnummer: " + orgNum);
                            Console.WriteLine("Företagsnamn: " + clientNumb);
                            Console.WriteLine("Adress: " + address + ", " + mailAdd);

                            Console.WriteLine("\nKonton: ");

                            foreach (var index in client.accountList.Where(x => x.CustomNumber == clientNumber || x.AccountNumber == clientNumber))
                            {
                                Console.WriteLine(index.AccountNumber.ToString() + ": " + index.Balance + " kr");
                                sum += index.Balance;
                            }

                            Console.WriteLine("Totala summan är kunden är: " + sum);
                        }

                        else
                        {
                            Console.WriteLine("Kunden finns inte");
                        }
                    }

                    if (input == 3)
                    {
                        var newID = client.List.Max(x => x.CustomNumber) + 1;
                        var newAccountID = client.accountList.Max(x => x.AccountNumber) + 1;

                        CreatingCustomer customer = new CreatingCustomer(newID, newAccountID, client.List, client.accountList);
                        //skickar in i konstruktorn för skapandet av kund
                    }

                    if (input == 4)
                    {
                        Console.WriteLine("*Ta bort kund*");
                        Console.Write("Kundnummer: ");
                        int numberOfAccount = int.Parse(Console.ReadLine());

                        if (client.List.Exists(x => x.CustomNumber == numberOfAccount))
                        {
                            if (client.accountList.Exists(x => x.CustomNumber == numberOfAccount)) //Om det finns ett konto
                            {
                                Console.WriteLine("Du kan inte ta bort om du har ett konto");
                            }

                            else if (client.accountList.Exists(x => x.CustomNumber != numberOfAccount)) //Om det inte finns ett konto
                            {
                                client.List.RemoveAll(x => x.CustomNumber == numberOfAccount);
                                Console.WriteLine("Borttaget");
                            }
                        }
                    }

                    if (input == 5)
                    {
                        Console.WriteLine("*Skapa konto*");
                        Console.Write("Ange kundnummer: ");
                        int existingAccount = int.Parse(Console.ReadLine());

                        if (client.List.Exists(x => x.CustomNumber == existingAccount)) //Om kunden finns
                        {
                            var newAccountID = client.accountList.Max(x => x.AccountNumber) + 1;
                            Account account = new Account(newAccountID, existingAccount, 0);
                            client.accountList.Add(account); //lägger in i listans minne för att sedan spara

                            Console.WriteLine("Ditt nya kontonummer är: " + account.AccountNumber);
                        }

                        else if (client.List.Exists(x => x.CustomNumber != existingAccount))
                        {
                            Console.WriteLine("Tyvärr fanns inte detta kontonumret.");
                        }
                    }

                    if (input == 6)
                    {
                        Console.WriteLine("*Ta bort konto*");
                        Console.Write("Ange kontonummer: ");
                        int account = int.Parse(Console.ReadLine());

                        if (client.accountList.Exists(x => x.AccountNumber == account)) //Finns kontot?
                        {
                            if (client.accountList.Exists(x => x.Balance == 0)) //Är kontot 0?
                            {
                                client.accountList.RemoveAll(x => x.AccountNumber == account);
                                Console.WriteLine("Kontot togs bort");
                            }

                            else if (client.accountList.Exists(x => x.Balance > 0)) //Är kontot mer än 0?
                            {
                                Console.WriteLine("Kontot kunde inte tas bort då det finns pengar på den!");
                            }
                        }
                    }

                    if (input == 7)
                    {
                        Console.WriteLine("*Insättningar*");
                        Console.Write("Till konto: ");
                        int numberOfAccount = int.Parse(Console.ReadLine());

                        var istrue = client.accountList.Any(x => x.AccountNumber == numberOfAccount);
                        var existing = client.accountList.Where(x => x.AccountNumber == numberOfAccount).FirstOrDefault();

                        if (istrue) //Finns kontonumret?
                        {
                            Console.WriteLine("*Skriv belopp med punkt för ören om ej hel tal*");

                            Console.Write("Belopp: ");
                            decimal addBalance = decimal.Parse(Console.ReadLine());

                            if (addBalance > 0) //Lägger du in belopp över 0?
                            {
                                var sum = existing.Balance += addBalance;
                                Console.WriteLine("Ditt nya belopp: " + sum);
                            }

                            else if (addBalance < 0) //Lägger du in minus 0?
                            {
                                Console.WriteLine("Du kan inte lägga in 0 kr eller mindre");
                            }

                            else if (client.accountList.Exists(x => x.AccountNumber != numberOfAccount))//Finns inte kontot?
                            {
                                Console.WriteLine("Felaktigt konto");
                            }
                        }
                    }

                    if (input == 8)
                    {
                        Console.WriteLine("*Uttag*");
                        Console.Write("Från konto: ");
                        int fromAccount = int.Parse(Console.ReadLine());

                        var exist = client.accountList.Any(x => x.AccountNumber == fromAccount);
                        var amountExist = client.accountList.Where(x => x.AccountNumber == fromAccount).FirstOrDefault(); //Här får vi fram belopp

                        if (exist) //Finns kontot?
                        {
                            Console.Write("Hur mycket vill du ta ut: ");
                            decimal amount = decimal.Parse(Console.ReadLine());

                            if (amountExist.Balance >= amount) //Finns pengarna?
                            {
                                Console.WriteLine("Du har tagit ut: " + amount);
                                decimal sum = amountExist.Balance -= amount; // -= för annars så uppdateras inte saldot

                                Console.WriteLine("Du har sammanlagt kvar på kontot: " + sum);
                            }

                            else if (amountExist.Balance < amount) // Tar du ut mer pengar än vad du har?
                            {
                                Console.WriteLine("Du kan inte ta ut mer pengar än vad du har på kontot!");
                            }
                        }
                    }

                    if (input == 9)
                    {
                        Console.WriteLine("*Överföring*");
                        Console.Write("Från konto: ");
                        int fromAccount = int.Parse(Console.ReadLine());

                        var fromAccountExist = client.accountList.Any(x => x.AccountNumber == fromAccount);
                        var amountFromAccount = client.accountList.Where(x => x.AccountNumber == fromAccount).FirstOrDefault();

                        if (fromAccountExist) //Finns från kontot?
                        {
                            Console.Write("\nTill konto:");
                            int toAccount = int.Parse(Console.ReadLine());

                            var toAccountExist = client.accountList.Any(x => x.AccountNumber == toAccount);
                            var amountToaccount = client.accountList.Where(x => x.AccountNumber == toAccount).FirstOrDefault();

                            if (toAccountExist) //Finns till kontot?
                            {
                                Console.Write("Belopp: ");
                                decimal balance = decimal.Parse(Console.ReadLine());

                                if (amountFromAccount.Balance >= balance) //Finns pengarna på från kontot?
                                {
                                    decimal sum = amountFromAccount.Balance -= balance;
                                    decimal sum2 = amountToaccount.Balance += balance;

                                    Console.WriteLine("Du skickade över " + balance + "kr till " + toAccount);
                                }

                                else if (amountFromAccount.Balance < balance) //Försöker du föra över mer pengar än vad du har?
                                {
                                    Console.WriteLine("Du kan inte skicka över mer pengar än vad du har");
                                }
                            }

                            else if (!toAccountExist) //Finns inte till kontot?
                            {
                                Console.WriteLine("Kontot existerar inte");
                            }
                        }

                        else if (!fromAccountExist) //Finns inte från kontot?
                        {
                            Console.WriteLine("Kontot existerar inte");
                        }
                    }
                }

                catch (FormatException) //Fel format hantering
                {
                    Console.WriteLine("Du kan bara skriva in siffror..");
                }

                catch (OverflowException) //För stor/litet tal hantering
                {
                    Console.WriteLine("Du kan inte skriva in en sådan lång siffra..");
                }
            }
        }
    }
}

