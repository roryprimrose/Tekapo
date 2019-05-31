namespace Tekapo
{
    using System;
    using System.ComponentModel;
    using EnsureThat;

    public class DisposeTrigger : Component
    {
        private readonly Action<bool> _action;

        public DisposeTrigger(Action<bool> action)
        {
            Ensure.Any.IsNotNull(action, nameof(action));

            _action = action;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            _action(disposing);
        }
    }
}