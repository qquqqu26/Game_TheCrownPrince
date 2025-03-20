using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEditor.U2D.Aseprite;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;

public interface ILoader<Key, Value>
{
    //키와 값을 가진 Dictionary를 반환하는 메서드
    Dictionary<Key, Value> MakeDict(); 
}

public class DataManager 
{
    private const string googleSheetsBaseUrl =
        "https://script.google.com/macros/s/AKfycbxEiyQLkjWDytu2-BhtvFv920zSm8Iob9xuks4UEcQurm11md5y07GFGfVaqHxNI1CSxQ/exec";

    public StartData Start { get; private set; }
    public Dictionary<int, UITextData> UITexts { get; private set; } = new Dictionary<int, UITextData>();
    public Dictionary<int, CharacterTextData> CharacterTexts { get; private set; } = new Dictionary<int, CharacterTextData>();
    public Dictionary<int, PlayerStatData> PlayerStats { get; private set; } = new Dictionary<int, PlayerStatData>();

    public Dictionary<string, _CharacterData> Characters { get; private set; } = new Dictionary<string, _CharacterData>();
    public Dictionary<int, StoryData> Stories { get; private set; } = new Dictionary<int, StoryData>();
    public Dictionary<int, BranchData> Branches { get; private set; } = new Dictionary<int, BranchData>();

    string filePath = Application.streamingAssetsPath + "StartData";

    public IEnumerator Init()
    {

        TextAsset _textAsset = Managers.Resource.Load<TextAsset>($"2Data/StartData");
        Start = JsonUtility.FromJson<StartData>( _textAsset.text );

        if (Start == null)
            Debug.LogWarning("스타트 파일이 널이래");

        yield return Managers.StartCoroutineStatic(LoadSheetData("UITextData", (json) =>
        {
            UITexts = JsonUtility.FromJson<UITextDataLoader>(json).MakeDict();

        }));


        yield return Managers.StartCoroutineStatic(LoadSheetData("PlayerStatData", (json) =>
        {
            PlayerStats = JsonUtility.FromJson<PlayerStatDataLoader>(json).MakeDict();

        }));

        if (Managers.Instance.doesStorySkip == false)
        {
            yield return Managers.StartCoroutineStatic(LoadSheetData("CharacterTextData", (json) =>
            {
                CharacterTexts = JsonUtility.FromJson<CharacterTextDataLoader>(json).MakeDict();

            }));

            yield return Managers.StartCoroutineStatic(LoadSheetData("CharacterData", (json) =>
            {
                Characters = JsonUtility.FromJson<_CharacterDataLoader>(json).MakeDict();

            }));

            yield return Managers.StartCoroutineStatic(LoadSheetData("StoryData", (json) =>
            {
                Stories = JsonUtility.FromJson<StoryDataLoader>(json).MakeDict();
            }));

            yield return Managers.StartCoroutineStatic(LoadSheetData("BranchData", (json) =>
            {
                Branches = JsonUtility.FromJson<BranchDataLoader>(json).MakeDict();
            }));

        }

    }



    public IEnumerator LoadSheetData(string sheetName, Action<string> callback)
    {
        string url = googleSheetsBaseUrl + "?sheet=" + sheetName;
        UnityWebRequest request = UnityWebRequest.Get(url);

        yield return request.SendWebRequest();

        if(request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError($"[DataManager] {sheetName} 데이터 로드 실패: {request.error}");
            yield break;
        }

        string jsonData = request.downloadHandler.text;

        if (string.IsNullOrEmpty(jsonData))
        {
            Debug.LogError($"[DataManager] {sheetName} 데이터 로드 실패: 응답이 비어 있음.");
            yield break;
        }

        Debug.Log($"[DataManager] {sheetName} 데이터 로드 성공: {jsonData}"); // 시트 이름 로그 출력

        callback?.Invoke(jsonData);
    }

}
