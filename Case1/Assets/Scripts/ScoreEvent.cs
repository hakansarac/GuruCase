using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class ScoreEvent : MonoBehaviour
{
    private void OnEnable()
    {
        GameManager.ScoreUpdate += NewScore;
    }

    private void OnDisable()
    {
        GameManager.ScoreUpdate -= NewScore;
    }

    
    /// <summary>
    ///     Sets score text
    /// </summary>
    void NewScore()
    {
        this.GetComponent<TextMeshProUGUI>().SetText("SCORE: "+ GameManager.Instance.Score);
    }
}
