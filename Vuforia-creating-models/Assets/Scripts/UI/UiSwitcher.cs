using UnityEngine;

namespace UI
{
    public class UiSwitcher : MonoBehaviour
    {
        public UiErrorMessage UiErrorMessage;
        public UiBackground   UiBackground;

        public void SetLostMarkerMode()
        {
            UiErrorMessage.Show();
            UiBackground.Hide();
        }

        public void SetFoundMarkerMode()
        {
            UiErrorMessage.Hide();
            UiBackground.Show();
        }
    }
}
