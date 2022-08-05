using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private int m_GraphSize;
    void Start()
    {
        m_GraphSize = Graph.Instance.graphSize;
        transform.position = new Vector3(m_GraphSize - 1, m_GraphSize * -2, (m_GraphSize * -4)-1);
    }

    private void OnEnable()
    {
        ButtonSize.OnClickedButton += NewGraphCam;
    }

    private void OnDisable()
    {
        ButtonSize.OnClickedButton -= NewGraphCam;
    }

    /// <summary>
    ///     set new camera position when user changes graph size 
    /// </summary>
    /// <param name="_newGraphSize"></param>
    private void NewGraphCam(int _newGraphSize)
    {
        if (_newGraphSize >= 2)
        {
            m_GraphSize = _newGraphSize;
            transform.position = new Vector3(m_GraphSize - 1, m_GraphSize * -2, (m_GraphSize * -4) - 1);
        }
    }
}
