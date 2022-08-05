using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Singleton
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }
    #endregion

    //score of user
    private int m_Score;
    public int Score
    {
        get { return m_Score; }
    }
    //if flagSuccess is equal two then user gets one point
    private int m_FlagSuccess;

    public delegate void ScoreAction();
    public static event ScoreAction ScoreUpdate;

    
    /// <summary>
    ///     Start is called before the first frame update.
    ///     sets score 0
    /// </summary>
    private void Start()
    {
        m_Score = 0;
        ScoreUpdate();
    }

    
    /// <summary>
    ///     control user if he/she is success
    ///     if last selected sprite has atleast two selected neighbor 
    ///          or one selected neighbor which also has atleast one selected neighbor
    ///          then user is successful and gets point
    /// </summary>
    /// <param name="_indexRow"> row index of last controlled cell </param>
    /// <param name="_indexCol"> col index of last controlled cell </param>
    /// <param name="lastSelectedRow"> row index of last selected cell </param>
    /// <param name="lastSelectedCol"> col index of last selected cell </param>
    public void ControlSuccess(int _indexRow,int _indexCol,int lastSelectedRow,int lastSelectedCol)
    {
        //Control up cell
        if(_indexRow -1 >= 0)
        {
            if(Graph.Instance.graph[_indexRow-1][_indexCol].stateCell == CellController.State.SELECTED && _indexRow-1 != lastSelectedRow)
            {
                m_FlagSuccess++;
                if (m_FlagSuccess == 2)
                {
                    NewPoint(lastSelectedRow,lastSelectedCol);
                    return;
                }
                else
                {
                    ControlSuccess(_indexRow-1,_indexCol,_indexRow,_indexCol);
                }
            }
        }

        //Control down cell
        if (_indexRow + 1 < Graph.Instance.graphSize)
        {
            if (Graph.Instance.graph[_indexRow + 1][_indexCol].stateCell == CellController.State.SELECTED && _indexRow+1 != lastSelectedRow)
            {
                m_FlagSuccess++;
                if (m_FlagSuccess == 2)
                {
                    NewPoint(lastSelectedRow, lastSelectedCol);
                    return;
                }
                else
                {
                    ControlSuccess(_indexRow + 1, _indexCol, _indexRow, _indexCol);
                }
            }
        }

        //Control left cell
        if (_indexCol - 1 >= 0)
        {
            if (Graph.Instance.graph[_indexRow][_indexCol-1].stateCell == CellController.State.SELECTED && _indexCol-1 != lastSelectedCol)
            {
                m_FlagSuccess++;
                if (m_FlagSuccess == 2)
                {
                    NewPoint(lastSelectedRow, lastSelectedCol);
                    return;
                }
                else
                {
                    ControlSuccess(_indexRow, _indexCol-1, _indexRow, _indexCol);
                }
            }
        }

        //Control right cell
        if (_indexCol + 1 < Graph.Instance.graphSize)
        {
            if (Graph.Instance.graph[_indexRow][_indexCol+1].stateCell == CellController.State.SELECTED && _indexCol+1 != lastSelectedCol)
            {
                m_FlagSuccess++;
                if (m_FlagSuccess == 2)
                {
                    NewPoint(lastSelectedRow, lastSelectedCol);
                    return;
                }
                else
                {
                    ControlSuccess(_indexRow, _indexCol+1, _indexRow, _indexCol);
                }
            }
        }
    }

    
    /// <summary>
    ///     recursive function to traverse selected cells path and set cells unselected
    /// </summary>
    /// <param name="_indexRow"> row index of last visited cell </param>
    /// <param name="_indexCol"> col index of last visited cell </param>
    private void ResetSuccessPathCells(int _indexRow,int _indexCol)
    {
        
        if (_indexRow < 0 || _indexRow >= Graph.Instance.graphSize || _indexCol < 0 || _indexCol >= Graph.Instance.graphSize) return;
        if (Graph.Instance.graph[_indexRow][_indexCol].Visited) return;
        
        Graph.Instance.graph[_indexRow][_indexCol].SetVisited();
        if (Graph.Instance.graph[_indexRow][_indexCol].stateCell == CellController.State.UNSELECTED) return;
        ResetSuccessPathCells(_indexRow - 1, _indexCol);
        ResetSuccessPathCells(_indexRow, _indexCol - 1);
        ResetSuccessPathCells(_indexRow + 1, _indexCol);
        ResetSuccessPathCells(_indexRow, _indexCol + 1);
        Graph.Instance.graph[_indexRow][_indexCol].SetUnselected();
    }

    /*
     * Summary:
     *     is called when user get a point
     *     it calles ResetSuccessPathCells to set cells unselected
     *     it calles SetUnvisitedCells to set cells unvisited
     */    
    /// <summary>
    ///     is called when user get a point
    ///     it calles ResetSuccessPathCells to set cells unselected
    /// it calles SetUnvisitedCells to set cells unvisited
    /// </summary>
    /// <param name="_lastSelectedRow"></param>
    /// <param name="_lastSelectedCol"></param>
    private void NewPoint(int _lastSelectedRow, int _lastSelectedCol)
    {
        m_Score++;
        ScoreUpdate();
        ResetSuccessPathCells(_lastSelectedRow, _lastSelectedCol);
        Graph.Instance.SetUnvisitedCells();
    }


    public void ControlNewCell(int _indexRow,int _indexCol)
    {
        m_FlagSuccess = 0;
        ControlSuccess(_indexRow, _indexCol, _indexRow, _indexCol);
    }
}
