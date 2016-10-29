namespace DroidDrawables.Preferences
{
    public static class AppSettings
    {
        public static string ResourceFolder 
        {
            get 
            { 
                return Properties.Settings.Default.ResFolder; 
            }
            set 
            { 
                Properties.Settings.Default.ResFolder = value; Properties.Settings.Default.Save(); 
            }
        }

        public static bool GenerateMipmap
        {
            get 
            { 
                return Properties.Settings.Default.GenerateMipmap; 
            }
            set 
            { 
                Properties.Settings.Default.GenerateMipmap = value; 
                Properties.Settings.Default.Save(); 
            }
        }

        public static bool ClearResourceFolder
        {
            get
            {
                return Properties.Settings.Default.ClearResourceFolder;
            }
            set
            {
                Properties.Settings.Default.ClearResourceFolder = value;
                Properties.Settings.Default.Save();
            }
        }
        /**
         * @brief Save settings to persistence store
         */
        public static void PersistSettings()
        {

        }

        /**
         * @brief Loads settings from persistence store and populates the AppSettings
         *  fields
         */
        public static void LoadSettings()
        {

        }
    }
}
