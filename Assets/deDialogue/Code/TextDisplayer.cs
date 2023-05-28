using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;

namespace deDialogue
{
    public class TextDisplayer : MonoBehaviour
    {
        public RectTransform Box;
        public Text DialogueBox;
        public Image NextButton;

        private bool _printing;
        
        public void Clear()
        {
            DialogueBox.text = "";
            NextButton.enabled = false;
        }
        
        public async void ShowBox(string line)
        {
            _printing = false;
            NextButton.enabled = false;

            TweenShowBox();
            
            await Task.Delay(500);
            Print(line);
        }
        
        public void HideBox()
        {
            Clear();
            TweenHideBox();
        }

        public async void Print(string line)
        {
            _printing = true;
            NextButton.enabled = false;

            int character = 0;
            while (character < line.Length)
            {
                char c = line[character];
                DialogueBox.text += c;

                character++;

                if (c == ' ')
                    await Task.Delay(16 * 1);
                else
                    await Task.Delay(16 * 2);
            }

            _printing = false;
            DialogueController.Instance.OnFinished();

            await Task.Delay(500);
            
            if(DialogueController.Instance.OnDialogue)
                NextButton.enabled = true;
        }

        private async void TweenShowBox()
        {
            float t = 0.0f;
            float duration = 0.5f;

            Box.sizeDelta = new Vector2(0, 250);
                
            while (t <= duration)
            {
                t += Time.deltaTime;

                Box.sizeDelta = new Vector2(Mathf.Lerp(0, 1300, t / duration), 250);

                await Task.Delay(16);
            }
            
            Box.sizeDelta = new Vector2(1300, 250);
        }
        
        private async void TweenHideBox()
        {
            float t = 0.0f;
            float duration = 0.5f;

            Box.sizeDelta = new Vector2(1300, 250);
                
            while (t <= duration)
            {
                t += Time.deltaTime;

                Box.sizeDelta = new Vector2(Mathf.Lerp(1300, 0, t / duration), 250);

                await Task.Delay(16);
            }
            
            Box.sizeDelta = new Vector2(0, 250);
        }

        private void Start()
        {
            NextButton.enabled = false;
        }
    }
}