namespace Storm.TechTask.UnitTests.Utilities.Comparison
{
    public static class State
    {
        public static T SameAs<T>(T expected)
        {
            return Moq.Match.Create<T>(o =>
            {
                try
                {
                    o.ShouldHaveSameStateAs(expected);
                    return true;
                }
                catch
                {
                    return false;
                }
            });
        }
    }

}
