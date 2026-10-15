using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boabab_Ridge_Wildlife_Rehabilitation_Records_System
{
    //internal class Animal
    //{
    //}

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


        //Parameterless constructor
        public Animal() { }

        //Full constructor used when laoding from file
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


        //Returns the animal as a pipe delimited line for animal.txt file
        public string ToFileLine()
        {
            return $"{AnimalID} | {Name} | {Species} | {Age} | {RecoveryScore} | {Status} | {HousingUnit}";
        }

    }

}
