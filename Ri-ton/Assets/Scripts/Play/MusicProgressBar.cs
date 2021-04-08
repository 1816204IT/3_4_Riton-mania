using UnityEngine;

/// <summary>
/// 曲進捗バークラス
/// </summary>
public class MusicProgressBar : MonoBehaviour
{
    [SerializeField]
    private Canvas canvas = null;

    private MusicPlayer musicPlayer = null;
    private RectTransform rectTransform = null;

    void Start()
    {
        musicPlayer = GameObject.FindGameObjectWithTag("MusicPlayer").GetComponent<MusicPlayer>();
        rectTransform = this.GetComponent<RectTransform>();
        NullCheck();
    }

    void Update()
    {
        Vector3 pos = this.transform.position;
        float posX = musicPlayer.GetSeekBarPosition() * 1280.0f;
        this.transform.position = new Vector3(posX, pos.y, pos.z);
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

    private void NullCheck()
    {
        musicPlayer.IsNull();
        rectTransform.IsNull();
        canvas.IsNull();
    }
}