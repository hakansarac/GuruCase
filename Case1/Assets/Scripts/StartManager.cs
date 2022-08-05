using UnityEngine;

public class StartManager : MonoBehaviour
{
    /// <summary>
    ///     Start is called before the first frame update
    /// </summary>
    void Start()
    {
        CheckGraphSize();
    }

    private void CheckGraphSize()
    {
        if (Graph.Instance.graphSize < 2)
        {
            Graph.Instance.graphSize = 2;
            Debug.Log("graph size cannot be less than 2.");
        }
    }
}
