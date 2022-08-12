using UnityEngine;

public class StackManager : MonoBehaviour
{
    //stack of platforms
    private GameObject[] m_Stack;
    //index of stack
    private int m_StackIndex;
    //error margin placement of platforms
    private const float ERROR_MARGIN = 0.20f;
    //speed of platforms
    public float m_SpeedPlatform = 6f;
    //last platform scale of x
    private float m_LastXScale = 3f;
    public float LastXScale
    {
        get { return m_LastXScale; }
    }
    //new level target
    private float m_NewTarget;
    //last level target
    private float m_LastTarget;
    //z position of new platform
    private float m_PositionZ;
    //star boost
    private int m_Boost;

    private bool cross;
    //check wrong placement
    private bool m_FailFlag;

    private Vector3 lastPlatformPosition;
    public Vector3 LastPlatformPosition
    {
        get { return lastPlatformPosition; }
    }
    //combo counter
    private int m_ComboCount;

    private enum Direction { LEFT, RIGHT };
    private Direction direction;

    public delegate void PlayClipAction(int combo);
    public static event PlayClipAction PlayAudioClip;


    /// <summary>
    ///     Set platforms for the beginning
    /// </summary>
    private void Start()
    {
        m_Stack = new GameObject[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            m_Stack[i] = transform.GetChild(i).gameObject;
        }

        m_StackIndex = transform.childCount - 1;
        m_ComboCount = 0;
        m_PositionZ = 6;
        m_SpeedPlatform = 6 + 0.6f * (GameManager.Instance.CurrentLevel - 1);
        m_FailFlag = false;
        SpawnNewPlatform();
    }

    /// <summary>
    ///     Move new platform and if user place platform spawn new platform
    /// </summary>
    private void Update()
    {
        if(GameManager.Instance.state == GameManager.State.PLAY)
        {
            if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began && m_FailFlag==false)
            {
                if (PlacePlatform())
                {                    
                    SpawnNewPlatform();                 
                }
            }
            MoveNewPlatform();
        }
    }

    /// <summary>
    ///     Spawn new platform
    /// </summary>
    private void SpawnNewPlatform()
    {       
        lastPlatformPosition = m_Stack[m_StackIndex].transform.position;
        m_StackIndex--;
        if (m_StackIndex < 0)
            m_StackIndex = transform.childCount - 1;
        if (m_PositionZ >= m_NewTarget) return;
        RandomDirectionGenerate();
        m_Stack[m_StackIndex].transform.position = direction == Direction.LEFT ? new Vector3(5, 0, m_PositionZ) : new Vector3(-5, 0, m_PositionZ);
        m_Stack[m_StackIndex].transform.localScale = new Vector3(m_LastXScale,0.4f,3);
        m_PositionZ += 3;
    }

    public float NewSpawnPosition()
    {
        return m_Stack[m_StackIndex].transform.position.x;
    }

    public float CrossMove()
    {
        //t is transform of new spawned platform
        Transform t = m_Stack[m_StackIndex].transform;
        //distance between last platform x and new platform x
        float delta = lastPlatformPosition.x - t.position.x;

        if (m_LastXScale - Mathf.Abs(delta) > 0) {
            float mid = lastPlatformPosition.x + t.position.x / 2;
            return mid;
        }
        else
        {
            return lastPlatformPosition.x;
        }
    } 
    /// <summary>
    ///     Set position and scale new platform and place it
    /// </summary>
    /// <returns> 
    ///     true if placed platform successfully;
    ///     false if cannot placed platform
    /// </returns>
    private bool PlacePlatform()
    {
        //t is transform of new spawned platform
        Transform t = m_Stack[m_StackIndex].transform;
        //distance between last platform x and new platform x
        float delta = lastPlatformPosition.x - t.position.x;
        //if distance between last platform x and new platform x is bigger than error margin
        if (Mathf.Abs(delta) > ERROR_MARGIN)
        {
            m_ComboCount = 0;
            float tempLastScale = m_LastXScale;
            m_LastXScale -= Mathf.Abs(delta);
            if (m_LastXScale <= 0) {
                m_FailFlag = true;
                t.localScale = new Vector3(0, 0, 0);
                //create outgrowth part of new placed platform
                CreateOutgrowth(
                new Vector3((t.position.x > 0)
                        ? t.position.x + (t.localScale.x / 2)
                        : t.position.x - (t.localScale.x / 2)
                        , t.transform.position.y - 0.2f
                        , t.position.z),
                new Vector3(tempLastScale, 0.4f, 3),
                0
            );
                return false;
            }            
            PlayAudioClip(m_ComboCount);
            float mid = lastPlatformPosition.x + t.position.x /2;
            //if there is a star boost make x scale maximum again
            if (m_Boost > 0)
            {
                PerformBoosted(t);
            }
            //if there is not a star boost
            else
            {
                t.localScale = new Vector3(m_LastXScale, 0.4f, 3);
                //create outgrowth part of new placed platform
                CreateOutgrowth(
                    new Vector3((t.position.x > 0)
                            ? t.position.x + (t.localScale.x / 2)
                            : t.position.x - (t.localScale.x / 2)
                            , t.transform.position.y - 0.2f
                            , t.position.z),
                    new Vector3(Mathf.Abs(delta), 0.4f, t.localScale.z),
                    delta
                );
            }
            
            t.localPosition = new Vector3(mid - (lastPlatformPosition.x / 2), 0, m_PositionZ - 3);
        }
        //if distance between last platform x and new platform x is smaller than error margin
        else
        {
            m_ComboCount++;
            PlayAudioClip(m_ComboCount);
            //if there is a star boost make x scale maximum again
            if (m_Boost > 0)
            {
                PerformBoosted(t);
            }
            t.localPosition = new Vector3(lastPlatformPosition.x, 0, m_PositionZ - 3);
        }
        return true;
    }

    /// <summary>
    ///     Movement of new spawned platform
    /// </summary>
    private void MoveNewPlatform()
    {
        
        Transform lastGeneratedPlatformTranform = m_Stack[m_StackIndex].transform;
        lastGeneratedPlatformTranform.position = Vector3.Lerp(
            lastGeneratedPlatformTranform.position,
            direction == Direction.LEFT ? lastGeneratedPlatformTranform.position - lastGeneratedPlatformTranform.right : lastGeneratedPlatformTranform.position + lastGeneratedPlatformTranform.right,
            m_SpeedPlatform * Time.deltaTime
        );
    }

    /// <summary>
    ///     Create new cube for error part
    /// </summary>
    /// <param name="_pos"> position of outgrowth</param>
    /// <param name="_scale"> scale of outgrowth</param>
    private void CreateOutgrowth(Vector3 _pos, Vector3 _scale,float _direction)
    {
        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        go.transform.localPosition = _pos;
        go.transform.localScale = _scale;
        if(_direction == 0)
        {
            go.AddComponent<Rigidbody>();
        }
        else if(_direction < 0)
        {
            go.AddComponent<Rigidbody>().angularVelocity = new Vector3(0, 0, -5);
        }
        else
        {
            go.AddComponent<Rigidbody>().angularVelocity = new Vector3(0, 0, 5);
        }
        
    }

    /// <summary>
    ///     Generate direction of new spawned platform
    /// </summary>
    private void RandomDirectionGenerate()
    {
        switch (Random.Range(0, 2))
        {
            case 0:
                direction = Direction.LEFT;
                break;
            case 1:
                direction = Direction.RIGHT;
                break;
            default:
                direction = Direction.LEFT;
                break;
        }
    }


    private void OnEnable()
    {
        GameManager.ResetLevel += ResetPlatforms;
        FinishController.FinishPositionChanged += SetTarget;
        GameManager.LevelUp += NewLevel;
        RemoveStar.StarBoost += StarBoosted;
    }

    private void OnDisable()
    {
        GameManager.ResetLevel -= ResetPlatforms;
        FinishController.FinishPositionChanged -= SetTarget;
        GameManager.LevelUp -= NewLevel;
        RemoveStar.StarBoost -= StarBoosted;
    }

    private void ResetPlatforms()
    {
        int i = transform.childCount;
        while (i > 0)
        {            
            if (i>2)
                m_Stack[m_StackIndex].transform.position = new Vector3(0, 0, -15);
            else if (i == 2)
                m_Stack[m_StackIndex].transform.position = new Vector3(0, 0, m_LastTarget == 0 ? m_LastTarget : -15);
            else if (i == 1)
                m_Stack[m_StackIndex].transform.position = new Vector3(0, 0, m_LastTarget+ 3);
            m_Stack[m_StackIndex].transform.localScale = new Vector3(3, 0.4f, 3);

            m_StackIndex--;
            if (m_StackIndex < 0)
                m_StackIndex = transform.childCount - 1;
            i--;
        }
        m_PositionZ = m_LastTarget + 6f;
        m_LastXScale = 3;
        m_Boost = 0;
        SpawnNewPlatform();
    }

    private void SetTarget(float _newTargetPosition)
    {
        m_LastTarget = m_NewTarget;
        m_NewTarget = _newTargetPosition;
    }

    private void NewLevel()
    {
        m_Stack[m_StackIndex].transform.position = new Vector3(0, 0, m_LastTarget+3);
        m_Stack[m_StackIndex].transform.localScale = new Vector3(3, 0.4f, 3);
        m_LastXScale = 3;
        m_PositionZ += 6;
        m_SpeedPlatform = 6 + 0.6f * (GameManager.Instance.CurrentLevel - 1);
        SpawnNewPlatform();
    }

    private void StarBoosted()
    {
        m_Boost = 3;
    }

    private void PerformBoosted(Transform _t)
    {
        m_Boost--;
        _t.localScale = new Vector3(3, 0.4f, 3);
        m_LastXScale = 3;
    }
}
