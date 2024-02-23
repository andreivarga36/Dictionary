using Xunit;

namespace Dictionary
{
    public class MyDictionaryFacts
    {
        [Fact]
        public void Add_DictionaryContainsOneElement_ShouldReturnExpectedResult()
        {
            var dictionary = new MyDictionary<int, string>
            {
                { 1, "a" }
            };

            Assert.Single(dictionary);
        }

        [Fact]
        public void Add_ThreeElementesAdded_ShouldReturnExpectedResult()
        {
            var dictionary = new MyDictionary<int, string>
            {
                { 1, "a" },
                { 2, "b" },
                { 3, "c" }
            };

            Assert.Equal(-1, dictionary.GetElement(1)!.Next);
            Assert.Equal(-1, dictionary.GetElement(2)!.Next);
            Assert.Equal(-1, dictionary.GetElement(3)!.Next);
            Assert.Equal(3, dictionary.Count);
        }

        [Fact]
        public void Add_CollisionIsHappening_ShouldReturnExpectedResult()
        {
            var dictionary = new MyDictionary<int, string>();
            KeyValuePair<int, string> first = new(4, "a");
            KeyValuePair<int, string> second = new(8, "b");
            KeyValuePair<int, string> third = new(9, "c");
            dictionary.Add(first);
            dictionary.Add(third);
            dictionary.Add(second);

            Assert.Equal(0, dictionary.GetElementIndex(4));
            Assert.Equal(0, dictionary.GetElement(8)!.Next);
            Assert.Equal(2, dictionary.GetElementIndex(8));
        }

        [Fact]
        public void Add_KeyIsNull_ShouldReturnExpectedResult()
        {
            var dictionary = new MyDictionary<object, string>();

            Assert.Throws<ArgumentNullException>(() => dictionary.Add(null, "something"));
        }

        [Fact]
        public void Add_AnElementWithTheSameKeyAlreadyExists_ShouldReturnExpectedResult()
        {
            var dictionary = new MyDictionary<string, int>
            {
                { "inter", 100 },
                { "roma", 50 }
            };

            Assert.Throws<ArgumentException>(() => dictionary.Add("inter", 20));
        }

        [Fact]
        public void Clear_DictionaryContainsThreeElements_ShouldReturnExpectedResult()
        {
            var dictionary = new MyDictionary<char, int>
            {
                { 'a', 100 },
                { 'b', 200 },
                { 'c', 300 }
            };

            dictionary.Clear();

            Assert.Empty(dictionary);
            Assert.Empty(dictionary.Keys);
            Assert.Empty(dictionary.Values);
        }

        [Fact]
        public void Clear_ElementsAreRemovedAndOneElementIsAddedAfter()
        {
            var dictionary = new MyDictionary<int, string>
            {
                { 10, "pacific" },
                { 12, "indian" },
                { 14, "atlantic" }
            };

            dictionary.Clear();
            dictionary.Add(16, "arctic");

            Assert.Equal(0, dictionary.GetElementIndex(16));
        }

        [Fact]
        public void Contains_ElementsArePresentInDictionary_ShouldReturnExpectedResult()
        {
            var dictionary = new MyDictionary<int, string>
            {
                { 1, "a" },
                { 2, "b" },
                { 3, null }
            };

            Assert.True(dictionary.Contains(new KeyValuePair<int, string>(1, "a")));
            Assert.True(dictionary.Contains(new KeyValuePair<int, string>(3, null)));
        }

        [Fact]
        public void Contains_ElementIsNotPresentInDictionary_ShouldReturnExpectedResult()
        {
            var myDictionary = new MyDictionary<int, string>
            {
                { 11, "eleven" }
            };

            Assert.False(myDictionary.Contains(new KeyValuePair<int, string>(10, "")));
        }

        [Fact]
        public void Contains_TwoElementsHaveSameKeyButDifferentValues_ShouldReturnExpectedResult()
        {
            var dictionary = new MyDictionary<int, string>();
            var paul = new KeyValuePair<int, string>(1, "paul");
            var john = new KeyValuePair<int, string>(2, "john");
            var mark = new KeyValuePair<int, string>(3, "mark");
            var christian = new KeyValuePair<int, string>(1, "christian");

            dictionary.Add(paul);
            dictionary.Add(john);
            dictionary.Add(mark);

            Assert.True(dictionary.Contains(paul)); 
            Assert.False(dictionary.Contains(christian));
        }

        [Fact]
        public void ContainsKey_DictionaryContainsKey_ShouldReturnExpectedResult()
        {
            var dictionary = new MyDictionary<double, string>
            {
                { 1.5, "grams" },
                { 7.2, "kilograms" }
            };

            Assert.True(dictionary.ContainsKey(1.5));
        }

        [Fact]
        public void ContainsKey_DictionaryDoNotContainsKey_ShouldReturnExpectedResult()
        {
            var dictionary = new MyDictionary<int, string>();

            Assert.False(dictionary.ContainsKey(10));
        }

        [Fact]
        public void ContainsKey_KeyIsNull_ShouldReturnExpectedResult()
        {
            var dictionary = new MyDictionary<string, int>
            {
                { "QW", 400 },
                { "ZX", 100 }
            };

            Assert.Throws<ArgumentNullException>(() => dictionary.ContainsKey(null));
        }

        [Fact]
        public void CopyTo_AllElementsAreCopied_ShouldReturnExpectedResult()
        {
            var array = new KeyValuePair<string, int>[5];
            var dictionary = new MyDictionary<string, int>
            {
                { "Greece", 100 },
                { "Spain", 140 },
                { "Italy", 160 },
                { "Norway", 180 }
            };

            dictionary.CopyTo(array, 1);
            Assert.Null(array[0].Key);
            Assert.True(array[1].Key != null);
            Assert.True(array[2].Key != null);
            Assert.True(array[3].Key != null);
            Assert.True(array[4].Key != null);
        }

        [Fact]
        public void CopyTo_ArrayIsNull_ShouldReturnExpectedResult()
        {
            var dictionary = new MyDictionary<string, int>
            {
                { "H", 8 }
            };

            Assert.Throws<ArgumentNullException>(() => dictionary.CopyTo(null, 0));
        }

        [Fact]
        public void CopyTo_IndexIsLessThanZero_ShouldReturnExpectedResult()
        {
            var array = new KeyValuePair<double, char>[10];
            var dictionary = new MyDictionary<double, char>
            {
                { 1.5, 'b' },
                { 2.8, 'c' }
            };

            Assert.Throws<ArgumentOutOfRangeException>(() => dictionary.CopyTo(array, -1));
        }

        [Fact]
        public void CopyTo_ArrayDoesNotHaveEnoughSpace_ShouldReturnExpectedResult()
        {
            var array = new KeyValuePair<string, double>[1];
            var dictionary = new MyDictionary<string, double>
            {
                { "BMW", 200.50 },
                { "MERCEDES", 320.80 }
            };

            Assert.Throws<ArgumentException>(() => dictionary.CopyTo(array, 0));
        }

        [Fact]
        public void GetEnumerator_DictionaryContainsFourElements_ShouldReturnExpectedResult()
        {
            var dictionary = new MyDictionary<string, int>
            {
                { "A", 1},
                { "B" , 2 },
                { "C" , 3 },
                { "D" , 4 }
            };

            var enumerator = dictionary.GetEnumerator();
            int result = 0;

            while (enumerator.MoveNext())
            {
                result += enumerator.Current.Value;
            }

            Assert.Equal(10, result);
        }

        [Fact]
        public void IndexProperty_KeysArePresentInDictionary_ShouldReturnExpectedResult()
        {
            var dictionary = new MyDictionary<int, string>
            {
                { 2, "first" },
                { 4, "second" },
                { 5, "third" }
            };

            Assert.Equal("first", dictionary[2]);
            Assert.Equal("second", dictionary[4]);
            Assert.Equal("third", dictionary[5]);
        }

        [Fact]
        public void IndexProperty_KeyIsPresentInDictionary_ShouldReturnExpectedResult()
        {
            var dictionary = new MyDictionary<int, string>
            {
                { 50, "fifty" }
            };

            dictionary[50] = "FIFTY";

            Assert.Equal("FIFTY", dictionary[50]);
        }

        [Fact]
        public void IndexProperty_KeyIsNotFound_ShouldReturnExpectedResult()
        {
            var dictionary = new MyDictionary<string, char>
            {
                { "Romania", 'R' },
                { "Belgium", 'B'},
                { "Spain", 'S' }
            };

            Assert.Throws<KeyNotFoundException>(() => dictionary["Q"]);
        }

        [Fact]
        public void IndexProperty_KeyIsNull_ShouldReturnExpectedResult()
        {
            var dictionary = new MyDictionary<string, int>
            {
                { "food", 200 }
            };

            Assert.Throws<ArgumentNullException>(() => dictionary[null]);
        }

        [Fact]
        public void Remove_KeyIsNull_ShouldReturnExpectedResult()
        {
            var dictionary = new MyDictionary<string, int>
            {
                { "ten", 10 },
                { "twenty", 20 }
            };

            Assert.Throws<ArgumentNullException>(() => dictionary.Remove(null));
        }

        [Fact]
        public void Remove_ElementIsFirstInBucket_ShouldReturnExpectedResult()
        {
            var dictionary = new MyDictionary<int, string>
            {
                { 4, "liverpool" },
                { 8, "arsenal" },
                { 9, "chelsea" }
            };

            Assert.True(dictionary.Remove(8));
            Assert.Equal(2, dictionary.Count);
            Assert.False(dictionary.ContainsKey(8));
        }

        [Fact]
        public void Remove_DictionaryContainsOneElement_ShouldReturnExpectedResult()
        {
            var dictionary = new MyDictionary<char, string>
            {
                { 'W', "Wales" }
            };

            Assert.True(dictionary.Remove('W'));
            Assert.Empty(dictionary);
        }

        [Fact]
        public void Remove_ElementIsSecondInBucket_ShouldReturnExpectedResult()
        {
            var dictionary = new MyDictionary<int, string>()
            {
                { 16, "sixteen" },
                { 24, "twentyfour" },
                { 32, "thirtytwo" },
                { 50, "fifty" }
            };

            Assert.True(dictionary.Remove(24));
            Assert.Equal(0, dictionary.GetElement(32)!.Next);
            Assert.Equal(3, dictionary.Count);
        }

        [Fact]
        public void Remove_OneElementIsRemovedAndAnotherIsAddedInSamePosition_ShouldReturnExpectedResult()
        {
            var dictionary = new MyDictionary<int, string>
            {
                { 70, "seventy" },
                { 85, "eighty-five" },
                { 20, "twenty" }
            };


            Assert.Equal(0, dictionary.GetElementIndex(70));
            Assert.True(dictionary.Remove(70));

            dictionary.Add(60, "sixty");
            Assert.Equal(0, dictionary.GetElementIndex(60));
        }

        [Fact]
        public void Remove_TwoElementsAreRemovedAndAnotherTwoAreAdded_ShouldReturnExpectedResult()
        {
            var dictionary = new MyDictionary<char, int>
            {
                { 'A', 10 },
                { 'B', 13 },
                { 'C', 25 }
            };

            Assert.True(dictionary.Remove('A'));
            Assert.True(dictionary.Remove('C'));
            Assert.Single(dictionary);

            dictionary.Add('X', 80);
            dictionary.Add('Y', 90);

            Assert.Equal(2, dictionary.GetElementIndex('X'));
            Assert.Equal(0, dictionary.GetElementIndex('Y'));
            Assert.Equal(3, dictionary.Count);
            Assert.Equal(3, dictionary.Keys.Count);
            Assert.Equal(3, dictionary.Values.Count);
        }

        [Fact]
        public void Remove_SecondElementFromTheFirstBucketIsRemoved_ShouldReturnExpectedResult()
        {
            var dictionary = new MyDictionary<int, string>
            {
                { 4, "lyon" },
                { 8, "psg" },
                { 10, "marseille" }
            };

            Assert.Equal(0, dictionary.GetElement(8)!.Next);
            Assert.True(dictionary.Remove(4));
            Assert.Equal(-1, dictionary.GetElement(8)!.Next);

            dictionary.Add(11, "rennes");

            Assert.Equal(0, dictionary.GetElementIndex(11));
        }

        [Fact]
        public void Remove_AKeyValuePairIsRemoved_ShouldReturnExpectedResult()
        {
            var dictionary = new MyDictionary<char, int>();
            var first = new KeyValuePair<char, int>('a', 1);
            var second = new KeyValuePair<char, int>('b', 2);
            dictionary.Add(first);
            dictionary.Add(second);

            Assert.True(dictionary.Remove(first));
            Assert.Single(dictionary);
            Assert.Single(dictionary.Keys);
            Assert.Single(dictionary.Values);
        }

        [Fact]
        public void Remove_InterHasNullValue_ShouldReturnExpectedResult()
        {
            var dictionary = new MyDictionary<int, string>();
            var roma = new KeyValuePair<int, string>(12, "roma");
            var inter = new KeyValuePair<int, string>(15, null);

            dictionary.Add(roma);
            dictionary.Add(inter);

            Assert.True(dictionary.Remove(inter));
        }


        [Fact]
        public void ResizeArray_DefaultInitialCapaticityIsFour_ShouldReturnExpectedResult()
        {
            var dictionary = new MyDictionary<int, string>
            {
                { 1, "first" },
                { 2, "second" },
                { 3, "third" },
                { 4, "fourth" },
                { 5, "fifth" }
            };

            Assert.Equal(5, dictionary.Count);
            Assert.Equal(5, dictionary.Keys.Count);
            Assert.Equal(5, dictionary.Values.Count);
        }

        [Fact]
        public void TryGetValue_ParisIsPresentInDictionaryAndRomeIsNot_ShouldReturnExpectedResult()
        {
            var dictionary = new MyDictionary<int, string>
            {
                { 10, "bucharest" },
                { 18, "paris" },
                { 24, "madrid" }
            };

            Assert.True(dictionary.TryGetValue(18, out string paris));
            Assert.False(dictionary.TryGetValue(12, out _));
            Assert.Equal("paris", paris);
        }

        [Fact]
        public void TryGetValue_ValueIsNull_ShouldReturnExpectedResult()
        {
            var dictionary = new MyDictionary<int, string>
            {
                { 1, null }
            };

            Assert.True(dictionary.TryGetValue(1, out _));
        }
    }
}