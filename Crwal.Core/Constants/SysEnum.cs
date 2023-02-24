namespace Crwal.Core.Constants
{
    public class SysEnum
    {
        public enum InstagramType
        {
            True = 2,
            False = 1,
            Done = 0
        }

        public enum IsLoading
        {
            BeforeLoading = -1,
            AfterLoading = 1
        }

        public enum LoopState
        {
            Continue,
            Break,
            Ok
        }

        public enum SiDemandSourceStatus
        {
            Wait = 0,
            Processing = 1,
            Done = 2,
            Error = -1
        }

        public enum State
        {
            Success = 1,
            DoubleLink = -2,
            Erorr = -1
        }

        public enum TikTokType
        {
            True = 6,
            False = 1,
            Done = 0
        }
    }
}