using Data.Entities;

namespace Logic
{
    /// <summary>
    /// this interface is served as the base for
    /// sorting methods
    /// </summary>
    /// <remarks>
    /// author: Xena Tat
    /// date: 14.04.2026
    /// </remarks>
    public interface ISortMethod<T>
    {
        ///<summary>
        /// <c>SortAscending</c>
        /// Sorts food items list in Ascending order
        /// 
        /// </summary>
        /// <param name="list"></param> Input list
        /// <returns> Returns sorted list</returns>
        List<T> SortAscending(List<T> list);

        ///<summary>
        /// <c>SortDescending</c>
        /// Sorts food items list in Descending order
        /// 
        /// </summary>
        /// <param name="list"></param> Input list
        /// <returns> Returns sorted list</returns>
        List<T> SortDescending(List<T> list);
    }
}
