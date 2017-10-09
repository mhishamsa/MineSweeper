using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
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
        int CellsDiscovered;
        bool GameStarted;
        bool Win;
        bool Lose;

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
                GameStarted = false;
                CellsDiscovered = 0;
                Win = false;
                Lose = false;
                SetImageToButton(Start_Button, "../MinesweeperIcons/smiley1.ico");
                Error_Message.Text = "";
                if (int.Parse(Rows_Value.Text) < 2){
                    Rows_Value.Text = "2";
                }
                if (int.Parse(Cols_Value.Text) < 2)
                {
                    Cols_Value.Text = "2";
                }
                MineField = new MineField("Just Another Random Miner!", Convert.ToInt32(Rows_Value.Text), Convert.ToInt32(Cols_Value.Text), Convert.ToInt32(Mines_Value.Text));
                Remaining_Value.Text = MineField.Mines.ToString();
                Time_Value.Text = MineField.Time.ToString();
                MineField.Time++;

                //timer
                timer.Stop();
                timer = new DispatcherTimer();
                timer.Interval = TimeSpan.FromSeconds(1);
                timer.Tick += Timer_Tick;
                //timer.Start();

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
                        CellButton.Checked += new RoutedEventHandler(ToggleButton_Checked);
                        CellButton.MouseRightButtonDown += new MouseButtonEventHandler(ToggleButton_MouseRightButtonDown);
                        CellButton.MouseDoubleClick += new MouseButtonEventHandler(ToggleButton_MouseDoubleClick);
                        CellButton.SetValue(Grid.RowProperty, i);
                        CellButton.SetValue(Grid.ColumnProperty, j);
                        //SetImageToButton(CellButton, "../MinesweeperIcons/tileDark.ico");
                        CellButton.Width = 25;
                        CellButton.Height = 25;
                        FieldGrid.Children.Add(CellButton);
                    }
                }
            }
        }

        private void ToggleButton_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            string[] tokens = ((ToggleButton)sender).Name.Split('X');
            int i = int.Parse(tokens[1]);
            int j = int.Parse(tokens[2]);

            if (MineField.Field[i, j].MinesAround != 0 && !MineField.Field[i, j].Mine && !MineField.Field[i, j].Flagged && !Win && !Lose)
            {
                if (((ToggleButton)FieldGrid.FindName("X" + (i + 1) + "X" + j + "X")) != null)
                {
                    ((ToggleButton)FieldGrid.FindName("X" + (i + 1) + "X" + j + "X")).RaiseEvent(new RoutedEventArgs(ToggleButton.CheckedEvent));
                }
                if (((ToggleButton)FieldGrid.FindName("X" + (i + 1) + "X" + (j + 1) + "X")) != null)
                {
                    ((ToggleButton)FieldGrid.FindName("X" + (i + 1) + "X" + (j + 1) + "X")).RaiseEvent(new RoutedEventArgs(ToggleButton.CheckedEvent));
                }
                if (((ToggleButton)FieldGrid.FindName("X" + (i + 1) + "X" + (j - 1) + "X")) != null)
                {
                    ((ToggleButton)FieldGrid.FindName("X" + (i + 1) + "X" + (j - 1) + "X")).RaiseEvent(new RoutedEventArgs(ToggleButton.CheckedEvent));
                }
                if (((ToggleButton)FieldGrid.FindName("X" + (i - 1) + "X" + j + "X")) != null)
                {
                    ((ToggleButton)FieldGrid.FindName("X" + (i - 1) + "X" + j + "X")).RaiseEvent(new RoutedEventArgs(ToggleButton.CheckedEvent));
                }
                if (((ToggleButton)FieldGrid.FindName("X" + (i - 1) + "X" + (j + 1) + "X")) != null)
                {
                    ((ToggleButton)FieldGrid.FindName("X" + (i - 1) + "X" + (j + 1) + "X")).RaiseEvent(new RoutedEventArgs(ToggleButton.CheckedEvent));
                }
                if (((ToggleButton)FieldGrid.FindName("X" + (i - 1) + "X" + (j - 1) + "X")) != null)
                {
                    ((ToggleButton)FieldGrid.FindName("X" + (i - 1) + "X" + (j - 1) + "X")).RaiseEvent(new RoutedEventArgs(ToggleButton.CheckedEvent));
                }
                if (((ToggleButton)FieldGrid.FindName("X" + i + "X" + (j + 1) + "X")) != null)
                {
                    ((ToggleButton)FieldGrid.FindName("X" + i + "X" + (j + 1) + "X")).RaiseEvent(new RoutedEventArgs(ToggleButton.CheckedEvent));
                }
                if (((ToggleButton)FieldGrid.FindName("X" + i + "X" + (j - 1) + "X")) != null)
                {
                    ((ToggleButton)FieldGrid.FindName("X" + i + "X" + (j - 1) + "X")).RaiseEvent(new RoutedEventArgs(ToggleButton.CheckedEvent));
                }
            }
        }

        private void TextBox_Value_TextChanged(object sender, TextChangedEventArgs e)
        {
            int num;
            bool success = int.TryParse(((TextBox)sender).Text, out num);
            if (((TextBox)sender).Name == "Mines_Value")
            {
                if (success && num < 1)
                {
                    ((TextBox)sender).Text = "1";
                }
                else if (!success)
                {
                    ((TextBox)sender).Text = "";
                }
            }
            else
            {
                if (success)
                {
                    if(num > 30)
                    {
                        ((TextBox)sender).Text = "30";
                    }
                }
                else
                {
                    ((TextBox)sender).Text = "";
                }
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            Time_Value.Text = MineField.Time.ToString();
            MineField.Time++;
        }

        private void ToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            MineField.Clicks++;
            string[] tokens = ((ToggleButton)sender).Name.Split('X');
            int i = int.Parse(tokens[1]);
            int j = int.Parse(tokens[2]);

            if (!GameStarted)
            {
                timer.Start();
                MineField.RandomizeMines(MineField.Field[i, j]);
                MineField.CountCellMines();
                GameStarted = true;
            }

            if (!MineField.Field[i, j].Flagged && !MineField.Field[i, j].Visited && !Win && !Lose)
            {
                if (MineField.Field[i, j].Mine)
                {
                    SetImageToButton((ToggleButton)sender, "../Images/MineWithOutlineRed.png");
                    SetImageToButton(Start_Button, "../MinesweeperIcons/smiley3.ico");
                    ((ToggleButton)sender).IsEnabled = false;
                    Lose = true;
                    DoExplosion(((ToggleButton)sender));
                    timer.Stop();
                    Error_Message.Text = "Mine Field exploded, Game Over!\nYour Score is: " + (int.Parse(Remaining_Value.Text) - MineField.Clicks - MineField.Time);
                    
                }
                else if (!MineField.Field[i, j].Mine && MineField.Field[i, j].Icon != "")
                {
                    CellsDiscovered++;
                    SetImageToButton((ToggleButton)sender, MineField.Field[i, j].Icon);
                    MineField.Field[i, j].Visited = true;
                    //((ToggleButton)sender).IsEnabled = false;
                }
                else
                {
                    ExploreTheEmptinessInYou((ToggleButton)sender);
                }
            }
            else
            {
                ((ToggleButton)sender).IsChecked = false;
            }

            if (CellsDiscovered == ((MineField.Rows * MineField.Cols) - MineField.Mines))
            {
                timer.Stop();
                Win = true;
                SetImageToButton(Start_Button, "../MinesweeperIcons/smiley.ico");
                ExploreMinesSafely();
                Error_Message.Text = "Congrats!! You Won!!\nYour Score is: " + +(int.Parse(Remaining_Value.Text) - MineField.Clicks - MineField.Time);
            }
        }

        private void ExploreTheEmptinessInYou(ToggleButton ClickedCell)
        {
            string[] tokens = ClickedCell.Name.Split('X');
            int Row = int.Parse(tokens[1]);
            int Col = int.Parse(tokens[2]);

            CellsDiscovered++;

            if (MineField.Field[Row, Col].Visited || MineField.Field[Row, Col].Flagged)
            {
                CellsDiscovered--;
                return;
            }
            else if (MineField.Field[Row, Col].MinesAround != 0)
            {
                Image img = new Image();
                img.Source = new BitmapImage(new Uri(MineField.Field[Row, Col].Icon, UriKind.Relative));
                ClickedCell.Content = img;
                //ClickedCell.IsEnabled = false;
                MineField.Field[Row, Col].Visited = true;
                return;
            }
            else if (Row == 0 && Col == 0)
            {
                ClickedCell.IsEnabled = false;
                MineField.Field[Row, Col].Visited = true;
                //SetImageToButton(ClickedCell, "../MinesweeperIcons/tile.ico");
                ExploreTheEmptinessInYou((ToggleButton)FieldGrid.FindName("X" + (Row + 1) + "X" + Col + "X"));
                ExploreTheEmptinessInYou((ToggleButton)FieldGrid.FindName("X" + Row + "X" + (Col + 1) + "X"));
                ExploreTheEmptinessInYou((ToggleButton)FieldGrid.FindName("X" + (Row + 1) + "X" + (Col + 1) + "X"));
                return;
            }
            else if (Row == 0 && Col == MineField.Cols - 1)
            {
                ClickedCell.IsEnabled = false;
                MineField.Field[Row, Col].Visited = true;
                //SetImageToButton(ClickedCell, "../MinesweeperIcons/tile.ico");
                ExploreTheEmptinessInYou((ToggleButton)FieldGrid.FindName("X" + (Row + 1) + "X" + Col + "X"));
                ExploreTheEmptinessInYou((ToggleButton)FieldGrid.FindName("X" + Row + "X" + (Col - 1) + "X"));
                ExploreTheEmptinessInYou((ToggleButton)FieldGrid.FindName("X" + (Row + 1) + "X" + (Col - 1) + "X"));
                return;
            }
            else if (Row == MineField.Rows - 1 && Col == 0)
            {
                ClickedCell.IsEnabled = false;
                MineField.Field[Row, Col].Visited = true;
                //SetImageToButton(ClickedCell, "../MinesweeperIcons/tile.ico");
                ExploreTheEmptinessInYou((ToggleButton)FieldGrid.FindName("X" + (Row - 1) + "X" + Col + "X"));
                ExploreTheEmptinessInYou((ToggleButton)FieldGrid.FindName("X" + Row + "X" + (Col + 1) + "X"));
                ExploreTheEmptinessInYou((ToggleButton)FieldGrid.FindName("X" + (Row - 1) + "X" + (Col + 1) + "X"));
                return;
            }
            else if (Row == MineField.Rows - 1 && Col == MineField.Cols - 1)
            {
                ClickedCell.IsEnabled = false;
                MineField.Field[Row, Col].Visited = true;
                //SetImageToButton(ClickedCell, "../MinesweeperIcons/tile.ico");
                ExploreTheEmptinessInYou((ToggleButton)FieldGrid.FindName("X" + (Row - 1) + "X" + Col + "X"));
                ExploreTheEmptinessInYou((ToggleButton)FieldGrid.FindName("X" + Row + "X" + (Col - 1) + "X"));
                ExploreTheEmptinessInYou((ToggleButton)FieldGrid.FindName("X" + (Row - 1) + "X" + (Col - 1) + "X"));
                return;
            }
            else if (Row == 0 && Col > 0 && Col < MineField.Cols - 1 )
            {
                ClickedCell.IsEnabled = false;
                MineField.Field[Row, Col].Visited = true;
                //SetImageToButton(ClickedCell, "../MinesweeperIcons/tile.ico");
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
                //SetImageToButton(ClickedCell, "../MinesweeperIcons/tile.ico");
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
                //SetImageToButton(ClickedCell, "../MinesweeperIcons/tile.ico");
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
                //SetImageToButton(ClickedCell, "../MinesweeperIcons/tile.ico");
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
                //SetImageToButton(ClickedCell, "../MinesweeperIcons/tile.ico");
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

        private void DoExplosion(ToggleButton explodedCell)
        {
            string[] tokens = explodedCell.Name.Split('X');
            int Row = int.Parse(tokens[1]);
            int Col = int.Parse(tokens[2]);

            int ExplodedMines = 1;

            for (int i = 0; i < MineField.Rows; i++)
            {
                for (int j = 0; j < MineField.Cols; j++)
                {
                    if (MineField.Field[i, j].Mine && (i != Row || j != Col))
                    {
                        ((ToggleButton)FieldGrid.FindName("X" + i + "X" + j + "X")).IsEnabled = false;
                        SetImageToButton((ToggleButton)FieldGrid.FindName("X" + i + "X" + j + "X"), "../Images/MineWithOutline.png");

                        ExplodedMines++;

                        if (ExplodedMines == MineField.Mines)
                        {
                            return;
                        }
                    }
                    else if (MineField.Field[i, j].Flagged)
                    {
                        SetImageToButton((ToggleButton)FieldGrid.FindName("X" + i + "X" + j + "X"), "../Images/MineWithOutlineWrong.png");
                    }
                }
            }
        }

        private void ExploreMinesSafely()
        {   
            int ExploredMines = 0;

            for (int i = 0; i < MineField.Rows; i++)
            {
                for (int j = 0; j < MineField.Cols; j++)
                {
                    if (MineField.Field[i, j].Mine)
                    {
                        MineField.Field[i, j].Flagged = true;
                        SetImageToButton((ToggleButton)FieldGrid.FindName("X" + i + "X" + j + "X"), "../MinesweeperIcons/flag.ico");
                        ExploredMines++;

                        if (ExploredMines == MineField.Mines)
                        {
                            return;
                        }
                    }
                }
            }
        }

        private void ToggleButton_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            string[] tokens = ((ToggleButton)sender).Name.Split('X');
            int Row = int.Parse(tokens[1]);
            int Col = int.Parse(tokens[2]);

            if (!GameStarted)
            {
                timer.Start();
                MineField.RandomizeMines();
                MineField.CountCellMines();
                GameStarted = true;
            }

            if (!MineField.Field[Row, Col].Visited && !Win && !Lose)
            {
                if (!MineField.Field[Row, Col].Flagged)
                {
                    if (int.Parse(Remaining_Value.Text) != 0)
                    {
                        SetImageToButton((ToggleButton)sender, "../MinesweeperIcons/flag.ico");
                        MineField.Field[Row, Col].Flagged = true;
                        Remaining_Value.Text = (int.Parse(Remaining_Value.Text) - 1).ToString();
                    }
                    else
                    {
                        Error_Message.Text = "You're out of flags!";
                    }
                }
                else
                {
                    //SetImageToButton((ToggleButton)sender, "../MinesweeperIcons/tileDark.ico");
                    ((ToggleButton)sender).Content = null;
                    MineField.Field[Row, Col].Flagged = false;
                    Remaining_Value.Text = (int.Parse(Remaining_Value.Text) + 1).ToString();
                    Error_Message.Text = "";
                }
            }
        }

        private void ToggleButton_MouseLeftButtonUp(object sender, RoutedEventArgs e)
        {
            SetImageToButton(Start_Button, "../MinesweeperIcons/smiley1.ico");
        }

        private void SetImageToButton(ToggleButton button, string path)
        {
            Image img = new Image();
            img.Source = new BitmapImage(new Uri(path, UriKind.Relative));
            button.Content = img;
        }

        private void SetImageToButton(Button button, string path)
        {
            Image img = new Image();
            img.Source = new BitmapImage(new Uri(path, UriKind.Relative));
            button.Content = img;
        }
    }
}