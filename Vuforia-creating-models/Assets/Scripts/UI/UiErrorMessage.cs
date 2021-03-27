using UnityEngine;

namespace UI
{
    public class UiErrorMessage : MonoBehaviour
    {
        public void Show() => gameObject.SetActive(true);
        public void Hide() => gameObject.SetActive(false);
    }
}
