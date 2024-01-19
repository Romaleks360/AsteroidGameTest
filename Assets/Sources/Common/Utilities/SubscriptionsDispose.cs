using System;

namespace Common.Utilities {
    
    public class SubscriptionsDispose : IDisposable {
        private readonly Action _onDispose;

        public SubscriptionsDispose(Action onDispose) => _onDispose = onDispose;

        public void Dispose() => _onDispose?.Invoke();
    }    
}


