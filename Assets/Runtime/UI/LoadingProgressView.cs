namespace Runtime.UI
{
    using System;
    using Cysharp.Threading.Tasks;
    using Unity.Mathematics;
    using UnityEngine;
    
    using Sirenix.OdinInspector;

    public sealed class LoadingProgressView : MonoBehaviour
    {
        [Serializable]
        public struct ProgressThrobberData
        {
            [SerializeField]
            [InlineProperty]
            public bool InitialDirection;
            
            [SerializeField]
            [InlineProperty]
            public float Interval;
        
            [SerializeField]
            [InlineProperty]
            public Color PingColor;
        
            [SerializeField]
            [InlineProperty]
            public Color PongColor;
        
            [SerializeField]
            [InlineProperty]
            public float RotationValue;
        
            [SerializeField]
            [InlineProperty]
            public float RotationSpeedLow;
        
            [SerializeField]
            [InlineProperty]
            public float RotationSpeedHigh;
        }

        [SerializeField]
        private UnityEngine.UI.Image _throbber;
        
        [SerializeField]
        private ProgressThrobberData _progressThrobberData;

        private bool _state;
        
        private void Start()
        {
            _state = _progressThrobberData.InitialDirection;
            DoSpinAsync().Forget();
        }

        private async UniTaskVoid DoSpinAsync()
        {
            while (gameObject.activeSelf)
            {
                await SpinAsync();
            }
        }

        private async UniTask SpinAsync()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(_progressThrobberData.Interval));

            var currRotationValue = 0f;
            var rotationDirection = _state ? 1 : -1;
            while (currRotationValue < _progressThrobberData.RotationValue)
            {
                var speedFactor = GetSpeed(currRotationValue / _progressThrobberData.RotationValue);
                var color = Color.Lerp(_progressThrobberData.PingColor, _progressThrobberData.PongColor, speedFactor);
                var rvalue = Time.deltaTime * math.lerp(_progressThrobberData.RotationSpeedLow, _progressThrobberData.RotationSpeedHigh, speedFactor);
                
                _throbber.transform.Rotate(0f, 0f, rvalue * rotationDirection);
                _throbber.color = color;
                currRotationValue += rvalue;

                await UniTask.Yield();
            }

            _state = !_state;
        }

        private float GetSpeed(float t)
        {
            return -4 * (t * t) + 4 * t;
        }
    }
}