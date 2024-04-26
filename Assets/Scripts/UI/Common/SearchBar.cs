using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Utils;

namespace UI.Common
{
	public class SearchBar : MonoBehaviour
	{
		[SerializeField] private TMP_InputField _inputField;
		[SerializeField] private Button _clear;
		[SerializeField] private int _delay = 200;

		private DelayedUpdater _valueUpdater;
		private string _value;

		public UnityEvent<string> ValueChanged;
		public string Value => _value;

		private void Awake()
		{
			_valueUpdater = new DelayedUpdater(this, UpdateValue, 200);
			
			_inputField.onValueChanged.AddListener(TextChanged);
		}

		private void OnEnable()
		{
			UpdateValue();
			_clear.gameObject.SetActive(string.IsNullOrEmpty(_value) ^ true);
		}

		public void Clear()
		{
			_inputField.SetTextWithoutNotify(string.Empty);
			OnEnable();
		}

		private void UpdateValue()
		{
			_value = _inputField.text;
			ValueChanged.Invoke(_value);
		}

		private void TextChanged(string value)
		{
			_clear.gameObject.SetActive(string.IsNullOrEmpty(value) ^ true);
			
			_valueUpdater.Update();
		}
	}
}