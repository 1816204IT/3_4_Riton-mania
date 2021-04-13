using BrunoMikoski.TextJuicer;
using UnityEngine;

/// <summary>
/// 文字を1文字ずつアニメーションさせるクラス
/// </summary>
public class TextOneByOneAnimation : MonoBehaviour
{
    [System.Serializable]
    public struct JuiceInfo
    {
        public bool isStarted { get; set; }
        public float startDelay;
        public JuicedText juiceText;
    }

    [SerializeField]
    private JuiceInfo[] juiceInfos;

    private float time = 0.0f;
    private int startedCnt = 0;

    private void Start()
    {
        for (int i = 0; i < juiceInfos.Length; i++)
        {
            juiceInfos[i].isStarted = false;
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
            if (juiceInfos[i].isStarted)
            {
                continue;
            }

            if (time >= juiceInfos[i].startDelay)
            {
                juiceInfos[i].juiceText.Play();
                juiceInfos[i].isStarted = true;
                startedCnt++;
            }
        }
    }
}
