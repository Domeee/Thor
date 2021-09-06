using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    [SerializeField] private Texture2D defaultCursor;
    [SerializeField] private Texture2D attackCursor;
    private CursorState _cursorState = CursorState.DEFAULT;

    enum CursorState
    {
        DEFAULT = 0,
        ATTACK = 1
    }

    private void Start()
    {
        Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.Auto);
    }

    /* Update is called every frame, if the MonoBehaviour is enabled. */
    void Update()
    {
        LayerMask attackableLayer = LayerMask.GetMask("Attackable");
        RaycastHit2D hit = Physics2D.Raycast(
            Camera.main.ScreenToWorldPoint(Input.mousePosition),
            Vector2.zero,
            Mathf.Infinity,
            attackableLayer);

        if (hit.collider != null && _cursorState == CursorState.DEFAULT)
        {
            Cursor.SetCursor(attackCursor, Vector2.zero, CursorMode.Auto);
            _cursorState = CursorState.ATTACK;
        }

        if (hit.collider is null && _cursorState == CursorState.ATTACK)
        {
            Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.Auto);
            _cursorState = CursorState.DEFAULT;
        }
    }
}