using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    //chibi controller object to follow chibi
    [SerializeField] private ChibiController chibi;

    //speed of camera
    private float m_Speed = 3f;
    
    Vector3 m_Offset;


    // Start is called before the first frame update
    void Start()
    {
        m_Offset = this.transform.position - chibi.transform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 playerPos = chibi.transform.position;
        Vector3 desiredPosition = playerPos + m_Offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, m_Speed * Time.deltaTime);
        transform.position = smoothedPosition;
    }
}
