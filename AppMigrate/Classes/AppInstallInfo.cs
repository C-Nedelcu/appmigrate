using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace AppMigrate
{
    class AppInstallInfo
    {
        // Bunch of info about the installed application
        public string DisplayName;
        public string DisplayVersion;
        public int MajorVersionNumber;
        public string KeyName;
        public bool Supported;
        public string Notes;

        /// <summary>
        /// Corresponding package in the catalog
        /// </summary>
        public AppPackage package;

        /// <summary>
        /// Filename for importing
        /// </summary>
        private string filename;

        public AppInstallInfo(string filename)
        {
            // TODO: Complete member initialization
            this.filename = filename;
        }

        /// <summary>
        /// Checks in the catalog of supported apps for whether the application is supported
        /// </summary>
        public bool CheckSupport()
        {
            ArrayList catalog = AppCatalog.GetSupportedApps();
            Supported = false;
            foreach (AppPackage app in catalog)
            {
                if (this.MatchesAppPackage(app))
                {
                    Supported = true;
                    package = app;
                    Notes = app.Notes;
                    break;
                }
            }

            return Supported;
        }


        /// <summary>
        /// Checks whether specified app (from catalog) matches current app (from system)
        /// </summary>
        /// <param name="app">Catalog app</param>
        /// <returns></returns>
        public bool MatchesAppPackage(AppPackage app)
        {
            try
            {
                Regex check = new Regex(app.MatchRegexp, RegexOptions.IgnoreCase);
                bool regmatch = check.IsMatch(this.DisplayName);

                // Not a match for the given regular expression
                if (!regmatch) return false;

                // Is there any minimum version specified?
                if (app.MinimumVersion != null)
                {
                    int miniVersion = int.Parse(app.MinimumVersion);
                    if (this.MajorVersionNumber < miniVersion) return false; // does not meet requirement
                }

                // Any maximum version specified
                if (app.MaximumVersion != null)
                {
                    int maxiVersion = int.Parse(app.MaximumVersion);
                    if (this.MajorVersionNumber > maxiVersion) return false; // does not meet requirement
                }

            }

            catch (Exception e)
            {
                MessageBox.Show(this.DisplayName+" => "+e.ToString());
            }
            return true;
        }
    }
}
