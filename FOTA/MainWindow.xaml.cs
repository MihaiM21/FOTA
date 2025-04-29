using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using System.Net.Http.Json;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace FOTA
{
    public partial class MainWindow : Window
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private byte[] _imageData; // image data storage variable
        private static RoundNumber _roundNumber = new RoundNumber();
        private static driversList _driversList = new driversList();
        
        
        
        public MainWindow()
        {
            InitializeComponent();
            StartProgram();
            PlotTypeComboBox.SelectionChanged += PlotTypeComboBox_SelectionChanged;
        }
        
        // Show the drivers and team selection options only for the plots that require them
        private void PlotTypeComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            Dispatcher.InvokeAsync(() =>
            {
                string selectedPlot = PlotTypeComboBox.Text;
                Console.WriteLine(selectedPlot);
                if (selectedPlot == "Drivers track comparison" || selectedPlot == "Speed traces" || selectedPlot == "Throttle graph" || selectedPlot == "Choose a plot type")
                {
                    Driver1ComboBox.Visibility = Visibility.Visible;
                    Driver2ComboBox.Visibility = Visibility.Visible;
                    Team1ComboBox.Visibility = Visibility.Visible;
                    Team2ComboBox.Visibility = Visibility.Visible;
                    D1TextBlock.Visibility = Visibility.Visible;
                    D2TextBlock.Visibility = Visibility.Visible;
                    T1TextBlock.Visibility = Visibility.Visible;
                    T2TextBlock.Visibility = Visibility.Visible;
                }
                else
                {
                    Driver1ComboBox.Visibility = Visibility.Collapsed;
                    Driver2ComboBox.Visibility = Visibility.Collapsed;
                    Team1ComboBox.Visibility = Visibility.Collapsed;
                    Team2ComboBox.Visibility = Visibility.Collapsed;
                    D1TextBlock.Visibility = Visibility.Collapsed;
                    D2TextBlock.Visibility = Visibility.Collapsed;
                    T1TextBlock.Visibility = Visibility.Collapsed;
                    T2TextBlock.Visibility = Visibility.Collapsed;
                }
                
                EventTypeModifier("test");
            });
            
            

        }
        
        // Changing the round names and order in the right way regarting to the selected year
        private void YearComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            // Clear RoundComboBox and put the default message
            RoundComboBox.Items.Clear();
            RoundComboBox.Items.Add("Choose a race round");
            RoundComboBox.Text = "Choose a race round";
            
            // Clear the driver1 and driver2 list
            Driver1ComboBox.Items.Clear();
            Driver1ComboBox.Items.Add("Driver 1");
            
            Driver2ComboBox.Items.Clear();
            Driver2ComboBox.Items.Add("Driver 2");
            
            
            //
            Dispatcher.InvokeAsync(() =>
            {
                // Get the selected year
                string selectedYear = YearComboBox.Text;
                Console.WriteLine(selectedYear);

                if (selectedYear == "2025")
                {
                    // Adding the race names and number for the 2025 year
                    foreach (var race in _roundNumber.Races2025)
                    {
                        RoundComboBox.Items.Add($"R{race.Key} - {race.Value}");
                    }
                    // Adding the correct drivers from the 2025 year
                    foreach (var driver in _driversList.drivers2025)
                    {
                        Driver1ComboBox.Items.Add(driver.Value);
                        Driver2ComboBox.Items.Add(driver.Value);
                    }
                }
                else if (selectedYear == "2024")
                {
                    // Adding the race names and number for the 2024 year
                    foreach (var race in _roundNumber.Races2024)
                    {
                        RoundComboBox.Items.Add($"R{race.Key} - {race.Value}");
                    }
                    // Adding the correct drivers from the 2024 year
                    foreach (var driver in _driversList.drivers2024)
                    {
                        Driver1ComboBox.Items.Add(driver.Value);
                        Driver2ComboBox.Items.Add(driver.Value);
                    }
                }
                else if (selectedYear == "Choose an year")
                {
                    return;
                }
                else
                {
                    MessageBox.Show("Please select a valid year.", "Error", MessageBoxButton.OK,
                        MessageBoxImage.Warning);
                    return;
                }
                
                // Setting it as default
                if (RoundComboBox.Items.Count > 0)
                {
                    RoundComboBox.SelectedIndex = 0;
                }
            });
            
        }
        
        // Update the driver list when changing the Driver 1 so you can't select the same driver twice
        private void Driver1ComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            Dispatcher.InvokeAsync(() =>
            {
                string driver1 = Driver1ComboBox.Text;
                Driver2ComboBox.Items.Clear();
                if (YearComboBox.Text == "2025")
                {
                    foreach (var driver in _driversList.drivers2025)
                    {
                        if (driver.Value != driver1)
                        {
                            Driver2ComboBox.Items.Add(driver.Value);
                        }
                    }
                }
                if (YearComboBox.Text == "2024")
                {
                    foreach (var driver in _driversList.drivers2024)
                    {
                        if (driver.Value != driver1)
                        {
                            Driver2ComboBox.Items.Add(driver.Value);
                        }
                    }
                }
            });    
        }
        // Check if the round has Sprint and Sprint Qualifying
        private void RoundComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Sprint races nr: 2, 6, 13, 19, 21, 22
            Dispatcher.InvokeAsync(() =>
            {
                EventTypeModifier("test");
            });
            
        }

        private void EventTypeModifier(string input)
        {
            string selectedRound = ExtractRoundNumber(RoundComboBox.Text);
            EventTypeComboBox.Items.Clear();
            if (selectedRound == "2" || selectedRound == "6" || selectedRound == "13" || selectedRound == "19" ||
                selectedRound == "21" || selectedRound == "22")
            {
                // For every plot type now
                if (PlotTypeComboBox.Text == "Top speed")
                {
                    EventTypeComboBox.Items.Add("FP1");
                    EventTypeComboBox.Items.Add("SQ");
                    EventTypeComboBox.Items.Add("S");
                    EventTypeComboBox.Items.Add("Q");
                    EventTypeComboBox.Items.Add("R");
                }
                else if (PlotTypeComboBox.Text == "Team pace" || PlotTypeComboBox.Text == "Strategy" ||
                         PlotTypeComboBox.Text == "Driver laptimes" ||
                         PlotTypeComboBox.Text == "Drivers laptimes distribution" ||
                         PlotTypeComboBox.Text == "Position changes")
                {
                    EventTypeComboBox.Items.Add("S");
                    EventTypeComboBox.Items.Add("R");
                }
                else if (PlotTypeComboBox.Text == "Throttle graph" ||
                         PlotTypeComboBox.Text == "Drivers track comparison" || PlotTypeComboBox.Text == "Speed trace")
                {
                    EventTypeComboBox.Items.Add("FP1");
                    EventTypeComboBox.Items.Add("SQ");
                    EventTypeComboBox.Items.Add("Q");
                }
                
                
            }
            else
            {
                if (PlotTypeComboBox.Text == "Top speed")
                {
                    EventTypeComboBox.Items.Add("FP1");
                    EventTypeComboBox.Items.Add("FP2");
                    EventTypeComboBox.Items.Add("FP3");
                    EventTypeComboBox.Items.Add("Q");
                    EventTypeComboBox.Items.Add("R");
                }
                else if (PlotTypeComboBox.Text == "Team pace" || PlotTypeComboBox.Text == "Strategy" ||
                         PlotTypeComboBox.Text == "Driver laptimes" ||
                         PlotTypeComboBox.Text == "Drivers laptimes distribution" ||
                         PlotTypeComboBox.Text == "Position changes")
                {
                    EventTypeComboBox.Items.Add("R");
                }
                else if (PlotTypeComboBox.Text == "Throttle graph" ||
                         PlotTypeComboBox.Text == "Drivers track comparison" || PlotTypeComboBox.Text == "Speed trace")
                {
                    EventTypeComboBox.Items.Add("FP1");
                    EventTypeComboBox.Items.Add("FP2");
                    EventTypeComboBox.Items.Add("FP3");
                    EventTypeComboBox.Items.Add("Q");
                }
                
            }
        }
        private async void ExecuteRequest_Click(object sender, RoutedEventArgs e)
        {
            // Data needed to be sent to the server
            string baseUrl = "https://api.t1f1.com/generate_plot";
            string plotType = PlotTypeComboBox.Text;
            string year = YearComboBox.Text;
            string round = ExtractRoundNumber(RoundComboBox.Text);
            string eventType = EventTypeComboBox.Text;
            string token = TokenTextBox.Text;
            string driver1 = Driver1ComboBox.Text;
            string driver2 = Driver2ComboBox.Text;
            string team1 = Team1ComboBox.Text;
            string team2 = Team2ComboBox.Text;
            
            // Printing the sent data
            Console.WriteLine(plotType);
            Console.WriteLine(year);
            Console.WriteLine(round);
            Console.WriteLine(eventType);
            Console.WriteLine(token);
            Console.WriteLine(driver1);
            Console.WriteLine(driver2);
            Console.WriteLine(team1);
            Console.WriteLine(team2);

            if (string.IsNullOrEmpty(plotType) || string.IsNullOrEmpty(year) || string.IsNullOrEmpty(round) || string.IsNullOrEmpty(eventType) || string.IsNullOrEmpty(token))
            {
                MessageBox.Show("Please fill in all required fields.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var requestBody = new
            {
                plot_type = plotType,
                year = year,
                round = round,
                eventType = eventType,
                token = token,
                driver1 = driver1,
                driver2 = driver2,
                team1 = team1,
                team2 = team2
            };
            // Showing the Loading Bar
            ProgressBar.Visibility = Visibility.Visible;
            // Removing the old Plot Image
            PlotImage.Visibility = Visibility.Collapsed;
            try
            {
                HttpResponseMessage response = await _httpClient.PostAsJsonAsync(baseUrl, requestBody);
                response.EnsureSuccessStatusCode();

                _imageData = await response.Content.ReadAsByteArrayAsync();
                var bitmap = new BitmapImage();
                using (var stream = new MemoryStream(_imageData))
                {
                    bitmap.BeginInit();
                    bitmap.StreamSource = stream;
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.EndInit();
                }
                // Removing the Loading Bar
                ProgressBar.Visibility = Visibility.Collapsed;
                //Showing the image generated
                PlotImage.Visibility = Visibility.Visible;
                PlotImage.Source = bitmap;
                
            }
            catch (Exception ex)
            {
                // Removing the Loading Bar
                ProgressBar.Visibility = Visibility.Collapsed;
                // Showing the error
                MessageBox.Show($"Error: {ex.Message}", "Request Failed", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DownloadImage_Click(object sender, RoutedEventArgs e)
        {
            if (_imageData == null || _imageData.Length == 0)
            {
                MessageBox.Show("No image to download. Please generate a plot first.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var saveFileDialog = new SaveFileDialog
            {
                Filter = "Image Files (*.png)|*.png",
                FileName = "plot_image.png"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    File.WriteAllBytes(saveFileDialog.FileName, _imageData);
                    MessageBox.Show("Image downloaded successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to save image: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void StartProgram()
        {
            PlotTypeComboBox.Text = "Choose a plot type";
            YearComboBox.Text = "Choose an year";
            RoundComboBox.Text = "Choose a race round";
            EventTypeComboBox.Text = "Choose an event";
            
            // Setting the Driver1/2 and Team1/2 not vissible at the start of the program
            Driver1ComboBox.Visibility = Visibility.Collapsed;
            Driver2ComboBox.Visibility = Visibility.Collapsed;
            Team1ComboBox.Visibility = Visibility.Collapsed;
            Team2ComboBox.Visibility = Visibility.Collapsed;
            D1TextBlock.Visibility = Visibility.Collapsed;
            D2TextBlock.Visibility = Visibility.Collapsed;
            T1TextBlock.Visibility = Visibility.Collapsed;
            T2TextBlock.Visibility = Visibility.Collapsed;
            
        }
        private string ExtractRoundNumber(string roundText)
        {
            // Extract only the numbers from the text
            // Used to send only the round number to the server
            Match match = Regex.Match(roundText, @"\d+");
            return match.Success ? match.Value : string.Empty;
        }

        
    }
}
