using Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Common
{
	public class LoadingWithProgress : Loading, IProgressDisplay
	{
		[SerializeField] private TextMeshProUGUI _text;
		[SerializeField] private Image _bar;

		public void UpdateProgress(float progress)
		{
			_text.text = $"{progress * 100:0.00}%";
			_bar.fillAmount = progress;
		}

		public void ResetProgress()
		{
			UpdateProgress(0);
		}
	}
}