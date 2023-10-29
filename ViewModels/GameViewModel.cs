using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KYRSOVA
{
    public class GameViewModel
    {
        public List<Game> Games { get; set; }
        public List<Game> SelectedGames { get; set; }

        public GameViewModel()
        {
            Games = new List<Game>();
            SelectedGames = new List<Game>();
        }

    }
}
