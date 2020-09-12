using BrunoMikoski.TextJuicer;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// 文字を1文字ずつアニメーションさせるクラス
public class Text_OneByOne_Animation : MonoBehaviour
{
    [System.Serializable]
    public struct JuiceInfo
    {
        public float startDelay;
        public JuicedText juiceText;
        private bool isStarted;

        public bool _isStarted
        { 
            set { isStarted = value; }
            get { return isStarted; }
        }
    }

    [SerializeField]
    private JuiceInfo[] juiceInfos;

    private float time = 0.0f;
    private int startedCnt = 0;

    private void Start()
    {
        for (int i = 0; i < juiceInfos.Length; i++)
        {
            juiceInfos[i]._isStarted = false;
        }
    }

    void Update()
    {
        if (startedCnt >= juiceInfos.Length)
        {
            return;
        }

        time += Time.deltaTime;

        for (int i = 0; i < juiceInfos.Length; i++)
        {
            if (juiceInfos[i]._isStarted)
            {
                continue;
            }

            if (time >= juiceInfos[i].startDelay)
            {
                juiceInfos[i].juiceText.Play();
                juiceInfos[i]._isStarted = true;
                startedCnt++;
            }
        }
    }
}
