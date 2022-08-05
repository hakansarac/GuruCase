using UnityEngine;

public class CollactableManager : MonoBehaviour
{
    [Header("Collactable Prefabs")]
    //coin prefab
    [SerializeField] GameObject coin;
    //diamond prefab
    [SerializeField] GameObject newDiamond;
    //star prefab
    [SerializeField] GameObject newStar;

    //diamond object
    GameObject m_Diamond;
    //star object
    GameObject m_Star;

    //new level target
    private float m_NewTarget;
    //last level target
    private float m_LastTarget;

    //array of coin objects
    private GameObject[] arrCoin = new GameObject[10];

    // Start is called before the first frame update
    // Summary: 
    //     Instantiate collactable objects
    void Start()
    {
        for(int i = 0; i < 10; i++)
        {
            GameObject newCoin = Instantiate(coin);
            newCoin.SetActive(false);
            arrCoin[i] = newCoin;
        }
        m_Diamond = Instantiate(newDiamond);
        m_Diamond.SetActive(false);
        m_Star = Instantiate(newStar);
        m_Star.SetActive(false);
    }

    private void OnEnable()
    {
        FinishController.FinishPositionChanged += SetNewLevel;
        GameManager.ResetLevel += ResetCollactables;
    }

    private void OnDisable()
    {
        FinishController.FinishPositionChanged -= SetNewLevel;
        GameManager.ResetLevel -= ResetCollactables;
    }

    /// <summary>
    ///     Set new target and last target when player passed to new level
    /// </summary>
    /// <param name="_newTargetPosition"> finish position of new level </param>
    void SetNewLevel(float _newTargetPosition) 
    {
        m_LastTarget = m_NewTarget;
        m_NewTarget = _newTargetPosition;
        SetNewPositions();
    }

    /// <summary>
    ///     Set new positions of collactable items
    /// </summary>
    void SetNewPositions()
    {
        int level = GameManager.Instance.CurrentLevel == 0 ? 1 : GameManager.Instance.CurrentLevel;

        m_Diamond.transform.position = new Vector3(0, 0.2f, m_NewTarget - 3);
        m_Diamond.SetActive(true);

        m_Star.transform.position = new Vector3(0, 0.2f, (m_NewTarget + m_LastTarget) / 2);
        m_Star.SetActive(true);

        for (int i = 0; i < arrCoin.Length; i++)
        {
            if (i < 3)
            {
                arrCoin[i].transform.position = new Vector3(0, 0.2f, (m_NewTarget + m_LastTarget) / 2 - (i * level));
            }
            else if (i < 6)
            {
                arrCoin[i].transform.position = new Vector3(0, 0.2f, (m_NewTarget + m_LastTarget) / 2 + (i * level));
            }
            else
            {
                arrCoin[i].transform.position = new Vector3(0, 0.2f, m_NewTarget - i * level);
            }
            arrCoin[i].SetActive(true);
        }
    }

    /// <summary>
    ///     Reset collactables when player is failed
    /// </summary>
    void ResetCollactables()
    {
        foreach(GameObject obj in arrCoin)
        {
            obj.SetActive(true);
        }
        m_Diamond.SetActive(true);
        m_Star.SetActive(true);
    }
}
