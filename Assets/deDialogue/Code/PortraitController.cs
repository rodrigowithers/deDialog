using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace deDialogue
{
    public class PortraitController : MonoBehaviour
    {
        public Image PortraitSprite;
        public CanvasGroup AlphaGroup;

        private string _lastId;
        
        public void Show(string id)
        {
            if (id == "0")
            {
                AlphaGroup.alpha = 0;
                _lastId = id;
                
                return;
            }
            
            if (id != _lastId)
            {
                AlphaGroup.alpha = 0;
             
                Sprite s = Resources.Load<Sprite>(id);
                PortraitSprite.sprite = s;
                
                ShowTween();
            }

            _lastId = id;
        }

        public void Hide()
        {
            HideTween();
        }

        private async void ShowTween()
        {
            AlphaGroup.alpha = 0f;
            
            float t = 0.0f;
            float duration = 0.5f;

            while (t <= duration)
            {
                t += Time.deltaTime;

                AlphaGroup.alpha = t / duration;

                await Task.Delay(16);
            }
            
            AlphaGroup.alpha = 1f;
        }

        private async void HideTween()
        {
            float t = 0.0f;
            float duration = 0.5f;

            AlphaGroup.alpha = 1f;
            
            while (t <= duration)
            {
                t += Time.deltaTime;

                AlphaGroup.alpha = 1 - (t / duration);

                await Task.Delay(16);
            }
            
            AlphaGroup.alpha = 0f;
        }
    }
}
