namespace Dictionary
{
    public class Element<TKey, TValue>
    {
        public Element(TKey key, TValue value)
        {
            Key = key;
            Value = value;
            Next = -1;
        }

        public TKey Key { get;  internal set; }

        public TValue? Value { get; internal set; }

        public int Next { get; internal set; }
    }
}