using System;
using System.Threading;
using System.Threading.Tasks;
using Interfaces;
using UnityEngine;

namespace UI.Common
{
	public class ErrorWindow : MonoBehaviour
	{
		public void Show()
		{
			gameObject.SetActive(true);
		}

		public void Hide()
		{
			gameObject.SetActive(false);
		}
	}
}