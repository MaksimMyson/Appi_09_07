using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Appi_09_07
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            LoadData();
        }

        private async void LoadData()
        {
            var characters = await GetCharactersAsync();
            CharacterListBox.ItemsSource = characters;
        }

        private async Task<List<Character>> GetCharactersAsync()
        {
            using var httpClient = new HttpClient { BaseAddress = new Uri("https://rickandmortyapi.com/api/") };
            var response = await httpClient.GetFromJsonAsync<ApiResponse>("character");
            return response?.Results ?? new List<Character>();
        }

        private async void CharacterListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CharacterListBox.SelectedItem is Character selectedCharacter)
            {
                CharacterImage.Source = new BitmapImage(new Uri(selectedCharacter.Image));
                CharacterName.Text = $"Name: {selectedCharacter.Name}";
                CharacterStatus.Text = $"Status: {selectedCharacter.Status}";
                CharacterSpecies.Text = $"Species: {selectedCharacter.Species}";
                CharacterGender.Text = $"Gender: {selectedCharacter.Gender}";
                CharacterOrigin.Text = $"Origin: {selectedCharacter.Origin?.Name}";
                CharacterLocation.Text = $"Location: {selectedCharacter.Location?.Name}";

                var episodes = await GetEpisodesAsync(selectedCharacter.Episode);
                EpisodesListBox.ItemsSource = episodes;
            }
        }

        private async Task<List<string>> GetEpisodesAsync(List<string> episodeUrls)
        {
            var episodes = new List<string>();
            using var httpClient = new HttpClient();

            foreach (var episodeUrl in episodeUrls)
            {
                var episode = await httpClient.GetFromJsonAsync<Episode>(episodeUrl);
                episodes.Add(episode?.Name ?? "Unknown Episode");
            }

            return episodes;
        }
    }

    public class Character
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string Species { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public Origin? Origin { get; set; }
        public Location? Location { get; set; }
        public string Image { get; set; } = string.Empty;
        public List<string> Episode { get; set; } = new List<string>();
    }

    public class Origin
    {
        public string Name { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
    }

    public class Location
    {
        public string Name { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
    }

    public class Episode
    {
        public string Name { get; set; } = string.Empty;
    }

    public class ApiResponse
    {
        public List<Character> Results { get; set; } = new List<Character>();
    }
}
