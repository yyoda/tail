using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Tail
{
    public class RingBuffer<T> : IEnumerable<T>
    {
        private int _size;
        private T[] _buffer;
        private bool[] _existence;
        private int _writeIndex;    // 書き終わった位置
        private int _readIndex;     // これから読み込む位置

        public RingBuffer(int size) 
        {
            _size = size;
            _buffer = new T[size];
            _existence = new bool[size];
            _writeIndex = -1;
            _readIndex = 0;
        }

        private int NextIndex(int ix)
        {
            return ++ix % _size;
        }

        public void Add(T value)
        {
            _writeIndex = NextIndex(_writeIndex);
            _buffer[_writeIndex] = value;
            if (_existence[_readIndex] && _writeIndex == _readIndex)
                _readIndex = NextIndex(_readIndex);
            _existence[_writeIndex] = true;
        }

        public T Get()
        {
            if (!Exists())
                throw new InvalidOperationException("バッファにデータはありません");
            T val = _buffer[_readIndex];
            _existence[_readIndex] = false;
            _readIndex = NextIndex(_readIndex);
            return val;
        }

        public bool Exists()
        {
            return _existence.Any(b => b == true);
        }

        public int Count
        {
            get
            {
                return _existence.Count(b => b == true);
            }
        }

        public void Clear()
        {
            _writeIndex = -1;
            _readIndex = 0;
            for (int i = 0; i < _size; i++)
                _existence[i] = false;
        }

        public IEnumerator<T> GetEnumerator()
        {
            while (Exists())
            {
                yield return Get();
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
