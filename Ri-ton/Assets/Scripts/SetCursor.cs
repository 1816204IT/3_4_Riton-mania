using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetCursor : MonoBehaviour
{
    [SerializeField]
    private Texture2D cursor = null;

    void Start()
    {
        if (cursor == null)
        {
            Debug.Log("nullを検知");
        }

        Vector2 hotSpot = new Vector2(180 / 2, 180 / 2);
        Cursor.SetCursor(cursor, hotSpot, CursorMode.ForceSoftware);
    }
}
