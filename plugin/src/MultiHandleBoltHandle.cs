using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FistVR;

namespace localscripts 
{
    public class MultiHandleBoltHandle : ClosedBoltHandle 
{
    [HeaderAttribute("Hover Over Variables To Read The Tooltips!!!")]

    [Header("Multiple Rotating Handles")]
   [Tooltip("Enable either this or the built in rotating handle bool. Do not use both.")]  public bool useMultipleHandles;
   [Tooltip("All handle Objs, listed in order. List should line up with other lists in the script, EX. handles[0] will match leftRots[0] and so on.")]  public Transform[] handles;
   [Tooltip("rotation for when the players hand is on the left. Lines up with other lists.")]  public Vector3[] leftRots;
   [Tooltip("rotation for when the players hand is on the right. Lines up with other lists.")]   public Vector3[] rightRots;
   [Tooltip("rotation for when the players hand is neutral/not grabbing. Lines up with other lists.")]   public Vector3[] neutralRots;
   [Tooltip("does the handle stay rotated. Lines up with other lists.")] public bool[] StayRotOnBack;
   [Tooltip("play a sound on handle grab. Lines up with other lists.")] public bool[] UseSoundOnGrab;

    public override void BeginInteraction(FVRViveHand hand)
    {
        for(int i = 0; i < handles.Length; i++)
        {
            if (UseSoundOnGrab[i])
            {
                Weapon.PlayAudioEvent(FirearmAudioEventType.HandleGrab);
            }
        }

        base.BeginInteraction(hand);
    }

    public override void UpdateInteraction(FVRViveHand hand)
    {
        base.UpdateInteraction(hand);
        if (HasRotatingPart)
        {
            Vector3 normalized = (base.transform.position - m_hand.PalmTransform.position).normalized;
            if (Vector3.Dot(normalized, base.transform.right) > 0f)
            {
                RotatingPart.localEulerAngles = RotatingPartLeftEulers;
            }
            else
            {
                RotatingPart.localEulerAngles = RotatingPartRightEulers;
            }
        }

        if(useMultipleHandles)
        {
            for (int i = 0; i < handles.Length; i++)
            {
                Vector3 normalized = (base.transform.position - m_hand.PalmTransform.position).normalized;
                if (Vector3.Dot(normalized, base.transform.right) > 0f)
                {
                    handles[i].localEulerAngles = leftRots[i];
                }
                else
                {
                    handles[i].localEulerAngles = rightRots[i];
                }
            }
        }
    }

    public override void EndInteraction(FVRViveHand hand)
    {
        if (useMultipleHandles && !StaysRotatedWhenBack)
        {
            for(int i = 0; i < handles.Length; i++)
            {
                if(!StayRotOnBack[i])
                {
                    handles[i].localEulerAngles = neutralRots[i];
                }
            }
        }

        base.EndInteraction(hand);
    }
}
}