namespace DocParser.Enums
{
    /// <summary>
    /// Document search options.
    /// </summary>
    [Flags]
    public enum DocSearchOptions
    {
        /// <summary>
        /// Case sensitive search.
        /// </summary>
        CaseSensitive = 1,
        /// <summary>
        /// Match exact whole words only.
        /// </summary>
        MatchExact = 2
    }
}
