namespace Tekapo
{
    using Tekapo.Processing;

    public static class Extensions
    {
        public static MediaOperationType AsMediaOperationType(this TaskType taskType)
        {
            if (taskType == TaskType.RenameTask)
            {
                return MediaOperationType.Read;
            }

            return MediaOperationType.ReadWrite;
        }
    }
}