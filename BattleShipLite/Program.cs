using BattleshipLiteLibrary;
using BattleshipLiteLibrary.Models;
using System.ComponentModel;
using System.Reflection;

namespace BattleShipLite
{
    class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Welcome to BattleShip game");
            PlayerInfoModel ActivePlayer = CreatePlayer("player1");
            PlayerInfoModel Opponent = CreatePlayer("player2");
            PlayerInfoModel winner = null;

            do
            {
                DisplayShotGrid(ActivePlayer);
                Console.WriteLine();

                RecordPlayerShot(ActivePlayer, Opponent);

                bool doesGameContinue = GameLogic.IsOpponentStillHaveChance(Opponent);

                if (doesGameContinue == true)
                {
                    // Swap positions
                    (ActivePlayer, Opponent) = (Opponent, ActivePlayer);
                }
                else
                {
                    winner = ActivePlayer;
                }
            } while (winner == null);

            IdentifyWinner(winner);


        }

        private static void IdentifyWinner(PlayerInfoModel winner)
        {
            Console.WriteLine($"congratulations {winner.Name} on winning the game!!");
            Console.WriteLine($"{winner.Name} took {GameLogic.GetShotCount(winner)} shots");
        }

        private static void RecordPlayerShot(PlayerInfoModel activePlayer, PlayerInfoModel opponent)
        {
            bool isValidShot = false;
            string row = "";
            int col = 0;
            string shotAsked;

            do
            {
                shotAsked = AskForShot(activePlayer);

                try
                {
                    (row, col) = GameLogic.SplitShotAsked(shotAsked);

                    isValidShot = GameLogic.ValidateShot(activePlayer, row, col);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    isValidShot = false;
                }

                if (!isValidShot)
                {
                    Console.WriteLine("That is an invalid shot, please try again!");
                } 
            } while (!isValidShot);

            bool isHit = GameLogic.IdentifyShotResult(opponent, row, col);

            GameLogic.MarkShotResult(activePlayer, row, col, isHit);

            if (isHit )
            {
                Console.WriteLine($"{shotAsked} is a hit");
            }
            else
            {
                Console.WriteLine($"{shotAsked} is a miss");
            }

            Console.WriteLine();
            Console.WriteLine();
        }

        private static string AskForShot(PlayerInfoModel activePlayer)
        {
            Console.WriteLine($"{activePlayer.Name} is hitting");
            Console.Write("select your shot: ");
            return Console.ReadLine();
        }

        private static void DisplayShotGrid(PlayerInfoModel activePlayer)
        {
            string CurrentRow = activePlayer.ShotGrid[0].SpotLetter;
            foreach(var spot in activePlayer.ShotGrid)
            {
                if(spot.SpotLetter != CurrentRow)
                {
                    Console.WriteLine();
                    CurrentRow = spot.SpotLetter;
                }

                if(spot.Status == GridSpotStatus.empty)
                {
                    Console.Write($"{ spot.SpotLetter }{spot.SpotNumber}  ");
                }
                else if(spot.Status == GridSpotStatus.hit){
                    Console.Write("x  ");
                }
                else if(spot.Status == GridSpotStatus.miss) {
                    Console.Write("o  ");
                }
                else
                {
                    Console.Write("?  ");
                }
            }
        }

        private static PlayerInfoModel CreatePlayer(string name)
        {
            PlayerInfoModel info = new PlayerInfoModel();

            Console.WriteLine($"Information of {name}:");

            info.Name = AskForName();

            GameLogic.InitializeShotGrid(info);

            AskUserForShipPlacements(info);

            Console.Clear();
            return info;
        }

        private static string AskForName()
        {
            Console.Write($"Enter your name: ");
            string name = Console.ReadLine();
            return name;
        }


        private static void AskUserForShipPlacements(PlayerInfoModel model) 
        {
            bool isValidLocation;
            do
            {
                Console.Write($"where do you want to place your ship {model.ShipLocation.Count + 1} : ");
                string location = Console.ReadLine();
                try
                {
                    isValidLocation = GameLogic.PlaceShips(model, location);
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    isValidLocation = false;
                }
                
                if (!isValidLocation)
                {
                    Console.WriteLine("The spot you selected is not valid, please try again..");
                }
            }
            while (model.ShipLocation.Count<5);
        }
    }
}