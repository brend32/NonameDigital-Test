using UnityEngine;

namespace UI.Tabs
{
	public abstract class Tab : MonoBehaviour
	{
		public virtual void Show()
		{
			gameObject.SetActive(true);
		}

		public virtual void Hide()
		{
			gameObject.SetActive(false);
		}
	}
}