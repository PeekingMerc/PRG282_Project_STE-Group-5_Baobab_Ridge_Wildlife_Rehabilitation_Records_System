using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.IO;
using System.Windows.Forms;

namespace Boabab_Ridge_Wildlife_Rehabilitation_Records_System
{
    //  calculates numbers using animal list and creates a report
    public class SummaryReport
    {
        //  so numbers line up with labels
        private const int ValueColumn = 26;

        public int TotalAnimals { get; set; }
        public string AverageAgeText { get; set; }
        public string AverageScoreText { get; set; }
        public int CriticalCount { get; set; }
        public int SeriousCount { get; set; }
        public int StableCount { get; set; }
        public int RecoveringCount { get; set; }
        public int ReleaseReadyCount { get; set; }

        //  summary.txt is stored in the same folder as the running app
        public string SummaryFilePath
        {
            get { return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "summary.txt"); }
        }

        //  gets totals, averages and count per status from the list
        public void GenerateSummary(List<Animal> animals)
        {
            //  resets all values first
            TotalAnimals = 0;
            CriticalCount = 0;
            SeriousCount = 0;
            StableCount = 0;
            RecoveringCount = 0;
            ReleaseReadyCount = 0;
            AverageAgeText = "N/A";
            AverageScoreText = "N/A";

            //  validation in case the data is empty
            if (animals == null || animals.Count == 0)
            {
                return;
            }

            int ageSum = 0;
            int scoreSum = 0;

            //  gets status counts
            foreach (Animal animal in animals)
            {
                ageSum += animal.Age;
                scoreSum += animal.RecoveryScore;

                //  updates counts based on status
                switch (animal.Status)
                {
                    case "Critical":
                        CriticalCount++;
                        break;
                    case "Serious":
                        SeriousCount++;
                        break;
                    case "Stable":
                        StableCount++;
                        break;
                    case "Recovering":
                        RecoveringCount++;
                        break;
                    case "Release-Ready":
                        ReleaseReadyCount++;
                        break;
                }
            }

            TotalAnimals = animals.Count;

            //  set to double so division keeps decimals. "InvariantCulture" forces "." as decimal seperator instead of "," apparently
            AverageAgeText = ((double)ageSum / TotalAnimals).ToString("F2", CultureInfo.InvariantCulture);
            AverageScoreText = ((double)scoreSum / TotalAnimals).ToString("F2", CultureInfo.InvariantCulture);
        }

        //  puts label and value on a line so they line up
        private string FormatLine(string label, string value)
        {
            return label.PadRight(ValueColumn) + value;
        }

        //  generates the report according to the layout
        public List<string> BuildReportLines()
        {
            List<string> lines = new List<string>();

            lines.Add("BAOBAB RIDGE WILDLIFE REHABILITATION CENTRE");
            lines.Add("ANIMAL SUMMARY REPORT");
            lines.Add("Generated: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture));
            lines.Add(new string('-', 40));
            lines.Add(FormatLine("Total animals:", TotalAnimals.ToString()));
            lines.Add(FormatLine("Average age (years):", AverageAgeText));
            lines.Add(FormatLine("Average recovery score:", AverageScoreText));
            lines.Add(new string('-', 40));
            lines.Add("Animals per status:");
            lines.Add(FormatLine("  Critical:", CriticalCount.ToString()));
            lines.Add(FormatLine("  Serious:", SeriousCount.ToString()));
            lines.Add(FormatLine("  Stable:", StableCount.ToString()));
            lines.Add(FormatLine("  Recovering:", RecoveringCount.ToString()));
            lines.Add(FormatLine("  Release-Ready:", ReleaseReadyCount.ToString()));

            return lines;
        }

        //  writes the report, replacing previous versions
        public bool WriteSummaryFile()
        {
            try
            {
                //  "false" = do not append so .txt gets overwritten
                using (StreamWriter writer = new StreamWriter(SummaryFilePath, false))
                {
                    foreach (string line in BuildReportLines())
                    {
                        writer.WriteLine(line);
                    }
                }

                return true;
            }
            //  exception in case of inadequate permission to write to .txt
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show("You don't have permission to write summary.txt", "Permission error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            //  exception in case of missing folder and or an open file
            catch (IOException ex)
            {
                MessageBox.Show("summary.txt could not be saved. If it's open, please close it. \n\n" + ex.Message, "File error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            return false;
        }
    }
}