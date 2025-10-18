using DocParser.Enums;

namespace DocParser.Interfaces
{
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
