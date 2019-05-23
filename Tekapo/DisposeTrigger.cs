namespace Tekapo
{
    using System;
    using System.ComponentModel;

    internal class DisposeTrigger : Component
    {
        private readonly Action<bool> _action;

        internal DisposeTrigger(Action<bool> action)
        {
            _action = action;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            _action(disposing);
        }
    }
}