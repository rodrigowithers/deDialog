using deDialogue;
using UnityEngine;

namespace Game.UI.Dialogue.Services
{
    public class GameController : MonoBehaviour
    {
        private DialogueController _controller;
        
        private void Start()
        {
            _controller.Initialize(new TextFileProvider(), new TextEventHandler(), new TextEffectHandler());
        }
    }
}