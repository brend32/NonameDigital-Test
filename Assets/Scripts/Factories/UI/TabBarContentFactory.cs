using Data;
using UI.Tabs;
using UnityEngine;
using Zenject;

namespace Factories.UI
{
	public class TabBarContentFactory : MonoBehaviour
	{
		[Inject] private DiContainer _diContainer;

		public Tab Get(TabBarOptionData data, Transform container)
		{
			return _diContainer.InstantiatePrefabForComponent<Tab>(data.Prefab, container);
		}
	}
}