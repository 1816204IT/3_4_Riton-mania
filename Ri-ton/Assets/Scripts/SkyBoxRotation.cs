using UnityEngine;

/// <summary>
/// スカイボックスを回転させる
/// </summary>
public class SkyBoxRotation : MonoBehaviour
{
    [SerializeField]
    private float anglePerFrame = 0.1f;    // 1フレームに何度回転するか
    private float rot = 0.0f;

    void Update()
    {
        rot += anglePerFrame;
        if (rot >= 360.0f)
        {
            rot -= 360.0f;
        }
        RenderSettings.skybox.SetFloat("_Rotation", rot);    // 回す
    }
}