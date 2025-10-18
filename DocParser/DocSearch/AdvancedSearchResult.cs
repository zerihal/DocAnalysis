using DocParser.Enums;
using DocParser.Interfaces;

namespace DocParser.DocSearch
{
    /// <inheritdoc/>
    public class AdvancedSearchResult : IAdvancedSearchResult
    {
        /// <inheritdoc/>
        public IEnumerable<ISearchResult> MatchesInParagraph { get; }

        /// <inheritdoc/>
        public AdvancedMatch MatchType { get; private set; }

        /// <inheritdoc/>
        public decimal MatchRating { get; private set; }

        /// <summary>
        /// Initialises a new instance of <see cref="AdvancedSearchResult"/>.
        /// </summary>
        /// <param name="matchesInParagraph"><see cref="ISearchResult"/> matches in paragraph.</param>
        /// <param name="searchStrings">Search strings that were used to obtain search result.</param>
        public AdvancedSearchResult(IEnumerable<ISearchResult> matchesInParagraph, IEnumerable<string> searchStrings)
        {
            MatchesInParagraph = matchesInParagraph;
            DetermineMatchType(searchStrings);
        }

        private void DetermineMatchType(IEnumerable<string> searchStrings)
        {
            if (MatchesInParagraph.Count() == 0 || searchStrings.Count() == 0)
            {
                MatchType = AdvancedMatch.None;
                return;
            }

            // This will append overall match rating
            var sentenceMultiMatches = 0;

            // First check if multiple matches are found in sentences
            var sentences = MatchesInParagraph.Select(m => m.Sentence).Distinct().ToList();
            var hasMultipleTermsInSentences = false;

            // ToDo: Do we also want some sort of metric for number of matches in paragraph?

            foreach (var sentence in sentences)
            {
                var termsInSentence = searchStrings.Count(s => sentence.IndexOf(s, StringComparison.OrdinalIgnoreCase) >= 0);
                if (termsInSentence > 1)
                {
                    sentenceMultiMatches += termsInSentence - 1;
                    hasMultipleTermsInSentences = true;
                }
            }

            if (hasMultipleTermsInSentences)
                MatchType |= AdvancedMatch.MultipleTermsInSentence;

            // Now check if multiple matches are found in the paragraph
            var paragraph = MatchesInParagraph.First().Paragraph;
            var termsInParagraph = searchStrings.Count(s => paragraph.IndexOf(s, StringComparison.OrdinalIgnoreCase) >= 0);

            if (termsInParagraph > 1)
            {
                // Very unlikely, but if there were 100+ multiple matches found in all sentences then decimal addition to rating will
                // just be capped at 1.0
                var ratingAddition = Math.Min(1.0m, sentenceMultiMatches / 100m);

                if (termsInParagraph == searchStrings.Count())
                {
                    MatchType |= AdvancedMatch.AllTermsInParagraph;
                    MatchRating = 3.0m + ratingAddition;
                }
                else
                {
                    MatchType |= AdvancedMatch.MultipleTermsInParagraph;
                    MatchRating = 2.0m + ratingAddition;
                }
            }
            else
            {
                MatchType |= AdvancedMatch.SingleTermsOnly;
                MatchRating = 1.0m;
            }
        }
    }
}
