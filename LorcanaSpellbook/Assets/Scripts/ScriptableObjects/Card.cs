using System;
using LorcanaSpellbook.Enums;
using UnityEngine;

namespace LorcanaSpellbook.ScriptableObjects
{

    [Serializable]
    public class Card : ScriptableObject
    {
        [SerializeField]
        public string FullName;

        [SerializeField]
        public string Name;

        [SerializeField]
        public string SubName;

        [SerializeField]
        public int Set;

        [SerializeField]
        public int Number;

        [SerializeField]
        public InkColor InkColor;

        [SerializeField]
        public Rarity Rarity;

        [SerializeField]
        public CardType CardType;

        [SerializeField]
        public string Franchise;

        [SerializeField]
        public Texture2D CardImage;
    }
}

