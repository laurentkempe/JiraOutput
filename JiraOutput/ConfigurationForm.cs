using System;
using System.Windows.Forms;

namespace JiraOutput
{
    public partial class ConfigurationForm : Form
    {
        public ConfigurationForm()
        {
            InitializeComponent();

            textBoxServerUrl.Text = JiraPreferences.ServerUrl;
            textBoxUsername.Text = JiraPreferences.Username;
            textBoxPassword.Text = JiraPreferences.Password;
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            JiraPreferences.ServerUrl = textBoxServerUrl.Text;
            JiraPreferences.Username = textBoxUsername.Text;
            JiraPreferences.Password = textBoxPassword.Text;

            JiraPreferences.Write();
            Close();
        }
    }
}
