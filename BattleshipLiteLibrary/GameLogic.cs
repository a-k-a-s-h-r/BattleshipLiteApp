using BattleshipLiteLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BattleshipLiteLibrary.Models;
using System.Data;

namespace BattleshipLiteLibrary
{
    public class GameLogic
    {
        public static bool PlaceShips(PlayerInfoModel model, string location)
        {
            (string row, int col) = SplitShotAsked(location);
            bool isValidGridSpot = ValidateGridSpot(row, col, model);
            bool isValidShipLocation = ValidateShipLocation(row, col, model);

            if(isValidGridSpot && isValidShipLocation)
            {
                model.ShipLocation.Add(new GridSpotModel
                {
                    SpotLetter = row,
                    SpotNumber = col,
                    Status = GridSpotStatus.ship
                });
                return true;
            };
            return false;
        }

        private static bool ValidateShipLocation(string row, int col, PlayerInfoModel model)
        {
            foreach(var item in model.ShipLocation)
            {
                if (item.SpotLetter.ToUpper() == row && item.SpotNumber == col)
                {
                    return false;
                }
            }
            return true;
        }

        private static bool ValidateGridSpot(string row, int col, PlayerInfoModel model)
        {
            foreach (var item in model.ShotGrid)
            {
                if(item.SpotLetter.ToUpper() == row && item.SpotNumber == col)
                {
                    return true;
                }
            }
            return false;
        }

        public static object GetShotCount(PlayerInfoModel winner)
        {
            int count = 0;
            foreach(var item in winner.ShotGrid)
            {
                if(item.Status == GridSpotStatus.hit || item.Status == GridSpotStatus.miss)
                {
                    count++;
                }
            }
            return count;
        }

        public static bool IdentifyShotResult(PlayerInfoModel opponent, string row, int col)
        {
            foreach(var item in opponent.ShipLocation)
            {
                if(item.SpotLetter == row && item.SpotNumber == col)
                {
                    item.Status = GridSpotStatus.sunk;
                    return true;                    
                }
            }
            return false;
        }

        public static void InitializeShotGrid(PlayerInfoModel info)
        {
            List<string> letters = new List<string>
            {
                "A", "B", "C", "D", "E"
            };
            List<int> numbers = new List<int>
            {
                1,2,3,4,5
            };

            foreach(string a in letters)
            {
                foreach(int b in numbers)
                {
                    AddShotGrid(a, b, info);
                }
            }

        }

        public static void MarkShotResult(PlayerInfoModel activePlayer, string row, int col, bool isHit)
        {
            foreach(var item in activePlayer.ShotGrid)
            {
                if(item.SpotLetter == row && item.SpotNumber == col)
                {
                    if (isHit) { item.Status = GridSpotStatus.hit; }
                    else item.Status = GridSpotStatus.miss;
                    
                }
            }
        }

        public static bool IsOpponentStillHaveChance(PlayerInfoModel opponent)
        {
            foreach(var item in opponent.ShipLocation)
            {
                if(item.Status != GridSpotStatus.sunk)
                {
                    return true;
                }
            }
            return false;
        }

        public static (string row, int col) SplitShotAsked(string shotAsked)
        {
            char[] chars = shotAsked.ToArray();
            string row;
            int col;

            if(chars.Length < 2)
            {
                throw new ArgumentException("This is not a valid spot. please try again.", "shotAsked");
            }
            try
            {
                row = chars[0].ToString().ToUpper();
                col = int.Parse(chars[1].ToString());
            }
            catch (Exception ex)
            {
                throw new Exception("That is an invalid format of shot selection", ex);
            }
            return (row, col);
        }

        public static bool ValidateShot(PlayerInfoModel activePlayer, string row, int col)
        {
            foreach (var item in activePlayer.ShotGrid)
            {
                if(item.SpotLetter == row && item.SpotNumber == col && item.Status == GridSpotStatus.empty) {
                    return true;

                }
            }
            return false;
        }

        private static void AddShotGrid(string letter, int number, PlayerInfoModel info)
        {
            GridSpotModel spot = new GridSpotModel()
            {
                SpotLetter = letter,
                SpotNumber = number,
                Status = GridSpotStatus.empty
            };
            info.ShotGrid.Add(spot);
        }
    }
}
