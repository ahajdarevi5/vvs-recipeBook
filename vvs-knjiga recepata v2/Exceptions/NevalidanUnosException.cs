namespace vvs_knjiga_recepata_v2.Exceptions
{
    public class NevalidanUnosException : Exception
    {
        public NevalidanUnosException() : base() { }
        public NevalidanUnosException(string message) : base(message) { }
        public NevalidanUnosException(string message, Exception innerException) : base(message, innerException) { }
    }
}
