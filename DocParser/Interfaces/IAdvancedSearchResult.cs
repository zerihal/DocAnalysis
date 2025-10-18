using DocParser.Enums;

namespace DocParser.Interfaces
{
    /// <summary>
    /// Advanced search result containing multiple matches within the same paragraph. Paragraph is assigned a match type and rating
    /// depeding on the matches found from multiple terms.
    /// </summary>
    public interface IAdvancedSearchResult
    {
        /// <summary>
        /// Matched results in the same paragraph.
        /// </summary>
        IEnumerable<ISearchResult> MatchesInParagraph { get; }

        /// <summary>
        /// Type of match within the paragraph based on <see cref="Enums.AdvancedMatch"/>.
        /// </summary>
        AdvancedMatch MatchType { get; }

        /// <summary>
        /// Rating of the matches within the paragraph. This higher the rating the better the match.
        /// </summary>
        decimal MatchRating { get; }
    }
}
