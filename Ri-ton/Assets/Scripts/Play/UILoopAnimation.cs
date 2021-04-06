using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 2D画像をアニメーション表示する
/// </summary>
public class UILoopAnimation : MonoBehaviour
{
    [SerializeField]
    private float duration = 0.1f;
    [SerializeField]
    private Image image = null;
    [SerializeField]
    private Sprite[] sprites = null;

    private float time = 0.0f;
    private int index = 0;

    void Start()
    {
        if (image == null || sprites.Length == 0)
        {
            Debug.Log("nullを検知");
        }
    }

    void Update()
    {
        time += Time.deltaTime;

        if (time >= duration)
        {
            time = 0.0f;
            index = (index < sprites.Length - 1) ? index + 1 : 0;
            image.sprite = sprites[index];
        }
    }
}