using System;
using UI.ModsTab;
using Services;
using UnityEngine;
using Zenject;

namespace UI.Tabs
{
	public class ModsTab : Tab
	{
		[SerializeField] private CategorySelector _categorySelector;
		[SerializeField] private ModPosts _posts;
		
		[Inject] private ModsService _modsService;

		private void Start()
		{
			_categorySelector.UpdateCategories(_modsService.Data.Categories);
			_posts.UpdateState();
		}
	}
}