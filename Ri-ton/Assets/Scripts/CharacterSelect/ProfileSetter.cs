using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProfileSetter : MonoBehaviour
{
    [SerializeField]
    private Text age = null;
    [SerializeField]
    private Text birthDay = null;
    [SerializeField]
    private Text like = null;
    [SerializeField]
    private Text illustrator = null;

    void Start()
    {
        if (age == null || birthDay == null || like == null || illustrator == null)
        {
            Debug.Log("nullを検知");
        }
    }

    public void UpdateProfile(Ritonmania.CharacterData data)
    {
        age.text = data.age.ToString();
        like.text = data.like;
        illustrator.text = data.illustrator;

        int month   = data.birthDay / 100;
        int day     = data.birthDay - (month * 100);
        birthDay.text = month.ToString("D2") + "月" + day.ToString("D2") + "日";

    }
}
