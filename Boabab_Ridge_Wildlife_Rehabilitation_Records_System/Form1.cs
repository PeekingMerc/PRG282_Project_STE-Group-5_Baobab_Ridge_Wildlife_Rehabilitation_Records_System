using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Boabab_Ridge_Wildlife_Rehabilitation_Records_System
{
    public partial class Form1 : Form
    {
        private List<Animal> animals = new List<Animal>();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            RefreshGrid();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string id = txtAnimalID.Text.Trim();
            string name = txtName.Text.Trim();
            string species = txtSpecies.Text.Trim();
            string age = txtAge.Text.Trim();
            string recoveryScore = txtRecoveryScore.Text.Trim();

            List<string> animalIDs = new List<string>();
            foreach (Animal animal in animals)
            {
                animalIDs.Add(animal.AnimalID);
            }

            //Validate AnimalID
            Regex animalIDPattern = new Regex(@"^WR-\d{4}$");
            if (string.IsNullOrEmpty(id) || !animalIDPattern.IsMatch(id) || animalIDs.Contains(id))
            {
                ShowValidationError(txtAnimalID, "Animal ID must be unique and start with WR- followed by exactly 4 digits.");
                return;
            }

            //Validate Animal Name
            if (string.IsNullOrEmpty(name) || name.Length > 30 || name.Contains("|"))
            {
                ShowValidationError(txtName, "Name must be 1–30 characters and cannot contain '|'.");
                return;
            }

            //Validate Animal Species
            if (string.IsNullOrEmpty(species) || species.Length > 30 || species.Contains("|"))
            {
                ShowValidationError(txtSpecies, "Species must be 1–30 characters and cannot contain '|'.");
                return;
            }

            //Validate Animal age
            if (!int.TryParse(age, out int ageInt) || ageInt < 0 || ageInt > 100)
            {
                ShowValidationError(txtAge, "Age must be a whole number between 0 and 100.");
                return;
            }

            //Validate Recovery Score
            if (!int.TryParse(recoveryScore, out int recovery) || recovery < 0 || recovery > 100)
            {
                ShowValidationError(txtRecoveryScore, "Recovery Score must be a whole number between 0 and 100.");
                return;
            }

            //Calculate Status and housing unit
            (string status, string housingUnit) = Classification.Classify(recovery);

            //Append to animals.txt
            Animal newAnimal = new Animal(id, name, species, ageInt, recovery, status, housingUnit);
            File.AppendAllText(FileHandler.AnimalsFilePath, newAnimal.ToFileLine() + Environment.NewLine, System.Text.Encoding.UTF8);
            MessageBox.Show($"Animal added successfully!\nStatus: {status}\nHousing Unit: {housingUnit}", "Confirmation", MessageBoxButtons.OK, MessageBoxIcon.Information);

            //Refresh grid
            RefreshGrid();

            //Clear input boxes
            txtAnimalID.Clear();
            txtName.Clear();
            txtSpecies.Clear();
            txtAge.Clear();
            txtRecoveryScore.Clear();
            txtAnimalID.Focus();
        }

        //Message box validation method for Add animal button 
        private void ShowValidationError(Control control, string message)
        {
            MessageBox.Show(message, "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            control.Focus();
        }

        //Refresh items in dgvAnimals
        private void RefreshGrid()
        {
            int skippedCount;
            dgvAnimals.Rows.Clear();
            animals.Clear();
            animals = FileHandler.LoadAnimals(out skippedCount);
            foreach (Animal animal in animals)
            {
                dgvAnimals.Rows.Add(animal.AnimalID, animal.Name, animal.Species, animal.Age, animal.RecoveryScore, animal.Status, animal.HousingUnit);
            }

            MessageBox.Show($"Animals loaded successfully. Skipped {skippedCount} entries.", "Successful load", MessageBoxButtons.OK);
        }

        private void dgvAnimals_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvAnimals.Rows[e.RowIndex];

                txtAnimalID.Text = row.Cells[0].Value?.ToString();
                txtName.Text = row.Cells[1].Value?.ToString();
                txtSpecies.Text = row.Cells[2].Value?.ToString();
                txtAge.Text = row.Cells[3].Value?.ToString();
                txtRecoveryScore.Text = row.Cells[4].Value?.ToString();
                txtStatus.Text = row.Cells[5].Value?.ToString();
                txtHousingUnit.Text = row.Cells[6].Value?.ToString();

                // Enter edit mode
                txtAnimalID.ReadOnly = true;
                btnAdd.Enabled = false;
                btnSaveChanges.Enabled = true;
                btnClear.Enabled = true;
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            // Exit edit mode
            txtAnimalID.ReadOnly = false;
            btnAdd.Enabled = true;
            btnSaveChanges.Enabled = false;
            btnClear.Enabled = false;

            txtAnimalID.Clear();
            txtName.Clear();
            txtSpecies.Clear();
            txtAge.Clear();
            txtRecoveryScore.Clear();
            txtStatus.Clear();
            txtHousingUnit.Clear();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            Animal match = animals.FirstOrDefault(a =>
            a.AnimalID.Equals(txtSearchID.Text.Trim()));

            if (match != null)
            {
                txtAnimalID.Text = match.AnimalID.ToString();
                txtName.Text = match.Name.ToString();
                txtSpecies.Text = match.Species.ToString();
                txtAge.Text = match.Age.ToString();
                txtRecoveryScore.Text = match.RecoveryScore.ToString();
                txtStatus.Text = match.Status.ToString();
                txtHousingUnit.Text = match.HousingUnit.ToString();

                txtAnimalID.ReadOnly = true;
                btnAdd.Enabled = false;
                btnSaveChanges.Enabled = true;
                btnClear.Enabled = true;
            }
            else
            {
                MessageBox.Show("Specified Animal ID not found.", "Animal ID not found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnSaveChanges_Click(object sender, EventArgs e)
        {
            string id = txtAnimalID.Text.Trim();
            string name = txtName.Text.Trim();
            string species = txtSpecies.Text.Trim();
            string age = txtAge.Text.Trim();
            string recoveryScore = txtRecoveryScore.Text.Trim();
            int index = animals.FindIndex(a => a.AnimalID.Equals(txtAnimalID.Text.Trim()));

            //Validate Animal Name
            if (string.IsNullOrEmpty(name) || name.Length > 30 || name.Contains("|"))
            {
                ShowValidationError(txtName, "Name must be 1–30 characters and cannot contain '|'.");
                return;
            }

            //Validate Animal Species
            if (string.IsNullOrEmpty(species) || species.Length > 30 || species.Contains("|"))
            {
                ShowValidationError(txtSpecies, "Species must be 1–30 characters and cannot contain '|'.");
                return;
            }

            //Validate Animal age
            if (!int.TryParse(age, out int ageInt) || ageInt < 0 || ageInt > 100)
            {
                ShowValidationError(txtAge, "Age must be a whole number between 0 and 100.");
                return;
            }

            //Validate Recovery Score
            if (!int.TryParse(recoveryScore, out int recovery) || recovery < 0 || recovery > 100)
            {
                ShowValidationError(txtRecoveryScore, "Recovery Score must be a whole number between 0 and 100.");
                return;
            }

            //Calculate Status and housing unit
            (string status, string housingUnit) = Classification.Classify(recovery);

            //Rewrite file
            animals[index] = new Animal(id, name, species, ageInt, recovery, status, housingUnit);
            File.WriteAllLines(FileHandler.AnimalsFilePath, animals.Select(a => a.ToFileLine()), System.Text.Encoding.UTF8);

            RefreshGrid();

            txtAnimalID.ReadOnly = false;
            btnAdd.Enabled = true;
            btnSaveChanges.Enabled = false;
            btnClear.Enabled = false;

            txtAnimalID.Clear();
            txtName.Clear();
            txtSpecies.Clear();
            txtAge.Clear();
            txtRecoveryScore.Clear();
            txtStatus.Clear();
            txtHousingUnit.Clear();
        }
    }
}
