namespace FilmStore.Test
{
    public class FilmTests
    {
        [Fact]
        public void IsIMDb_WithNull_ReturnFalse()
        {
            bool actual = Film.IsIMDb(null);
            Assert.False(actual);
        }

        [Fact]
        public void IsIMDb_WithBlankString_ReturnFalse()
        {
            bool actual = Film.IsIMDb("   ");
            Assert.False(actual);
        }

        [Fact]
        public void IsIMDb_WithInvalidIMDb_ReturnFalse()
        {
            bool actual = Film.IsIMDb("ID 123");
            Assert.False(actual);
        }

        [Fact]
        public void IsIMDb_WithIMDb10_ReturnTrue()
        {
            bool actual = Film.IsIMDb("ID 1234567");
            Assert.True(actual);
        }

        [Fact]
        public void IsIMDb_WithIMDb13_ReturnTrue()
        {
            bool actual = Film.IsIMDb("ID 123-456-7");
            Assert.True(actual);
        }

        [Fact]
        public void IsIMDb_WithTrashStart_ReturnFalse()
        {
            bool actual = Film.IsIMDb("xxx ID 123-456-789 0123 yyy");
            Assert.False(actual);
        }
    }
}