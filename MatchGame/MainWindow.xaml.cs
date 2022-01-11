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
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace MatchGame
{
    using System.Windows.Threading;


    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer timer = new DispatcherTimer();
        int tenthsOfSecondsElapsed;
        int matchesFound;
        string result;

        public MainWindow()
        {
            InitializeComponent();

            timer.Interval = TimeSpan.FromSeconds(.1);
            timer.Tick += Timer_Tick;
            SetUpGame();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {

            tenthsOfSecondsElapsed++;
            timeTextBlock.Text = (tenthsOfSecondsElapsed / 10F).ToString("0.0 s");
            if (matchesFound == 8)
            {
                timer.Stop();
                result = timeTextBlock.Text;
                char[] charsToTrim = {'B', 'e', 's', 't', ' ', 't', 'i', 'm', 'e', 's'};
                char[] charsToTrim2 = { ' ','s' };
                timeTextBlock.Text = timeTextBlock.Text + " - Play again?";

                if (bestTimeTextBlock.Text == "")
                {
                    bestTimeTextBlock.Text = "Best time" + result;
                }

                else if (Convert.ToDecimal(bestTimeTextBlock.Text.Trim(charsToTrim)) > Convert.ToDecimal(result.Trim(charsToTrim2)))
                {
                    bestTimeTextBlock.Text = "Best time" + result;
                }

            }
        }

        private void SetUpGame()

        {
            List<string> lettersFullList = new List<string>() // List of pairs of letters
            {
            "A","A",
            "B","B",
            "C","C",
            "D","D",
            "E","E",
            "F","F",
            "G","G",
            "H","H",
            "I","I",
            "J","J",
            "K","K",
            "L","L",
            "M","M",
            };

            List<string> letters = new List<string>(); // new list. This is the place for only 8 pairs of letters

            Random randomValue = new Random(); // Create a new random number generator

            for (int i = 0; i < 8; i++) // We need only 8 random pairs of letters
            {
                int index = randomValue.Next(lettersFullList.Count); //Pick a random number between 0 and the number of letters left in the full list and call it "index"
                string newValue = lettersFullList[index]; // Use the random number called "index" to get a random letter from the list
                letters.Add(newValue); //Add a letter to a new list
                lettersFullList.Remove(newValue); // Delete this letter from the old list

                if (lettersFullList.Contains(newValue)) //Check if there is another occurence of the letter in the full list
                {
                    letters.Add(newValue); // If YES add this occurence to a new list
                    lettersFullList.Remove(newValue); //...and delete this occurence from the older list
                }
            }


            Random random = new Random(); // Create a new random number generator

            foreach (TextBlock textBlock in mainGrid.Children.OfType<TextBlock>()) // Find every TextBlock in the main grid and repeat the following statements for each of them
                
            {
                if (textBlock.Name != "timeTextBlock" && textBlock.Name != "bestTimeTextBlock")
                {
                    textBlock.Visibility = Visibility.Visible;
                    int index = random.Next(letters.Count); //Pick a random number between 0 and the number of letters left in the list and call it "index"
                    string nextLetter = letters[index]; // Use the random number called "index" to get a random letter from the list
                    textBlock.Text = nextLetter; // Update the TextBlock with the random letter from the list
                    letters.RemoveAt(index); // Remove the random letter from the list
                }

            }

            timer.Start();
            tenthsOfSecondsElapsed = 0;
            matchesFound = 0;
        }

        TextBlock lastTextBlockClicked;
        bool findingMatch = false;


        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            /* If it's the first in the pair being clicked, keep track of which TextBlock
            * was clicked and make the animal disappear. 
            * If it's the second one, either make it disappear (if it's a match) or
            * bring back the first one (if it's not).
            */

            TextBlock textBlock = sender as TextBlock;
            
            if (findingMatch == false)
            {
                textBlock.Visibility = Visibility.Hidden;
                lastTextBlockClicked = textBlock;
                findingMatch = true;
            }

            else if (textBlock.Text == lastTextBlockClicked.Text)
            {
                matchesFound++;
                textBlock.Visibility = Visibility.Hidden;
                findingMatch = false;
            }

            else
            {
                lastTextBlockClicked.Visibility = Visibility.Visible;
                findingMatch = false;
            }
        }

        private void TimeTextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (matchesFound == 8)
            {
                SetUpGame();
            }
        }
    }
}
