using System.Collections.Generic;
using LorcanaLorebook.ScriptableObjects;
using UnityEngine;
using LorcanaLorebook.UI;

namespace LorcanaLorebook
{

    public class LorebookManager : MonoBehaviour
    {
        public static LorebookManager Instance { get; private set; }

        [SerializeField]
        public List<LorebookPage> SerializedPages;
        public List<Card> Cards { get; private set; }

        private Dictionary<LorebookState, LorebookPage> _pagesByState = new Dictionary<LorebookState, LorebookPage>();

        private LorebookPage _activePage;

        // Start is called before the first frame update
        void Start()
        {
            if (Instance != null)
            {
                Debug.LogError("Woah! Who made another lorebook manager? Instance is nonnull");
                return;
            }

            Instance = this;

            //Get all pages
            foreach(LorebookPage page in SerializedPages)
            {
                _pagesByState.Add(page.LorebookState, page);
            }

            //Get the startup page and init.
            _activePage = _pagesByState[LorebookState.Startup];
            _activePage.Initialize(this);
        }

        public void OnCardsLoaded(List<Card> cards)
        {
            Cards = cards;
            ChangeState(LorebookState.MainMenu);
        }

        public void ChangeState(LorebookState newState)
        {
            if(newState == _activePage.LorebookState){
                Debug.LogWarning("Trying to set Lorebook state to the same state: " + newState.ToString());
                return;
            }

            _activePage.Disable();
            _activePage = _pagesByState[newState];
            _activePage.Initialize(this);
        }
    }
}