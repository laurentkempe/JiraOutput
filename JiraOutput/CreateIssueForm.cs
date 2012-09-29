using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Atlassian.Jira;
using System.Linq;

namespace JiraOutput
{
    public partial class CreateIssueForm : Form
    {
        private readonly string _savedScreenCaptureFilename;
        private readonly Jira _jira;
        private readonly IEnumerable<Project> _projects;
        private IEnumerable<IssuePriority> _issuePriorities;
        private IEnumerable<IssueType> _issueTypes;

        public CreateIssueForm(string savedScreenCaptureFilename)
        {
            InitializeComponent();

            _savedScreenCaptureFilename = savedScreenCaptureFilename;

            _jira = new Jira(JiraPreferences.ServerUrl, JiraPreferences.Username, JiraPreferences.Password);
            _projects = _jira.GetProjects();

            foreach (var project in _projects)
            {
                comboBoxProject.Items.Add(project.Name);
            }

            _issuePriorities = _jira.GetIssuePriorities();

            foreach (var issuePriority in _issuePriorities)
            {
                comboBoxPriority.Items.Add(issuePriority.Name);
            }
        }

        private void buttonCreateIssue_Click(object sender, EventArgs e)
        {
            var selectedProject = _projects.FirstOrDefault(project => project.Name == comboBoxProject.SelectedItem);
            var selectedIssueType = _issueTypes.FirstOrDefault(i => i.Name == comboBoxIssueType.SelectedItem);
            var selectedPriority = _issuePriorities.FirstOrDefault(prio => prio.Name == comboBoxPriority.SelectedItem);
            
            var issue = _jira.CreateIssue(selectedProject.Key);
            issue.Summary = richTextBoxSummary.Text;
            issue.Description = richTextBoxDescription.Text;
            issue.Type = selectedIssueType.Id;
            issue.Priority = selectedPriority.Id;

            issue.SaveChanges();

            issue.AddAttachment(Path.GetFileName(_savedScreenCaptureFilename), File.ReadAllBytes(_savedScreenCaptureFilename));

            issue.SaveChanges();

            Close();
        }

        private void comboBoxProject_SelectionChangeCommitted(object sender, EventArgs e)
        {
            var selectedProject = _projects.FirstOrDefault(project => project.Name == comboBoxProject.SelectedItem);

            _issueTypes = _jira.GetIssueTypes(selectedProject.Key);

            foreach (var issueType in _issueTypes)
            {
                comboBoxIssueType.Items.Add(issueType.Name);
            }
        }
    }
}
