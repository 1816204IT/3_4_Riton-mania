using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicProgressBar : MonoBehaviour
{
    private MusicPlayer musicPlayer = null;
    private RectTransform rectTransform = null;
    [SerializeField]
    private Canvas canvas = null;

    void Start()
    {
        musicPlayer = GameObject.FindGameObjectWithTag("MusicPlayer").GetComponent<MusicPlayer>();
        rectTransform = this.GetComponent<RectTransform>();

        if (musicPlayer == null || rectTransform == null || canvas == null)
        {
            Debug.Log("nullを検知");
        }
    }

    void Update()
    {
        Vector3 pos = this.transform.position;
        float posX = musicPlayer.GetSeekBarPosition() * 1280.0f;
        this.transform.position = new Vector3(posX, pos.y, pos.z);
        //rectTransform.localPosition = new Vector3(pos.x, pos.y, pos.x);
        //this.transform.position = GetWorldPositionFromRectPosition(rectTransform);
    }

    private Vector3 GetWorldPositionFromRectPosition(RectTransform rect)
    {
        //UI座標からスクリーン座標に変換
        Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(canvas.worldCamera, rect.position);

        //ワールド座標
        Vector3 result = Vector3.zero;

        //スクリーン座標→ワールド座標に変換
        RectTransformUtility.ScreenPointToWorldPointInRectangle(rect, screenPos, canvas.worldCamera, out result);

        return result;
    }
}
