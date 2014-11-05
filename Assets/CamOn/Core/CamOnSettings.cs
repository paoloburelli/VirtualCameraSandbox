using System;
using UnityEngine;

[Serializable]
public class CamOnSettings: ScriptableObject
	{	
		private static CamOnSettings instance=null;
		public static CamOnSettings Instance {
			get {
				if (instance == null)
					instance = (CamOnSettings)UnityEngine.Resources.Load(typeof(CamOnSettings).Name,typeof(CamOnSettings));
					
				if (instance == null)
					instance = ScriptableObject.CreateInstance<CamOnSettings>();
			
				return instance;
			}
		}
		
		[HideInInspector]
		public float _reactivity=0.1f;
		public float Reactivity {
			get { return _reactivity; }
			set { _reactivity = Mathf.Clamp (value, 0.001f, 1); }
		}
		
		[HideInInspector]
		public float _speed = 1.0f;
		public float Speed {
			get { return _speed; }
			set { _speed = value > 0 ? value : 0.01f;}
		}
		
		[HideInInspector]
		public SubjectEvaluator MainSubject = null;
		[HideInInspector]
		public bool DebugOnScreen = true;
		
		[HideInInspector]
		public int _minFPS = 30;
		public int MinimumFramerate {
			get { return _minFPS; }
			set { _minFPS = value > 0 ? value : 1; }
		}
	}

