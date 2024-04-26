using UI.ModsTab;
using UnityEngine;
using Zenject;

namespace Factories.UI
{
	public class ModPostFactory : MonoBehaviour
	{
		[SerializeField] private ModPost _prefab;
		
		[Inject] private DiContainer _container;

		public ModPost Get(Transform container)
		{
			return _container.InstantiatePrefabForComponent<ModPost>(_prefab.gameObject, container);
		}
	}
}