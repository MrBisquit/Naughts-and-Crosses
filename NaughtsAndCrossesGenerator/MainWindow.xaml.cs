using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NaughtsAndCrossesGenerator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        /**
         * \_ | \_ | \_
           -------
           \_ | \_ | \_
           -------
           \_ | \_ | \_
        */

        int[,] figures = new int[3, 3] { { 0, 0, 0 }, { 0, 0, 0 }, { 0, 0, 0 } };
        int turn = 0; // 0 = o, 1 = x, 2 = none
        char[] chars = { '_', 'o', 'x' };
        int[] lastMove = { 0, 0 };
        bool started = false;

        bool isUsingBot = false;
        public Bot.Bot bot;
        Bot.BotInfo botInfo;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            /*DiscordCopyAndPaste.Text = @"\_ | \_ | \_" +
                "\n-------\n" +
                @"\_ | \_ | \_" +
                "\n-------\n" +
                @"\_ | \_ | \_";*/

            //DiscordCopyAndPaste.Text = WorkOutString();
            Update();
            Stop();
        }

        private void Start()
        {
            started = true;

            Update();

            RestartBtn.IsEnabled = true;
        }

        private void Restart()
        {
            figures = new int[3, 3] { { 0, 0, 0 }, { 0, 0, 0 }, { 0, 0, 0 } };
            turn = 0;
            lastMove  = new int[2] { 0, 0 };
            ClearButtons();
            Start();
        }

        private void Stop()
        {
            A1.IsEnabled = false;
            A2.IsEnabled = false;
            A3.IsEnabled = false;

            B1.IsEnabled = false;
            B2.IsEnabled = false;
            B3.IsEnabled = false;

            C1.IsEnabled = false;
            C2.IsEnabled = false;
            C3.IsEnabled = false;

            RestartBtn.IsEnabled = false;
            started = false;
        }

        private void SetWinner(int winner)
        {
            Stop();
            Winner.Text = (winner == 0 ? 'o' : 'x').ToString();
            MessageBox.Show($"Winner: {(winner == 0 ? 'o' : winner == 1 ? 'x' : "No winner? 👍")}");
        }

        private void ClearButtons()
        {
            A1.Background = new SolidColorBrush(Color.FromRgb(221, 221, 221));
            A2.Background = new SolidColorBrush(Color.FromRgb(221, 221, 221));
            A3.Background = new SolidColorBrush(Color.FromRgb(221, 221, 221));

            B1.Background = new SolidColorBrush(Color.FromRgb(221, 221, 221));
            B2.Background = new SolidColorBrush(Color.FromRgb(221, 221, 221));
            B3.Background = new SolidColorBrush(Color.FromRgb(221, 221, 221));

            C1.Background = new SolidColorBrush(Color.FromRgb(221, 221, 221));
            C2.Background = new SolidColorBrush(Color.FromRgb(221, 221, 221));
            C3.Background = new SolidColorBrush(Color.FromRgb(221, 221, 221));
        }

        private string WorkOutString()
        {
            string firstRow = $"{(figures[0, 0] == 0 ? @"\_" : figures[0, 0] == 1 ? 'o' : 'x')} | " +
                $"{(figures[0, 1] == 0 ? @"\_" : figures[0, 1] == 1 ? 'o' : 'x')} | " +
                $"{(figures[0, 2] == 0 ? @"\_" : figures[0, 2] == 1 ? 'o' : 'x')}";

            string secondRow = $"{(figures[1, 0] == 0 ? @"\_" : figures[1, 0] == 1 ? 'o' : 'x')} | " +
                $"{(figures[1, 1] == 0 ? @"\_" : figures[1, 1] == 1 ? 'o' : 'x')} | " +
                $"{(figures[1, 2] == 0 ? @"\_" : figures[1, 2] == 1 ? 'o' : 'x')}";

            string thirdRow = $"{(figures[2, 0] == 0 ? @"\_" : figures[2, 0] == 1 ? 'o' : 'x')} | " +
                $"{(figures[2, 1] == 0 ? @"\_" : figures[2, 1] == 1 ? 'o' : 'x')} | " +
                $"{(figures[2, 2] == 0 ? @"\_" : figures[2, 2] == 1 ? 'o' : 'x')}";

            return $"{firstRow}\n-------\n{secondRow}\n-------\n{thirdRow}";
        }

        private void RegisterTurn(int x, int y)
        {
            if(turn == 0)
            {
                figures[x, y] = 1;
                turn = 1;
            } else if(turn == 1)
            {
                figures[x, y] = 2;
                turn = 0;
            } else
            {

            }

            lastMove[0] = x;
            lastMove[1] = y;

            Update();
        }

        private void Update()
        {
            DiscordCopyAndPaste.Text = WorkOutString();

            // Disable buttons when they are clicked
            A1.IsEnabled = figures[0, 0] == 0;
            A2.IsEnabled = figures[0, 1] == 0;
            A3.IsEnabled = figures[0, 2] == 0;

            B1.IsEnabled = figures[1, 0] == 0;
            B2.IsEnabled = figures[1, 1] == 0;
            B3.IsEnabled = figures[1, 2] == 0;

            C1.IsEnabled = figures[2, 0] == 0;
            C2.IsEnabled = figures[2, 1] == 0;
            C3.IsEnabled = figures[2, 2] == 0;

            // Update the text inside of them
            A1.Content = chars[figures[0, 0]];
            A2.Content = chars[figures[0, 1]];
            A3.Content = chars[figures[0, 2]];

            B1.Content = chars[figures[1, 0]];
            B2.Content = chars[figures[1, 1]];
            B3.Content = chars[figures[1, 2]];

            C1.Content = chars[figures[2, 0]];
            C2.Content = chars[figures[2, 1]];
            C3.Content = chars[figures[2, 2]];

            // Figure out if there are any winners
            // Columns
            int colA = CheckRow(figures[0, 0], figures[0, 1], figures[0, 2]);
            int colB = CheckRow(figures[1, 0], figures[1, 1], figures[1, 2]);
            int colC = CheckRow(figures[2, 0], figures[2, 1], figures[2, 2]);

            // Diagonal
            int rowAC = CheckRow(figures[0, 0], figures[1, 1], figures[2, 2]);
            int rowCA = CheckRow(figures[0, 2], figures[1, 1], figures[2, 0]);

            // Rows
            int rowA = CheckRow(figures[0, 0], figures[1, 0], figures[2, 0]);
            int rowB = CheckRow(figures[0, 1], figures[1, 1], figures[2, 1]);
            int rowC = CheckRow(figures[0, 2], figures[1, 2], figures[2, 2]);

            if(colA != 2)
            {
                A1.Background = new SolidColorBrush(Colors.Green);
                B1.Background = new SolidColorBrush(Colors.Green);
                C1.Background = new SolidColorBrush(Colors.Green);

                SetWinner(colA);
            }

            if (colB != 2)
            {
                A2.Background = new SolidColorBrush(Colors.Green);
                B2.Background = new SolidColorBrush(Colors.Green);
                C2.Background = new SolidColorBrush(Colors.Green);

                SetWinner(colB);
            }

            if (colC != 2)
            {
                A3.Background = new SolidColorBrush(Colors.Green);
                B3.Background = new SolidColorBrush(Colors.Green);
                C3.Background = new SolidColorBrush(Colors.Green);

                SetWinner(colC);
            }

            if (rowAC != 2)
            {
                A1.Background = new SolidColorBrush(Colors.Green);
                B1.Background = new SolidColorBrush(Colors.Green);
                C1.Background = new SolidColorBrush(Colors.Green);

                SetWinner(rowAC);
            }

            if (rowCA != 2)
            {
                A3.Background = new SolidColorBrush(Colors.Green);
                B2.Background = new SolidColorBrush(Colors.Green);
                C1.Background = new SolidColorBrush(Colors.Green);

                SetWinner(rowCA);
            }

            if (rowA != 2)
            {
                A1.Background = new SolidColorBrush(Colors.Green);
                A2.Background = new SolidColorBrush(Colors.Green);
                A3.Background = new SolidColorBrush(Colors.Green);

                SetWinner(rowA);
            }

            if (rowB != 2)
            {
                B1.Background = new SolidColorBrush(Colors.Green);
                B2.Background = new SolidColorBrush(Colors.Green);
                B3.Background = new SolidColorBrush(Colors.Green);

                SetWinner(rowB);
            }

            if (rowC != 2)
            {
                C1.Background = new SolidColorBrush(Colors.Green);
                C2.Background = new SolidColorBrush(Colors.Green);
                C3.Background = new SolidColorBrush(Colors.Green);

                SetWinner(rowC);
            }

            Turn.Text = (turn == 0 ? 'o' : 'x').ToString();
            if(started) LastMove.Text = $"{(lastMove[0] == 0 ? 'A' : lastMove[0] == 1 ? 'B' : 'C')}{lastMove[1]}";

            CA.Text = colA != 2 ? "Yes" : "No";
            CB.Text = colB != 2 ? "Yes" : "No";
            CC.Text = colC != 2 ? "Yes" : "No";

            RA.Text = rowA != 2 ? "Yes" : "No";
            RB.Text = rowB != 2 ? "Yes" : "No";
            RC.Text = rowC != 2 ? "Yes" : "No";

            DA.Text = rowAC != 2 ? "Yes" : "No";
            DB.Text = rowCA != 2 ? "Yes" : "No";

            bool tie = true;
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    if (figures[x, y] != 0) tie = false;
                }
            }

            Tie.Text = tie ? "Yes" : "No";
        }

        private int CheckRow(int a, int b, int c)
        {
            if(a == b && b == c && a == c && a != 0 && b != 0 && c != 0) return a;
            return 2;
        }

        private void A1_Click(object sender, RoutedEventArgs e)
        {
            RegisterTurn(0, 0);
        }

        private void A2_Click(object sender, RoutedEventArgs e)
        {
            RegisterTurn(0, 1);
        }

        private void A3_Click(object sender, RoutedEventArgs e)
        {
            RegisterTurn(0, 2);
        }

        private void B1_Click(object sender, RoutedEventArgs e)
        {
            RegisterTurn(1, 0);
        }

        private void B2_Click(object sender, RoutedEventArgs e)
        {
            RegisterTurn(1, 1);
        }

        private void B3_Click(object sender, RoutedEventArgs e)
        {
            RegisterTurn(1, 2);
        }

        private void C1_Click(object sender, RoutedEventArgs e)
        {
            RegisterTurn(2, 0);
        }

        private void C2_Click(object sender, RoutedEventArgs e)
        {
            RegisterTurn(2, 1);
        }

        private void C3_Click(object sender, RoutedEventArgs e)
        {
            RegisterTurn(2, 2);
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            Start();
        }

        private void Restart_Click(object sender, RoutedEventArgs e)
        {
            Restart();
        }

        private void PlayAgainstBot_Click(object sender, RoutedEventArgs e)
        {
            if (PlayAgainstBot.IsChecked == true)
            {
                MessageBox.Show("The bot is constantly trained while you are playing, so hopefully it " +
                    "will become better over time.\n\n" +
                    "A window will now open, this will display information about the bot, " +
                    "You will need to open (Or save) a bot file for it to work.");

                botInfo = new Bot.BotInfo();
                botInfo.Show();
            } else
            {
                /* Close Bot Window */
                if (botInfo != null) botInfo.Close();
            }
        }
    }
}