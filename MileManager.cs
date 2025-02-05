using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T2MD
{
    public class MileManager
    {
        private static int TotalSteps { get; set; }
        public static double HeightInInches { get; set; } = 35;
        public static double MilesPerStep => (HeightInInches*0.413)/(5280*12);
        public static double DistanceToMtDoom => MajorMilestones["Mount Doom"];
        public static Dictionary<string, double> MajorMilestones => new Dictionary<string, double> {
            { "Hobbitton", 0 }
            ,{ "Rivendell", 458 }
            ,{ "Lothlorien", 920 }
            ,{ "Rauros", 1309 }
            ,{ "Mount Doom", 1779 }
            ,{ "Minus Tirith", 2314 }
            ,{ "Isengard", 3007 }
            ,{ "Rivendell (2)", 3007 }
            ,{ "Bag End", 3404 }
            ,{ "Grey Havens", 3644 }
        };
        public static Dictionary<string, double> MinorMilestones => new Dictionary<string, double> {
            { "Bag End", 0 }
            ,{ "Cross Brandywine Bridge", 5 }
            ,{ "Rescued by Tom Bombadil", 95 }
            ,{ "Meet Strider at the Prancing Pony", 135 }
            ,{ "Climb Weathertop", 240 }
            ,{ "Reach Stone Trolls", 393 }
            ,{ "Attack at the Ford", 450 }
            ,{ "Redhorn Pass", 750 }
            ,{ "Warg Attack", 778 }
            ,{ "Reach Doors of Moria", 798 }
            ,{ "YOU SHALL NOT PASS", 839 }
            ,{ "Fields of Celebrant", 1023 }
            ,{ "South Undeep", 1142 }
            ,{ "Orc Attack", 1267 }
            ,{ "Pass the Argonath", 1288 }
            ,{ "Shelob's Lair", 1600 }
            ,{ "Sam Carries Frodo", 1775 }
            ,{ "Enter Rohan", 2003 }
            ,{ "Mountains of Moria Rise Ahead", 3254 }
            ,{ "Frodo's Farewell", 3384 }
            ,{ "Sunset at Bag End", 3644 }
        };
        public static async Task<int> GetTotalSteps()
        {
            if (TotalSteps == 0){
                if(int.TryParse(await Constants.PullSecure("TotalSteps"), out int ts))
                    TotalSteps = ts;
            }

            return TotalSteps;
        }
        public static void SetTotalSteps(int ts)
        {
            TotalSteps = ts;
        }

        public static async Task<double> StepsToMiles()
        {
            string totalSteps;
            if(TotalSteps == 0)
            {
                totalSteps = await Constants.PullSecure("TotalSteps");
                if(int.TryParse(totalSteps, out int ts))
                    TotalSteps = ts;
            }
            else
                totalSteps = TotalSteps.ToString();

            if (double.TryParse(totalSteps, out double steps)){
                return StepsToMiles(steps);
            }

            return 0;
        }
        public static double StepsToMiles(double steps)
        {
            return double.Round(MilesPerStep * (steps * 1.00), 2);
        }
        public static (string, string) GetMilestones(double miles)
        {
            var MajorKeys = MajorMilestones.Keys.ToArray();
            var MinorKeys = MinorMilestones.Keys.ToArray();
            string major = MajorKeys[0];
            string minor = MinorKeys[0];
            for (int index = 1; index < MajorKeys.Length; index++)
            {
                if (miles < MajorMilestones[MajorKeys[index]])
                    break;
                else
                    major = MajorKeys[index];
            }

            for (int index = 1; index < MinorKeys.Length; index++)
            {
                if (miles < MinorMilestones[MinorKeys[index]])
                    break;
                else
                    minor = MinorKeys[index];
            }

            return (major, minor);
        }
    }
}
