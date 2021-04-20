using UnityEngine;

/// <summary>
/// 曲プログレスバークラス。プレイ中の曲がどの程度まで進んでいるかを表現する。
/// </summary>
public class MusicProgressBar : MonoBehaviour
{
    [SerializeField]
    private Canvas canvas = null;

    private MusicPlayer musicPlayer = null;
    private RectTransform rectTransform = null;
    private Vector2 canvasSize = Vector2.zero;

    void Start()
    {
        musicPlayer = GameObject.FindGameObjectWithTag("MusicPlayer").GetComponent<MusicPlayer>();
        rectTransform = this.GetComponent<RectTransform>();
        NullCheck();
        canvasSize = canvas.GetComponent<RectTransform>().sizeDelta;
    }

    void Update()
    {
        Vector3 pos = this.transform.position;
        float posX = musicPlayer.GetSeekBarPosition() * canvasSize.x;
        this.transform.position = new Vector3(posX, pos.y, pos.z);
    }

    private void NullCheck()
    {
        musicPlayer.IsNull();
        rectTransform.IsNull();
        canvas.IsNull();
    }
}