namespace WordLikeFb.Tests.Serialization
{
    public class FictionBookReaderTests
    {
        [Theory]
        [InlineData("<p>123</p>")]
        public void ReadParagraph_works_well(string input)
        {
            var sut = new FictionBookReader();
        }
    }
}
