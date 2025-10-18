namespace DocParser.Enums
{
    /// <summary>
    /// Advanced match types for search results.
    /// </summary>
    [Flags]
    public enum AdvancedMatch
    {
        /// <summary>
        /// No matches.
        /// </summary>
        None = 0,
        /// <summary>
        /// Single term match only.
        /// </summary>
        SingleTermsOnly = 1,
        /// <summary>
        /// Multiple terms found in the same paragraph.
        /// </summary>
        MultipleTermsInParagraph = 2,
        /// <summary>
        /// Multiple terms found in the same sentence.
        /// </summary>
        MultipleTermsInSentence = 4,
        /// <summary>
        /// All search terms found in the same paragraph.
        /// </summary>
        AllTermsInParagraph = 8
    }
}
