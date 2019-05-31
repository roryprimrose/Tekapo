namespace Tekapo.UnitTests
{
    using System;
    using FluentAssertions;
    using Xunit;
    using Xunit.Abstractions;

    public class DisposeTriggerTests
    {
        private readonly ITestOutputHelper _output;

        public DisposeTriggerTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void CanDisposeMultipleTimes()
        {
            var callCount = 0;

            Action<bool> action = disposing =>
            {
                callCount++;
                _output.WriteLine("Run the disposal action - " + disposing);
            };

            using (var sut = new DisposeTrigger(action))
            {
                sut.Dispose();
            }

            callCount.Should().Be(2);
        }

        [Fact]
        public void DisposeInvokesProvidedAction()
        {
            var callCount = 0;

            Action<bool> action = disposing =>
            {
                callCount++;
                _output.WriteLine("Run the disposal action - " + disposing);
            };

            using (var sut = new DisposeTrigger(action))
            {
            }

            callCount.Should().Be(1);
        }

        [Fact]
        public void ThrowsExceptionWhenCreatedWithNullAction()
        {
            Action action = () =>
            {
                using (new DisposeTrigger(null))
                {
                }
            };

            action.Should().Throw<ArgumentNullException>();
        }
    }
}