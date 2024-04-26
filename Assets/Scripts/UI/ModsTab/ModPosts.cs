using System.Collections.Generic;
using System.Linq;
using Data;
using Factories.UI;
using Services;
using UnityEngine;
using Zenject;

namespace UI.ModsTab
{
	public class ModPosts : MonoBehaviour
	{
		[SerializeField] private Transform _container;
		[SerializeField] private ModPostFactory _factory;
		[SerializeField] private GameObject _noPostFoundMessage;
		
		[Inject] private ModsService _modsService;

		private readonly List<ModPost> _posts = new();
		private string _category;
		private string _searchQuery;

		public void SetCategory(string category)
		{
			_category = category;
			UpdateState();
		}

		public void SetSearchQuery(string searchQuery)
		{
			_searchQuery = searchQuery;
			UpdateState();
		}
		
		public void UpdateState()
		{
			var posts = GetPosts();
			
			if (_posts.Count < posts.Count)
			{
				var difference = posts.Count - _posts.Count;
				for (int i = 0; i < difference; i++)
				{
					_posts.Add(Create());
				}
			}
			else if (_posts.Count > posts.Count)
			{
				var difference = _posts.Count - posts.Count;
				for (int i = 0; i < difference; i++)
				{
					_posts[i].gameObject.SetActive(false);
				}
			}

			for (int i = 0; i < posts.Count; i++)
			{
				ModPost post = _posts[i];
				ModPostData data = posts[i];
				
				post.gameObject.SetActive(true);
				post.Display(data);
			}
			
			_noPostFoundMessage.SetActive(posts.Count == 0);
		}

		private List<ModPostData> GetPosts()
		{
			IEnumerable<ModPostData> source = _modsService.Data.Mods;

			if (string.IsNullOrEmpty(_category) == false && _category != "All")
				source = source.Where(mod => mod.Category == _category);

			if (string.IsNullOrEmpty(_searchQuery) == false)
				source = source
					.Where(mod =>
						mod.Title.Contains(_searchQuery) ||
						mod.Description.Contains(_searchQuery)
						);

			return source.ToList();
		}

		private ModPost Create()
		{
			return _factory.Get(_container);
		}
	}
}