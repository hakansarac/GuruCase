using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{

    [SerializeField] private CellController prefab;

    //Singleton to create only one ObjectPooler object 
    #region Singleton
    public static PoolManager Instance;

    private void Awake()
    {
        Instance = this;
    }
    #endregion
    //end singleton

    public List<CellController> cellPool;


    
    /// <summary>
    ///     Start is called before the first frame update.
    ///     Creates pool
    /// </summary>
    private void Start()
    {
        cellPool = new List<CellController>();
        CreateNewCell((int) Mathf.Pow(Graph.Instance.graphSize, 2));
    }

    
    /// <summary>
    ///     Creates new cell object
    /// </summary>
    /// <param name="_size"> size of new cells </param>
    public void CreateNewCell(int _size)
    {
        for (int i = 0; i < _size; i++)
        {
            CellController obj = Instantiate(prefab);
            AddCellToPool(obj);
        }
    }

    
    /// <summary>
    ///     Add cell to pool and set default values
    /// </summary>
    /// <param name="_cell"> the object which will be added pool </param>
    public void AddCellToPool(CellController _cell)
    {
        _cell.gameObject.SetActive(false);
        _cell.SetIndexes(_indexRow: -1, _indexCol: -1);
        _cell.transform.position = new Vector3(-1,-1,0);
        cellPool.Add(_cell);
    }

    
    /// <summary>
    ///     Get cell from end of pool 
    /// </summary>
    /// <returns> CellController from last index of pool </returns>
    public CellController GetCellFromPool()
    {
        CellController cell = cellPool[cellPool.Count - 1];
        RemoveCellFromPool();
        return cell;
    }

    
    /// <summary>
    ///     remove cell from end of pool
    /// </summary>
    private void RemoveCellFromPool()
    {
        cellPool.RemoveAt(cellPool.Count - 1);
    }
}
