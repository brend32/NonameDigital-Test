using System;
using System.Linq;
using UI.Common;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Home
{
	public class TabBar : MonoBehaviour
	{
		[SerializeField] private TogglesGroup _group;
		[SerializeField] private TabContent _content;
		[SerializeField] private Color _selectedColor;
		[SerializeField] private Color _normalColor;

		private ToggleGroupOption _selected;

		public Color SelectedColor => _selectedColor;
		public Color NormalColor => _normalColor;

		private void Awake()
		{
			LinkOptions();
			
			_group.SelectionChanged.AddListener(SelectionChanged);
		}

		private void SelectionChanged(ToggleGroupOption option)
		{
			var barOption = (TabBarOption)option;
			if (barOption == _selected)
				return;

			_selected = barOption;
			ChangeActiveScreen(barOption);
		}

		private void ChangeActiveScreen(TabBarOption option)
		{
			_content.ChangeContent(option);
		}

		private void LinkOptions()
		{
			foreach (TabBarOption option in _group.Options.Cast<TabBarOption>())
			{
				option.Link(this);
			}
		}
	}
}