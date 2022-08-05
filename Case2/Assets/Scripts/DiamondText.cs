using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class DiamondText : MonoBehaviour
{
    //amount of diamond
    int m_Diamond;

    private void Start()
    {
        m_Diamond = PlayerPrefs.GetInt("diamond");
        this.GetComponent<TextMeshProUGUI>().SetText(m_Diamond.ToString());
    }
    private void OnEnable()
    {
        GameManager.DiamondTextUpdate += NewDiamond;
    }

    private void OnDisable()
    {
        GameManager.DiamondTextUpdate -= NewDiamond;
    }

    /// <summary>
    ///     Sets diamond text
    /// </summary>
    void NewDiamond()
    {
        this.GetComponent<TextMeshProUGUI>().SetText(GameManager.Instance.Diamond.ToString());
    }
}
