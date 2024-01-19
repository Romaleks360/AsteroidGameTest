using System;

namespace Common.Observables {

    public interface IObservableValue<T> {
        
        T Value { get; set; }
        
        event Action<T> Changed;
    }

    public class ObservableValue<T> : IObservableValue<T> {

        private readonly bool _notifyAtSubscription;
        
        private event Action<T> _changed;
        private T _value;

        public T Value {
            get => _value;
            set {
                if (!value.Equals(_value)) {
                    _value = value;
                    _changed?.Invoke(_value);
                }
            }
        }
        public event Action<T> Changed {
            add {
                _changed += value;
                
                if(_notifyAtSubscription)
                    value?.Invoke(_value);
            }
            remove => _changed -= value;
        }

        public ObservableValue(T defaultValue = default, bool notifyAtSubscription = true) {
            _value = defaultValue;
            _notifyAtSubscription = notifyAtSubscription;
        }
    }
    
}

