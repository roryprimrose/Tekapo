namespace Tekapo.Processing
{
    using System;

    public class SearchStageEventArgs : EventArgs
    {
        public SearchStageEventArgs(SearchStage stage)
        {
            Stage = stage;
        }

        public static SearchStageEventArgs For(SearchStage stage)
        {
            return new SearchStageEventArgs(stage);
        }

        public SearchStage Stage { get; }
    }
}