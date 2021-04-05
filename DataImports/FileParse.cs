using System;
using System.Collections.Generic;
using System.IO;
using ShippingDiscount.Actions;
using ShippingDiscount.Domains;

namespace ShippingDiscount.DataImports
{
    public class FileParse
    {
        public string ReadFile(string FileName)
        {
            string textFile = $"{AppDomain.CurrentDomain.BaseDirectory} \\..\\..\\..\\..\\DataImports\\DataFiles\\{FileName}";
            try
            {
                string text = File.ReadAllText(textFile);
                return text;
            }
            catch (InvalidDataException e)
            {
                Console.WriteLine("File doesn't exist: " + e);
                return null;
            }
        }
        public List<Transaction> ParseDataToTransactions(string Text)
        {
            if (Text != null)
            {
                CreateTransactions CreateTransactions = new CreateTransactions();
                List<Transaction> Transactions = new List<Transaction>();
                string[] lines = Text.Split(("\n\n").ToCharArray());
                foreach (string line in lines)
                {
                    string[] transaction = line.Split(" ".ToCharArray());
                    if (transaction.Length == 3)
                        Transactions.Add(CreateTransactions.CreateTransaction(transaction[0].Trim(), transaction[1].Trim(), transaction[2].Trim()));
                    else if (transaction.Length == 2)
                        Transactions.Add(CreateTransactions.CreateTransaction(transaction[0].Trim(), transaction[1].Trim(), null));

                }
                return Transactions;
            }
            else
            {
                return null;
            }

        }
    }
}
