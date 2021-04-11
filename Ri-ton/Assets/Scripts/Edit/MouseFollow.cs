using UnityEngine;

/// <summary>
/// オブジェクトをマウスに追従させるクラス
/// </summary>
public class MouseFollow : MonoBehaviour
{
    //X, Y座標の移動可能範囲
    [System.Serializable]
    public class Bounds
    {
        public float xMin, xMax, yMin, yMax;
    }

    [SerializeField] 
    private Bounds bounds = null;
    [SerializeField]
    private LayerMask layerMask = 0;

    private NoteEdit noteEditor = null;
    private NoteDataConverter noteDataConverter = null;

    void Start()
    {
        noteEditor = GameObject.FindGameObjectWithTag("NoteEditor").GetComponent<NoteEdit>();
        NullCheck();
    }

    void Update()
    {
        //マウス座標を取得
        var targetPos = ConvertMousePos();

        if (targetPos == new Vector3())
        {
            return;
        }

        //X,Y座標の範囲を制限する
        targetPos.x = Mathf.Clamp(targetPos.x, (float)bounds.xMin, bounds.xMax);
        targetPos.y = Mathf.Clamp(targetPos.y, bounds.yMin, bounds.yMax);

        //このスクリプトがアタッチされたゲームオブジェクトを、マウス位置に追従
        //ノーツが置ける位置にスナップさせる
        Vector3 pos = noteEditor.GetSnappedPos(targetPos);
        transform.position = pos;

        float time = 0.0f;
        noteEditor.IsClickedPosValid(ref pos, ref time);
    }

    //マウス座標をプレイエリア平面座標に変換する
    public Vector3 ConvertMousePos()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 3000.0f, layerMask))
        {
            return hit.point;
        }
        return new Vector3();
    }

    //マウスの座標がプレイエリア上にあるかどうか
    public bool IsMousePosValid()
    {
        if (ConvertMousePos() == new Vector3())
        {
            return false;
        }
        return true;
    }

    public Vector3 GetMouseFollowNotePos()
    {
        return transform.position;
    }

    private void NullCheck()
    {
        noteEditor.IsNull();
    }
}