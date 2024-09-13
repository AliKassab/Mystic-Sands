using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PuzzlePiece : MonoBehaviour
{
    [SerializeField] private Image _renderer;

    
    [SerializeField] private AudioSource _scource;
    [SerializeField] private AudioClip _pickUpClip, _dropClip;

    private bool _dragging;

    private Vector2 _offset ,_originalPosition;

    private PuzzleSlot _slot;
    public void Init(PuzzleSlot slot)
    {

        _renderer.sprite = slot.Renderer.sprite;
        _slot = slot;
    }

    void Awake()
    {
        _originalPosition = transform.position;
    }

    void Update()
    {
        if (!_dragging) return;

        var mousePosition = GetMousePos();

        transform.position = mousePosition - _offset;
    }

    void OnMouseDown()
    {
        _dragging = true;
        _scource.PlayOneShot(_pickUpClip);
        _offset = GetMousePos() - (Vector2)transform.position;

    }

    void OnMouseUp()
    {
        transform.position = _originalPosition;
        _dragging = false;
        _scource.PlayOneShot(_dropClip);
    }

    Vector2 GetMousePos()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
    
}
