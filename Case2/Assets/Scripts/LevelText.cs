using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class LevelText : MonoBehaviour
{
    private void OnEnable()
    {
        GameManager.LevelTextUpdate += NewLevel;
    }

    private void OnDisable()
    {
        GameManager.LevelTextUpdate -= NewLevel;
    }

    /// <summary>
    ///     Sets level text
    /// </summary>
    void NewLevel()
    {
        this.GetComponent<TextMeshProUGUI>().SetText("LEVEL " + GameManager.Instance.CurrentLevel);
    }
}
