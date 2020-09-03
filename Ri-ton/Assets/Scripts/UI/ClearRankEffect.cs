using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClearRankEffect : MonoBehaviour
{
    private bool isEnable = false;

    private RectTransform rt;
    [SerializeField]
    private float startScale = 7.0f;
    [SerializeField]
    private float subScale = 0.07f;

    private Image image;
    private float alpha = 0.0f;
    private float addAlpha = 0.0f;

    [SerializeField]
    private Animator animator = null;

    void Start()
    {
        rt = this.GetComponent<RectTransform>();
        image = this.GetComponent<Image>();

        if (rt == null || image == null || animator == null)
        {
            Debug.Log("nullを検知");
        }

        image.color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            StartEffect();
        }

        if (isEnable == false)
        {
            return;
        }

        if (startScale >= 1.0f + subScale) 
        {
            startScale = startScale - subScale;
            rt.transform.localScale = new Vector3(startScale, startScale, startScale);

            alpha += addAlpha;
            image.color = new Color(1.0f, 1.0f, 1.0f, alpha);
        }
        else
        {
            rt.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            image.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            isEnable = false;

            animator.SetBool("isStart", true);
        }
    }

    public void StartEffect()
    {
        isEnable = true;
        rt.transform.localScale = new Vector3(startScale, startScale, startScale); ;
        addAlpha = 1.0f / ((startScale - 1.0f) / subScale);
    }
}
