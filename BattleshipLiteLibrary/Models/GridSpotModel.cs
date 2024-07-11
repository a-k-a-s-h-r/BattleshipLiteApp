using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleshipLiteLibrary.Models
{
    public class GridSpotModel
    {
        public int SpotNumber { get; set; }

        public string SpotLetter { get; set; }

        public GridSpotStatus Status { get; set; } = GridSpotStatus.empty;

    }
}
