using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Boabab_Ridge_Wildlife_Rehabilitation_Records_System
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        //  -------  PLACEHOLDER TEXT  -------
        //  generates summary, shows it on the form and saves to summary.txt
        private void btnGenerateSummary_Click(object sender, EventArgs e)
        {
            int skippedCount;
            List<Animal> animals = FileHandler.LoadAnimals(out skippedCount);

            SummaryReport report = new SummaryReport();
            report.GenerateSummary(animals);

            textTotalAnimals.Text = report.TotalAnimals.ToString();
            textAverageAge.Text = report.AverageAgeText;
            textAverageScore.Text = report.AverageScoreText;
            textCritical.Text = report.CriticalCount.ToString();
            textSerious.Text = report.SeriousCount.ToString();
            textStable.Text = report.StableCount.ToString();
            textRecovering.Text = report.RecoveringCount.ToString();
            textReleaseReady.Text = report.ReleaseReadyCount.ToString();

            report.WriteSummaryFile();
        }
    }
}
