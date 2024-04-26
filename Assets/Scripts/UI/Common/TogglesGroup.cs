using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace UI.Common
{
	public class TogglesGroup : MonoBehaviour
	{
		public UnityEvent<ToggleGroupOption> SelectionChanged;
		
		[SerializeField] private List<ToggleGroupOption> _options = new();
		[SerializeField] private ToggleGroupOption _selected;

		public ToggleGroupOption Selected => _selected;
		public IReadOnlyList<ToggleGroupOption> Options => _options;

		private void Awake()
		{
			LinkOptions();
		}

		private void Start()
		{
			if (_options.Count == 0)
				return;
			
			if (_selected == null)
				_selected = _options[0];
			
			Select(_selected);
		}

		public void UpdateAndLinkOptions(IEnumerable<ToggleGroupOption> options)
		{
			foreach (ToggleGroupOption option in _options)
			{
				option.Unlink(this);
			}

			_options.Clear();
			_options.AddRange(options);
			LinkOptions();

			Select(_options.Contains(_selected) ? _selected : _options[0]);
		}

		public void Select(ToggleGroupOption option, bool notify = true)
		{
			if (option == null)
				throw new Exception("Can't select, option is null");

			if (_options.Contains(option) == false)
				throw new Exception($"Can't select, group doesn't contain this option {option.GetType().Name} ({option.name})");

			Unselect();
			_selected = option;
			_selected.OnSelected();
			if (notify)
				SelectionChanged.Invoke(option);
		}

		private void LinkOptions()
		{
			foreach (ToggleGroupOption option in _options)
			{
				option.Link(this);
			}
		}

		private void Unselect()
		{
			if (_selected == null)
				return;

			_selected.OnUnselected();
		}
	}
}