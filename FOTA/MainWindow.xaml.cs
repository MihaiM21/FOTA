﻿using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using System.Net.Http.Json;
using System.IO;
using System.Text.RegularExpressions;
using static FOTA.RoundNumber;

namespace FOTA
{
    public partial class MainWindow : Window
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private byte[] _imageData; // image data storage variable
        private static RoundNumber _roundNumber = new RoundNumber();

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
                if (selectedPlot == "DriversTrackComparison" || selectedPlot == "SpeedTraces" || selectedPlot == "Throttle Graph" || selectedPlot == "Choose a plot type")
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
            });
            

        }
        
        // Changing the round names and order in the right way regarting to the selected year
        private void YearComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            // Clear RoundComboBox and put the default message
            RoundComboBox.Items.Clear();
            RoundComboBox.Items.Add("Choose a race round");
            RoundComboBox.Text = "Choose a race round";

            Dispatcher.InvokeAsync(() =>
            {
                // Get the selected year
                string selectedYear = YearComboBox.Text;
                Console.WriteLine(selectedYear);

                if (selectedYear == "2025")
                {
                    foreach (var race in _roundNumber.Races2025)
                    {
                        RoundComboBox.Items.Add($"R{race.Key} - {race.Value}");
                    }
                }
                else if (selectedYear == "2024")
                {
                    foreach (var race in _roundNumber.Races2024)
                    {
                        RoundComboBox.Items.Add($"R{race.Key} - {race.Value}");
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

        private async void ExecuteRequest_Click(object sender, RoutedEventArgs e)
        {
            string baseUrl = "http://127.0.0.1:5000/generate_plot";
            string plotType = PlotTypeComboBox.Text;
            string year = YearComboBox.Text;
            string round = ExtractRoundNumber(RoundComboBox.Text);
            string eventType = EventTypeComboBox.Text;
            string token = TokenTextBox.Text;
            string driver1 = Driver1ComboBox.Text;
            string driver2 = Driver2ComboBox.Text;
            string team1 = Team1ComboBox.Text;
            string team2 = Team2ComboBox.Text;

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

                PlotImage.Source = bitmap;
            }
            catch (Exception ex)
            {
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
            
        }
        private string ExtractRoundNumber(string roundText)
        {
            // Folosește o expresie regulată pentru a extrage numerele
            Match match = Regex.Match(roundText, @"\d+");
            return match.Success ? match.Value : string.Empty;
        }
    }
}
