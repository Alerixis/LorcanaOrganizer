using System;
using LorcanaLorebook.Enums;
using UnityEngine;

namespace LorcanaLorebook.ScriptableObjects
{

    [Serializable]
    public class Card : ScriptableObject
    {
        [SerializeField]
        public int CardHash;

        [SerializeField]
        public string FullName;

        [SerializeField]
        public string Name;

        [SerializeField]
        public string SubName;

        [SerializeField]
        public int Set;

        /// <summary>
        /// Is a string due to Puppies. 4a, 4b, etc.
        /// </summary>
        [SerializeField]
        public string Number;

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

