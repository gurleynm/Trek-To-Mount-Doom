﻿namespace T2MD
{
    public class Constants
    {
        public static EventHandler<string> CurPageChanged = (sender, value) => { };

        public static bool FirstLoad = true;
        private static bool UseSecure = true;
        public static string InitDate => new DateTime().ToString("MMM dd, yyyy");
        public static string StartDate { get; set; } = InitDate;
        public static string Avatar { get; set; } = "";
        private static string curPage;
        public static string CurPage
        {
            get => curPage;
            set
            {
                if (curPage != value)
                {
                    curPage = value;
                    CurPageChanged?.Invoke(typeof(Constants), curPage);
                }
            }
        }
        public static Dictionary<string, string> PageColors = new Dictionary<string, string>(){
                                                                {"", "white" }
                                                                ,{"steps", "white" }
                                                                ,{"settings", "white" }
                                                                ,{"stats", "white" }
                                                                };
        public static void UpdatePage(string page)
        {
            foreach (var key in PageColors.Keys)
                PageColors[key] = key == page ? "goldenrod" : "white";
        }

        public static async Task<bool> SecureContainsKey(string name)
        {
            return (await SecureStorage.Default.GetAsync(name)) != null;
        }
        public static async Task<int> PullSecureInt(string name)
        {
            if (!UseSecure)
                return 0;

            return int.TryParse(await SecureStorage.Default.GetAsync(name), out int tosser) ? tosser : 0;
        }
        public static async Task<string> PullSecure(string name)
        {
            if (!UseSecure)
                return "";

            return await SecureStorage.Default.GetAsync(name);
        }
        
        public static async Task<bool> PushSecure(string name, int num)
        {
            return await PushSecure(name, num.ToString());
        }
        public static async Task<bool> PushSecure(string name, double num)
        {
            return await PushSecure(name, num.ToString());
        }
        public static async Task<bool> PushSecure(string name, string value = "")
        {
            if (!UseSecure)
                return false;

            if (string.IsNullOrEmpty(value))
                SecureStorage.Default.Remove(name);

            await SecureStorage.Default.SetAsync(name, value);
            return true;
        }
        public static void ClearSecure(string name)
        {
            if (UseSecure)
                SecureStorage.Default.Remove(name);
        }
        public static string Format(double num)
        {
            if (num < 10)
                return Math.Round(num, 2).ToString();
            else if (num < 999)
                return Math.Round(num, 1).ToString();

            return ((int)num).ToString();
        }
    }
}
