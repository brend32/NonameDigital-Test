using System;
using System.Collections.Generic;
using Interfaces;
using UnityEngine;

namespace Utils
{
	public class GroupProgressAggregator
	{
		private readonly IProgressDisplay _consumer;
		private readonly List<Source> _sources = new();
		private int _sourcesCount;
		private int _currentSourceIndex;

		public GroupProgressAggregator(IProgressDisplay consumer, int sourcesCount)
		{
			if (consumer == null)
				throw new NullReferenceException("Consumer can't be null");
			
			_consumer = consumer;
			SourcesCount = sourcesCount;
		}

		public float Progress { get; private set; }
		public int SourcesCount
		{
			get => _sourcesCount;
			set
			{
				_sourcesCount = value;
				UpdateSourcesCount();
			}
		}

		private void UpdateSourcesCount()
		{
			SyncSourcesCount();
			
			for (int i = 0; i < _sources.Count; i++)
			{
				_sources[i].Weight = 1f / _sourcesCount;
			}
		}

		public IProgressDisplay GetNextSource()
		{
			if (_currentSourceIndex >= SourcesCount)
				throw new IndexOutOfRangeException("Not enough sources");
			
			return _sources[_currentSourceIndex++];
		}
		
		public void ResetProgress()
		{
			for (int i = 0; i < _sources.Count; i++)
			{
				_sources[i].Progress = 0;
			}

			Progress = 0;
			_consumer.UpdateProgress(0);
		}

		private void SyncSourcesCount()
		{
			if (_sources.Count < SourcesCount)
			{
				var difference = SourcesCount - _sources.Count;
				for (int i = 0; i < difference; i++)
				{
					_sources.Add(new Source()
					{
						ReportDifference = ReportDifference
					});
				}
			}
			else if (_sources.Count > SourcesCount)
			{
				var difference = _sources.Count - SourcesCount;
				for (int i = 0; i < difference; i++)
				{
					_sources[i].ReportDifference = null;
				}
				_sources.RemoveRange(SourcesCount, difference);
			}
		}

		private void ReportDifference(float difference)
		{
			Progress += difference;
			_consumer.UpdateProgress(Progress);
		}
		
		private class Source : IProgressDisplay
		{
			public float Progress;
			public float Weight;
			public Action<float> ReportDifference;
			
			public void UpdateProgress(float progress)
			{
				if (ReportDifference == null)
					return;
				
				progress = Mathf.Clamp01(progress);
				var difference = progress - Progress;
				Progress = progress;

				ReportDifference(difference * Weight);
			}

			public void ResetProgress()
			{
				UpdateProgress(0);
			}
		}
	}
}