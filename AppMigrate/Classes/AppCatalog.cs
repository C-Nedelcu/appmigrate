using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using Microsoft.Win32;
using System.Xml;
using System.Windows.Forms;
using AppMigrate.Classes;

namespace AppMigrate
{
    class AppCatalog
    {
        public static ArrayList SystemAppCatalog = null;
        public static ArrayList SupportedAppCatalog = null;

        static public ArrayList GetSupportedApps(bool reload=false)
        {
            if (!reload && SupportedAppCatalog != null) return SupportedAppCatalog;
            SupportedAppCatalog = new ArrayList();

            // Create XML Document
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load("AppCatalog.xml");

                // Get Application nodes
                XmlNodeList apps = xmlDoc.GetElementsByTagName("App");
                foreach (XmlNode appNode in apps)
                {
                    // Create instance and initialize values from XML
                    var app = new AppPackage()
                    {
                        Name = appNode.Attributes["Name"] == null ? "" : appNode.Attributes["Name"].Value,
                        UniqueID = appNode.Attributes["UniqueID"] == null ? "" : appNode.Attributes["UniqueID"].Value,
                        MatchRegexp = appNode.Attributes["MatchRegexp"] == null ? "" : appNode.Attributes["MatchRegexp"].Value,
                        MinimumVersion = appNode.Attributes["MinimumVersion"] == null ? null : appNode.Attributes["MinimumVersion"].Value,
                        MaximumVersion = appNode.Attributes["MaximumVersion"] == null ? null : appNode.Attributes["MaximumVersion"].Value,
                        Notes = appNode.Attributes["Notes"] == null ? "" : appNode.Attributes["Notes"].Value
                    };
                    app.RegistryKeys = new ArrayList();
                    app.Folders = new ArrayList();

                    // Read registry keys
                    foreach (XmlNode node in appNode.ChildNodes)
                    {
                        if (node.Name == "RegistryKeys")
                        {
                            // We're checking out the registry keys; so browse the node
                            foreach (XmlNode keyNode in node.ChildNodes)
                            {
                                RegKey key = new RegKey()
                                {
                                    Root = keyNode.Attributes["Root"].Value,
                                    Key = keyNode.InnerText
                                };
                                key.App = app;
                                app.RegistryKeys.Add(key);
                            }
                        }
                        else if (node.Name == "Folders")
                        {
                            // We're checking out the folders; so browse the node
                            foreach (XmlNode folderNode in node.ChildNodes)
                            {
                                Folder folder = new Folder()
                                {
                                    Root = folderNode.Attributes["Root"].Value,
                                    ID = folderNode.Attributes["ID"].Value,
                                    Path = folderNode.InnerText
                                };
                                folder.App = app;
                                app.Folders.Add(folder);
                            }

                        }

                    }

                    SupportedAppCatalog.Add(app);
                }

                return SupportedAppCatalog;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Lists applications installed on the system
        /// </summary>
        /// <returns>ArrayList of AppInstallInfo instances</returns>
        static public ArrayList GetInstalledApps(bool reload = false)
        {
            if (!reload && SystemAppCatalog != null) return SystemAppCatalog;

            try
            {
                SystemAppCatalog = new ArrayList();
                
                string uninstallKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
                string uninstallKey32 = @"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall";

                for (var i = 0; i < 4; i++)
                {
                    // We're doing four passes: first pass from LocalMachine hive, second pass from CurrentUser
                    // Third pass: LocalMachine Wow6432Node, Fourth pass: CurrentUser Wow6432Node
                    // Normally most apps should be listed in LocalMachine, but some apps such as Torch (web browser) 
                    // for some reason only appear in CurrentUser

                    // Try it, if it fails, continue to the next pass
                    try
                    {
                        RegistryKey rk;

                        // Set folder for each pass
                        if (i == 1) rk = Registry.CurrentUser.OpenSubKey(uninstallKey);
                        else if (i == 2) rk = Registry.LocalMachine.OpenSubKey(uninstallKey32);
                        else if (i == 3) rk = Registry.CurrentUser.OpenSubKey(uninstallKey32);
                        else rk = Registry.LocalMachine.OpenSubKey(uninstallKey);


                        foreach (string skName in rk.GetSubKeyNames())
                        {
                            using (RegistryKey sk = rk.OpenSubKey(skName))
                            {
                                try
                                {
                                    if (sk.GetValueKind("DisplayName") == RegistryValueKind.String)
                                    {

                                        AppInstallInfo app = new AppInstallInfo("");
                                        app.KeyName = skName;
                                        app.DisplayName = (string)sk.GetValue("DisplayName");// +skName;
                                        app.DisplayVersion = "";

                                        // Attempt to get the version
                                        try
                                        {
                                            app.DisplayVersion = (string)sk.GetValue("DisplayVersion");
                                        }
                                        catch {
                                            app.DisplayVersion = "";
                                        }

                                        // If there is a tag for the major version number
                                        var version = -1;
                                        try
                                        {
                                            if (sk.GetValueKind("MajorVersion") == RegistryValueKind.String)
                                                version = int.Parse((string)sk.GetValue("MajorVersion"));
                                            else
                                                version = (int)sk.GetValue("MajorVersion");
                                        }
                                        catch { }
                                        if (version == -1)
                                        {
                                            try
                                            {
                                                if (sk.GetValueKind("VersionMajor") == RegistryValueKind.String)
                                                    version = int.Parse((string)sk.GetValue("VersionMajor"));
                                                else
                                                    version = (int)sk.GetValue("VersionMajor");
                                            }
                                            catch { }
                                        }

                                        // Attempt to figure out the version by looking at DisplayVersion
                                        if (version == -1 && app.DisplayVersion != null)
                                        {
                                            var parts = app.DisplayVersion.Split('.');
                                            if (parts.Length > 0)
                                            {
                                                var possibleMajorVersion = parts[0];
                                                try
                                                {
                                                    version = int.Parse(possibleMajorVersion);
                                                }
                                                catch { }
                                            }
                                        }

                                        if (version == -1) version = 0;
                                        app.MajorVersionNumber = version;
                                        app.Supported = true;

                                        // Is it a driver?
                                        if (app.DisplayName.Contains("Windows Driver Package")) continue;
                                        if (app.DisplayName.Contains("Update for Microsoft")) continue;
                                        if (app.DisplayName.Contains("Microsoft") && app.DisplayName.Contains("MUI")) continue;

                                        // Check whether the app is supported or not
                                        app.CheckSupport();

                                        // Already there?
                                        if (!AlreadyInCatalog(app))
                                            SystemAppCatalog.Add(app);

                                    }
                                }
                                catch
                                {
                                    //MessageBox.Show("STEP 2 " + ex.ToString());
                                }
                            }
                        }

                    }
                    catch
                    {
                        // Nothing, continue
                        //MessageBox.Show("STEP 1 "+ex.ToString() );
                    }

                }
                return SystemAppCatalog;

            }
            catch
            {
                return null;

            }
        }

        /// <summary>
        /// Checks whether given app is already listed in the catalog (checks display name + version)
        /// This can occur when both 32-bit and 64-bit versions are installed on the system
        /// </summary>
        static bool AlreadyInCatalog(AppInstallInfo app)
        {
            if (SystemAppCatalog == null) return false;
            foreach (AppInstallInfo inf in SystemAppCatalog)
            {
                if (inf.DisplayName.ToLower() == app.DisplayName.ToLower() && inf.DisplayVersion.ToLower() == app.DisplayVersion.ToLower())
                    return true;
            }
            return false;
        }


        /// <summary>
        /// Tells us whether the application is in the app database
        /// </summary>
        /// <param name="appID">(string) application ID to check for in the db</param>
        /// <returns>True/false if installed or not</returns>
        public static AppPackage GetAppPackage(string appID)
        {
            if (SupportedAppCatalog == null) return null;
            foreach (AppPackage app in SupportedAppCatalog)
            {
                if (app.UniqueID == appID) return app;
            }
            return null;
        }
    }
}
