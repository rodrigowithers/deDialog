using System.IO;
using UnityEngine;
using deDialogue.Services;

namespace Game.UI.Dialogue.Services
{
    public class TextFileProvider : ITextFileProvider
    {
        public string GetTextFileLocation()
        {
            return Path.Combine(Application.dataPath, "Resources", "Dialogue", "text.dtf");
        }
    }
}