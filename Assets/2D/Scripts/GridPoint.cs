using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(SpriteRenderer))]
public class GridPoint : MonoBehaviour
{
    [SerializeField] float animationDuration = 0.1f;
    [SerializeField] bool alive;

    private SpriteRenderer spriteRenderer;


    public bool Alive
    {
        get { return alive; }
        set
        {
            alive = value;
            UpdateUI();
        }
    }

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SwichState()
    {
        alive = !alive;
        UpdateUI();
    }

    private void UpdateUI()
    {
        spriteRenderer.material.DOColor(alive ? Color.black : Color.white, animationDuration); 
    }
}
