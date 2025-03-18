using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KTool.Attribute;

namespace KTool.Loading
{
	public interface ILoadUi
	{
		#region Properties
		public const string TEXT_PROGRESS_FORMAT = "{0} %";
		public static ILoadUi Instance
		{
			get;
			protected set;
		}

		public float Progress
		{
			get;
			set;
		}
		public string TaskName
		{
			get;
			set;
		}

		public bool IsShow
		{
			get;
		}
		public bool IsChanging
		{
			get;
		}
		#endregion
		
		#region Menu Anim
		public void Show();
		public void Show(float time);
		public void Hide();
		public void Hide(float time);
		#endregion
	}
}
