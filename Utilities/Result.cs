namespace UdpFileServer.Utilities
{
    internal class Result
    {
        public string Message { get; private set; }
        public string Value { get; private set; }
        public bool IsSuccessful { get; private set; }

        private Result(bool Success, string Message = null, string Value = null)
        {
            IsSuccessful = Success;
            this.Message = Message;
            this.Value = Value;
        }


        public static Result Success(string Message) => new Result(true, Message);
        public static Result Success(string Message, string Value) => new Result(true, Message, Value);
        public static Result Fail(string Message) => new Result(false, Message);

        public override string ToString()
        {
            return Message;
        }
    }
}
