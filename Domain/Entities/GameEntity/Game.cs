using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace KYRSOVA 
{


    public class GameEntity
    {
        public List<Game> Games { get; set; }

        public GameEntity(List<Game> games)
        {
            Games = games;
        }
    }
}
