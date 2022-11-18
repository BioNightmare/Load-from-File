using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml.Linq;

namespace Load_from_File
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Person giacomo = new() { Name="Giacomo", LastName="Rossi", Age=20};
            Logger(giacomo);
            string percorso = Path.Combine(Environment.CurrentDirectory, $"{giacomo.GetType().Name}.csv");
            List<Person> lista = new List<Person>();
            lista = ReadfromFile<Person>(percorso);

        }
        public static void Logger<T>(T item) where T : class
        {
            var proprieta = item.GetType().GetProperties();
            string percorso = Path.Combine(Environment.CurrentDirectory, $"{item.GetType().Name}.csv");
            if (!File.Exists(percorso))
            {
                foreach (var prop in proprieta)
                {
                    File.AppendAllText(percorso, prop.Name);
                    File.AppendAllText(percorso, " ");
                }
            }
            File.AppendAllText(percorso, "\n");
            foreach (var prop in proprieta)
            {
                File.AppendAllText(percorso, prop.GetValue(item).ToString());
                File.AppendAllText(percorso, " ");
            }
            File.AppendAllText(percorso, "\n");
        }
        public static List<T> ReadfromFile<T>(string percorso) where T : class, new()
        {
            List<T> list = new List<T>();
            var rows = File.ReadAllLines(percorso).ToList();
            string[] headers = rows.ElementAt(0).Split(" ");
            T entry = new T();
            rows.RemoveAt(0);
            bool f = false;
            bool v = true;
            var proprieta = entry.GetType().GetProperties();

            if (proprieta.Length == headers.Length)
            {
                for (int x = 0; x < proprieta.Length; x++)
                {

                    if (proprieta.ElementAt(x).Name == headers[x])
                    {
                        f = true;
                    }
                    else v = false;

                }
            }
            if (f && v)
            {
                foreach (var row in rows)
                {
                    int j = 0;
                    string[] colonne = row.Split(" ");
                    entry = new T();
                    foreach (var col in colonne)
                    {
                        entry.GetType().GetProperty(headers[j]).SetValue(entry, Convert.ChangeType(col, entry.GetType().GetProperty(headers[j]).PropertyType));
                        j++;
                    }
                    list.Add(entry);
                }
            }
            else
            {
                Console.WriteLine("L'elemento non corrisponde");
            }
            return list;
        }

    }
    public class Person
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }

        public Person()
        {
        }
    }
}
