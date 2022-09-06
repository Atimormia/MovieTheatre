using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Utils
{
    public class ObjectsPool<T> where T:Component
    {
        private readonly List<T> _pool = new List<T>();
        private Transform _parent;
        private readonly T _prefab;
        
        public ObjectsPool(Transform parent, T prefab)
        {
            _parent = parent;
            _prefab = prefab;
        }

        public void SetParent(Transform parent)
        {
            _parent = parent;
        }

        public T GetFromPool(int i, Action<T> onCreate = null)
        {
            if (_pool.Count > i && _pool[i] != null)
            {
                _pool[i].transform.parent = _parent;
                return _pool[i];
            }

            var res = Object.Instantiate(_prefab, _parent);
            _pool.Add(res);
                
            onCreate?.Invoke(res);
            return res;
        }

        public void Clear()
        {
            foreach (var o in _pool)
            {
                Object.Destroy(o.gameObject);
            }
            _pool.Clear();
        }

        public void DisableAll()
        {
            foreach (var o in _pool)
            {
                o.gameObject.SetActive(false);
            }
        }
    }
}