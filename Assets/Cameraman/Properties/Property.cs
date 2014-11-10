using System;
using UnityEngine;
using System.Collections.Generic;

[Serializable]
public class Property
{
	public enum PropertyType {ProjectionSize, VantageAngle, PositionOnScreen, RelativePosition};
	
	public PropertyType Type{
		get {return type;}
	}

	public float Weight{
		get {return weight;}
		set {weight = (value <= 1) ? (value >= 0) ? value : 0 : 1;}
	}
	
	[SerializeField]
	protected PropertyType type;
	
	[SerializeField]
	protected float[] desiredValues;
	
	[SerializeField]
	protected int[] subjectReferences;
	
	[SerializeField]
	private float weight;

	private float satisfaction;
	public float Satisfaction {
		get {
			return satisfaction;
		}
	}
	
	public Property(Property p){
		weight = p.Weight;
		type = p.Type;
		desiredValues = p.desiredValues;
		subjectReferences = p.subjectReferences;
	}
	
	public Property (float weight)
	{
		this.Weight = weight;
		subjectReferences = new int[1];
		desiredValues = new float[1];
	}

	public float Evaluate(Subject[] subjectsList){
		satisfaction = evaluate(subjectsList);
		return satisfaction;
	}

	protected virtual float evaluate(Subject[] subjectsList){
		return 1;
	}
	
	public int Subject {
		get {return subjectReferences[0];}
		set {subjectReferences[0] = value;}
	}
	
	public float DesiredValue {
		set {desiredValues[0] = value;}
		get {return desiredValues[0];}
	}
	
}

