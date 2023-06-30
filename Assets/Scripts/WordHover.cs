using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordHover : MonoBehaviour
{
    [SerializeField] AudioSource audio;
    [SerializeField] Material material;
    [SerializeField] MeshRenderer myWordMesh;
    [SerializeField] MeshRenderer otherWordMesh;
    [SerializeField] BoxCollider myBoxCollider;
    [SerializeField] BoxCollider otherBoxCollider;
    [SerializeField] bool correct;

    public GameObject[] Mazes;
    public float height;
    private GameObject currentMaze;
    [SerializeField]  float timer = 10;
    private float total_timer = 0.55f;
    private int l = 0;

    private void Start()
    {
        material.color = Color.white;
        if (!correct)
        {
            int r = Random.Range(0, 5);
            currentMaze = Instantiate(Mazes[r]);
            l = r;
            currentMaze.transform.position = new Vector3(0, 0, 0);
        }
        
    }

    private void Update()
    {
        Randomizer();
    }

    private void Randomizer()
    {
        timer += Time.deltaTime;
        if (timer < total_timer)
        {
            currentMaze.transform.position -= new Vector3(0, height, 0);
        }
        else if (timer < total_timer * 2)
        {
            Destroy(currentMaze);
            int r = l;
            while (l == r)
            {
                r = Random.Range(0, 5);
            }
            l = r;
            currentMaze = Instantiate(Mazes[r]);
            currentMaze.transform.position = new Vector3(0, -50f, 0);
            timer = total_timer * 2;
        }
        else if (timer < total_timer * 3)
        {
            currentMaze.transform.position += new Vector3(0, height, 0);
        }
    }

    private void OnMouseEnter()
    {
        material.color = Color.yellow;
       // if (audio.isPlaying != true)
            audio.Play();
    }

    private void OnMouseExit()
    {
        material.color= Color.white;

    }

    private void OnMouseDown()
    {
        myWordMesh.enabled = false;
        otherWordMesh.enabled = false;
        myBoxCollider.enabled = false;
        otherBoxCollider.enabled = false;   

        if (!correct)
        { 
            timer = 0;
        }
    }
}
