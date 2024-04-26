using System;
using Data;
using TMPro;
using UI.Common;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Home
{
	public class TabBarOption : ToggleGroupOption
	{
		[SerializeField] private TabBarOptionData _data;
		[SerializeField] private Image _icon;
		[SerializeField] private TextMeshProUGUI _text;

		private TabBar _bar; 

		public TabBarOptionData Data => _data;

		protected override void Awake()
		{
			base.Awake();
			_text.text = _data.Name;
			_icon.sprite = _data.Icon;
		}

		private void Start()
		{
			UpdateState();
		}

		public void Link(TabBar bar)
		{
			_bar = bar;
		}

		private void UpdateState()
		{
			Color color = Selected ? _bar.SelectedColor : _bar.NormalColor;

			_text.color = color;
			_icon.color = color;
		}

		public override void OnSelected()
		{
			base.OnSelected();
			UpdateState();
		}

		public override void OnUnselected()
		{
			base.OnUnselected();
			UpdateState();
		}
		
#if UNITY_EDITOR
		private void OnValidate()
		{
			_text.text = _data.Name;
			_icon.sprite = _data.Icon;
		}
#endif
	}
}