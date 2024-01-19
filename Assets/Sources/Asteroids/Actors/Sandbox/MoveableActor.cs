using Common.Observables;
using UnityEngine;

public abstract class MoveableActor : MonoBehaviour {
    [SerializeField] private Collider _collider;
    [SerializeField] private float _speed;
    
    public Collider Collider => _collider;
    
    public IObservableValue<bool> Alive { get; } = new ObservableValue<bool>(true);
    public IObservableValue<Vector3> Position { get; } = new ObservableValue<Vector3>();
    public IObservableValue<Vector3> Direction { get; } = new ObservableValue<Vector3>();
    public IObservableValue<float> Speed { get; } = new ObservableValue<float>();
    public IObservableValue<float> MaxSpeed { get; } = new ObservableValue<float>(3);
    
    private void OnEnable() {
        Direction.Value = transform.rotation.eulerAngles;
        Position.Value = transform.position;
        Speed.Value = _speed;
        
        Position.Changed += OnPositionChanged;
    }
    
    private void OnPositionChanged(Vector3 position) {
        transform.position = position;
    }
    
    private void OnDisable() {
        Position.Changed -= OnPositionChanged;
    }
}
