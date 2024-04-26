using TMPro;
using UI.Common;
using UnityEngine;
using UnityEngine.UI;

namespace UI.ModsTab
{
	public class CategoryOption : ToggleGroupOption
	{
		[SerializeField] private Image _image;
		[SerializeField] private TextMeshProUGUI _text;

		private CategorySelector _group;
		
		public string Value { get; private set; }

		private void Start()
		{
			UpdateState();
		}

		public void Link(CategorySelector group)
		{
			_group = group;
		}

		public void SetValue(string value)
		{
			_text.text = value;
			Value = value;
		}

		private void UpdateState()
		{
			_image.sprite = Selected ? _group.SelectedState : _group.NormalState;
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
	}
}