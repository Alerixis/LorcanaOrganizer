using System.Collections.Generic;
using System.IO;
using Codice.Client.Common.GameUI;
using LorcanaSpellbook.Enums;
using LorcanaSpellbook.ScriptableObjects;
using LorcanaSpellbook.Utils;
using UnityEditor;
using UnityEditor.EditorTools;
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
                    break;
                case ToolState.Complete:
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
                    EditorGUILayout.BeginScrollView(_scrollPos);
                    for (int i = 0; i < _parsedCards.Count; i++)
                    {
                        RenderCard(_parsedCards[i]);
                    }
                    EditorGUILayout.EndScrollView();
                }
            }
        }

        private void RenderCard(Card card)
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                
            }
        }

        private void ParseCSV()
        {
            Debug.Log("CSV selected");
            _loadedCSV = File.ReadAllLines(_filePath);
            _parsedCards.Clear();
            for (int i = 1; i < _loadedCSV.Length - 1; i++)
            {
                //Take each line. And parse it out to each card.
                string[] splitLine = _loadedCSV[i].Split(",");

                Card newCard = new Card
                {
                    Name = splitLine[0],
                    Set = int.Parse(splitLine[1]),
                    Number = int.Parse(splitLine[2]),
                    InkColor = GeneralUtils.ConvertStringToEnum<InkColor>(splitLine[3]),
                    Rarity = GeneralUtils.ConvertStringToEnum<Rarity>(splitLine[4]),
                    CardType = GeneralUtils.ConvertStringToEnum<CardType>(splitLine[5]),
                    Franchise = splitLine[6]
                };

                _parsedCards.Add(newCard);
            }

            _currentState = ToolState.ShowResults;
        }
    }
}
