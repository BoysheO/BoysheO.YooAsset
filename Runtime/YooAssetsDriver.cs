using System.Diagnostics;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Debug = UnityEngine.Debug;

namespace YooAsset
{
    [ExecuteAlways]
    internal class YooAssetsDriver : MonoBehaviour
    {
        private static int LastestUpdateFrame = 0;
        private static YooAssetsDriver _yooAssetsDriverSinglon;

        public static YooAssetsDriver Instance
        {
            get
            {
                if (_yooAssetsDriverSinglon != null) return _yooAssetsDriverSinglon;
                var go = new UnityEngine.GameObject($"[{nameof(YooAssets)}]");
                _yooAssetsDriverSinglon = go.AddComponent<YooAssetsDriver>();
                go.hideFlags = HideFlags.DontSave;
                if(Application.isPlaying) UnityEngine.Object.DontDestroyOnLoad(go);
                return _yooAssetsDriverSinglon;
            }
        }
        
        void Update()
        {
            if (_yooAssetsDriverSinglon != null && _yooAssetsDriverSinglon!=this)
            {
                if(Application.isEditor) DestroyImmediate(gameObject);
                else Destroy(gameObject);
                return;
            }

            if (_yooAssetsDriverSinglon == null)
            {
                _yooAssetsDriverSinglon = this;
            }
            
            DebugCheckDuplicateDriver();
            YooAssets.Update();
        }

        [Conditional("DEBUG")]
        private void DebugCheckDuplicateDriver()
        {
            if (LastestUpdateFrame > 0)
            {
                if (LastestUpdateFrame == Time.frameCount)
                    YooLogger.Warning($"There are two {nameof(YooAssetsDriver)} in the scene. Please ensure there is always exactly one driver in the scene.");
            }

            LastestUpdateFrame = Time.frameCount;
        }
        
    }
}