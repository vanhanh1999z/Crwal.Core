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

    public class SysFbEnum
    {
        public enum Process
        {
            Token = 1,
            Login = 2,
            TokenAll = 3,
            TokenJob = 4
        }

        public enum DbType
        {
            String = 0,
            Int = 1,
            Boolean = 2,
            Date = 3,
            DateTime = 4,
            Time = 5,
            Float = 6,
            Decimal = 7,
            Int16 = 8,
            Byte = 9,
            Image = 10,
            Long = 11,
            BinaryString = 12
        }
        public enum CompareOperator
        {
            Equal,
            NotEqual,
            GreaterThan,
            GreaterOrEqual,
            LessThan,
            LessOrEqual,
            Contains,
            In,
            NotIn,
            StartWith,
            EndWith,
            IsNull,
            NotNull
        }
        public enum OrderType
        {
            Ascending,
            Descending
        }
        public enum LogicalOperator
        {
            And,
            Or
        }
    }
}