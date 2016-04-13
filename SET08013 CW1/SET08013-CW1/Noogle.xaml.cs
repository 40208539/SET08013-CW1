using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SET08013_CW1
{
    /// <summary>
    /// Interaction logic for Noogle.xaml
    /// </summary>
    public partial class Noogle : Window
    {
        MessageProcessor processor = new MessageProcessor();
        List<Message> applications = new List<Message>();
        List<string> quarantinedMessages = new List<string>();

        public Noogle()
        {
            InitializeComponent();
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            processor.InputMessage(txtInputMessage.Text);
            txtInputMessage.Clear();
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            lstApplications.Items.Clear();
            processor.ProcessValidMessages();
            applications = processor.GetApplications(); 
            int id = 0;
            foreach (Message m in applications)
            {
                lstApplications.Items.Add("#" + id);
                id++;
            }
        }

        private void lstApplications_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = lstApplications.SelectedIndex;
            if (index != -1)
            {
                Message application = applications[index];
                txtMessage.Clear();
                txtLevel.Clear();
                txtMessage.Text = application.Body;
                txtLevel.Text = application.level;
                lstUni.Items.Clear();
                lstSubjects.Items.Clear();
                foreach (string university in application.Universities)
                {
                    lstUni.Items.Add(university);
                }
                foreach (string subject in application.Subjects)
                {
                    lstSubjects.Items.Add(subject);
                }
            }
        }

        private void btnQuarUpdate_Click(object sender, RoutedEventArgs e)
        {
            lstQuarantine.Items.Clear();
            quarantinedMessages = processor.GetQuarantinedMessages();
            int id = 0;
            foreach(string message in quarantinedMessages)
            {
                lstQuarantine.Items.Add("#" + id);
                id++;
            }
        }

        private void lstQuarantine_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            txtQuarMessage.Clear();
            int index = lstQuarantine.SelectedIndex;
            if (index != -1)
            {
                txtQuarMessage.Text = quarantinedMessages[index];
            }
        }
    }
}
