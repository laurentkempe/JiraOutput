using System;
using System.IO;
using System.Runtime.InteropServices;
using SNAGITLib;

namespace JiraOutput
{
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    [Guid("05BD448D-BFC3-4CCC-8601-50DE6CC36BB2")]
    public class JiraOutput : MarshalByRefObject, IComponentInitialize, IComponentTerminate, IOutput, IOutputMenu, IPackageOptionsUI, IComponentWantsCategoryPreferences
    {
        private ISnagIt _snagIt;

        [ComVisible(true)]
        public void InitializeComponent(object pExtensionHost, IComponent pComponent, componentInitializeType initType)
        {
            _snagIt = pExtensionHost as ISnagIt;
            if (_snagIt == null)
            {
                throw new InvalidOperationException("Unable to communicate with Snagit");
            }
        }

        [ComVisible(true)]
        public void TerminateComponent(componentTerminateType termType)
        {
        }

        [ComVisible(true)]
        public void Output()
        {
            var createIssueForm = new CreateIssueForm(SaveScreenCapture());
            var dialogResult = createIssueForm.ShowDialog();

            SaveScreenCapture();
        }

        [ComVisible(true)]
        public string GetOutputMenuData()
        {
            return "<menu> " +
                   "    <menuitem label=\"" + "Create Issue" + "\" id=\"0\" />" +
                   "    <menuitem label=\"" + "Attach to Issue" + "\" id=\"1\" />" +
                   "    <menuseparator /> " +
                   "    <menuitem label=\"" + "Server..." + "\" id=\"2\" />" +
                   "</menu>";
        }
            
        [ComVisible(true)]
        public void SelectOutputMenuItem(string outputMenuID)
        {
            if (outputMenuID == "0")
            {
                Output();
            }
            else if (outputMenuID == "1")
            {
            }
            else if (outputMenuID == "2")
            {
                var configurationForm = new ConfigurationForm();
                var dialogResult = configurationForm.ShowDialog();                
            }
        }

        [ComVisible(true)]
        public void ShowPackageOptionsUI()
        {
        }

        public static string PackageDirectory { get; set; }

        public static string PreferenceDirectory { get; set; }

        public static string SettingsFileName = "JiraOutputSettings.xml";


        public static string PrefsPathWithFileName
        {
            get
            {
                return string.Format("{0}\\{1}", PreferenceDirectory, SettingsFileName);
            }
        }

        [ComVisible(true)]
        public void SetComponentCategoryPreferences(SnagItOutputPreferences prefs)
        {
            string packageDir, preferencesDir;
            prefs.PackageDir(out packageDir);
            prefs.PreferencesDir(out preferencesDir);

            PackageDirectory = packageDir;
            PreferenceDirectory = preferencesDir;
            
            JiraPreferences.Setup();
            JiraPreferences.Read();
        }

        public string SaveScreenCapture()
        {
            ISnagItDocument snagItDocument = _snagIt.SelectedDocument;
            var imageDocumentSave = snagItDocument as ISnagItImageDocument as ISnagItImageDocumentSave;
            var filename = Path.GetTempFileName() + ".png";
            imageDocumentSave.SaveToFile(filename, snagImageFileType.siftPNG, null);
            return filename;
        }
    }
}