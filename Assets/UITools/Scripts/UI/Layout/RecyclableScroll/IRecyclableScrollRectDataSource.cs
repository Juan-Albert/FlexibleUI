
/// <summary>
/// Interface for creating DataSource
/// Recyclable Scroll Rect must be provided a Data source which must inherit from this.
/// </summary>
namespace UITools
{
    public interface IRecyclableScrollRectDataSource
    {
        int GetItemCount();
        void SetCell(ICell cell, int index);
    }
}
