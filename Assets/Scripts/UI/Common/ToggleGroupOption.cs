using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Common
{
	[RequireComponent(typeof(Button))]
	public class ToggleGroupOption : MonoBehaviour
	{
		public TogglesGroup Group { get; private set; }
		public bool Selected { get; private set; }

		protected virtual void Awake()
		{
			GetComponent<Button>().onClick.AddListener(Select);
		}

		public void Link(TogglesGroup group)
		{
			Group = group;
		}

		public void Unlink(TogglesGroup group)
		{
			if (Group != group)
				return;

			Group = null;
		}

		public void Select()
		{
			if (Group == null)
				throw new Exception("Can't select option, option is not linked to the group");
			
			Group.Select(this);
		}

		public virtual void OnSelected()
		{
			Selected = true;
		}

		public virtual void OnUnselected()
		{
			Selected = false;
		}
	}
}