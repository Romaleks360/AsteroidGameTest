using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Common {
    
    public static class CommonExtensions {
        public static IEnumerable<T> Each<T>(this IEnumerable<T> source, Action<T> onItem) {
            var buffer = source.ToArray();

            foreach (var item in buffer) {
                onItem?.Invoke(item);
            }

            return buffer;
        }
        
        public static IEnumerable<T> For<T>(this IEnumerable<T> source, Action<int, T> onItem) {
            var buffer = source.ToArray();

            for (var index = 0; index < buffer.Length; index++) {
                onItem?.Invoke(index, buffer[index]);
            }

            return buffer;
        }
        
        public static Bounds OrthographicBounds(this Camera camera)
        {
            var screenAspect = Screen.width / (float)Screen.height;
            var cameraHeight = camera.orthographicSize;

            return new Bounds(
                camera.transform.position,
                new Vector3(cameraHeight * screenAspect, cameraHeight, 0));
        }

        public static TResult Take<TResult>(this IEnumerable<object> source) => source.OfType<TResult>().FirstOrDefault();
    }
    
}


