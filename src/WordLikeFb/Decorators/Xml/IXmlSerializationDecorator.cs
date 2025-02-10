namespace WordLikeFb.Decorators.Xml
{
    public interface IXmlSerializationDecorator<T>
    {
        T DecorationTarget { get; }
        List<object> Elements { get; }
    }
}
