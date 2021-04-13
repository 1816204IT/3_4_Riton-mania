using UnityEngine;

/// <summary>
/// 便利関数群クラス
/// </summary>
static public class Utility
{
    // Nullチェック関数
    // スクリプト専用
    static public bool IsNull<T>(this T target)
    {
        if (target == null)
        {
            Debug.LogError("target is Null");
            return true;
        }
        return false;
    }

    // Nullチェック関数
    // GameObject型専用
    static public bool IsNull(this GameObject obj)
    {
        if (obj == null)
        {
            Debug.LogError("object is Null");
            return true;
        }
        return false;
    }
}
