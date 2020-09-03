using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexagonRotation : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer sprite1 = null;
    [SerializeField]
    private SpriteRenderer sprite2 = null;

    [SerializeField]
    private float stopTime = 0.5f;
    [SerializeField]
    private int cntNum = 5;
    [SerializeField]
    private float spriteAlpha = 0.5f;

    private const float threeGearForm   = 37.5f;    // 3個の歯車
    private const float RhombusForm     = 60.0f;    // 無数のひし形
    private const float threeGearForm2  = 81.5f;    // 3個の歯車
    private const float RhombusForm2    = 120.0f;   // 無数のひし形

    private List<float> rotList = new List<float>();
    private List<Color> colorList = new List<Color>();
    private int idx = 0;
    private float addRot = 0.0f;
    private float nextStopRot = 0.0f;
    private float stopTimer = 0.0f;
    private float time = 0.0f;
    private int rotAddCnt = 0;

    private void Start()
    {
        rotList.Add(threeGearForm);
        rotList.Add(RhombusForm);
        rotList.Add(threeGearForm2);
        rotList.Add(RhombusForm2);

        colorList.Add(Color.red);
        colorList.Add(Color.green);
        colorList.Add(Color.red);
        colorList.Add(Color.green);

        nextStopRot = rotList[idx];

        addRot = nextStopRot / (float)cntNum;
    }

    private void Update()
    {
        if (stopTimer > 0.0f)
        {
            stopTimer -= Time.deltaTime;
            return;
        }
        else
        {
            time += Time.deltaTime;
            ChangeSpriteColor(Color.gray, spriteAlpha);
        }

        Vector3 tmpRot = transform.eulerAngles;
        if (time >= 1.0f)
        {
            time = 0.0f;
            tmpRot.z += addRot;
            rotAddCnt++;
        }
        else
        {
            return;
        }

        if (rotAddCnt >= cntNum)
        {
            rotAddCnt = 0;
            ChangeSpriteColor(colorList[idx], spriteAlpha);
            stopTimer = stopTime;
            transform.rotation = Quaternion.Euler(0.0f, 0.0f, nextStopRot);
            idx = (idx < rotList.Count - 1) ? ++idx : 0;
            addRot = (rotList[idx] - nextStopRot) / (float)cntNum;
            nextStopRot = rotList[idx];

            if (idx == 0)
            {
                transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
                addRot = nextStopRot / (float)cntNum;
                ChangeSpriteColor(Color.blue, spriteAlpha);
            }
            
            return;
        }

        transform.rotation = Quaternion.Euler(0.0f, 0.0f, tmpRot.z);
    }

    private void ChangeSpriteColor(Color color, float alpha)
    {
        sprite1.color = new Color(color.r, color.g, color.b, alpha);
        sprite2.color = new Color(color.r, color.g, color.b, alpha);
    }
}
