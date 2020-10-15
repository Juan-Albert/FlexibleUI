using UnityEngine;
using UnityEngine.UI;
using UITools;

//Cell class for demo. A cell in Recyclable Scroll Rect must have a cell class inheriting from ICell.
//The class is required to configure the cell(updating UI elements etc) according to the data during recycling of cells.
//The configuration of a cell is done through the DataSource SetCellData method.
//Check RecyclableScrollerDemo class
public class ExampleCell : MonoBehaviour, ICell
{
    //UI
    public Text idLabel;

    //Model
    private CellData _cellData;
    private int _cellIndex;

    private void Start()
    {
        //Can also be done in the inspector
        GetComponent<Button>().onClick.AddListener(ButtonListener);
    }

    //This is called from the SetCell method in DataSource
    public void ConfigureCell(CellData cellData,int cellIndex)
    {
        _cellIndex = cellIndex;
        _cellData = cellData;

        idLabel.text = cellData.id;
    }

    private void ButtonListener()
    {
        Debug.Log("Index : " + _cellIndex);
    }
}
