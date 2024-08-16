using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeRenderer : MonoBehaviour
{

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

    private void Draw(WallState[,] maze)
{
    for (int i = 0; i < width; ++i)
    {
        for (int j = 0; j < height; ++j)
        {   
            var cell = maze[i, j];
            var position = new Vector3(-width / 2 + i, 0, -height / 2 + j);

            if(i==0 && j==0)
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
                InstantiateWall(position, Vector3.forward * size / 2, Vector3.one * size, Quaternion.identity);

            if (cell.HasFlag(WallState.LEFT))
                InstantiateWall(position, Vector3.left * size / 2, Vector3.one * size, Quaternion.Euler(0, 90, 0));

            if (i == width - 1 && cell.HasFlag(WallState.RIGHT))
                InstantiateWall(position, Vector3.right * size / 2, Vector3.one * size, Quaternion.Euler(0, 90, 0));

            if (j == 0 && cell.HasFlag(WallState.DOWN))
                InstantiateWall(position, Vector3.back * size / 2, Vector3.one * size, Quaternion.identity);
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