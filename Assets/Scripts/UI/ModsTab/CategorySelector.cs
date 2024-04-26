using System.Collections.Generic;
using System.Linq;
using UI.Common;
using UnityEngine;
using UnityEngine.Events;

namespace UI.ModsTab
{
	public class CategorySelector : MonoBehaviour
	{
		[SerializeField] private TogglesGroup _group;
		[SerializeField] private Sprite _normalState;
		[SerializeField] private Sprite _selectedState;
		[SerializeField] private Transform _container;
		[SerializeField] private CategoryOption _prefab;

		private readonly List<CategoryOption> _options = new();
		private CategoryOption _selected;

		public UnityEvent<string> CategoryChanged;
		public Sprite SelectedState => _selectedState;
		public Sprite NormalState => _normalState;
		
		private void Awake()
		{
			LinkOptions();
			
			_group.SelectionChanged.AddListener(SelectionChanged);
		}

		public void UpdateCategories(IList<string> categories)
		{
			if (_options.Count < categories.Count)
			{
				var difference = categories.Count - _options.Count;
				for (int i = 0; i < difference; i++)
				{
					_options.Add(CreateOption());
				}
			}
			else if (_options.Count > categories.Count)
			{
				var difference = _options.Count - categories.Count;
				for (int i = 0; i < difference; i++)
				{
					_options[i].gameObject.SetActive(false);
				}
			}

			for (int i = 0; i < categories.Count; i++)
			{
				CategoryOption option = _options[i];
				var category = categories[i];
				
				option.gameObject.SetActive(true);
				option.SetValue(category);
			}

			_selected = null;
			_group.UpdateAndLinkOptions(_options.Take(categories.Count));
		}

		private CategoryOption CreateOption()
		{
			CategoryOption option = Instantiate(_prefab, _container);
			option.Link(this);
			return option;
		}

		private void SelectionChanged(ToggleGroupOption option)
		{
			var barOption = (CategoryOption)option;
			if (barOption == _selected)
				return;

			_selected = barOption;
			CategoryChanged.Invoke(_selected.Value);
		}

		private void LinkOptions()
		{
			foreach (CategoryOption option in _group.Options.Cast<CategoryOption>())
			{
				option.Link(this);
			}
		}
	}
}