using System;

namespace NHL_API.Model.Web
{
    public class DataResult
    {
        public Exception Exception { get; set; }

        public bool HasError {
            get
            {
                return Exception != null;
            }
        }

        public object EntityResult { get; set; }
    }
}
