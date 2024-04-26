using System.Collections.Generic;
using Factories.UI;
using UI.Tabs;
using UnityEngine;
using Zenject;

namespace UI.Home
{
	public class TabContent : MonoBehaviour
	{
		[SerializeField] private TabBarContentFactory _contentFactory;
		[SerializeField] private Transform _container;

		private readonly Dictionary<TabBarOption, Tab> _objects = new();
		private Tab _current;

		public void ChangeContent(TabBarOption option)
		{
			Hide(_current);
			Tab content = GetContentObject(option);
			Show(content);
			_current = content;
		}

		private Tab GetContentObject(TabBarOption option)
		{
			if (_objects.TryGetValue(option, out Tab content))
			{
				return content;
			}
			
			content = _contentFactory.Get(option.Data, _container);
			_objects.Add(option, content);
			return content;
		}

		private void Show(Tab target)
		{
			target.Show();
		}

		private void Hide(Tab target)
		{
			if (target == null)
				return;
			
			target.Hide();
		}
	}
}