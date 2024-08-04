using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LorcanaSpellbook.Enums;
using LorcanaSpellbook.ScriptableObjects;
using LorcanaSpellbook.Utils;
using UnityEditor;
using UnityEngine;

namespace LorcanaSpellbook.Editor
{
    public class CardImportTool : EditorWindow
    {
        private enum ToolState
        {
            SelectCSV,
            ShowResults,
            CreateCards,
            Complete,
        }

        private ToolState _currentState = ToolState.SelectCSV;
        private string _filePath;
        private string[] _loadedCSV;
        private Vector2 _scrollPos;
        private List<Card> _parsedCards = new List<Card>();
        private Dictionary<string, Texture2D> _loadedCardImages = new Dictionary<string, Texture2D>();

        [MenuItem("Custom Tools/Card Importer")]
        public static void OpenCardImportTool()
        {
            CardImportTool tool = GetWindow<CardImportTool>();
            tool.titleContent = new GUIContent("Card Import");
        }

        public void OnGUI()
        {
            switch (_currentState)
            {
                case ToolState.SelectCSV:
                    RenderCSVState();
                    break;
                case ToolState.ShowResults:
                    RenderResults();
                    break;
                case ToolState.CreateCards:
                    RenderCreateState();
                    break;
                case ToolState.Complete:
                    RenderComplete();
                    break;
            }
        }

        private void RenderCSVState()
        {
            using (new EditorGUILayout.VerticalScope("box"))
            {
                using (new EditorGUILayout.HorizontalScope())
                {
                    EditorGUILayout.LabelField("Selected file to import: " + (string.IsNullOrEmpty(_filePath) ? "select a file" : _filePath));
                }
                using (new EditorGUILayout.HorizontalScope())
                {
                    if (GUILayout.Button("Select CSV File"))
                    {
                        _filePath = EditorUtility.OpenFilePanel("CSV", "", "");
                    }
                }

                using (new EditorGUI.DisabledScope(string.IsNullOrEmpty(_filePath)))
                {
                    if (GUILayout.Button("Parse CSV for Cards"))
                    {
                        ParseCSV();
                    }
                }
            }
        }

        private void RenderResults()
        {
            using (new EditorGUILayout.VerticalScope("box"))
            {
                EditorGUILayout.LabelField("Results:");
                using (new EditorGUILayout.VerticalScope("box"))
                {
                    _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos);
                    for (int i = 0; i < _parsedCards.Count; i++)
                    {
                        RenderCard(_parsedCards[i]);
                    }
                    EditorGUILayout.EndScrollView();

                    if (GUILayout.Button("Continue"))
                    {
                        _currentState = ToolState.CreateCards;
                    }
                }
            }
        }

        private void RenderCard(Card card)
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                EditorGUILayout.LabelField(card.FullName);
            }
        }

        private void RenderCreateState()
        {
            if (GUILayout.Button("Import Cards"))
            {
                CreateOrUpdateCardAssets();
            }
        }

        private void ParseCSV()
        {
            _loadedCSV = File.ReadAllLines(_filePath);
            _parsedCards.Clear();

            LoadImages();

            for (int i = 1; i < _loadedCSV.Length - 1; i++)
            {
                //Take each line. And parse it out to each card.
                string[] splitLine = _loadedCSV[i].Split(",");
                if (splitLine.Length != 7)
                {
                    continue;
                }

                Card newCard = CreateInstance<Card>();
                newCard.FullName = splitLine[0];
                newCard.Set = int.Parse(splitLine[1]);
                newCard.Number = splitLine[2];
                newCard.InkColor = GeneralUtils.ConvertStringToEnum<InkColor>(splitLine[3]);
                newCard.Rarity = GeneralUtils.ConvertStringToEnum<Rarity>(splitLine[4]);
                newCard.CardType = GeneralUtils.ConvertStringToEnum<CardType>(splitLine[5]);
                newCard.Franchise = splitLine[6];

                string[] splitName = newCard.FullName.Split(" - ");
                if (splitName.Length != 2)
                {
                    newCard.Name = splitName[0];
                }
                else
                {
                    newCard.Name = splitName[0];
                    newCard.SubName = splitName[1];
                }

                //Keep ourselves from creating new cards when we have matching hash codes. Will allow for easy updating as well.
                string cardHashString = newCard.FullName + newCard.Set + newCard.Number;
                newCard.CardHash = cardHashString.GetHashCode();

                newCard.CardImage = GetImageForCard(newCard.FullName);

                _parsedCards.Add(newCard);
            }

            _currentState = ToolState.ShowResults;
        }

        private void LoadImages()
        {
            _loadedCardImages.Clear();
            string imagePath = Path.Combine(Application.dataPath, "Images/Cards");
            DirectoryInfo imageDir = new DirectoryInfo(imagePath);
            FileInfo[] setFiles = imageDir.GetFiles();
            for (int i = setFiles.Length - 1; i >= 0; i--)
            {
                if (setFiles[i].Name.EndsWith(".meta"))
                {
                    continue;
                }

                var image = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Images/Cards/" + setFiles[i].Name);

                if(image == null)
                {
                    Debug.LogError("No image loaded for: " + setFiles[i].Name);
                    continue;
                }

                _loadedCardImages.Add(setFiles[i].Name.Replace(".png", "").ToLower().Replace(".jpg", ""), image);
            }
        }

        private Texture2D GetImageForCard(string cardName)
        {
            if(!_loadedCardImages.TryGetValue(cardName.ToLower(), out Texture2D image))
            {
                Debug.LogError("Unable to find image for card: " + cardName.ToLower());
                return null;
            }
            return image;
        }

        private void CreateOrUpdateCardAssets()
        {

            //Load all currently existing Card SO's
            string cardPath = Path.Combine(Application.dataPath, "Data/Cards");
            DirectoryInfo cardDir = new DirectoryInfo(cardPath);
            FileInfo[] allCardFiles = cardDir.GetFiles("*.*");
            List<Card> existingCards = new List<Card>();
            foreach (FileInfo file in allCardFiles)
            {
                if (!file.Name.EndsWith("meta"))
                {
                    existingCards.Add(AssetDatabase.LoadAssetAtPath<Card>("Assets/Data/Cards/" + file.Name));
                }
            }

            for (int i = 0; i < _parsedCards.Count; i++)
            {
                //If this card matches an existing hash code. Just update that card.
                var matchingCards = existingCards.Where(card => card.CardHash == _parsedCards[i].CardHash).ToArray();
                if (matchingCards.Length > 0)
                {
                    if (matchingCards.Length == 1)
                    {
                        //Just ignore this card. 
                        Debug.Log("Card already imported: " + _parsedCards[i].FullName + " updating card with this info");
                        matchingCards[0] = _parsedCards[i];
                        
                        AssetDatabase.SaveAssetIfDirty(matchingCards[0]);
                        continue;
                    }
                    else
                    {
                        Debug.LogError("There are multiple cards matching hash: " + _parsedCards[i].CardHash + " this is not good. Validate the csv for card index: " + i + " and name: " + _parsedCards[i].FullName);
                        continue;
                    }
                }

                //Otherwise, make a new one.
                Card newCard = _parsedCards[i];

                AssetDatabase.CreateAsset(newCard, "Assets/Data/Cards/" + newCard.Name + (string.IsNullOrEmpty(newCard.SubName) ? "" : "_" + newCard.SubName) + "_" + newCard.Set + "_" + newCard.Number + ".asset");
                Debug.Log("Card Created: " + newCard.FullName);
            }

            _currentState = ToolState.Complete;
        }

        private void RenderComplete()
        {
            EditorGUILayout.LabelField("All done!");
            if (GUILayout.Button("Restart?"))
            {
                _currentState = ToolState.SelectCSV;
                _filePath = string.Empty;
                _parsedCards.Clear();
            }
        }
    }
}
