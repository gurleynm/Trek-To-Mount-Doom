using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T2MD.Shared;

namespace T2MD
{
    public class MileManager
    {
        private static readonly string and_base = "ANDROID_BASELINE";
        private static int TotalSteps { get; set; }
        public static double HeightInInches { get; set; } = 35;
        public static double MilesPerStep => (HeightInInches * 0.413) / (5280 * 12);
        public static double DistanceToMtDoom => MajorMilestones["Mount Doom"];
        public static Dictionary<string, string> MajorImages => new Dictionary<string, string> {
            { "Hobbitton", "https://endlessfamilytravels.com/wp-content/uploads/2023/05/Hobbiton-Houses5.jpeg" }
            ,{ "Rivendell", "https://static1.cbrimages.com/wordpress/wp-content/uploads/2021/03/The-Lord-of-the-Rings-Rivendell.jpg" }
            ,{ "Lothlorien", "https://usa-forum.d5cdn.com/optimized/2X/8/8fa194e33a766e4a16e43f2db31907610b8b1fb2_2_690x379.jpeg" }
            ,{ "Rauros", "https://qph.cf2.quoracdn.net/main-qimg-988145e34db2d173556d657cc10f20e8-lq" }
            ,{ "Mount Doom", "https://platform.polygon.com/wp-content/uploads/sites/2/chorus/uploads/chorus_asset/file/22280932/ROTK_eye_of_sauron.jpg?quality=90&strip=all&crop=19.044811320755,0,61.910377358491,100" }
            ,{ "Minus Tirith", "https://images4.alphacoders.com/118/thumb-1920-118904.jpg" }
            ,{ "Isengard", "https://cdn-images-1.medium.com/v2/resize:fit:918/1*1kCNdOayi5rTBVoYhb_7AA.jpeg" }
            ,{ "Rivendell (2)", "https://static1.cbrimages.com/wordpress/wp-content/uploads/2021/03/The-Lord-of-the-Rings-Rivendell.jpg" }
            ,{ "Bag End", "https://www.pinyourfootsteps.com/wp-content/uploads/2019/12/NZNI-96-scaled.jpg" }
            ,{ "Grey Havens", "https://static0.gamerantimages.com/wordpress/wp-content/uploads/2024/04/4k-returnofking-movie-screencaps-com-34174-cropped.jpg" }
        };
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
            ,{ "DESTROY THE RING", 1779 }
            ,{ "Ride the Eagles to Safety", 1782 }
            ,{ "Aragorn's Corination", 1850 }
            ,{ "Enter Rohan", 2003 }
            ,{ "Mountains of Moria Rise Ahead", 3254 }
            ,{ "Frodo's Farewell", 3384 }
            ,{ "Sunset at Bag End", 3644 }
        };
        public static async Task<int> GetTotalSteps()
        {
            if (TotalSteps == 0)
            {
                TotalSteps = await Constants.PullSecureInt("TotalSteps");
            }

            return TotalSteps;
        }
        public static void SetTotalSteps(int ts)
        {
            TotalSteps = ts;
        }

        public static async Task<double> StepsToMiles()
        {
            if (TotalSteps == 0)
                TotalSteps = await Constants.PullSecureInt("TotalSteps");

            return StepsToMiles(TotalSteps);
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

        public static async Task<int> RetrieveSteps()
        {
            if (TotalSteps == 0)
                TotalSteps = await Constants.PullSecureInt("TotalSteps");

            int InitSteps = TotalSteps;

            if (!Pedometer.Default.IsSupported)
                return TotalSteps;

            int Current = await Pedometer.Default.GetCurrentReading();
#if ANDROID
            bool containsKey = await Constants.SecureContainsKey(and_base);
            if (containsKey)
            {
                int baseline = await Constants.PullSecureInt(and_base);
                Constants.PushSecure(and_base, Current);

                if (Current >= baseline)
                    Current -= baseline;

                TotalSteps += Current;
            }
            else
                Constants.PushSecure(and_base, Current);
#endif
#if IOS
            TotalSteps = Current;
#endif
            if (InitSteps != TotalSteps)
                await Constants.PushSecure("TotalSteps", TotalSteps);

            return TotalSteps;
        }

        public static async Task<int> UpdateStepsSecure(int steps)
        {
            if (TotalSteps == 0)
                TotalSteps = await Constants.PullSecureInt("TotalSteps");

            TotalSteps += steps;
            await Constants.PushSecure("TotalSteps", TotalSteps);

            return TotalSteps;
        }
    }
}
