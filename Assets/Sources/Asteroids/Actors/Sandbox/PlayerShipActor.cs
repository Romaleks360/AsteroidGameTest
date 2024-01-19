using Asteroids.Actors;
using Asteroids.Systems;
using Common.Observables;
using UnityEngine;

public class PlayerShipActor : MonoBehaviour, IControllableActor {
    [SerializeField] private Collider _collider;
    [SerializeField] private Transform _aim;
    
    public Collider Collider => _collider;
    
    public IObservableValue<bool> Alive { get; } = new ObservableValue<bool>(true);
    public IObservableValue<Vector3> Position { get; } = new ObservableValue<Vector3>();
    public IObservableValue<Vector3> Direction { get; } = new ObservableValue<Vector3>();
    public IObservableValue<Vector3> Aim { get; } = new ObservableValue<Vector3>();
    public IObservableValue<float> Speed { get; } = new ObservableValue<float>(1);
    
    public IObservableValue<float> MaxSpeed { get; } = new ObservableValue<float>(10);
    
    public IObservableValue<Vector2> Input { get; } = new ObservableValue<Vector2>();
    
    
    public IObservableValue<InputAction> Action { get; } = new ObservableValue<InputAction>();
    
    private void OnEnable() {
        Position.Changed += OnPositionChanged;
        Aim.Changed += OnAimChanged;
    }
    
    private void OnAimChanged(Vector3 rotation) {
        _aim.rotation = Quaternion.Euler(rotation);
    }
    
    private void OnPositionChanged(Vector3 position) {
        transform.position = position;
    }
    
    private void OnDisable() {
        Position.Changed -= OnPositionChanged;
        Aim.Changed += OnAimChanged;
    }

}