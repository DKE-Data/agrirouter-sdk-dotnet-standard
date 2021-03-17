using System;
using System.Collections.Generic;
using System.Text;

namespace Agrirouter.Impl.Service.Onboard
{
    public class ParameterDictionary<TKey, TValue> : Dictionary<TKey, TValue>
    {
        public ParameterDictionary() : base() { }

        public TValue GetValueOrDefault(TKey key)
        {
            TValue resultValue;
            if (base.TryGetValue(key,out resultValue) == true)
            {
                return resultValue;
            } else
            {
                return default(TValue);
            }

        }
    }
}
