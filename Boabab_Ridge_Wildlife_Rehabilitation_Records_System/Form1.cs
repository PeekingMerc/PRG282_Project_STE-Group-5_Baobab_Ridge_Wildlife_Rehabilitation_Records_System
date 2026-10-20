using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
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
        //In-memory list of animals currently loaded from the file
        private List<Animal> animals = new List<Animal>();
        // List of IDs used for quick validation
        List<string> animalIDs = new List<string>();
        // Tracks if the form is currently editing an existing animal
        bool editMode = false;

        public Form1()
        {
            InitializeComponent();
        }

        //Loads animals from the text file into the grid and shows a warning if any lines were skipped.
        public void LoadAndDisplayAnimals()
        {
            animals = FileHandler.LoadAnimals(out int skipped);
            RefreshGrid();

            if (skipped > 0)
            {
                MessageBox.Show($"{skipped} line(s) in animals.txt were malformed and were skipped.",
                                "Load Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // Triggers the initial load of data when the form starts.
        private void Form1_Load(object sender, EventArgs e)
        {
            LoadAndDisplayAnimals();
        }

        //Validates input, calculates status, and appends a new animal to the text file.
        private void btnAdd_Click(object sender, EventArgs e)
        {
            string id = txtAnimalID.Text.Trim();
            string name = txtName.Text.Trim();
            string species = txtSpecies.Text.Trim();
            string age = txtAge.Text.Trim();
            string recoveryScore = txtRecoveryScore.Text.Trim();

            //Validate Input
            string validate = FileHandler.Validate(id, name, species, age, recoveryScore, animalIDs, editMode);

            if (validate != null)
            {
                ShowValidationError(txtAnimalID, validate);
                return;
            }

            //Calculate Status and housing unit
            (string status, string housingUnit) = Classification.Classify(int.Parse(recoveryScore));

            //Append to animals.txt
            Animal newAnimal = new Animal(id, name, species, int.Parse(age), int.Parse(recoveryScore), status, housingUnit);
            File.AppendAllText(FileHandler.AnimalsFilePath, newAnimal.ToFileLine() + Environment.NewLine, System.Text.Encoding.UTF8);
            MessageBox.Show($"Animal added successfully!\nStatus: {status}\nHousing Unit: {housingUnit}", "Confirmation", MessageBoxButtons.OK, MessageBoxIcon.Information);

            RefreshGrid();
            ClearInputboxes();
            txtAnimalID.Focus();
        }

        //Message box validation method for Add animal button 
        private void ShowValidationError(Control control, string message)
        {
            MessageBox.Show(message, "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            control.Focus();
        }

        //Clears and reloads the DataGridView from the file to ensure no duplicate rows appear.
        private void RefreshGrid()
        {
            int skippedCount;
            dgvAnimals.Rows.Clear();
            animals.Clear();
            animals = FileHandler.LoadAnimals(out skippedCount);
            animalIDs.Clear();

            //Populate the grid with animals
            foreach (Animal animal in animals)
            {
                dgvAnimals.Rows.Add(animal.AnimalID, animal.Name, animal.Species, animal.Age, animal.RecoveryScore, animal.Status, animal.HousingUnit);
            }

            //Rebuild the list of unique IDs for validation

            foreach (Animal animal in animals)
            {
                animalIDs.Add(animal.AnimalID);
            }
        }

        //Enters edit mode
        private void EnterEditMode()
        {
            txtAnimalID.ReadOnly = true;
            btnAdd.Enabled = false;
            btnSaveChanges.Enabled = true;
            btnClear.Enabled = true;
            editMode = true;
        }

        //Exits edit mode
        private void ExitEditMode()
        {
            txtAnimalID.ReadOnly = false;
            btnAdd.Enabled = true;
            btnSaveChanges.Enabled = false;
            btnClear.Enabled = false;
            editMode = false;
        }

        //Clears all textboxes on the form.
        private void ClearInputboxes()
        {
            txtAnimalID.Clear();
            txtName.Clear();
            txtSpecies.Clear();
            txtAge.Clear();
            txtRecoveryScore.Clear();
            txtStatus.Clear();
            txtHousingUnit.Clear();
        }

        //Loads the selected animal from the grid into the textboxes for editing
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

                EnterEditMode();
            }
        }

        // Resets the form back to Add mode and clears the boxes.
        private void btnClear_Click(object sender, EventArgs e)
        {
            ExitEditMode();
            ClearInputboxes();
        }

        // Searches for an animal by ID and loads it into the textboxes.
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

                EnterEditMode();
            }
            else
            {
                MessageBox.Show("Specified Animal ID not found.", "Animal ID not found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // Updates the existing animal's data, recalculates status, and saves to the file.
        private void btnSaveChanges_Click(object sender, EventArgs e)
        {
            string id = txtAnimalID.Text.Trim();
            string name = txtName.Text.Trim();
            string species = txtSpecies.Text.Trim();
            string age = txtAge.Text.Trim();
            string recoveryScore = txtRecoveryScore.Text.Trim();
            int index = animals.FindIndex(a => a.AnimalID.Equals(txtAnimalID.Text.Trim()));

            //Validate Input
            string validate = FileHandler.Validate(id, name, species, age, recoveryScore, animalIDs, editMode);

            if (validate != null)
            {
                ShowValidationError(txtAnimalID, validate);
                return;
            }
            //Calculate Status and housing unit
            (string status, string housingUnit) = Classification.Classify(int.Parse(recoveryScore));

            //Rewrite file
            animals[index] = new Animal(id, name, species, int.Parse(age), int.Parse(recoveryScore), status, housingUnit);
            File.WriteAllLines(FileHandler.AnimalsFilePath, animals.Select(a => a.ToFileLine()), System.Text.Encoding.UTF8);

            RefreshGrid();
            ExitEditMode();
            ClearInputboxes();
        }

        // Deletes the selected animal from the list and the file, with a confirmation prompt.
        private void btnDelete_Click(object sender, EventArgs e)
        {
            //Check if a row is selected
            if (dgvAnimals.CurrentRow == null)
            {
                MessageBox.Show("Please select an animal from the grid to delete.",
                                "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            //Get the selected Animal ID
            string selectedID = dgvAnimals.CurrentRow.Cells[0].Value.ToString();

            //Find the animal in the list
            Animal animalToDelete = animals.Find(a => a.AnimalID == selectedID);
            if (animalToDelete == null) return;

            //Show confirmation
            DialogResult result = MessageBox.Show(
                $"Delete {animalToDelete.AnimalID} ({animalToDelete.Name})? This cannot be undone.",
                "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            //If Yes, delete and save
            if (result == DialogResult.Yes)
            {
                // Remove from the list
                animals.Remove(animalToDelete);

                // Rewrite the file
                try
                {
                    File.WriteAllLines(FileHandler.AnimalsFilePath, animals.Select(a => a.ToFileLine()), System.Text.Encoding.UTF8);
                }
                catch (IOException ex)
                {
                    MessageBox.Show("The animals file is in use by another program.\n\n" + ex.Message,
                                    "File Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                catch (UnauthorizedAccessException)
                {
                    MessageBox.Show("You do not have permission to write animals.txt.",
                                    "Permission Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                // Refresh the grid
                RefreshGrid();

                // Clear the textboxes just in case the deleted animal was currently displayed
                ExitEditMode();
                ClearInputboxes();
            }
        }

        //  generates summary, shows it on the form and saves to summary.txt
        private void btnGenerateSummary_Click(object sender, EventArgs e)
        {
            int skippedCount;
            List<Animal> animals = FileHandler.LoadAnimals(out skippedCount);

            SummaryReport report = new SummaryReport();
            report.GenerateSummary(animals);

            txtTotalAnimals.Text = report.TotalAnimals.ToString();
            txtAverageAge.Text = report.AverageAgeText;
            txtAverageScore.Text = report.AverageScoreText;
            txtCritical.Text = report.CriticalCount.ToString();
            txtSerious.Text = report.SeriousCount.ToString();
            txtStable.Text = report.StableCount.ToString();
            txtRecovering.Text = report.RecoveringCount.ToString();
            txtReleaseReady.Text = report.ReleaseReadyCount.ToString();

            report.WriteSummaryFile();
        }

    }
}
