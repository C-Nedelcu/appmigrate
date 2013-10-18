using System;
using System.Collections.Generic;
using System.Text;

namespace AppMigrate.Classes
{
    class RegKey
    {
        public string Root;
        public string Key;
        public AppPackage App;
        public string GetFullKey()
        {
            return (Root == "HKLM" ? "HKEY_LOCAL_MACHINE" : "HKEY_CURRENT_USER") + "\\" + Key;
        }

        private string GetUniqueFilename()
        {
            return this.Root + "-" + this.Key.Replace('\\', '-');
        }

        public string GetSavePath()
        {
            return App.GetAppTemporaryFolder() + GetUniqueFilename() + ".reg";
        }
    }
}
