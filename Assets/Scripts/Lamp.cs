using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Lamp : MonoBehaviour
{

    public WeightedRandomList<GameObject> lootTable;

    public MeshRenderer showZone;
    public GameObject[] respawnPrefab;
    public GameObject[] respawnPrefab2;





    // Start is called before the first frame update
    void Start()
    {
        showZone = GetComponent<MeshRenderer>();
        respawnPrefab = GameObject.FindGameObjectsWithTag("Cube");
        respawnPrefab2 = GameObject.FindGameObjectsWithTag("Cube");
        for (int i = 0; i < 4; i++)
        {
            respawnPrefab[i].SetActive(false);
        }

    }

    // Update is called once per frame
    void Update()
    {
        

        
    }


    void OnCollisionEnter(Collision collision)
    {
        WeightedRandomList<GameObject>.Pair item = lootTable.GetRandom();


        item.item.gameObject.SetActive(true);
        item.item2.gameObject.SetActive(true);

        //respawnPrefab2[0] = item.item;
        //respawnPrefab2[1] = item.item2;

        //respawnPrefab2[0].SetActive(true);
        //respawnPrefab2[1].SetActive(true);


        //respawnPrefab2.SetActive(true);
        //respawnPrefab3.SetActive(true);



    }
}
