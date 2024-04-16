using System.Collections;

namespace Dictionary.Classes
{
    public class MyDictionary<TKey, TValue> : IDictionary<TKey, TValue>
    {
        private readonly int[] buckets = new int[4];
        private Element<TKey, TValue>[] elements = new Element<TKey, TValue>[4];
        private int freeIndex = -1;

        public MyDictionary()
        {
            Array.Fill(buckets, -1);
        }

        public MyDictionary(int size)
        {
            buckets = new int[size];
            elements = new Element<TKey, TValue>[size];
            Array.Fill(buckets, -1);
        }

        public ICollection<TKey> Keys
        {
            get
            {
                var list = new List<TKey>();

                foreach (var element in this)
                {
                    list.Add(element.Key);
                }

                return list;
            }
        }

        public ICollection<TValue> Values
        {
            get
            {
                var list = new List<TValue>();

                foreach (var element in this)
                {
                    list.Add(element.Value);
                }

                return list;
            }
        }

        public int Count { get; private set; }

        public bool IsReadOnly => false;

        public TValue this[TKey key]
        {
            get
            {
                if (TryGetValue(key, out TValue value))
                {
                    return value;
                }

                throw new KeyNotFoundException("The given key was not present in dictionary");
            }

            set
            {
                int index = GetElementIndex(key, out int _);

                if (index != -1)
                {
                    elements[index].Value = value;
                }
                else
                {
                    Add(key, value);
                }
            }
        }

        public void Add(TKey key, TValue value)
        {
            ValidateKey(key);
            ResizeArray();

            int index = GetFreeIndex();
            int targetBucket = GetTargetBucket(key);

            elements[index] = new Element<TKey, TValue>(key, value)
            {
                Next = buckets[targetBucket]
            };

            buckets[targetBucket] = index;
            Count++;
        }

        public void Add(KeyValuePair<TKey, TValue> item) => Add(item.Key, item.Value);

        public void Clear()
        {
            Array.Fill(buckets, -1);
            Count = 0;
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            bool valuesAreNull = item.Value is null && this[item.Key] is null;
            int index = GetElementIndex(item.Key, out int _);

            return index != -1 && valuesAreNull
                || index != -1 && this[item.Key].Equals(item.Value);
        }

        public bool ContainsKey(TKey key) => GetElementIndex(key, out int _) != -1;

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            ValidateArray(array);
            ValidateArrayIndex(arrayIndex);
            ValidateArrayAvailableSpace(array, arrayIndex);
            CopyElements(array, arrayIndex);
        }

        public Element<TKey, TValue>? GetElement(TKey key)
        {
            int index = GetElementIndex(key, out int _);

            return index != -1 ? elements[index] : null;
        }

        public int GetElementIndex(TKey key) => GetElementIndex(key, out int _);

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            for (int i = 0; i < buckets.Length; i++)
            {
                for (int j = buckets[i]; j != -1; j = elements[j].Next)
                {
                    yield return new KeyValuePair<TKey, TValue>(elements[j].Key!, elements[j].Value!);
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public bool Remove(TKey key)
        {
            int index = GetElementIndex(key, out int previousIndex);

            if (index == -1)
            {
                return false;
            }

            if (previousIndex == -1)
            {
                int targetBucket = GetTargetBucket(key);
                buckets[targetBucket] = elements[index].Next;
            }
            else
            {
                elements[previousIndex].Next = elements[index].Next;
            }

            elements[index].Next = freeIndex;
            freeIndex = index;
            Count--;
            return true;
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            bool valuesAreNull = item.Value is null && this[item.Key] is null;

            return valuesAreNull && Remove(item.Key)
                || this[item.Key].Equals(item.Value) && Remove(item.Key);
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            int index = GetElementIndex(key!, out int _);

            if (index == -1)
            {
                value = default;
                return false;
            }

            value = elements[index].Value;
            return true;
        }

        private static void ValidateArray(KeyValuePair<TKey, TValue>[] array) => ArgumentNullException.ThrowIfNull(array);

        private static void ValidateArrayIndex(int arrayIndex)
        {
            if (arrayIndex >= 0)
            {
                return;
            }

            throw new ArgumentOutOfRangeException(nameof(arrayIndex));
        }

        private void CopyElements(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            for (int i = 0; i < buckets.Length; i++)
            {
                for (int j = buckets[i]; j != -1; j = elements[j].Next)
                {
                    array[arrayIndex] = new KeyValuePair<TKey, TValue>(elements[j].Key!, elements[j].Value!);
                    arrayIndex++;
                }
            }
        }

        private int GetElementIndex(TKey key, out int previousIndex)
        {
            ArgumentNullException.ThrowIfNull(key);

            int targetBucket = GetTargetBucket(key);
            previousIndex = -1;

            for (int i = buckets[targetBucket]; i != -1; i = elements[i].Next)
            {
                if (elements[i].Key!.Equals(key))
                {
                    return i;
                }

                previousIndex = i;
            }

            return -1;
        }

        private int GetFreeIndex()
        {
            if (freeIndex == -1)
            {
                return Count;
            }

            var tempIndex = freeIndex;
            freeIndex = elements[freeIndex].Next;
            return tempIndex;
        }

        private int GetTargetBucket(TKey key) => Math.Abs(key!.GetHashCode()) % buckets.Length;

        private void ResizeArray()
        {
            if (Count < elements.Length)
            {
                return;
            }

            Array.Resize(ref elements, elements.Length * 2);
        }

        private void ValidateKey(TKey key)
        {
            if (GetElementIndex(key, out int _) == -1)
            {
                return;
            }

            throw new ArgumentException($"An element with Key \"{key}\" already exists");
        }

        private void ValidateArrayAvailableSpace(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            if (Count <= array.Length - arrayIndex)
            {
                return;
            }

            throw new ArgumentException("The number of elements in the source ICollection is greater than the available" +
                "space from index to the end of the destination array");
        }
    }
}