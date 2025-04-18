using DialogueEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeRenderer : MonoBehaviour
{



    private void Start()
    {
        buildNewMaze();
    }

    //public bool Randomize = false;

    [SerializeField]
    [Range(1, 50)]
    private int width = 10;

    [SerializeField]
    [Range(1, 50)]
    private int height = 10;

    [SerializeField]
    private float size = 1f;

    [SerializeField]
    private Transform wallPrefab = null;
    [SerializeField]
    private GameObject triggerPrefab = null;

    [SerializeField]
    public GameObject startTimerTrigger;
    [SerializeField]
    public GameObject endGameTrigger;

    [SerializeField]
    private Transform doorPrefab = null;

    [SerializeField]
    private PanelManager panelManager = null;
    
    [SerializeField]
    private NPCConversation[] doorConversations;


    public void buildNewMaze()
    {
        var maze = MazeGenerator.Generate(width, height);
        var currentMaze = maze;
        for (var i = transform.childCount - 1; i >= 0; i--)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
        Draw(maze);
    }
    private void PlaceDoor(Position position, WallState cell)
    {
        Vector3 doorPosition = new Vector3(-width / 2 + position.X, 0, -height / 2 + position.Y);
        Quaternion doorRotation = Quaternion.identity;

        if (cell.HasFlag(WallState.LEFT) && cell.HasFlag(WallState.RIGHT))
        {
            // Position the door in the center between LEFT and RIGHT walls
            doorPosition += Vector3.right * size / 2;  // Adjust position if necessary
            doorRotation = Quaternion.Euler(-90, 90, 0);  // Rotate to align with the z-axis
        }
        else if (cell.HasFlag(WallState.UP) && cell.HasFlag(WallState.DOWN))
        {
            // Position the door in the center between UP and DOWN walls
            doorPosition += Vector3.forward * size / 2;  // Adjust position if necessary
            doorRotation = Quaternion.Euler(-90, 0, 0);  // Rotate to align with the x-axis
        }

        Instantiate(doorPrefab, doorPosition, doorRotation);
    }
    private void Draw(WallState[,] maze)
{

        int doorCount = 3;
        int step = MazeGenerator.solutionPath.Count / (doorCount + 1);

        int doorIndex = 0;

        for (int i = 0; i < width; ++i)
    {
        for (int j = 0; j < height; ++j)
        {   
            var cell = maze[i, j];
            var position = new Vector3(-width / 2 + i, 0, -height / 2 + j);






                if (MazeGenerator.solutionPath.Contains(new Position { X = i, Y = j }) && MazeGenerator.solutionPath.IndexOf(new Position { X = i, Y = j }) % step == 0)
                {

                    var door = Instantiate(doorPrefab, position, Quaternion.identity);

                   
                    var testStart = door.GetComponent<TestStart>();
                    if (testStart != null && doorIndex < doorConversations.Length)
                    {
                        testStart.Initialize(panelManager, doorIndex, doorConversations[doorIndex]);
                    }

                    doorIndex++; 
                }

                if (i==0 && j==0)
            {
                startTimerTrigger = Instantiate(triggerPrefab, position, Quaternion.identity);
                startTimerTrigger.tag = nameof(startTimerTrigger);
                continue;
            }

            if(i==width-1 && j==height-1)
            {
                endGameTrigger = Instantiate(triggerPrefab, position, Quaternion.identity);
                endGameTrigger.tag = nameof(endGameTrigger);
                continue;
            }

                if (cell.HasFlag(WallState.UP))
                {
                    
                    InstantiateWall(position, Vector3.forward * size / 2, Vector3.one * size, Quaternion.identity);
                }

                if (cell.HasFlag(WallState.LEFT))
                {
                    
                    InstantiateWall(position, Vector3.left * size / 2, Vector3.one * size, Quaternion.Euler(0, 90, 0));
                }


                if (i == width - 1 && cell.HasFlag(WallState.RIGHT))
                {
                    
                    InstantiateWall(position, Vector3.right * size / 2, Vector3.one * size, Quaternion.Euler(0, 90, 0));
                }

                if (j == 0 && cell.HasFlag(WallState.DOWN))
                {
                    
                    InstantiateWall(position, Vector3.back * size / 2, Vector3.one * size, Quaternion.identity);
                }
        }
    }
}

    private void InstantiateWall(Vector3 position, Vector3 offset, Vector3 scale, Quaternion rotation)
    {
        var wall = Instantiate(wallPrefab, transform) as Transform;
        wall.position = position + offset;
        wall.localScale = scale;
        wall.rotation = rotation;
    }

}