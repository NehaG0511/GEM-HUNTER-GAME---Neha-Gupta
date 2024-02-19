﻿using System;

class Position
{
    public int X { get; set; }
    public int Y { get; set; }

    public Position(int x, int y)
    {
        X = x;
        Y = y;
    }
}

class Player
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

    public void Move(char direction)
    {
        Position newPosition = new Position(Position.X, Position.Y);

        switch (direction)
        {
            case 'U':
                newPosition.Y = Math.Max(0, Position.Y - 1);
                break;
            case 'D':
                newPosition.Y = Math.Min(5, Position.Y + 1);
                break;
            case 'L':
                newPosition.X = Math.Max(0, Position.X - 1);
                break;
            case 'R':
                newPosition.X = Math.Min(5, Position.X + 1);
                break;
        }

        Position = newPosition;
    }
}

class Cell
{
    public string Occupant { get; set; }
}

class Board
{
    public Cell[,] Grid { get; }

    public Board()
    {
        Grid = new Cell[6, 6];
        InitializeBoard();
    }

    private void InitializeBoard()
    {
        // Initialize the grid with empty cells
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

    private void PlaceRandomElements(string element, int count)
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

    public void Display()
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

    public bool IsValidMove(Player player, char direction)
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
//game ends
