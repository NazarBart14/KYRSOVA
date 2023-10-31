using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace KYRSOVA
{
    class GameParser : MainWindow
    {
        private MainWindow _mainWindow;

        // Конструктор, в якому отримуємо посилання на головне вікно        
        public GameParser(MainWindow mainWindow)
        {
            _mainWindow = mainWindow;
            _httpClient = new HttpClient(); // Ініціалізуємо _httpClient
            ParseGamesAsync();
        }
        public void SetGames(List<Game> games)
        {
            Games = games;
            UpdateGamesListView();
        }

        private int _currentPage = 1;
        private int _itemsPerPage = 25; // Кількість елементів на одній сторінці

        // Метод для парсингу ігор
        public async Task ParseGamesAsync()
        {
            var games = new List<Game>();
            var urlBase = "https://store.steampowered.com/search/?sort_by=_ASC&supportedlang=ukrainian&category1=1&page=";

            for (int page = 1; page <= 2; page++)
            {
                var url = urlBase + page;
                var web = new HtmlWeb();
                var doc = await web.LoadFromWebAsync(url);
                var nodes = doc.DocumentNode.SelectNodes("//div[@id='search_resultsRows']/a");

                foreach (var node in nodes)
                {
                    var nameNode = node.SelectSingleNode(".//span[@class='title']");
                    var priceNode = node.SelectSingleNode(".//div[@class='col search_price_discount_combined responsive_secondrow']");
                    var imageNode = node.SelectSingleNode(".//img");

                    var name = nameNode?.InnerText?.Trim();
                    var price = priceNode?.InnerText?.Trim();
                    var imageUrl = imageNode?.GetAttributeValue("src", "") ?? "Empty";

                    var game = new Game(name, price, imageUrl);
                    games.Add(game);
                }
            }

            // Ініціалізуємо _gameEntity та передаємо йому список games
            _gameEntity = new GameEntity(games);

            // Оновлюємо інтерфейс для відображення ігор, наприклад, за допомогою вашої OnPropertyChanged() методу.
            OnPropertyChanged(nameof(Games));
        }

       
        private void UpdatePageInfo()
        {
            int totalItems = Games.Count; // Загальна кількість ігор
            int totalPages = (int)Math.Ceiling((double)totalItems / _itemsPerPage);
            PageInfoTextBlock.Text = $"Page {_currentPage} of {totalPages}";
        }

        private void UpdateGamesListView()
        {
            // Відображення ігор на поточній сторінці
            var gamesOnCurrentPage = Games.Skip((_currentPage - 1) * _itemsPerPage).Take(_itemsPerPage).ToList();
            GamesListView.ItemsSource = gamesOnCurrentPage;
            UpdatePageInfo();
        }


        private readonly HttpClient _httpClient;


        //public async Task<List<string>> GetRemoteDataAsync()
        //{
        //    List<string> remoteData = new List<string>();

        //    try
        //    {
        //        // URL віддаленого сервера, до якого ви хочете зробити запит
        //        string apiUrl = "https://example.com/api/data";

        //        // Виконання GET запиту до сервера
        //        HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);

        //        // Перевірка, чи запит був успішним
        //        if (response.IsSuccessStatusCode)
        //        {
        //            // Отримання відповіді у вигляді рядка
        //            string responseData = await response.Content.ReadAsStringAsync();

        //            // Розділення рядка на окремі рядки (наприклад, розділити за символом нового рядка)
        //            remoteData = responseData.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList();
        //        }
        //        else
        //        {
        //            // Обробка помилки, якщо запит був неуспішним
        //            Console.WriteLine("Помилка: " + response.StatusCode);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // Обробка винятку, якщо сталася помилка під час виконання запиту
        //        Console.WriteLine("Помилка: " + ex.Message);
        //    }

        //    return remoteData;
        //}
    }
}