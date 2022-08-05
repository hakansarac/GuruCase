using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class GoldText : MonoBehaviour
{
    //amount of gold
    int m_Gold;
    private void Start()
    {
        m_Gold = PlayerPrefs.GetInt("gold");
        this.GetComponent<TextMeshProUGUI>().SetText(m_Gold.ToString());
    }

    private void OnEnable()
    {
        GameManager.GoldTextUpdate += NewGold;
    }

    private void OnDisable()
    {
        GameManager.GoldTextUpdate -= NewGold;
    }

    /// <summary>
    ///     Sets gold text
    /// </summary>
    void NewGold()
    {
        this.GetComponent<TextMeshProUGUI>().SetText(GameManager.Instance.Gold.ToString());
    }
}
