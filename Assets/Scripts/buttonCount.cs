using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement; // シーン移動に必要

public class buttonCount : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI countDownText;

    private int score = 0;
    private float timeLeft = 5.0f;
    private bool isGameActive = false;
    private bool isEnded = false;

    // ボタンを押した時の処理
    public void OnButtonClick()
    {
        if (isEnded) return;

        // 初回クリックでゲーム開始
        if (!isGameActive)
        {
            isGameActive = true;
        }

        score++;
        scoreText.text = "Score: " + score;
    }

    private void Update()
    {
        if (isGameActive && !isEnded)
        {
            timeLeft -= Time.deltaTime;
            countDownText.text = "Time Left: " + Mathf.Max(0, timeLeft).ToString("F1");

            if (timeLeft <= 0)
            {
                EndGame();
            }
        }
    }

    void EndGame()
    {
        isGameActive = false;
        isEnded = true;
        countDownText.text = "Time's Up!";

        // サーバーにスコアを送る（名前は適当にYamagataにしています）
        if (sendScore.Instance != null)
        {
            string nameToSend = sendScore.Instance.currentPlayerName;
            sendScore.Instance.SendScore(score, nameToSend);
        }
        else
        {
            Debug.LogWarning("NetworkManagerがありません！配置してください。");
        }

        // 3秒後にランキングシーンへ
        StartCoroutine(WaitAndMoveScene());
    }

    IEnumerator WaitAndMoveScene()
    {
        yield return new WaitForSeconds(3.0f);
        SceneManager.LoadScene("Score"); // シーン名は正確に！
    }
}