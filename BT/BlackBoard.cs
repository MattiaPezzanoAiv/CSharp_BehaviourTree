using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BT
{
    public sealed class BlackBoard
    {
        private Dictionary<string, object> values;

        public BlackBoard()
        {
            values = new Dictionary<string, object>();
        }

        /// <summary>
        /// WARNING: if you try to set an alredy existing key with a different object(also type) the previous will be overwritten
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void SetValue(string key, object value)
        {
            values[key] = value;
        }

        /// <summary>
        /// It's your responsability to query the right type at the right key
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T ReadValue<T>(string key)
        {
            return (T)values[key];
        }



        public bool IsEqual(string obj1, string obj2)
        {
            return obj1 == obj2;
        }
    }
}
