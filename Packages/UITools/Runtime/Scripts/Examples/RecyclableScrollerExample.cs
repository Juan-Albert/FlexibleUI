using System.Collections.Generic;
using UITools;
using UnityEngine;

/// <summary>
/// Demo controller class for Recyclable Scroll Rect. 
/// A controller class is responsible for providing the scroll rect with datasource. Any class can be a controller class. 
/// The only requirement is to inherit from IRecyclableScrollRectDataSource and implement the interface methods
/// </summary>

//Dummy Data model for demostraion
public struct CellData
{
    public string id;
}

public class RecyclableScrollerExample : MonoBehaviour, IRecyclableScrollRectDataSource
{
    [SerializeField]
    RecyclableScrollRect _recyclableScrollRect;

    [SerializeField]
    private int _dataLength;

    //Dummy data List
    private List<CellData> _cellList = new List<CellData>();

    //Recyclable scroll rect's data source must be assigned in Awake.
    private void Awake()
    {
        InitData();
        _recyclableScrollRect.dataSource = this;
    }

    //Initialising _contactList with dummy data 
    private void InitData()
    {
        if (_cellList != null) _cellList.Clear();

        string[] genders = { "Male", "Female" };
        for (int i = 0; i < _dataLength; i++)
        {
            CellData obj = new CellData();
            obj.id = "item " + i;
            _cellList.Add(obj);
        }
    }

    #region DATA-SOURCE

    /// <summary>
    /// Data source method. return the list length.
    /// </summary>
    public int GetItemCount()
    {
        return _cellList.Count;
    }

    /// <summary>
    /// Data source method. Called for a cell every time it is recycled.
    /// Implement this method to do the necessary cell configuration.
    /// </summary>
    public void SetCell(ICell cell, int index)
    {
        //Casting to the implemented Cell
        var item = cell as ExampleCell;
        item.ConfigureCell(_cellList[index],index);
    }
    
    #endregion
}


