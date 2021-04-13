using UnityEngine;
using System.IO;
using NoteEditor.DTO;
using System.Text;

/// <summary>
/// Jsonファイルの読み込みと書き込みを行うクラス
/// </summary>
public class JsonManager : MonoBehaviour
{
    private StringBuilder PathBuilder;

    private void Start()
    {
        PathBuilder = new StringBuilder();
    }

    private void CreatePathBuilder()
    {
        if (PathBuilder == null)
        {
            PathBuilder = new StringBuilder();
        }
    }

    //曲別に譜面データをセーブ
    public void SaveMapInfo(MapInfo mapInfo)
    {
        CreatePathBuilder();

        PathBuilder.Clear();
        PathBuilder.AppendFormat("MapData/{0}/{0}.json", mapInfo.musicName);

        StreamWriter writer = new StreamWriter(PathBuilder.ToString(), false);
        string jsonStr = JsonUtility.ToJson(mapInfo);
        writer.Write(jsonStr);
        writer.Flush();
        writer.Close();
    }

    //曲別に譜面データのロード
    public MapInfo LoadMapInfo(string musicName)
    {
        CreatePathBuilder();

        PathBuilder.Clear();
        PathBuilder.AppendFormat("MapData/{0}", musicName);

        string directoryPath = PathBuilder.ToString();

        //フォルダが存在しない場合は作成する
        if (Directory.Exists(directoryPath) == false)
        {
            Directory.CreateDirectory(directoryPath);
        }

        PathBuilder.Clear();
        PathBuilder.AppendFormat("{0}/{1}.json", directoryPath, musicName);

        string filePath = PathBuilder.ToString();

        //ファイルが存在しない場合は作成する
        if (File.Exists(filePath) == false)
        {
            MapInfo mapInfo = new MapInfo();
            mapInfo.musicName = musicName;
            SaveMapInfo(mapInfo);
        }

        StreamReader reader = new StreamReader(filePath, System.Text.Encoding.GetEncoding("utf-8"));
        string readData = reader.ReadToEnd();
        reader.Close();
        return JsonUtility.FromJson<MapInfo>(readData);
    }

    //難易度別にノーツデータをセーブ
    public void SaveNoteData(MusicDTO.MapData mapData, string musicName, string difficultyName)
    {
        CreatePathBuilder();

        PathBuilder.Clear();
        PathBuilder.AppendFormat("MapData/{0}/{0}[{1}].json", musicName, difficultyName);

        StreamWriter writer = new StreamWriter(PathBuilder.ToString(), false);
        string json = JsonUtility.ToJson(mapData);
        writer.Write(json);
        writer.Flush();
        writer.Close();
    }

    //難易度別にノーツデータをロード
    public MusicDTO.MapData LoadMapData(string musicName, string difficultyName)
    {
        CreatePathBuilder();

        PathBuilder.Clear();
        PathBuilder.AppendFormat("MapData/{0}", musicName);

        string directoryPath = PathBuilder.ToString();

        //フォルダが存在しない場合は作成する
        if (Directory.Exists(directoryPath) == false)
        {
            Directory.CreateDirectory(directoryPath);
        }

        PathBuilder.Clear();
        PathBuilder.AppendFormat("{0}/{1}[{2}].json", directoryPath, musicName, difficultyName);

        string filePath = PathBuilder.ToString();

        //ファイルが存在しない場合は作成する
        if (File.Exists(filePath) == false)
        {
            MusicDTO.MapData mapData = new MusicDTO.MapData();
            SaveNoteData(mapData, SelectedMap.Instance.MusicName, SelectedMap.Instance.DifficultyName);
        }

        StreamReader reader = new StreamReader(filePath, System.Text.Encoding.GetEncoding("utf-8"));
        string readData = reader.ReadToEnd();
        reader.Close();
        return JsonUtility.FromJson<MusicDTO.MapData>(readData);
    }

    // ユーザー設定の保存
    public void SaveUserPreference(Ritonmania.LocalUserData data)
    {
        CreatePathBuilder();

        PathBuilder.Clear();
        PathBuilder.AppendFormat("UserPreference/UserPreference.json");

        StreamWriter writer = new StreamWriter(PathBuilder.ToString(), false);
        string jsonStr = JsonUtility.ToJson(data);
        writer.Write(jsonStr);
        writer.Flush();
        writer.Close();
    }

    // ユーザー設定の読み込み1
    public Ritonmania.LocalUserData LoadUserPreference()
    {
        CreatePathBuilder();

        PathBuilder.Clear();
        PathBuilder.AppendFormat("UserPreference");

        string directoryPath = PathBuilder.ToString();

        //フォルダが存在しない場合は作成する
        if (Directory.Exists(directoryPath) == false)
        {
            Directory.CreateDirectory(directoryPath);
        }

        PathBuilder.Clear();
        PathBuilder.AppendFormat("{0}/UserPreference.json", directoryPath);

        string filePath = PathBuilder.ToString();

        //ファイルが存在しない場合は作成する
        if (File.Exists(filePath) == false)
        {
            Ritonmania.LocalUserData data = new Ritonmania.LocalUserData();
            SaveUserPreference(data);
        }

        StreamReader reader = new StreamReader(filePath);
        string readData = reader.ReadToEnd();
        reader.Close();
        return JsonUtility.FromJson<Ritonmania.LocalUserData>(readData);
    }
}