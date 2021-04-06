using UnityEngine;

/// <summary>
/// 指定のオブジェクトのサイズと同期する
/// </summary>
public class SyncSizeDelta : MonoBehaviour
{
    [SerializeField]
    private RectTransform targetRectTransform = null;
    private RectTransform myRectTransform;


    void Start()
    {
        myRectTransform = GetComponent<RectTransform>();
        NullCheck();
    }

    void Update()
    {
        myRectTransform.sizeDelta = targetRectTransform.sizeDelta;
    }

    private void NullCheck()
    {
        myRectTransform.IsNull(nameof(myRectTransform));
        targetRectTransform.IsNull(nameof(targetRectTransform));
    }
}
