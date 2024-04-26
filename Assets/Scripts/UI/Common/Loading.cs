using UnityEngine;

namespace UI.Common
{
	public class Loading : MonoBehaviour
	{
		private bool _visible = true;

		public bool Visible => _visible;

		public void Show()
		{
			_visible = true;
			gameObject.SetActive(true);
		}

		public void Hide()
		{
			_visible = false;
			gameObject.SetActive(false);
		}
	}
}