using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PuzzleSlot : MonoBehaviour
{
    public Image Renderer;

    [SerializeField] private AudioSource _scource;
    [SerializeField] private AudioClip _completeClip;


    public void Placed()
    {
        _scource.PlayOneShot(_completeClip);
    }
}
