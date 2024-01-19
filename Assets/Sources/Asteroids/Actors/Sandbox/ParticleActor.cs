using Asteroids.Actors;
using Common.Observables;
using UnityEngine;

public class ParticleActor : MonoBehaviour, IConsumableActor {
    
    [SerializeField] private float _speed;
    [SerializeField] private float _lifetime = 1f;
    
    public Collider Collider => GetComponent<Collider>();
    public IObservableValue<Vector3> Position { get; } = new ObservableValue<Vector3>();
    public IObservableValue<Vector3> Direction { get; } = new ObservableValue<Vector3>();
    public IObservableValue<float> Speed { get; } = new ObservableValue<float>();
    public IObservableValue<float> MaxSpeed { get; } = new ObservableValue<float>(3);
    public IObservableValue<float> LifeEnd { get; } = new ObservableValue<float>();
    
    public IObservableValue<bool> Alive { get; } = new ObservableValue<bool>(true);
    
    public IObservableValue<IActor> Surrogate { get; } = new ObservableValue<IActor>();
    
    private void OnEnable() {
        Direction.Value = transform.rotation.eulerAngles;
        Position.Value = transform.position;
        Speed.Value = _speed;
        
        LifeEnd.Value = Time.time + _lifetime;
        
        Position.Changed += OnPositionChanged;
    }
    
    private void OnPositionChanged(Vector3 position) {
        transform.position = position;
    }
    
    private void OnDisable() {
        Position.Changed -= OnPositionChanged;
    }
}
