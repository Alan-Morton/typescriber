using System.Numerics;

namespace ToTypeScript.Test.Examples.Dom
{
    public abstract class Universe<T> where T : Planet
    {
        public abstract T[] GetPlanets();
    }

    public abstract class UniversePeople : Universe<People>
    {
    }
}
