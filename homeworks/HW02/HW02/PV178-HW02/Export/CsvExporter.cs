using System.Collections;
using System.Runtime.InteropServices;

namespace HW02.Export
{
    internal class CsvExporter
    {
        public static void Export(string fileName, ICollection collection) // you can update the second argument type based on your collection
        {
            string path;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                path = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName, fileName); // update the path if necessary (Windows)
            }
            else
            {
                path = Path.Combine(Directory.GetCurrentDirectory(), fileName); // update the path if necessary (Linux, Mac)
            }

            using FileStream fs = File.Create(path);
            using var sr = new StreamWriter(fs);

            sr.WriteLine("id;activity");

            foreach (var record in collection)
            {
                var caseID = "";   // update this based on your collection
                var activity = ""; // update this based on your collection
                sr.WriteLine($"{caseID};{activity}");
            }
        }
    }
}
