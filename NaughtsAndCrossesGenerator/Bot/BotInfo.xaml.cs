using Newtonsoft.Json;
using Ookii.Dialogs.Wpf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.Marshalling;
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

        string saveLocation = string.Empty;

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
                LoadExisting();
            } else if(result == create)
            {
                CreateNew();
            }
        }

        private void LoadExisting()
        {
            VistaOpenFileDialog ofd = new VistaOpenFileDialog();
            ofd.Filter = "JSON File (*.json)|*.json";
            ofd.ShowDialog();

            if(File.Exists(ofd.FileName))
            {
                saveLocation = ofd.FileName;

                Global.mainWindow.bot = JsonConvert.DeserializeObject<Bot>(File.ReadAllText(ofd.FileName));
            } else
            {
                MessageBox.Show("Invalid file location.");
                Global.mainWindow.PlayAgainstBot.IsChecked = false;
                Global.mainWindow.isUsingBot = false;
                Global.mainWindow.bot = null;
                Global.mainWindow.botInfo.Close();
            }
        }
        private void CreateNew()
        {
            VistaSaveFileDialog sfd = new VistaSaveFileDialog();
            sfd.Filter = "JSON File (*.json)|*.json";
            sfd.ShowDialog();

            if (!sfd.FileName.EndsWith(".json")) sfd.FileName += ".json";

            Global.mainWindow.bot = new Bot();
            //File.WriteAllText(sfd.FileName, JsonConvert.SerializeObject(Global.mainWindow.bot));

            saveLocation = sfd.FileName;

            Save();
        }

        public void Save()
        {
            Debug.WriteLine("Save called");
            if(saveLocation != "")
            {
                File.WriteAllText(saveLocation, JsonConvert.SerializeObject(Global.mainWindow.bot,
                    Global.IndentJSONFiles ? Formatting.Indented : Formatting.None));
                Debug.WriteLine("Saved");
            }
        }
    }
}
