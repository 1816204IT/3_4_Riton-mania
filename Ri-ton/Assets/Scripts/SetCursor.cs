using UnityEngine;

/// <summary>
/// マウスカーソルを任意の画像に変更する
/// </summary>
public class SetCursor : MonoBehaviour
{
    [SerializeField]
    private Texture2D cursor = null;

    void Start()
    {
        cursor.IsNull();

        Vector2 hotSpot = new Vector2(180 / 2, 180 / 2);
        Cursor.SetCursor(cursor, hotSpot, CursorMode.ForceSoftware);
    }
}