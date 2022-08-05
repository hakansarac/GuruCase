using UnityEngine;

public class FinishController : MonoBehaviour
{
    //stack of finish platforms
    private GameObject[] m_StackFinish;
    //level of game
    private int m_Level;
    //target distance
    private float m_TargetDistance;

    public delegate void FinishPosition(float _newTargetPosition);
    public static event FinishPosition FinishPositionChanged;

    //index of finish platforms stack
    private int m_StackIndex;


    // Start is called before the first frame update
    void Start()
    {
        //stack operations
        m_StackFinish = new GameObject[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            m_StackFinish[i] = transform.GetChild(i).gameObject;
        }
        m_StackIndex = transform.childCount - 1;

        m_Level = GameManager.Instance.CurrentLevel;
        m_TargetDistance = 15 * (m_Level + 2);

        //finish platform operations
        m_StackFinish[m_StackIndex].transform.position = new Vector3(0,0.2f, m_TargetDistance);
        FinishPositionChanged(m_StackFinish[m_StackIndex].transform.position.z);
    }

    /// <summary>
    ///     to get finish position
    /// </summary>
    /// <returns> finish position </returns>
    public float GetFinishPosition()
    {
        return m_StackFinish[m_StackIndex].transform.position.z;
    }

    private void OnEnable()
    {
        GameManager.LevelUp += NewLevel;
    }

    private void OnDisable()
    {
        GameManager.LevelUp -= NewLevel;
    }


    /// <summary>
    ///     set finish platform according to new level values
    /// </summary>
    private void NewLevel()
    {
        //stack operations
        float lastFinish = m_StackFinish[m_StackIndex].transform.position.z;
        m_StackIndex--;
        if (m_StackIndex < 0)
        {
            m_StackIndex = transform.childCount - 1;
        }

        m_Level = GameManager.Instance.CurrentLevel;
        //target distance is changed
        m_TargetDistance = 15 * (m_Level + 2);

        //finsh plaftorm operations
        m_StackFinish[m_StackIndex].transform.position = new Vector3(0, 0.2f, lastFinish+ m_TargetDistance);
        FinishPositionChanged(m_StackFinish[m_StackIndex].transform.position.z);
    }
}
