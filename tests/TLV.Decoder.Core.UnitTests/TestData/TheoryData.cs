using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace TLV.Decoder.Core.UnitTests.TestData
{
    public abstract class TheoryData : IEnumerable<object[]>
    {
        readonly List<object[]> _data = new List<object[]>();

        protected void AddRow(params object[] values)
        {
            _data.Add(values);
        }

        public IEnumerator<object[]> GetEnumerator()
        {
            return _data.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
