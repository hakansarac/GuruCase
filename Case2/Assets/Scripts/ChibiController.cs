using UnityEngine;

[RequireComponent(typeof(Animator))]
public class ChibiController : MonoBehaviour
{
    //a stack manager object to calculate x position of chibi
    [SerializeField] private StackManager stackManager;
    //speed of chibi
    public float m_Speed = 3.5f;
    //new target position
    private float m_NewTarget;
    //last target position
    private float m_LastTarget;
    //flag to detect successed
    private bool flag = false;

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    /// <summary>
    ///     if gamemanager state is play, move chibi;
    ///     if chibi fell make gamemanager state failed;
    ///     if chibi arrived to new target make gamemanager state successed 
    /// </summary>
    private void Move()
    {
        
        if (GameManager.Instance.state == GameManager.State.PLAY)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(stackManager.LastPlatformPosition.x, transform.position.y, transform.position.z) + transform.forward, m_Speed * Time.deltaTime);
            if (transform.position.y < -2)
            {
                GameManager.Instance.Failed();
            }
            if(transform.position.z >= m_NewTarget && !flag)
            {
                flag = true;
                GameManager.Instance.Successed();
            }
        }  
    }

    private void OnEnable()
    {
        GameManager.GameStateChanged += ChangeAnimation;
        GameManager.ResetLevel += ResetPosition;
        FinishController.FinishPositionChanged += SetTarget;
    }

    private void OnDisable()
    {
        GameManager.GameStateChanged -= ChangeAnimation;
        GameManager.ResetLevel -= ResetPosition;
        FinishController.FinishPositionChanged -= SetTarget;
    }

    /// <summary>
    ///     Change animation according to gamemanager state
    /// </summary>
    private void ChangeAnimation()
    {
        if (GameManager.Instance.state == GameManager.State.MENU || GameManager.Instance.state == GameManager.State.TRYAGAIN)
        {
            this.GetComponent<Animator>().enabled = false;
            flag = false;
        }
        else if(GameManager.Instance.state == GameManager.State.PLAY){
            m_Speed = 3.5f + 0.35f * (GameManager.Instance.CurrentLevel - 1);
            this.GetComponent<Animator>().enabled = true;
            this.GetComponent<Animator>().SetFloat("runSpeed", 1f + 0.1f * (GameManager.Instance.CurrentLevel - 1));
            this.GetComponent<Animator>().SetBool("isPlay", true);
        }
        else if(GameManager.Instance.state == GameManager.State.LEVELCOMPLETED)
        {
            this.GetComponent<Animator>().SetBool("isPlay", false);
        }
    }

    /// <summary>
    ///     reset position when player is failed
    /// </summary>
    private void ResetPosition()
    {
        transform.position = new Vector3(0, 0.3f, m_LastTarget);
    }

    /// <summary>
    ///     Set new target and last target when player passed to new level
    /// </summary>
    /// <param name="_newTargetPosition"> finish position of new level </param>
    private void SetTarget(float _newTargetPosition)
    {
        m_LastTarget = m_NewTarget;
        m_NewTarget = _newTargetPosition;
    }
}
