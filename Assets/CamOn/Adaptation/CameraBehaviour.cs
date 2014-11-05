using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CamOn.Properties;

namespace CamOn.Adaptation
{
	public class Setting
	{
		protected string category = "";
		protected Property property;
		public Setting (string category, Property property)
		{
			this.category = category;
			this.property = property;
		}
		
		public virtual void Apply(GameObject gameObject){
			if (gameObject.name == category && gameObject.tag != SubjectBackend.IGNORE_TAG) {
				if (gameObject.GetComponent<SubjectEvaluator> () == null) {
					gameObject.AddComponent<SubjectEvaluator> ();
				}
				Property p = property.Clone ();
				gameObject.GetComponent<SubjectEvaluator> ().AddProperty(p);
			}
		}
	}
	
	public class BoundsSetting : Setting {
		protected Vector3 boundsPosition, boundsScale;
		protected Quaternion boundsRotation;
		protected PrimitiveType boundsType;
		
		public BoundsSetting (string category, PrimitiveType type, Vector3 position, Quaternion rotation, Vector3 scale) : base(category, null)
		{
			boundsPosition = position;
			boundsRotation = rotation;
			boundsScale = scale;
			boundsType = type;
		}
		
		public override void Apply (GameObject gameObject)
		{
			if (gameObject.name == category && gameObject.tag != SubjectBackend.IGNORE_TAG) {
				if (gameObject.GetComponent<SubjectEvaluator> () == null) {
					gameObject.AddComponent<SubjectEvaluator> ();
				}
				gameObject.GetComponent<SubjectEvaluator> ().CameraBoundType = boundsType;
				gameObject.GetComponent<SubjectEvaluator> ().CameraBoundsCenter = boundsPosition;
				gameObject.GetComponent<SubjectEvaluator> ().CameraBoundsRotation = boundsRotation;
				gameObject.GetComponent<SubjectEvaluator> ().CameraBoundsScale = boundsScale;
			}
		}
	}
	
	public class CameraBehaviour
	{
		public static string AVATAR = "Player";
		
		public List<Setting> Settings = new List<Setting>();
		public float Reactivity = 0.1f;
		
		public readonly string Name;
		public CameraBehaviour (string name)
		{
			this.Name = name;
		}
		
		public static void DeactivateAll ()
		{
			foreach (SubjectEvaluator g in GameObject.FindObjectsOfType (typeof(SubjectEvaluator))) {
				g.Destroy ();
			}
		}
		
		public void Activate (GameObject rootNode, MonoBehaviour activator)
		{
			CamOnSettings.Instance.Reactivity = Reactivity;
			activator.StartCoroutine (activate (rootNode));
		}
		
		private IEnumerator activate (GameObject rootNode)
		{
			DeactivateAll ();
			yield return new WaitForEndOfFrame ();
			applySettings (GameObject.Find (AVATAR));
			yield return new WaitForEndOfFrame ();
			foreach (Transform t in rootNode.transform) {
				applySettings (t.gameObject);
			}
		}
		
		private void applySettings (GameObject gameObject)
		{
			foreach (Setting s in Settings)
				s.Apply (gameObject);
		}
	}
}

