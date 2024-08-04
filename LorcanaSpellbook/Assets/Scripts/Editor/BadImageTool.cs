using System.Collections.Generic;
using System.IO;
using System.Text;
using LorcanaLorebook.ScriptableObjects;
using UnityEditor;
using UnityEngine;

namespace LorcanaLorebook.Editor
{
    public class BadImageTool : EditorWindow
    {
        private static StringBuilder _failedImages = new StringBuilder();
        private static List<string> _failedCards = new List<string>();

        [MenuItem("Custom Tools/Bad Image Tool")]
        public static void FireTool()
        {
            Run();
        }

        public static void Run()
        {
            _failedCards.Clear();
            //Load all currently existing Card SO's
            string cardPath = Path.Combine(Application.dataPath, "Data/Cards");
            DirectoryInfo cardDir = new DirectoryInfo(cardPath);
            FileInfo[] allCardFiles = cardDir.GetFiles("*.*");
            foreach (FileInfo file in allCardFiles)
            {
                if (file.Name.EndsWith("meta"))
                {
                    continue;
                }

                Card card = AssetDatabase.LoadAssetAtPath<Card>("Assets/Data/Cards/" + file.Name);
                Texture2D image = card.CardImage;

                if(image.width < 1468 || image.height < 2048)
                {
                    _failedCards.Add(card.FullName + " Size: " + image.width + "," + image.height);
                }
            }

            if(_failedCards.Count > 0)
            {
                StringBuilder stringBuilder= new StringBuilder("Failed cards: " + _failedCards.Count + "\n\n");
                foreach(string card in _failedCards){
                    stringBuilder.AppendLine(card);
                }
                Debug.LogError(stringBuilder.ToString());
            }
        }
    }
}