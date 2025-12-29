using UnityEngine;
using TMPro;
using System.Collections.Generic;
// ▼▼▼ これを追加！（シーン移動に必要） ▼▼▼
using UnityEngine.SceneManagement;
// ▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲

public class RankingManager : MonoBehaviour
{
    public GameObject rankingItemPrefab;
    public Transform contentParent;

    void Start()
    {
        if (sendScore.Instance != null)
        {
            sendScore.Instance.GetRanking(ShowRanking);
        }
    }

    void ShowRanking(RankingResponse response)
    {
        foreach (Transform child in contentParent)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < response.result.Length; i++)
        {
            ScoreData data = response.result[i];
            GameObject item = Instantiate(rankingItemPrefab, contentParent);
            TextMeshProUGUI textComp = item.GetComponent<TextMeshProUGUI>();

            if (textComp != null)
            {
                textComp.text = $"{i + 1}. {data.name} - {data.score}pt";
            }
        }
    }

    // ▼▼▼ この関数を追加！ ▼▼▼
    public void OnRetryButtonClick()
    {
        // ゲーム画面（SampleScene）に戻る
        // ※あなたのゲーム画面のシーン名に合わせてください
        SceneManager.LoadScene("SampleScene");
    }

    public void OnTitleButtonClick()
    {
        // タイトル画面に戻る場合
        // ※あなたのタイトル画面のシーン名に合わせてください
        SceneManager.LoadScene("Name");
    }
    // ▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲
}