using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using LorcanaLorebook.ScriptableObjects;
using TMPro;

namespace LorcanaLorebook.UI
{
    public class BinderDisplaySubpage : BinderSortSubpage
    {
        public override BinderSortState State { get => BinderSortState.Display; }

        public List<CardUI> CardSlots;
        public LorebookButton LeftButton;
        public LorebookButton RightButton;
        public TextMeshProUGUI PageCounterText;

        private List<Card> _cards;

        private int _cardsPerPage = 12;
        private int _pageCount;
        private int _currentPage;

        public override void InitializeSubpage(LorebookBinderSortPage owner)
        {
            base.InitializeSubpage(owner);


            //We need to sort all the cards like we want.
            _cards = _manager.Cards;

            //TODO: Users should be able to define sort order by any value on a card.
            _cards = _cards.OrderBy(card => card.InkColor)
                .ThenBy(card => card.Franchise)
                .ThenBy(card => card.CardType)
                .ThenBy(card => card.Name)
                .ThenBy(card => card.SubName).ToList();

            _pageCount = (int)MathF.Ceiling(_cards.Count / _cardsPerPage);
            _currentPage = 0;

            UpdateUI();

            LeftButton.Button.onClick.AddListener(() => OnPageButtonClick(true));
            RightButton.Button.onClick.AddListener(() => OnPageButtonClick(false));
        }

        public void OnPageButtonClick(bool isBack)
        {
            if(isBack)
            {
                _currentPage--;
            }            
            else
            {
                _currentPage++;
            }

            UpdateUI();
        }

        private void UpdateUI()
        {
            //Determine if the left or right button should be deactivated.
            LeftButton.Button.interactable = _currentPage == 0 ? false : true;
            RightButton.Button.interactable = _currentPage == _pageCount ? false : true;
            PageCounterText.text = $"Page: {_currentPage + 1} / {_pageCount}";
            PopulateCardSlots();
        }

        private void PopulateCardSlots()
        {
            //Populate the card slots.
            for (int i = 0; i < CardSlots.Count; i++)
            {
                CardSlots[i].InitializeCardUI(_cards[(_currentPage * _cardsPerPage) + i]);
            }
        }
    }
}