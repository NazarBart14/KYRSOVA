using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KYRSOVA
{
    public class GameUseCases
    {
        private readonly GameEntity _gameEntity;

        public GameUseCases(GameEntity gameEntity)
        {
            _gameEntity = gameEntity;
        }

        public List<Game> GetSelectedGames(string searchText)
        {
            if (string.IsNullOrEmpty(searchText))
            {
                // Якщо текст пошуку порожній, повернути усі ігри без фільтрації.
                return _gameEntity.Games;
            }
            else
            {
                // Фільтрувати ігри за текстом пошуку у нижньому регістрі.
                searchText = searchText.ToLower();
                return _gameEntity.Games.Where(game => game.Name.ToLower().Contains(searchText)).ToList();
            }
        }

    }

}
