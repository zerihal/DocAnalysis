namespace DocParser.Enums
{
    [Flags]
    public enum AdvancedMatch
    {
        None = 0,
        SingleTermsOnly = 1,
        MultipleTermsInParagraph = 2,
        MultipleTermsInSentence = 4,
        AllTermsInParagraph = 8
    }
}
