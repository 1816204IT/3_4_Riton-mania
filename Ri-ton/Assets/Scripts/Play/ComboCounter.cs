using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// コンボ数管理クラス
/// </summary>
public class ComboCounter : MonoBehaviour
{
    public int MaxCombo { get; private set; } = 0;

    private Text text = null;
    private int comboCnt = 0;
    private int defauliFontSize = 0;
    private float fontSize = 0;

    void Start()
    {
        text = this.GetComponent<Text>();
        NullCheck();
        defauliFontSize = text.fontSize;
        fontSize = defauliFontSize;
    }

    private void Update()
    {
        if (fontSize > defauliFontSize)
        {
            fontSize -= Time.deltaTime * 100;
        }
        text.fontSize = (int)fontSize;
    }

    /// <summary>
    /// コンボ追加
    /// </summary>
    public void AddCombo()
    {
        comboCnt++;
        MaxCombo = (comboCnt > MaxCombo) ? comboCnt : MaxCombo;
        fontSize = defauliFontSize + 10;
        if (comboCnt > 9)
        {
            text.text = comboCnt.ToString();
        }
    }

    /// <summary>
    /// コンボ数をゼロで初期化する
    /// </summary>
    public void ComboZero()
    {
        comboCnt = 0;
        text.text = "";
    }

    private void NullCheck()
    {
        text.IsNull();
    }
}
