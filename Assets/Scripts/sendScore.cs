using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Text;
using System; // Actionを使うために必要

// ▼ 通信用のデータ設計図（クラスの外に書く）
[System.Serializable]
public class ScoreData
{
    public string name;
    public int score;
}

[System.Serializable]
public class RankingResponse
{
    public ScoreData[] result;
}

// ▼ マネージャークラス本体
public class sendScore : MonoBehaviour
{
    // シングルトン設定（どこからでも呼べるようにする）
    public static sendScore Instance;

    public string currentPlayerName = "NoName";
    // サーバーのURL（自分の環境に合わせて書き換えてください）
    private string serverUrl = "http://localhost:8080/ranking";

    private void Awake()
    {
        // シーン遷移しても壊れないようにする
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // ■ スコア送信機能 (POST)
    public void SendScore(int score, string playerName = "Player")
    {
        StartCoroutine(PostScoreCoroutine(score, playerName));
    }

    IEnumerator PostScoreCoroutine(int score, string playerName)
    {
        // 1. 送るデータを作成
        ScoreData data = new ScoreData();
        data.name = playerName;
        data.score = score;

        // 2. JSONに変換
        string json = JsonUtility.ToJson(data);

        // 3. リクエスト作成
        using (UnityWebRequest request = new UnityWebRequest(serverUrl, "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("送信成功: " + request.downloadHandler.text);
            }
            else
            {
                Debug.LogError("送信失敗: " + request.error);
            }
        }
    }

    // ■ ランキング取得機能 (GET)
    public void GetRanking(Action<RankingResponse> callback)
    {
        StartCoroutine(GetRankingCoroutine(callback));
    }

    IEnumerator GetRankingCoroutine(Action<RankingResponse> callback)
    {
        using (UnityWebRequest request = UnityWebRequest.Get(serverUrl))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("受信データ: " + request.downloadHandler.text);

                // JSONを箱に入れて、呼び出し元に渡す
                RankingResponse res = JsonUtility.FromJson<RankingResponse>(request.downloadHandler.text);
                callback(res);
            }
            else
            {
                Debug.LogError("取得失敗: " + request.error);
            }
        }
    }
}