using System;

class Position  //Create postion class and initialize it with X and Y 
{
    public int X { get; set; }
    public int Y { get; set; }

    public Position(int x, int y)
    {
        X = x;
        Y = y;
    }
}

class Player //Create prayer class
{
    public string Name { get; }
    public Position Position { get; set; }
    public int GemCount { get; set; }

    public Player(string name, Position position)
    {
        Name = name;
        Position = position;
        GemCount = 0;
    }

    public void Move(char direction)  //Declare method move for choosing directions
    {
        Position newPosition = new Position(Position.X, Position.Y);

        switch (direction)
        {
            case 'U':  //for up
                newPosition.Y = Math.Max(0, Position.Y - 1);
                break;
            case 'D': //for down
                newPosition.Y = Math.Min(5, Position.Y + 1);
                break;
            case 'L': //for left
                newPosition.X = Math.Max(0, Position.X - 1);
                break;
            case 'R': //for right
                newPosition.X = Math.Min(5, Position.X + 1);
                break;
        }

        Position = newPosition; //postion getting upgraded as per next moves of player 1 and player 2 i.e., p1 and p2
    }
}

class Cell //creating class cell for occupants - gems and obstacles 
{
    public string Occupant { get; set; }
}

class Board  //board with matrix of 6*6 
{
    public Cell[,] Grid { get; }

    public Board()
    {
        Grid = new Cell[6, 6]; 
        InitializeBoard();  //board intitalized
    }

    private void InitializeBoard()  
    {
        // Initialize the grid 
        for (int i = 0; i < 6; i++)   
        {
            for (int j = 0; j < 6; j++)
            {
                Grid[i, j] = new Cell { Occupant = "-" };
            }
        }

        // PlACING PLAYERS P1 ON TOPLEFT AND P2 ON BOTTOM RIGHT  on the board
        Grid[0, 0].Occupant = "P1";
        Grid[5, 5].Occupant = "P2";

        // Place gems and obstacles 
        PlaceRandomElements("G", 5);
        PlaceRandomElements("O", 5);
    }

    private void PlaceRandomElements(string element, int count)  //placing randomly the elemnetes such as obstacles and gems
    {
        Random random = new Random();
        for (int i = 0; i < count; i++)
        {
            int x, y;
            do
            {
                x = random.Next(6);
                y = random.Next(6);
            } while (Grid[x, y].Occupant != "-");

            Grid[x, y].Occupant = element;
        }
    }

    public void Display()   //this will display the board 
     {
        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                Console.Write(Grid[i, j].Occupant + " ");
            }
            Console.WriteLine();
        }
        Console.WriteLine();
    }

    public bool IsValidMove(Player player, char direction)  //calling methods for moving the players for turns when postion is valid
    {
        int newX = player.Position.X;
        int newY = player.Position.Y;

        switch (direction)
        {
            case 'U':
                newY--;
                break;
            case 'D':
                newY++;
                break;
            case 'L':
                newX--;
                break;
            case 'R':
                newX++;
                break;
        }

        // Check if the new position is within bounds
        if (newX < 0 || newX >= 6 || newY < 0 || newY >= 6)
        {
            return false;
        }

        // Check if the new position contains an obstacle
        if (Grid[newX, newY].Occupant == "O")
        {
            return false;
        }

        return true;
    }

    public bool CollectGem(Player player)
    {
        if (Grid[player.Position.X, player.Position.Y].Occupant == "G")
        {
            player.GemCount++;
            Grid[player.Position.X, player.Position.Y].Occupant = "-";
            return true;
        }
        return false;
    }
}

class Game
{
    public Board Board { get; }
    public Player Player1 { get; }
    public Player Player2 { get; }
    public Player CurrentTurn { get; set; }
    public int TotalTurns { get; set; }

    public Game()
    {
        Board = new Board();
        Player1 = new Player("P1", new Position(0, 0));
        Player2 = new Player("P2", new Position(5, 5));
        CurrentTurn = Player1;
        TotalTurns = 0;
    }
    public void Start()
    {
        while (!IsGameOver())
        {
            Board.Display();
            Console.WriteLine($"{CurrentTurn.Name}'s turn. Enter move (U/D/L/R): ");
            char move = char.ToUpper(Console.ReadKey().KeyChar);

            if (IsValidMove(CurrentTurn, move))
            {
                CurrentTurn.Move(move);
                if (Board.CollectGem(CurrentTurn))
                {
                    Console.WriteLine($"{CurrentTurn.Name} collected a gem!");
                }

                SwitchTurn();
                TotalTurns++;
            }
            else
            {
                Console.WriteLine("Invalid move. Try again.");
            }
        }

        Board.Display();
        AnnounceWinner();
    }

    private void SwitchTurn() //switch turns 
    {
        CurrentTurn = (CurrentTurn == Player1) ? Player2 : Player1;
    }

    private bool IsGameOver()
    {
        return TotalTurns >= 30; //game's toal turns that this condition satisfies loop of turns switching i.e.,15 for each player
    }

    private void AnnounceWinner()  //announing winner as per the player with maximum gems win the game
    {
        Console.WriteLine("Game Over!");

        if (Player1.GemCount > Player2.GemCount)  //p1 having gems more than p2
        {
            Console.WriteLine($"{Player1.Name} wins with {Player1.GemCount} gems!");
        }
        else if (Player2.GemCount > Player1.GemCount)  //p2 having gems more than p1
        {
            Console.WriteLine($"{Player2.Name} wins with {Player2.GemCount} gems!");
        }
        else
        {
            Console.WriteLine("It's a tie!");  //p1 p2 ahaving equal gems
        }
    }

    private bool IsValidMove(Player player, char direction)  //valid move is only when in 6by6 matric have null position and p1 and p2 don't go outside the array
        
    {
        int newX = player.Position.X;
        int newY = player.Position.Y;

        switch (direction)
        {
            case 'U':
                newY--;
                break;
            case 'D':
                newY++;
                break;
            case 'L':
                newX--;
                break;
            case 'R':
                newX++;
                break;
        }

        // Check if the new position is within bounds
        if (newX < 0 || newX >= 6 || newY < 0 || newY >= 6)
        {
            return false;
        }

        // Check if the new position contains an obstacle
        if (Board.Grid[newX, newY].Occupant == "O")
        {
            return false;
        }

        return true;
    }
}
class Program //main program
{
    static void Main(string[] args)
    {
        Game gemHunters = new Game();
        gemHunters.Start();
    }
}
//Game Ends
