using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Boabab_Ridge_Wildlife_Rehabilitation_Records_System
{
    //internal class FileHandler
    //{
    //}

    public static class FileHandler
    {
        //Path to animals.txt in the same folder as the running .exe.  
        public static string AnimalsFilePath => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "animals.txt");

        //Loads all valid animals from animal.txt
        //Creates an empty file if it doesn't exist
        public static List<Animal> LoadAnimals(out int skippedCount)
        {
            var animals = new List<Animal>();
            skippedCount = 0;

            try
            {
                if (!File.Exists(AnimalsFilePath))
                {
                    File.Create(AnimalsFilePath).Close();
                    return animals;
                }

                string[] lines = File.ReadAllLines(AnimalsFilePath, System.Text.Encoding.UTF8);

                foreach (string line in lines)
                {
                    if (string.IsNullOrWhiteSpace(line))
                    {
                        skippedCount++;
                        continue;
                    }

                    string[] parts = line.Split('|');

                    if (parts.Length != 7)
                    {
                        skippedCount++;
                        continue;
                    }

                    if (!IsValidID(parts[0].Trim()))
                    {
                        skippedCount++;
                        continue;
                    }

                    if (!int.TryParse(parts[3].Trim(), out int age) || age < 0 || age > 100)
                    {
                        skippedCount++;
                        continue;

                    }

                    if (!int.TryParse(parts[4].Trim(), out int recoveryScore) || recoveryScore < 0 || recoveryScore > 100)
                    {
                        skippedCount++;
                        continue;

                    }

                    animals.Add(new Animal(
                        parts[0].Trim().ToUpper(),
                        parts[1].Trim(),
                        parts[2].Trim(),
                        age,
                        recoveryScore,
                        parts[5].Trim(),
                        parts[6].Trim()
                        ));
                }

            }
            catch (IOException ex)
            {
                MessageBox.Show(
                    "The animals file is in use by another program.\n\n" + ex.Message,
                    "File Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show(
                    "You do not have permission to read animals.txt.",
                    "Permission Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            return animals;

        }
        public static bool IsValidID(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return false;
            id = id.Trim().ToUpper();

            if (id.Length != 7) return false;
            if (!id.StartsWith("WR-")) return false;

            for (int i = 3; i < 7; i++)
                if (!char.IsDigit(id[i])) return false;

            return true;
        }

        //validate animal Input
        public static string Validate(string id, string name, string species, string age, string recoveryScore, List<string> animalIDs, bool editMode)
        {
            if (!editMode)
            {
                //Validate AnimalID
                Regex animalIDPattern = new Regex(@"^WR-\d{4}$");
                if (string.IsNullOrEmpty(id) || !animalIDPattern.IsMatch(id) || animalIDs.Contains(id))
                {
                    return "Animal ID must be unique and start with WR- followed by exactly 4 digits.";
                }
            }

            //Validate Animal Name
            if (string.IsNullOrEmpty(name) || name.Length > 30 || name.Contains("|"))
            {
                return "Name must be 1–30 characters and cannot contain '|'.";
            }

            //Validate Animal Species
            if (string.IsNullOrEmpty(species) || species.Length > 30 || species.Contains("|"))
            {
                return "Species must be 1–30 characters and cannot contain '|'.";
            }

            //Validate Animal age
            if (!int.TryParse(age, out int ageInt) || ageInt < 0 || ageInt > 100)
            {
                return "Age must be a whole number between 0 and 100.";
            }

            //Validate Recovery Score
            if (!int.TryParse(recoveryScore, out int recovery) || recovery < 0 || recovery > 100)
            {
                return "Recovery Score must be a whole number between 0 and 100.";
            }

            return null; //null means valid in this context
        }

    }

}
