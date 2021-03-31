using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// キー押下時の六角形エフェクト表示クラス
/// </summary>
public class TapEffect : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem hexParticle_L = null;
    [SerializeField]
    private ParticleSystem hexParticle_R = null;
    [SerializeField]
    private Camera p_camera = null; // パーティクル専用カメラ

    private Vector3 pos_lane_0 = new Vector3(-4.0f, -3.0f, 0.0f);
    private Vector3 pos_lane_1 = new Vector3(-1.3f, -3.0f, 0.0f);
    private Vector3 pos_lane_2 = new Vector3(1.3f, -3.0f, 0.0f);
    private Vector3 pos_lane_3 = new Vector3(4.0f, -3.0f, 0.0f);

    void Start()
    {
        NullCheck();
    }

    void Update()
    {
        if (Input.GetButtonDown("Lane0"))
        {
            hexParticle_L.transform.position = pos_lane_0;
            hexParticle_L.Emit(1);
        }
        if (Input.GetButtonDown("Lane1"))
        {
            hexParticle_L.transform.position = pos_lane_1;
            hexParticle_L.Emit(1);
        }
        if (Input.GetButtonDown("Lane2"))
        {
            hexParticle_R.transform.position = pos_lane_2;
            hexParticle_R.Emit(1);
        }
        if (Input.GetButtonDown("Lane3"))
        {
            hexParticle_R.transform.position = pos_lane_3;
            hexParticle_R.Emit(1);
        }
    }

    private void NullCheck()
    {
        hexParticle_L.IsNull(nameof(hexParticle_L));
        hexParticle_R.IsNull(nameof(hexParticle_R));
        p_camera.IsNull(nameof(p_camera));
    }
}