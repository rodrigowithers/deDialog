using System;

namespace deDialogue
{
    [Serializable]
    public class DialogueData
    {
        public int ID;
        public DialogueLine[] Lines;
    }

    [Serializable]
    public class DialogueLine
    {
        public string PortraitID;
        public int EffectID;
        public int Event;
        public string Text;
    }
}