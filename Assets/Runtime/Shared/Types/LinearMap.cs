namespace Runtime.Shared
{
    using System;

    public struct LinearMap<TKey, TValue> where TKey : Enum
    {
        private readonly TValue[] _values;

        public LinearMap(int size)
        {
            _values = new TValue[size];
        }

        public TValue this[int key]
        {
            get
            {
                var index = ValidateKey(key);
                return _values[index];
            }
            
            set
            {
                var index = ValidateKey(key);
                _values[index] = value;
            }
        }

        private int ValidateKey(int key)
        {
            if (key < 0 || key >= _values.Length)
            {
                throw new IndexOutOfRangeException();
            }
                
            return key;
        }
        
        public static LinearMap<TKey, TValue> Create()
        {
            var enumLength = Enum.GetValues(typeof(TKey)).Length;
            return new LinearMap<TKey, TValue>(enumLength);
        }
    }
}