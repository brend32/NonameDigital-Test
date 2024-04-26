using System;
using System.Collections;
using UnityEngine;

namespace Utils
{
    public sealed class DelayedUpdater
    {
        private readonly Action _onUpdate;
        private readonly int _timeoutMilliseconds;
        private readonly MonoBehaviour _owner;

        private Coroutine _coroutine;
        
        public DelayedUpdater(MonoBehaviour owner, Action onUpdate, int timeoutMilliseconds)
        {
            _owner = owner;
            _onUpdate = onUpdate;
            _timeoutMilliseconds = timeoutMilliseconds;
        }

        public void Update()
        {
            if (_coroutine != null)
                _owner.StopCoroutine(_coroutine);

            _coroutine = _owner.StartCoroutine(Delay());
        }

        private IEnumerator Delay()
        {
            yield return new WaitForSeconds(_timeoutMilliseconds / 1000f);
            _onUpdate();
        }
    }
}