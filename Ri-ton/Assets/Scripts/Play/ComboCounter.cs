using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// コンボ数管理クラス
/// </summary>
public class ComboCounter : MonoBehaviour
{
    private Text text = null;
    private int comboCnt = 0;
    private int maxCombo = 0;
    private int defauliFontSize = 0;
    private float fontSize = 0;

    void Start()
    {
        text = this.GetComponent<Text>();

        if (text == null)
        {
            Debug.Log("nullを検知");
        }

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

    public void AddCombo()
    {
        comboCnt++;
        maxCombo = (comboCnt > maxCombo) ? comboCnt : maxCombo;
        fontSize = defauliFontSize + 10;
        if (comboCnt > 9)
        {
            text.text = comboCnt.ToString();
        }
    }

    public void ComboZero()
    {
        comboCnt = 0;
        text.text = "";
    }

    public int _maxCombo
    {
        get { return maxCombo; }
    }
}
