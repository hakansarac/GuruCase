using UnityEngine;
using TMPro;

public class ButtonSize : MonoBehaviour
{
    [Header("User Input")]
    [SerializeField] TMP_InputField inputField;

    public delegate void ClickButtonAction(int newGraphSize);
    public static event ClickButtonAction OnClickedButton;

    /// <summary>
    ///     if clicked button, try to get graph size text and call OnClicked event 
    /// </summary>
    public void ButtonClicked()
    {
        
        int newGraphSize;
        bool result = int.TryParse(inputField.text, out newGraphSize);
        if (result)
        {
            OnClickedButton(newGraphSize);          
        }
        else
        {
            Debug.Log("user input cannot be turned to integer value.");
        }        
    }
}
