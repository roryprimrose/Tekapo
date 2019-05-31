namespace Tekapo.UnitTests
{
    using FluentAssertions;
    using Tekapo.Processing;
    using Xunit;

    public class ExtensionsTests
    {
        [Theory]
        [InlineData(TaskType.RenameTask, MediaOperationType.Read)]
        [InlineData(TaskType.ShiftTask, MediaOperationType.ReadWrite)]
        public void AsMediaOperationTypeReturnsExpectedValue(TaskType taskType, MediaOperationType expected)
        {
            var actual = taskType.AsMediaOperationType();

            actual.Should().Be(expected);
        }
    }
}