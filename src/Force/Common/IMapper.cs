namespace Force.Common
{
    /// <summary>
    /// Data mapper
    /// </summary>
    public interface IMapper
    {
        /// <summary>
        /// Map object to destination type
        /// </summary>
        /// <param name="src">Source object</param>
        /// <typeparam name="TDest">Destination type</typeparam>
        /// <returns></returns>
        TDest Map<TDest>(object src);

        /// <summary>
        /// Map source object to existing destination object
        /// </summary>
        /// <param name="src">Source object</param>
        /// <param name="dest">Destination object</param>
        /// <typeparam name="TDest">Destination type</typeparam>
        /// <returns></returns>
        TDest Map<TDest>(object src, TDest dest);
    }
}