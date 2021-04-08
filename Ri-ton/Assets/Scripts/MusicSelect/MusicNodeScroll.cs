using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

/// <summary>
/// 曲選択画面で曲アイコンを横スクロールさせる
/// </summary>
public class MusicNodeScroll : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private AudioSource menuHitSE = null;
    [SerializeField]
    private float midSpacint = 0.0f;
    [SerializeField]
    private float spacing = 0.0f;
    [SerializeField]
    private float smallScall = 0.0f;
    [SerializeField]
    private float moveCompleteTime = 0.0f;
    [SerializeField]
    private int max_scroll_input_num = 0; // scrollInputListに記憶させる入力情報の最大数
    [SerializeField]
    private ScoreView scoreView = null;
    [SerializeField]
    private GameObject tutorialCanvas = null;

    private float nodeWidth = 0.0f;
    private bool isMoving = false;

    private enum ScrollDir
    {
        LEFT,
        RIGHT
    }
    private List<ScrollDir> scrollInputList = new List<ScrollDir>(); // 入力されたマウスホイールの情報を記憶する

    private struct MovementInfo
    {
        public Vector3 movedPos;    // 最終到達座標
        public Vector3 addPos;      // 加算する移動量
        public Vector3 movedScale;  // 最終拡大率
        public Vector3 addScale;    // 加算する拡大率
    }

    private List<RectTransform> nodeList = new List<RectTransform>();
    private List<MovementInfo> movementInfoList = new List<MovementInfo>();
    private BigNodeInformation bigNode = null;
    private MusicSelect musicSelect = null;
    private bool isOnPointerEnter = false;

    private void Start()
    {
        nodeList = this.GetComponent<MusicNodeInstance>().nodeRectTransformList;
        bigNode = GameObject.FindGameObjectWithTag("BigNode").GetComponent<BigNodeInformation>();
        musicSelect = GameObject.FindGameObjectWithTag("MusicSelect").GetComponent<MusicSelect>();

        {
            Debug.Log("nullを検知");
        }
        if (SceneManager.GetActiveScene().name == "PlaySongSelect")
        {
            if (scoreView == null)
            {
                Debug.Log("nullを検知");
            }
        }

        nodeWidth = nodeList[0].sizeDelta.x * smallScall;
        SelectedNodeChangesFunc(SelectedMap.instance.musicIndex, SelectedMap.instance.musicName);
    }

    private void Update()
    {
        // マウスホイールの入力情報をmovementInfoListに格納していく
        AddScrollInput();

        // movementInfoListを元に次の移動先を決定
        ExcuteMovementInfoList();

        // ノードの移動
        NodeMove();
    }

    private void AddScrollInput()
    {
        if (tutorialCanvas.activeSelf == true)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            AddRightScrollInput();
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            AddLeftScrollInput();
        }

        if (isOnPointerEnter == false)
        {
            return;
        }

        float scrollValue = Input.GetAxis("Mouse ScrollWheel");
        if (scrollValue < 0.0f)
        {
            AddLeftScrollInput();
        }
        if (scrollValue > 0.0f)
        {
            AddRightScrollInput();
        }
    }

    // movementInfoListを元に次の移動先を決定
    private void ExcuteMovementInfoList()
    {
        if (isMoving || (scrollInputList.Count == 0))
        {
            return;
        }

        int musicIndex = SelectedMap.instance.musicIndex;
        for (int i = 0; i < scrollInputList.Count; i++)
        {
            ScrollDir dir = scrollInputList[i];
            scrollInputList.RemoveAt(i);
            if (dir == ScrollDir.LEFT)
            {
                if (musicIndex < MusicInfoList.instance.MusicNum() - 1)
                {
                    musicIndex++;
                    SelectedNodeChangesFunc(musicIndex, MusicInfoList.instance.GetMusicName(musicIndex));
                    break;
                }
            }
            else if (dir == ScrollDir.RIGHT)
            {
                if (musicIndex > 0)
                {
                    musicIndex--;
                    SelectedNodeChangesFunc(musicIndex, MusicInfoList.instance.GetMusicName(musicIndex));
                    break;
                }
            }
        }
    }

    // ノードの移動を行う
    private void NodeMove()
    {
        if (isMoving == false)
        {
            return;
        }

        int musicNum = MusicInfoList.instance.MusicNum();
        int moveEndNodeCnt = 0;
        for (int i = 0; i < musicNum; i++)
        {
            Vector3 nowPos = nodeList[i].localPosition;
            Vector3 nowScale = nodeList[i].localScale;
            if (AddPos(ref nowPos, movementInfoList[i].movedPos, movementInfoList[i].addPos))
            {
                moveEndNodeCnt++;
            }
            AddScale(ref nowScale, movementInfoList[i].movedScale, movementInfoList[i].addScale);
            nodeList[i].localPosition = nowPos;
            nodeList[i].localScale = nowScale;
        }

        if (moveEndNodeCnt >= musicNum)
        {
            isMoving = false;
            //曲を再生
            musicSelect.SetNewMusic(SelectedMap.instance.musicIndex);
            //ランキング更新
            if (SceneManager.GetActiveScene().name == "PlaySongSelect")
            {
                scoreView.UpdateResultData();
            }
        }
    }

    //返り値 true 最終座標まで移動完了
    private bool AddPos(ref Vector3 nowPos, Vector3 endPos, Vector3 addPos)
    {
        float sing = endPos.x - nowPos.x;   //右にスクロールしているか左にスクロールしているか符号で判断する
        nowPos += addPos * Time.deltaTime;

        //右にスクロールしている場合
        if (sing > 0.0f)
        {
            if (nowPos.x >= endPos.x)
            {
                nowPos = endPos;
                return true;
            }
        }
        //左にスクロールしている場合
        else
        {
            if (nowPos.x <= endPos.x)
            {
                nowPos = endPos;
                return true;
            }
        }

        return false;
    }

    private void AddScale(ref Vector3 nowScale, Vector3 endScale, Vector3 addScale)
    {
        float sing = endScale.x - nowScale.x;   //右にスクロールしているか左にスクロールしているか符号で判断する
        nowScale += addScale * Time.deltaTime;

        //右にスクロールしている場合
        if (sing > 0.0f)
        {
            if (nowScale.x >= endScale.x)
            {
                nowScale = endScale;
            }
        }
        //左にスクロールしている場合
        else
        {
            if (nowScale.x <= endScale.x)
            {
                nowScale = endScale;
            }
        }
    }

    public void SelectedNodeChangesFunc(int musicIndex, string musicName)
    {
        if (isMoving)
        {
            return;
        }

        SelectedMap.instance.musicIndex = musicIndex;
        SelectedMap.instance.musicName = musicName;
        //大画面情報の更新
        bigNode.InformationUpdate();
        //ノードの移動情報をセットする
        SetMovementInfo();
        //SEを再生
        menuHitSE.Play();      
    }

    public void SetMovementInfo()
    {
        movementInfoList.Clear();   // リストクリア
        isMoving = true;
        float posX = 0;
        int musicNum = MusicInfoList.instance.MusicNum();

        for (int i = 0; i < musicNum; i++)
        {
            MovementInfo info = new MovementInfo();
            
            //座標の移動
            int distanceFromMedian = i - SelectedMap.instance.musicIndex;
            posX = nodeWidth * distanceFromMedian;
            posX += spacing * distanceFromMedian;
            if (distanceFromMedian > 0)
            {
                posX += midSpacint;
            }
            else if (distanceFromMedian < 0)
            {
                posX -= midSpacint;
            }
            Vector3 nowPos = nodeList[i].localPosition;
            info.movedPos = new Vector3(posX, nowPos.y, nowPos.z);
            info.addPos = (info.movedPos - nowPos) * (1.0f / moveCompleteTime);

            //スケーリング
            if (i == SelectedMap.instance.musicIndex)
            {
                info.movedScale = Vector3.one;
            }
            else
            {
                info.movedScale = new Vector3(smallScall, smallScall, 1.0f);
            }
            Vector3 nowScale = nodeList[i].localScale;
            info.addScale = (info.movedScale - nowScale) * (1.0f / moveCompleteTime);

            //リストに追加する
            movementInfoList.Add(info);
        }
    }

    private void AddLeftScrollInput()
    {
        if (scrollInputList.Count >= max_scroll_input_num - 1)
        {
            scrollInputList.RemoveAt(0);
        }
        scrollInputList.Add(ScrollDir.LEFT);
    }

    private void AddRightScrollInput()
    {
        if (scrollInputList.Count >= max_scroll_input_num - 1)
        {
            scrollInputList.RemoveAt(0);
        }
        scrollInputList.Add(ScrollDir.RIGHT);
    }

    public void OnClickLeftArrow()
    {
        if (isMoving)
        {
            return;
        }

        // 左矢印ボタン押下時は右にスクロールさせる
        AddRightScrollInput();
    }

    public void OnClickRightArrow()
    {
        if (isMoving)
        {
            return;
        }

        // 右矢印ボタン押下時は左にスクロールさせる
        AddLeftScrollInput();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isOnPointerEnter = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isOnPointerEnter = false;
    }

    private void NullCheck()
    {
        if (nodeList.Count == 0)
        {
            Debug.LogError("nodeList is Null");
        }

        bigNode.IsNull();
        musicSelect.IsNull();
        menuHitSE.IsNull();
        tutorialCanvas.IsNull();
    }
}
