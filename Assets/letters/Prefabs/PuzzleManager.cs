using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    [SerializeField] private List<PuzzleSlot> _slotPrefabs;
    [SerializeField] private PuzzlePiece _piecePrefab;
    [SerializeField] private Transform _slotParent, _pieceParent;

    void Start()
    {
        Spawn();
    }

    void Spawn()
    {
        var randomSet = _slotPrefabs.OrderBy(s => Random.value).Take(3).ToList();

        for (int i = 0; i < randomSet.Count; i++)
        {
            // Instantiate slots and set as child of the canvas slot parent
            var spawnedSlot = Instantiate(randomSet[i], _slotParent);
            spawnedSlot.transform.localPosition = _slotParent.GetChild(i).localPosition;

            // Instantiate pieces and set as child of the canvas piece parent
            var spawnedPiece = Instantiate(_piecePrefab, _pieceParent);
            spawnedPiece.transform.localPosition = _pieceParent.GetChild(i).localPosition;

            // Initialize the spawned piece with the spawned slot
            spawnedPiece.Init(spawnedSlot);
        }
    }
}
