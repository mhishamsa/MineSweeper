using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace MineSweeper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MineField MineField;
        DispatcherTimer timer = new DispatcherTimer();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Start_Button_Click(object sender, RoutedEventArgs e)
        {

            if (Rows_Value.Text == "" || Cols_Value.Text == "" || Mines_Value.Text == "")
            {
                Error_Message.Text = "Please enter all numbers.";
            }
            else if (Convert.ToInt32(Mines_Value.Text) > Convert.ToInt32(Rows_Value.Text) * Convert.ToInt32(Cols_Value.Text))
            {
                Error_Message.Text = "Mines shouldnt be more than number of cells.";
            }
            else
            {
                Error_Message.Text = "";
                MineField = new MineField("Just Another Random Miner!", Convert.ToInt32(Rows_Value.Text), Convert.ToInt32(Cols_Value.Text), Convert.ToInt32(Mines_Value.Text));
                Remaining_Value.Text = MineField.Mines.ToString();
                Time_Value.Text = MineField.Time.ToString();
                MineField.Time++;

                //timer
                timer.Stop();
                timer = new DispatcherTimer();
                timer.Interval = TimeSpan.FromSeconds(1);
                timer.Tick += Timer_Tick;
                timer.Start();

                FieldGrid.RowDefinitions.Clear();
                FieldGrid.ColumnDefinitions.Clear();
                FieldGrid.Children.Clear();
                for (int i = 0; i < Convert.ToInt32(Rows_Value.Text); i++)
                {
                    FieldGrid.RowDefinitions.Add(new RowDefinition());
                    for (int j = 0; j < Convert.ToInt32(Cols_Value.Text); j++)
                    {
                        if (i == 0)
                        {
                            FieldGrid.ColumnDefinitions.Add(new ColumnDefinition());
                        }
                        ToggleButton CellButton = new ToggleButton();
                        object o = FieldGrid.FindName("X" + i + "X" + j + "X");
                        if (o != null)
                        {
                            FieldGrid.UnregisterName("X" + i + "X" + j + "X");
                        }
                        CellButton.Name = "X" + i + "X" + j + "X";              //Name format for buttons will be "XRowXColumnX"
                        FieldGrid.RegisterName(CellButton.Name, CellButton);
                        CellButton.MouseRightButtonDown += new MouseButtonEventHandler(ToggleButton_MouseRightButtonDown);
                        CellButton.Checked += new RoutedEventHandler(ToggleButton_Checked);
                        CellButton.SetValue(Grid.RowProperty, i);
                        CellButton.SetValue(Grid.ColumnProperty, j);
                        CellButton.Width = 25;
                        CellButton.Height = 25;
                        FieldGrid.Children.Add(CellButton);
                    }
                }
            }
        }

        private void TextBox_Value_TextChanged(object sender, TextChangedEventArgs e)
        {
            int num;
            string previousText = "";
            bool success = int.TryParse(((TextBox)sender).Text, out num);
            if (((TextBox)sender).Name == "Mines_Value")
            {
                if (success)
                    previousText = ((TextBox)sender).Text;
                else
                    ((TextBox)sender).Text = previousText;
            }
            else
            {
                if (success && num <= 30)
                    previousText = ((TextBox)sender).Text;
                else
                    ((TextBox)sender).Text = previousText;
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            Time_Value.Text = MineField.Time.ToString();
            MineField.Time++;
        }

        private void ToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            string[] tokens = ((ToggleButton)sender).Name.Split('X');
            int i = int.Parse(tokens[1]);
            int j = int.Parse(tokens[2]);

            if (!MineField.Field[i, j].Flagged)
            {
                Image img = new Image();
                if (MineField.Field[i, j].Mine)
                {
                    MineField.Clicks++;
                    img.Source = new BitmapImage(new Uri("../MinesweeperIcons/mine.ico", UriKind.Relative));
                    ((ToggleButton)sender).Content = img;
                    ((ToggleButton)sender).IsEnabled = false;
                    DoExplosion(((ToggleButton)sender));
                    timer.Stop();
                    Error_Message.Text = "Mine Field exploded, Game Over!\nYour Score is: " + (int.Parse(Remaining_Value.Text) - MineField.Clicks - MineField.Time);
                    
                }
                else if (!MineField.Field[i, j].Mine && MineField.Field[i, j].Icon != "")
                {
                    MineField.Clicks++;
                    img.Source = new BitmapImage(new Uri(MineField.Field[i, j].Icon, UriKind.Relative));
                    ((ToggleButton)sender).Content = img;
                    ((ToggleButton)sender).IsEnabled = false;
                }
                else
                {
                    MineField.Clicks++;
                    ExploreTheEmptinessInYou((ToggleButton)sender);
                }
            }
            else
            {
                ((ToggleButton)sender).IsChecked = false;
            }
        }

        private void ExploreTheEmptinessInYou(ToggleButton ClickedCell)
        {
            string[] tokens = ClickedCell.Name.Split('X');
            int Row = int.Parse(tokens[1]);
            int Col = int.Parse(tokens[2]);

            if (!ClickedCell.IsEnabled || MineField.Field[Row, Col].Flagged)
            {
                return;
            }
            else if (MineField.Field[Row, Col].MinesAround != 0)
            {
                Image img = new Image();
                img.Source = new BitmapImage(new Uri(MineField.Field[Row, Col].Icon, UriKind.Relative));
                ClickedCell.Content = img;
                ClickedCell.IsEnabled = false;
                MineField.Field[Row, Col].Visited = true;
                return;
            }
            else if (Row == 0 && Col == 0)
            {
                ClickedCell.IsEnabled = false;
                MineField.Field[Row, Col].Visited = true;
                ExploreTheEmptinessInYou((ToggleButton)FieldGrid.FindName("X" + (Row + 1) + "X" + Col + "X"));
                ExploreTheEmptinessInYou((ToggleButton)FieldGrid.FindName("X" + Row + "X" + (Col + 1) + "X"));
                ExploreTheEmptinessInYou((ToggleButton)FieldGrid.FindName("X" + (Row + 1) + "X" + (Col + 1) + "X"));
                return;
            }
            else if (Row == 0 && Col == MineField.Cols - 1)
            {
                ClickedCell.IsEnabled = false;
                MineField.Field[Row, Col].Visited = true;
                ExploreTheEmptinessInYou((ToggleButton)FieldGrid.FindName("X" + (Row + 1) + "X" + Col + "X"));
                ExploreTheEmptinessInYou((ToggleButton)FieldGrid.FindName("X" + Row + "X" + (Col - 1) + "X"));
                ExploreTheEmptinessInYou((ToggleButton)FieldGrid.FindName("X" + (Row + 1) + "X" + (Col - 1) + "X"));
                return;
            }
            else if (Row == MineField.Rows - 1 && Col == 0)
            {
                ClickedCell.IsEnabled = false;
                MineField.Field[Row, Col].Visited = true;
                ExploreTheEmptinessInYou((ToggleButton)FieldGrid.FindName("X" + (Row - 1) + "X" + Col + "X"));
                ExploreTheEmptinessInYou((ToggleButton)FieldGrid.FindName("X" + Row + "X" + (Col + 1) + "X"));
                ExploreTheEmptinessInYou((ToggleButton)FieldGrid.FindName("X" + (Row - 1) + "X" + (Col + 1) + "X"));
                return;
            }
            else if (Row == MineField.Rows - 1 && Col == MineField.Cols - 1)
            {
                ClickedCell.IsEnabled = false;
                MineField.Field[Row, Col].Visited = true;
                ExploreTheEmptinessInYou((ToggleButton)FieldGrid.FindName("X" + (Row - 1) + "X" + Col + "X"));
                ExploreTheEmptinessInYou((ToggleButton)FieldGrid.FindName("X" + Row + "X" + (Col - 1) + "X"));
                ExploreTheEmptinessInYou((ToggleButton)FieldGrid.FindName("X" + (Row + 1) + "X" + (Col - 1) + "X"));
                return;
            }
            else if (Row == 0 && Col > 0 && Col < MineField.Cols - 1 )
            {
                ClickedCell.IsEnabled = false;
                MineField.Field[Row, Col].Visited = true;
                ExploreTheEmptinessInYou((ToggleButton)FieldGrid.FindName("X" + Row + "X" + (Col - 1) + "X"));
                ExploreTheEmptinessInYou((ToggleButton)FieldGrid.FindName("X" + Row + "X" + (Col + 1) + "X"));
                ExploreTheEmptinessInYou((ToggleButton)FieldGrid.FindName("X" + (Row + 1) + "X" + Col + "X"));
                ExploreTheEmptinessInYou((ToggleButton)FieldGrid.FindName("X" + (Row + 1) + "X" + (Col - 1) + "X"));
                ExploreTheEmptinessInYou((ToggleButton)FieldGrid.FindName("X" + (Row + 1) + "X" + (Col + 1) + "X"));
                return;
            }
            else if (Row == MineField.Rows - 1 && Col > 0 && Col < MineField.Cols - 1)
            {
                ClickedCell.IsEnabled = false;
                MineField.Field[Row, Col].Visited = true;
                ExploreTheEmptinessInYou((ToggleButton)FieldGrid.FindName("X" + Row + "X" + (Col - 1) + "X"));
                ExploreTheEmptinessInYou((ToggleButton)FieldGrid.FindName("X" + Row + "X" + (Col + 1) + "X"));
                ExploreTheEmptinessInYou((ToggleButton)FieldGrid.FindName("X" + (Row - 1) + "X" + Col + "X"));
                ExploreTheEmptinessInYou((ToggleButton)FieldGrid.FindName("X" + (Row - 1) + "X" + (Col + 1) + "X"));
                ExploreTheEmptinessInYou((ToggleButton)FieldGrid.FindName("X" + (Row - 1) + "X" + (Col - 1) + "X"));
                return;
            }
            else if (Col == 0 && Row > 0 && Row < MineField.Rows - 1)
            {
                ClickedCell.IsEnabled = false;
                MineField.Field[Row, Col].Visited = true;
                ExploreTheEmptinessInYou((ToggleButton)FieldGrid.FindName("X" + (Row + 1) + "X" + Col + "X"));
                ExploreTheEmptinessInYou((ToggleButton)FieldGrid.FindName("X" + (Row - 1) + "X" + Col + "X"));
                ExploreTheEmptinessInYou((ToggleButton)FieldGrid.FindName("X" + Row + "X" + (Col + 1) + "X"));
                ExploreTheEmptinessInYou((ToggleButton)FieldGrid.FindName("X" + (Row + 1) + "X" + (Col + 1) + "X"));
                ExploreTheEmptinessInYou((ToggleButton)FieldGrid.FindName("X" + (Row - 1) + "X" + (Col + 1) + "X"));
                return;
            }
            else if (Col == MineField.Cols - 1 && Row > 0 && Row < MineField.Rows - 1)
            {
                ClickedCell.IsEnabled = false;
                MineField.Field[Row, Col].Visited = true;
                ExploreTheEmptinessInYou((ToggleButton)FieldGrid.FindName("X" + (Row + 1) + "X" + Col + "X"));
                ExploreTheEmptinessInYou((ToggleButton)FieldGrid.FindName("X" + (Row - 1) + "X" + Col + "X"));
                ExploreTheEmptinessInYou((ToggleButton)FieldGrid.FindName("X" + Row + "X" + (Col - 1) + "X"));
                ExploreTheEmptinessInYou((ToggleButton)FieldGrid.FindName("X" + (Row + 1) + "X" + (Col - 1) + "X"));
                ExploreTheEmptinessInYou((ToggleButton)FieldGrid.FindName("X" + (Row - 1) + "X" + (Col - 1) + "X"));
                return;
            }
            else if (Row > 0 && Col > 0 && Row < MineField.Rows - 1 && Col < MineField.Cols - 1)
            {
                ClickedCell.IsEnabled = false;
                MineField.Field[Row, Col].Visited = true;
                ExploreTheEmptinessInYou((ToggleButton)FieldGrid.FindName("X" + Row + "X" + (Col + 1) + "X"));
                ExploreTheEmptinessInYou((ToggleButton)FieldGrid.FindName("X" + Row + "X" + (Col - 1) + "X"));
                ExploreTheEmptinessInYou((ToggleButton)FieldGrid.FindName("X" + (Row + 1) + "X" + Col + "X"));
                ExploreTheEmptinessInYou((ToggleButton)FieldGrid.FindName("X" + (Row - 1) + "X" + Col + "X"));
                ExploreTheEmptinessInYou((ToggleButton)FieldGrid.FindName("X" + (Row + 1) + "X" + (Col + 1) + "X"));
                ExploreTheEmptinessInYou((ToggleButton)FieldGrid.FindName("X" + (Row + 1) + "X" + (Col - 1) + "X"));
                ExploreTheEmptinessInYou((ToggleButton)FieldGrid.FindName("X" + (Row - 1) + "X" + (Col - 1) + "X"));
                ExploreTheEmptinessInYou((ToggleButton)FieldGrid.FindName("X" + (Row - 1) + "X" + (Col + 1) + "X"));
                return;
            }
        }

        private void DoExplosion(ToggleButton ExplodedCell)
        {
            string[] tokens = ExplodedCell.Name.Split('X');
            int Row = int.Parse(tokens[1]);
            int Col = int.Parse(tokens[2]);

            int ExplodedMines = 1;

            for (int i = 0; i < MineField.Rows; i++)
            {
                for (int j = 0; j < MineField.Cols; j++)
                {
                    if (MineField.Field[i, j].Mine && (i != Row || j != Col))
                    {
                        MineField.Field[Row, Col].Visited = true;
                        ((ToggleButton)FieldGrid.FindName("X" + i + "X" + j + "X")).IsEnabled = false;
                        Image img = new Image();
                        img.Source = new BitmapImage(new Uri("../MinesweeperIcons/mine2.ico", UriKind.Relative));
                        ((ToggleButton)FieldGrid.FindName("X" + i + "X" + j + "X")).Content = img;
                        ExplodedMines++;

                        //if (ExplodedMines == MineField.Mines)
                        //{
                        //    return;
                        //}
                    }
                    else if (MineField.Field[i, j].Flagged)
                    {
                        Image img = new Image();
                        img.Source = new BitmapImage(new Uri("../MinesweeperIcons/mine3.ico", UriKind.Relative));
                        ((ToggleButton)FieldGrid.FindName("X" + i + "X" + j + "X")).Content = img;
                    }
                }
            }
        }

        private void ToggleButton_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            string[] tokens = ((ToggleButton)sender).Name.Split('X');
            int Row = int.Parse(tokens[1]);
            int Col = int.Parse(tokens[2]);

            if (((ToggleButton)sender).Content == null)
            {
                Image img = new Image();
                img.Source = new BitmapImage(new Uri("../MinesweeperIcons/tile3.ico", UriKind.Relative));
                ((ToggleButton)sender).Content = img;
                MineField.Field[Row, Col].Flagged = true;
                Remaining_Value.Text = (int.Parse(Remaining_Value.Text) - 1).ToString();
            }
            else
            {
                ((ToggleButton)sender).Content = null;
                MineField.Field[Row, Col].Flagged = false;
                Remaining_Value.Text = (int.Parse(Remaining_Value.Text) + 1).ToString();
            }
        }
    }
}