using System.Collections.Generic;

/// <summary>
/// ノーツデータクラス
/// </summary>
namespace NoteEditor.DTO
{
    [System.Serializable]
    public class MusicDTO
    {
        [System.Serializable]
        public class MapData
        {
            public string mapperName;   // 譜面制作者名
            public int difficulty;      // 難易度名
            public List<Note> noteList; // ノーツ情報
        }

        // ノーツ情報クラス
        [System.Serializable]
        public class Note
        {
            public int LPB;             // Lines Per Beat 1ビート(1/4)を何分割するか
            public int num;             // LPB4,num14なら3拍目+2/4拍子目
            public int lane;            // レーン番号
            public int type;            // 0なら単推し　1ならロングノーツの始点　2ならロングノーツの終点　3ならロングノーツの中割ノーツ
            public bool isJudgment;     // 判定し終えたかどうか　プレイ時に使用する
            public bool isLongNote;     // 判定する際にロングノーツは単発ノーツに分解する　もともとロングノーツだった場合はtrueとする プレイ時に使用する
            public List<Note> endNote;  // type=1の場合のみ有効
        }
    }
}