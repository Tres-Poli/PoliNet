namespace Runtime.UI.LoadingScreen.Elements
{
    using System;
    using System.Threading;
    using Cysharp.Threading.Tasks;
    using Sirenix.OdinInspector;
    using Unity.Mathematics;
    using UnityEngine;

    public sealed class ThrobberUIElement : MonoBehaviour
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

        private bool _pingPongState;

        public async UniTaskVoid DoSpinAsync(CancellationToken ct)
        {
            _pingPongState = _progressThrobberData.InitialDirection;
            while (!ct.IsCancellationRequested)
            {
                await SpinAsync(ct);
            }
        }

        private async UniTask SpinAsync(CancellationToken ct)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(_progressThrobberData.Interval), cancellationToken: ct);

            var currRotationValue = 0f;
            var rotationDirection = _pingPongState ? 1 : -1;
            while (currRotationValue < _progressThrobberData.RotationValue && !ct.IsCancellationRequested)
            {
                var speedFactor = GetSpeed(currRotationValue / _progressThrobberData.RotationValue);
                var color = Color.Lerp(_progressThrobberData.PingColor, _progressThrobberData.PongColor, speedFactor);
                var rvalue = Time.deltaTime * math.lerp(_progressThrobberData.RotationSpeedLow, _progressThrobberData.RotationSpeedHigh, speedFactor);
                
                _throbber.transform.Rotate(0f, 0f, rvalue * rotationDirection);
                _throbber.color = color;
                currRotationValue += rvalue;

                await UniTask.Yield();
            }

            _pingPongState = !_pingPongState;
        }

        private float GetSpeed(float t)
        {
            return -4 * (t * t) + 4 * t;
        }
    }
}