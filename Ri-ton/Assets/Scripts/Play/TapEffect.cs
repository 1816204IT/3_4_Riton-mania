using UnityEngine;

/// <summary>
/// キー押下時の六角形エフェクト表示クラス
/// </summary>
public class TapEffect : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem particle = null;
    [SerializeField]
    private int laneNum = -1;

    void Start()
    {
        NullCheck();
    }

    void Update()
    {
        string lane = "Lane" + laneNum.ToString();
        if (Input.GetButtonDown(lane))
        {
            particle.transform.position = transform.position;
            particle.Emit(1);
        }
    }

    private void NullCheck()
    {
        particle.IsNull();
    }
}