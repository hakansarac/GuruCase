using UnityEngine;
using UnityEngine.EventSystems;

public class CellController : MonoBehaviour, IPointerDownHandler
{
    //graph row index of cell
    private int m_IndexRow;
    //graph column index of cell
    private int m_IndexCol;

    //to check traverse selected cells path
    private bool m_Visited;
    public bool Visited
    {
        get { return m_Visited; }
    }


    //State of Cell -> selected or unselected
    public enum State { SELECTED, UNSELECTED };
    public State stateCell;

    
    /// <summary>
    ///     Start is called before the first frame update.
    ///     State of cell will be unselected. 
    /// </summary>
    private void Start()
    {
        stateCell = State.UNSELECTED;
        m_Visited = false;
    }

    
    /// <summary>
    ///     is called when clicked to cell.
    ///     if cell is not selected, makes it selected.
    /// </summary>
    /// <param name="_eventData"></param>
    public void OnPointerDown(PointerEventData _eventData) 
    {
        if(stateCell == State.UNSELECTED)
        {
            SetSelected();
            GameManager.Instance.ControlNewCell(m_IndexRow, m_IndexCol);
        }            
    }

    
    /// <summary>
    ///    set unselected cells selected.
    /// </summary>
    private void SetSelected()
    {
        stateCell = State.SELECTED;
        GameObject close = FindChild.FindGameObjectInChildWithTag(this.gameObject, "Close");
        close.gameObject.SetActive(true);
    }

   
    /// <summary>
    ///     set selected cells unselected.
    /// </summary>
    public void SetUnselected()
    {
        stateCell = State.UNSELECTED;
        GameObject close = FindChild.FindGameObjectInChildWithTag(this.gameObject, "Close");
        close.gameObject.SetActive(false);
    }

    
    /// <summary>
    ///     set graph indexes of cell
    /// </summary>
    /// <param name="_indexRow"></param>
    /// <param name="_indexCol"></param>
    public void SetIndexes(int _indexRow, int _indexCol)
    {
        m_IndexRow = _indexRow;
        m_IndexCol = _indexCol;        
    }

    
    /// <summary>
    ///     set visited if visited by recursive function
    /// </summary>
    public void SetVisited()
    {
        m_Visited = true;
    }

    
    /// <summary>
    ///     set unvisited 
    /// </summary>
    public void SetUnvisited()
    {
        m_Visited = false;
    }
}
