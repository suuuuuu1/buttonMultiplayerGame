using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement; // シーン移動用
using System.Text.RegularExpressions; // 正規表現（文字チェック）用

public class TitleManager : MonoBehaviour
{
    public TMP_InputField nameInput;
    public TextMeshProUGUI errorText;

    public void OnStartButtonClick()
    {
        string inputName = nameInput.text;

        // 1. 空欄チェック
        if (string.IsNullOrEmpty(inputName))
        {
            errorText.text = "input name";
            return;
        }

        // 2. 文字数チェック（念のため）
        if (inputName.Length > 8)
        {
            errorText.text = "max 8";
            return;
        }

        // 3. ローマ字（半角英字）チェック
        // Regex.IsMatch: このパターンに当てはまるか調べる
        // "^[a-zA-Z]+$": 先頭から末尾まで全部アルファベット（大文字小文字）という意味
        if (!Regex.IsMatch(inputName, "^[a-zA-Z]+$"))
        {
            errorText.text = "ro-maji only";
            return;
        }

        // --- ここまで来たら合格！ ---

        // 名前を NetworkManager に保存する（後述）
        if (sendScore.Instance != null)
        {
            sendScore.Instance.currentPlayerName = inputName;
        }

        // ゲーム画面へ移動（シーン名は合わせる）
        SceneManager.LoadScene("SampleScene");
    }
}