using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Flags]
public enum WallState
{
    // 0000 -> NO WALLS
    // 1111 -> LEFT,RIGHT,UP,DOWN
    LEFT = 1, // 0001
    RIGHT = 2, // 0010
    UP = 4, // 0100
    DOWN = 8, // 1000

    VISITED = 128, // 1000 0000
}

public struct Position
{
    public int X;
    public int Y;
}

public struct Neighbour
{
    public Position Position;
    public WallState SharedWall;
}

public class MazeGenerator
{
    #region Singleton
    private static MazeGenerator _instance;
    private static readonly object _lock = new object();
    private MazeGenerator() { }
    public static MazeGenerator Instance
    {
        get
        {
            lock (_lock)
            {
                if (_instance == null)
                {
                    _instance = new MazeGenerator();
                }
                return _instance;
            }
        }
    }
    #endregion
    private static WallState GetOppositeWall(WallState wall)
    {
        switch (wall)
        {
            case WallState.RIGHT: return WallState.LEFT;
            case WallState.LEFT: return WallState.RIGHT;
            case WallState.UP: return WallState.DOWN;
            case WallState.DOWN: return WallState.UP;
            default: return WallState.LEFT;
        }
    }
    public static List<Position> solutionPath = new List<Position>();

    private static WallState[,] ApplyRecursiveBacktracker(WallState[,] maze, int width, int height)
    {
        // here we make changes
        var rng = new System.Random(/*seed*/);
        var positionStack = new Stack<Position>();
        var position = new Position { X = rng.Next(0, width), Y = rng.Next(0, height) };

        maze[position.X, position.Y] |= WallState.VISITED;  // 1000 1111
        positionStack.Push(position);
        solutionPath.Add(position); 

        while (positionStack.Count > 0)
        {
            var current = positionStack.Pop();
            var neighbours = GetUnvisitedNeighbours(current, maze, width, height);

            if (neighbours.Count > 0)
            {
                positionStack.Push(current);

                var randIndex = rng.Next(0, neighbours.Count);
                var randomNeighbour = neighbours[randIndex];

                var nPosition = randomNeighbour.Position;
                maze[current.X, current.Y] &= ~randomNeighbour.SharedWall;
                maze[nPosition.X, nPosition.Y] &= ~GetOppositeWall(randomNeighbour.SharedWall);
                maze[nPosition.X, nPosition.Y] |= WallState.VISITED;

                positionStack.Push(nPosition);
                solutionPath.Add(nPosition); 

            }
        }

        return maze;
    }

    private static List<Neighbour> GetUnvisitedNeighbours(Position p, WallState[,] maze, int width, int height)
    {
        var list = new List<Neighbour>();

        CheckNeighbour(p, -1, 0, WallState.LEFT, maze, width, height, list);
        CheckNeighbour(p, 0, -1, WallState.DOWN, maze, width, height, list);
        CheckNeighbour(p, 0, 1, WallState.UP, maze, width, height, list);
        CheckNeighbour(p, 1, 0, WallState.RIGHT, maze, width, height, list);

        return list;
    }

    private static void CheckNeighbour(Position p, int xOffset, int yOffset, WallState wallState, WallState[,] maze, int width, int height, List<Neighbour> list)
        {
            int x = p.X + xOffset;
            int y = p.Y + yOffset;
            
            if (x >= 0 && y >= 0 && x < width && y < height && !maze[x, y].HasFlag(WallState.VISITED))
            {
                list.Add(new Neighbour
                {
                    Position = new Position { X = x, Y = y },
                    SharedWall = wallState
                });
            }
        }
    public static WallState[,] Generate(int width, int height)
    {
        WallState[,] maze = new WallState[width, height];
        WallState initial = WallState.RIGHT | WallState.LEFT | WallState.UP | WallState.DOWN;
        for (int i = 0; i < width; ++i)
        {
            for (int j = 0; j < height; ++j)
            {
                maze[i, j] = initial;  // 1111
            }
        }

        return ApplyRecursiveBacktracker(maze, width, height);
    }
}