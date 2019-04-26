namespace Tekapo.Processing
{
    public class RenameConfiguration
    {
        public bool IncrementOnCollision { get; set; }

        public int MaxCollisionIncrement { get; set; }

        public string RenameFormat { get; set; }
    }
}