using Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.ModsTab
{
	public class ModPost : MonoBehaviour
	{
		[SerializeField] private TextMeshProUGUI _title;
		[SerializeField] private TextMeshProUGUI _description;
		[SerializeField] private ModPostImage _modPostImage;
		[SerializeField] private ModPostActionButton _actionButton;

		public void Display(ModPostData data)
		{
			_title.text = data.Title;
			_description.text = data.Description;
			_modPostImage.Load(data.PreviewPath);
			_actionButton.UpdateState(data);
		}
	}
}