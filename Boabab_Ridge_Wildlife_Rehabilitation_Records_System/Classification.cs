using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boabab_Ridge_Wildlife_Rehabilitation_Records_System
{
    

    public static class Classification
    {

        public static (string Status, string HousingUnit) Classify(int recoveryscore)
        {


            if(recoveryscore >= 0 && recoveryscore <= 19)
                return ("Critical", "Intensive Care Unit");

            if(recoveryscore >= 20 && recoveryscore <= 39)
                return ("Serious", "High Dependency Ward");

            if(recoveryscore >= 40 && recoveryscore <= 59)
                return ("Stable", "Recovery Ward");

            if(recoveryscore >= 60 && recoveryscore <= 79)
                return ("Recovering", "Outdoor Enclosure");

            if(recoveryscore >= 80 && recoveryscore <= 100)
                return ("Release Ready", "Pre Release Camp");

            return ("", "");
        }

    }

}
