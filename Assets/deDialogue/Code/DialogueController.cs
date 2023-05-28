using System;
using System.IO;
using UnityEngine;
using System.Linq;
using Newtonsoft.Json;
using deDialogue.Services;
using System.Collections.Generic;

namespace deDialogue
{
    public class DialogueController : MonoBehaviour
    {
        private ITextFileProvider _fileProvider;
        private ITextEventHandler _eventHandler;
        private ITextEffectHandler _effectHandler;
        
        public static DialogueController Instance { get; private set; }

        public TextPresenter TextPresenter;
        public PortraitController Portrait;

        public bool OnDialogue => _onDialogue;

        public event Action OnDialogueFinished = () => { };

        private List<DialogueData> _texts;
        private DialogueData _data;

        private bool _onDialogue;

        private int _line = 0;
        private bool _lineFinished;

        [ContextMenu("Build Text File")]
        private void BuildTextFile()
        {
            var l = new List<DialogueData>
            {
                new DialogueData
                {
                    ID = -1,
                    Lines = new[]
                    {
                        new DialogueLine
                        {
                            PortraitID = "-1",
                            Text = "This is a test dialogue, you shouldn't be seeing this..."
                        }
                    }
                },
                new DialogueData
                {
                    ID = 0,
                    Lines = new[]
                    {
                        new DialogueLine
                        {
                            PortraitID = "0",
                            Text =
                                "I woke up with this strange feeling in my head... Somehow, I only knew I had to get out of here."
                        }
                    }
                }
            };

            File.WriteAllText(_fileProvider.GetTextFileLocation(), JsonConvert.SerializeObject(l));
        }

        public void OnFinished()
        {
            _lineFinished = true;
        }

        public void ShowDialogue(int id)
        {
            // Load Dialogue Data
            string json = File.ReadAllText(_fileProvider.GetTextFileLocation());

            _texts = JsonConvert.DeserializeObject<List<DialogueData>>(json);
            _data = _texts.First(l => l.ID == id);

            _onDialogue = true;

            _line = 0;
            _lineFinished = false;

            var currentLine = _data.Lines[_line];

            _eventHandler.Handle(currentLine.Event);
            _effectHandler.Handle(currentLine.EffectID);

            TextPresenter.ShowBox(currentLine.Text);
            Portrait.Show(currentLine.PortraitID);
        }

        public void TriggerLineEnd()
        {
            if (!_lineFinished || !_onDialogue)
                return;
            
            NextLine();
        }
        
        public void Initialize(ITextFileProvider fileProvider, 
            ITextEventHandler eventHandler, ITextEffectHandler effectHandler)
        {
            _fileProvider = fileProvider;
            _eventHandler = eventHandler;
            _effectHandler = effectHandler;
        }

        private void NextLine()
        {
            TextPresenter.Clear();
            _lineFinished = false;

            _line++;
            if (_line >= _data.Lines.Length)
            {
                TextPresenter.HideBox();
                Portrait.Hide();
                
                OnDialogueFinished.Invoke();
                _onDialogue = false;
                return;
            }
            
            var currentLine = _data.Lines[_line];

            _eventHandler.Handle(currentLine.Event);
            _effectHandler.Handle(currentLine.EffectID);
            
            TextPresenter.Print(currentLine.Text);
            Portrait.Show(currentLine.PortraitID);
        }

        // private void HandleEvent(int id)
        // {
        //     switch (id)
        //     {
        //         case 0:
        //             break;
        //         
        //         case 1: // Intro Next Animation
        //             IntroController.Instance.NextAnimation();
        //             break;
        //     }
        // }
        
        // private void HandleEffect(int id)
        // {
        //     switch (id)
        //     {
        //         case 0:
        //             break;
        //         
        //         case 1: // Shake
        //             TextDisplayer.transform.DOShakePosition(0.5f, new Vector3(100, 0), 100).SetDelay(0.2f);
        //             Portrait.transform.DOShakePosition(0.5f, new Vector3(100, 0), 100).SetDelay(0.2f);
        //             break;
        //         
        //         case 2: // Flash
        //             DOTween.To(() => Flash.color, x => Flash.color = x, new Color(1f, 1f, 1f, 1f), 0.4f)
        //                 .From()
        //                 .SetDelay(0.2f);
        //             break;
        //     }
        // }

        private void Awake()
        {
            Instance = this;
        }
    }
}