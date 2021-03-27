using UnityEngine;
using UI;
using Controllers;

namespace Internals
{
    public class Locator : MonoBehaviour
    {
        private static Locator _instance;

        [SerializeField] private UiSwitcher _uiSwitcher;
        public static UiSwitcher UiSwitcher => _instance._uiSwitcher;

        [SerializeField] private SceneController _sceneController;
        public static SceneController SceneController => _instance._sceneController;

        private void Awake()
        {
            if (_instance != null)
            {
                Destroy(this);
                return;
            }
            _instance = this;
        }
    }
}
