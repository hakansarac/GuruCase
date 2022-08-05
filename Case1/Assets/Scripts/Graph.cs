using System.Collections.Generic;
using UnityEngine;

public class Graph : MonoBehaviour, IGraphDesign
{
    #region Singleton
    public static Graph Instance { get; set; }

    private void Awake()
    {
        Instance = this;
    }
    #endregion

    //our graph
    public List<List<CellController>> graph = new List<List<CellController>>();
    //graph size
    public int graphSize;
    //pool size
    private int m_MaxCellPoolSize;

    
    /// <summary>
    ///     Start is called before the first frame update.
    ///     Creates the graph
    /// </summary>
    private void Start()
    {
        m_MaxCellPoolSize = graphSize;
        CreateGraph();
    }

    
    /// <summary>
    ///     creates graph from cell pool objects
    /// </summary>
    public void CreateGraph()
    {
        for (int i = 0; i < graphSize; i++)
        {
            List<CellController> newRow = new List<CellController>();
            for (int j = 0; j < graphSize; j++)
            {
                CellController newCell = PoolManager.Instance.GetCellFromPool();
                newCell.SetIndexes(_indexRow: i, _indexCol: j);
                newCell.GetComponent<Transform>().position = new Vector3(j * 2, i * -2, 0);
                newRow.Add(newCell);
                newCell.gameObject.SetActive(true);
            }
            graph.Add(newRow);
        }
    }

   
    /// <summary>
    ///     is called to change graph size
    /// </summary>
    public void ChangeGraphSize()
    {
        if (graph.Count < graphSize)
        {
            AddCells();
        }
        else if (graph.Count > graphSize)
        {
            RemoveCells();
        }
    }

    
    /// <summary>
    ///     Decreases graph size
    /// </summary>
    public void RemoveCells()
    {
        int curGraphSize = graph.Count;
        for (int i = curGraphSize - 1; i > graphSize - 1; i--)
        {
            for (int j = curGraphSize - 1; j >= 0; j--)
            {               
                CellController cell = graph[i][j];
                PoolManager.Instance.AddCellToPool(cell);

                if (i == j) continue;
                
                cell = graph[j][i];
                PoolManager.Instance.AddCellToPool(cell);

                //remove object from last column
                graph[j].RemoveAt(i);
            }
            curGraphSize--;
            //remove last row
            graph.RemoveAt(i);
        }
    }

    
    /// <summary>
    ///     Increases graph size
    /// </summary>
    public void AddCells()
    {
        int newGraphSize = graphSize;
        //if there are no cells in the cell pool enough for graph, create new cell objects to add to the pool
        if (graphSize > m_MaxCellPoolSize)
        {
            for (int i = graphSize; i > m_MaxCellPoolSize; i--)
            {
                PoolManager.Instance.CreateNewCell((i * 2) - 1);
            }
            m_MaxCellPoolSize = graphSize;
        }      
        
        int curGraphSize = graph.Count;

        //add columns to exist rows
        for (int i = curGraphSize; i < newGraphSize; i++)
        {
            for (int j = 0; j < curGraphSize; j++)
            {
                CellController newCell = PoolManager.Instance.GetCellFromPool();
                newCell.SetIndexes(_indexRow: j, _indexCol: i);
                newCell.GetComponent<Transform>().position = new Vector3(i * 2, j * -2, 0);
                newCell.gameObject.SetActive(true);
                graph[j].Add(newCell);
            }
        }

        //add new rows
        for (int i = curGraphSize; i < newGraphSize; i++)
        {
            List<CellController> newRow = new List<CellController>();
            for (int j = 0; j < newGraphSize; j++)
            {
                CellController newCell = PoolManager.Instance.GetCellFromPool();
                newCell.SetIndexes(_indexRow: i, _indexCol: j);
                newCell.GetComponent<Transform>().position = new Vector3(j * 2, i * -2, 0);
                newCell.gameObject.SetActive(true);
                newRow.Add(newCell);
            }
            graph.Add(newRow);
        }
    }

    
    /// <summary>
    ///     set all cells of graph unselected
    /// </summary>
    private void ResetCells()
    {
        for(int i = 0; i<graph.Count; i++)
        {
            for(int j = 0; j < graph.Count; j++)
            {
                graph[i][j].SetUnselected();
            }
        }
    }

    
    /// <summary>
    ///     set all cells of graph unvisited
    /// </summary>
    public void SetUnvisitedCells()
    {
        for (int i = 0; i < graph.Count; i++)
        {
            for (int j = 0; j < graph.Count; j++)
            {
                graph[i][j].SetUnvisited();
            }
        }
    }



    private void OnEnable()
    {
        ButtonSize.OnClickedButton += NewGraphSize;
    }

    private void OnDisable()
    {
        ButtonSize.OnClickedButton -= NewGraphSize;
    }

    /*
     * Summary:
     *     set new graph size and call ResetCells and ChangeGraphSize functions
     * Parameters:
     *     _newGraphSize: new graph size
     */     
    private void NewGraphSize(int _newGraphSize)
    {
        if (_newGraphSize < 2)
        {
            Debug.Log("graph size cannot be less than 2.");
        }
        else
        {
            graphSize = _newGraphSize;
            ResetCells();
            ChangeGraphSize();
        }               
    }
}
