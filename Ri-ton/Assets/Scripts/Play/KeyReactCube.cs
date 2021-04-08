using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// キー入力に応じてキューブを光らせる
/// </summary>
public class KeyReactCube : MonoBehaviour
{
    [SerializeField]
    float maxIntensity = 0.0f;
    [SerializeField]
    float minIntensity = 0.0f;

    public struct CubeBloom
    {
        public MeshRenderer renderer;
        public Color color;
        public float intensity;
        public int isHolding; // ノーツホールド中かどうか　ホールド中はレーンの数字を入れる　-1=未ホールド
    }

    [SerializeField]
    private GameObject[] cubeObjList;
    private CubeBloom[] cubeBloomList;

    void Start()
    {
        NullCheck();
        Array.Resize(ref cubeBloomList, cubeObjList.Length);

        for (int i = 0; i < cubeObjList.Length; i++)
        {
            cubeBloomList[i].renderer = cubeObjList[i].GetComponent<MeshRenderer>();
            cubeBloomList[i].color = cubeBloomList[i].renderer.material.GetColor("_EmissionColor");
            cubeBloomList[i].intensity = minIntensity;
            cubeBloomList[i].isHolding = -1;
        }
    }

    void Update()
    {
        for (int laneNum = 0; laneNum <= 3; laneNum++)
        {
            KeyCheck(laneNum);
        }

        for (int i = 0; i < cubeBloomList.Length; i++)
        {
            if (cubeBloomList[i].intensity >= minIntensity)
            {
                if (cubeBloomList[i].isHolding != -1)
                {
                    string key = "Lane" + cubeBloomList[i].isHolding.ToString();
                    if (Input.GetButton(key))
                    {
                        continue;
                    }
                    else
                    {
                        cubeBloomList[i].isHolding = -1;
                    }
                }

                cubeBloomList[i].intensity -= Time.deltaTime * 5.0f;
                float intensity = cubeBloomList[i].intensity;
                Color color = cubeBloomList[i].color;
                cubeBloomList[i].renderer.material.SetColor("_EmissionColor", new Color(color.r * intensity, color.g * intensity, color.b * intensity, color.a));
            }
            else
            {
                cubeBloomList[i].renderer.material.SetColor("_EmissionColor", new Color(0, 0, 0));
            }
        }
    }

    private void KeyCheck(int laneNum)
    {
        string key = "Lane" + laneNum.ToString();
        if (Input.GetButtonDown(key))
        {
            List<int> indexList = new List<int>();
            for (int i = 0; i < cubeBloomList.Length; i++)
            {
                indexList.Add(i);
            }

            for (int i = 0; i < cubeBloomList.Length; i++)
            {
                int randNum = UnityEngine.Random.Range(0, indexList.Count);
                int index = indexList[randNum];

                if (cubeBloomList[index].intensity <= minIntensity)
                {
                    cubeBloomList[index].intensity = maxIntensity;
                    cubeBloomList[index].isHolding = laneNum;

                    Color color = cubeBloomList[index].color;
                    cubeBloomList[index].renderer.material.SetColor("_EmissionColor", new Color(color.r * maxIntensity, color.g * maxIntensity, color.b * maxIntensity, color.a));
                    break;
                }
                else
                {
                    indexList.RemoveAt(randNum);//indexListのrandNum番目の要素を削除
                }
            }
        }
    }

    private void NullCheck()
    {
        if (cubeObjList.Length == 0)
        {
            Debug.LogError("cubeObjList is Null");
        }
    }
}
