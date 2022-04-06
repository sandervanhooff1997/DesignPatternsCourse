using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using static System.Console;

/**
 * A class should only have one reason to change
 * Relates to separation of concerns pattern: different classes handling different, independent tasks/problems. 
 */
namespace SingleResponsibilityPrinciple
{
    public class Journal
    {
        private readonly List<string> entries = new List<string>();

        private static int count = 0;

        public int AddEntry(string text)
        {
            entries.Add($"{++count}: {text}");
            return count; // memento pattern
        }

        public void RemoveEntry(int index)
        {
            entries.RemoveAt(index);
        }

        public override string ToString()
        {
            return string.Join(Environment.NewLine, entries);
        }
    }

    public class Persistence
    {
        public void SaveToFile(Journal j, string filename, bool overwrite = false)
        {
            if (overwrite || !File.Exists(filename))
                File.WriteAllText(filename, j.ToString());
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var j = new Journal();
            j.AddEntry("I cried today");
            j.AddEntry("I ate a bug");
            WriteLine(j);

            var p = new Persistence();
            var filename = @"C:\temp\journal.txt";
            p.SaveToFile(j, filename, true);
            Process.Start(new ProcessStartInfo() { FileName = filename, UseShellExecute = true });
        }
    }
}
