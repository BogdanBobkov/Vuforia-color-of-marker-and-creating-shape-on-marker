using UnityEngine;

namespace UI
{
    public abstract class UiAbstractMenu : MonoBehaviour
    {
        public void Show() => gameObject.SetActive(true);
        public void Hide() => gameObject.SetActive(false);
    }
}
