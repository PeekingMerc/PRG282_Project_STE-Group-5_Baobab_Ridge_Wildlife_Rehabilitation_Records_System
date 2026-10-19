using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using System.IO;


namespace Boabab_Ridge_Wildlife_Rehabilitation_Records_System
{


    public partial class Form1 : Form
    {

        private List<Animal> animals = new List<Animal>();
        public Form1()
        {
            InitializeComponent();
        }

        //Load Animals from file and display them in the DataGridView
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

        //Rebinds the grid to the in-memory animal list to prevent duplicate rows.

        private void RefreshGrid()
        {
            dgvAnimals.DataSource = null;
            dgvAnimals.DataSource = animals;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadAndDisplayAnimals();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

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
                
                
                File.WriteAllLines(FileHandler.AnimalsFilePath, animals.Select(a => a.ToFileLine()), System.Text.Encoding.UTF8);
                
               
                // Refresh the grid
                RefreshGrid();

                // Clear the textboxes just in case the deleted animal was currently displayed
                textAnimalID.Clear();
                textName.Clear();
                textSpecies.Clear();
                textAge.Clear();
                textRecoveryScore.Clear();
                textStatus.Clear();
                textHousingUnit.Clear();
            }
        }
    

    }
}
