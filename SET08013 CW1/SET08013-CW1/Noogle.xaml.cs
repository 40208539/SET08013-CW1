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

        public Noogle()
        {
            InitializeComponent();
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            processor.InputMessage(txtInputMessage.Text);
            txtInputMessage.Clear();
        }
    }
}
