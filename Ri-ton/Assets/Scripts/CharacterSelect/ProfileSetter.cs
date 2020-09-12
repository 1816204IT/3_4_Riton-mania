using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProfileSetter : MonoBehaviour
{
    [SerializeField]
    private Text height = null;
    [SerializeField]
    private Text age = null;
    [SerializeField]
    private Text birthDay = null;
    [SerializeField]
    private Text personality = null;
    [SerializeField]
    private Text likes = null;
    [SerializeField]
    private Text unLikes = null;
    [SerializeField]
    private Text illustrator = null;

    void Start()
    {
        if (age == null || birthDay == null || likes == null || illustrator == null)
        {
            Debug.Log("nullを検知");
        }
    }

    public void UpdateProfile(Ritonmania.CharacterData data)
    {
        height.text         = "  " + data.height;
        age.text            = "  " + data.age;
        birthDay.text       = "  " + data.birthDay;
        personality.text    = "  " + data.personality;
        likes.text          = "  " + data.likes;
        unLikes.text        = "  " + data.unLikes;
        illustrator.text    = "  " + data.illustrator;
    }
}
