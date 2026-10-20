using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Boabab_Ridge_Wildlife_Rehabilitation_Records_System
{
   

    //Represents a single animal in the rehabilitation center
    //Status and HousingUnit are claculated from the RecoveryScore.

    public class Animal
    {

        public string AnimalID { get; set; }
        public string Name { get; set; }
        public string Species { get; set; }
        public int Age { get; set; }
        public int RecoveryScore { get; set; }
        public string Status { get; set; }
        public string HousingUnit { get; set; }


        //Parameterless constructor required for DatagridView data binding.
        public Animal() { }

        //Full constructor used when laoding an animal from the text file.
        public Animal(string _animalid, string _name, string _species, int _age, int _recoveryscore, string _status, string housingunit)
        {

            AnimalID = _animalid;
            Name = _name;
            Species = _species;
            Age = _age;
            RecoveryScore = _recoveryscore;
            Status = _status;
            HousingUnit = housingunit;

        }


        //Converts the Animal object into a pipe-delimited string for writing to animals.txt
        public string ToFileLine()
        {
            
            return $"{AnimalID}|{Name}|{Species}|{Age}|{RecoveryScore}|{Status}|{HousingUnit}";
        }
    }

}
