using System;
using System.Collections.Generic;
using System.Linq;
using Common;
using UnityEngine;

namespace Asteroids.Actors {
    
    [CreateAssetMenu(menuName = "Create FactoryMethod", fileName = "FactoryMethod", order = 0)]
    public class FactoryMethod : ScriptableObject {
        
        [SerializeField] private GameObject _prefab;
        
        public bool ActorIs<TActor>() where TActor : class, IActor => _prefab.gameObject.TryGetComponent<TActor>(out _);
        public bool ActorIs(Type type) => _prefab.gameObject.TryGetComponent(type, out _);
        
        public IActor Create(Vector3 position, Vector3 direction) {
            var instance = Instantiate(_prefab, position, Quaternion.Euler(direction));
            
            return instance.GetComponent<IActor>();
        }
        
        public void DestroyActor(IActor actor) {
            if(actor is MonoBehaviour mono) Destroy(mono.gameObject);
        }
    }
}
