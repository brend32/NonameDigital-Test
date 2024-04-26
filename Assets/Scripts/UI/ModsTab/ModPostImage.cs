using UI.Common;
using UnityEngine;
using UnityEngine.UI;

namespace UI.ModsTab
{
	public class ModPostImage : NetworkImage
	{
		[SerializeField] private AspectRatioFitter _ratioFitter;

		public override void UpdateImage(Texture2D texture)
		{
			base.UpdateImage(texture);
			if (texture == null)
				return;

			_ratioFitter.aspectRatio = texture.width / (float)texture.height;
		}
	}
}