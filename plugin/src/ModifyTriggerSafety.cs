using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FistVR;

namespace localscripts 
{
	public class ModifyTriggerSafety : MonoBehaviour 
{
	public Transform TriggerSafety;
	public float Pressed;
	public float Unpressed;
	public FVRPhysicalObject.Axis axis;
	private Handgun hg;

	void Start () 
	{
		hg  = GetComponentInParent<Handgun>();
	}
	
	void Update () 
	{
		if(hg.HasTriggerSafety)
		{
			hg.TriggerSafety = TriggerSafety;
			hg.TriggerSafetyAxis = axis;
			hg.TriggerSafetyPressed = Pressed;
			hg.TriggerSafetyUnpressed = Unpressed;
		}
	}
}

}