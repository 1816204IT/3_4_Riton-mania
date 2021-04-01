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
    private ParticleSystem hexParticleLeft = null;
    [SerializeField]
    private ParticleSystem hexParticleRight = null;

    private Vector3 posLane0 = new Vector3(-4.0f, -3.0f, 0.0f);
    private Vector3 posLane1 = new Vector3(-1.3f, -3.0f, 0.0f);
    private Vector3 posLane2 = new Vector3(1.3f, -3.0f, 0.0f);
    private Vector3 posLane3 = new Vector3(4.0f, -3.0f, 0.0f);

    void Start()
    {
        NullCheck();
    }

    void Update()
    {
        if (Input.GetButtonDown("Lane0"))
        {
            hexParticleLeft.transform.position = posLane0;
            hexParticleLeft.Emit(1);
        }
        if (Input.GetButtonDown("Lane1"))
        {
            hexParticleLeft.transform.position = posLane1;
            hexParticleLeft.Emit(1);
        }
        if (Input.GetButtonDown("Lane2"))
        {
            hexParticleRight.transform.position = posLane2;
            hexParticleRight.Emit(1);
        }
        if (Input.GetButtonDown("Lane3"))
        {
            hexParticleRight.transform.position = posLane3;
            hexParticleRight.Emit(1);
        }
    }

    private void NullCheck()
    {
        hexParticleLeft.IsNull(nameof(hexParticleLeft));
        hexParticleRight.IsNull(nameof(hexParticleRight));
    }
}