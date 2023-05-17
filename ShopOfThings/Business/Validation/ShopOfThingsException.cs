using System.Runtime.Serialization;

namespace Business.Validation
{
    [Serializable()]
    public class ShopOfThingsException : Exception
    {
        public ShopOfThingsException() { }
        public ShopOfThingsException(string message) : base(message) { }
        public ShopOfThingsException(string message, Exception inner) : base(message, inner) { }
        protected ShopOfThingsException(SerializationInfo info, StreamingContext context) : base(info, context) { }

    }
}
