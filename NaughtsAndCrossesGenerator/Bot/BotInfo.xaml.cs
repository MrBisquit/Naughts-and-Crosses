using Ookii.Dialogs.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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

namespace NaughtsAndCrossesGenerator.Bot
{
    /// <summary>
    /// Interaction logic for BotInfo.xaml
    /// </summary>
    public partial class BotInfo : Window
    {
        public BotInfo()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            TaskDialog td = new TaskDialog()
            {
                WindowTitle = "Naughts and Crosses Bot",
                Content = "Would you like to load an existing bot training file or create a new one and start from scratch?",
                ButtonStyle = TaskDialogButtonStyle.CommandLinks
            };

            TaskDialogButton existing = new TaskDialogButton()
            {
                Text = "Existing training file"
            };

            TaskDialogButton create = new TaskDialogButton()
            {
                Text = "Create a new training file"
            };

            td.Buttons.Add(existing);
            td.Buttons.Add(create);

            TaskDialogButton result = td.ShowDialog();

            if(result == existing)
            {
                VistaOpenFileDialog ofd = new VistaOpenFileDialog();
                ofd.Filter = "*.json | (JSON File)";
                ofd.ShowDialog();
            } else if(result == create)
            {
                VistaSaveFileDialog sfd = new VistaSaveFileDialog();
                sfd.Filter = "*.json | (JSON File)";
                sfd.ShowDialog();
            }
        }
    }
}
