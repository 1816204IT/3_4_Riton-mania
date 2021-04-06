using UnityEngine;

/// <summary>
/// 便利関数群クラス
/// </summary>
static public class Utility
{
    // Nullチェック関数
    // スクリプト専用
    static public bool IsNull<T>(this T target, string scriptName)
    {
        if (target == null)
        {
            Debug.LogError(scriptName + " is Null");
            return true;
        }
        return false;
    }

    // Nullチェック関数
    // GameObject型専用
    static public bool IsNull(this GameObject obj, string objectName)
    {
        if (obj == null)
        {
            Debug.LogError(objectName + " is Null");
            return true;
        }
        return false;
    }
}
