using System.Collections.Generic;
using System.Linq;

namespace Common.StateMachines {
    
    public interface IState {
        void Enter();
        void Exit();
    }
    
    public abstract class StateMachine<TState> where TState: class, IState {

        private readonly HashSet<TState> _states = new();
        private readonly Stack<TState> _history = new();

        private TState _currentState;

        protected void AddState(TState state) {
            _states.Add(state);
        }
        
        public void Back() {
            if (_history.Count == 0) return;
            
            ChangeState(_history.Pop());
        }

        public void ChangeState<T>() where T : class, IState {
            var state = _states.OfType<T>().FirstOrDefault();
            
            if(_currentState != null) _history.Push(_currentState);
            
            ChangeState(state as TState);
        }
        
        private void ChangeState(TState state) {
            if (_currentState != null) {
                _currentState.Exit();
                OnStateExit(_currentState);
            }
            
            _currentState = state;
            
            if (_currentState != null) {
                _currentState.Enter();
                OnStateEnter(_currentState);
            }
        }

        protected virtual void OnStateEnter(TState state) {}
        protected virtual void OnStateExit(TState state) {}

    }
    
}
