using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace HeroShooterMakerDemo
{
    
    public class UIMessageText : MonoBehaviour
    {
        public TextMeshProUGUI Text;
        public void ShowMessage(string message, float duration)
        {
            Text.text = message;
            Invoke("RemoveMessage", duration);
        }

        void RemoveMessage()
        {
            Text.text = "";
        }
    }

}
