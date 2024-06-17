using System;
using System.Collections.Generic;
using UnityEngine;
using OpenScripts2;
using FistVR;
using ModularWorkshop;

namespace localscripts
{
    public class ModifyOnModularPartsAttached : MonoBehaviour
    {
        [Header("Hover Over Variables To Read The Tooltips!")]

        [Header("Setup")]

        [Tooltip("Your weapon object.")] public GameObject ModularWeapon;
        public WeaponTypeEnum WeaponType;
        public string PartGroupID;
        public string[] PartNames;

        [Header("Modifications - Enable Bools For Wanted Options")]
        [Header("Enabling/Disabling")]
        [Tooltip("Objects will enable when specific parts are selected, and disable when not.")] public bool EnableGOs;
        public GameObject[] ObjsToEnable;

        [Tooltip("Objects will disable when specific parts are selected, and enable when not.")] public bool DisableGOs;
        public GameObject[] ObjsToDisable;

        [Header("Collider")]
        [Tooltip("Will change the size and center of a box collider when specific parts are selected, and resets back to default when not.")] public bool ModifyBoxCollider;
        public BoxCollider BC;
        public Vector3 NewCenter;
        public Vector3 NewSize;

        [Header("Trigger")]
        [Tooltip("Will change the trigger object and its properties when specific parts are selected, and resets back to default when not.")] public bool ModifyTrigger;
        public Transform Trigger;
        [Tooltip("Interp style specifically for closed bolt weapons + handguns. Blame anton's dumbass for making these two separate.")] public FVRPhysicalObject.InterpStyle TriggerInterp;
        [Tooltip("Interp style specifically for open bolt weapons. Blame anton's dumbass for making these two separate.")] public OpenBoltReceiver.InterpStyle TriggerInterpOpenBolt;
        public float TriggerFireThreshold;
        public float TriggerResetThreshold;
        [Tooltip("Only available for closed bolt weapons.")] public float TriggerDualStageThreshold;

        [Header("Fire Selector")]
        public bool ModifyFireSelector;

        private Handgun hg;
        private OpenBoltReceiver obr;
        private ClosedBoltWeapon cbw;

        private bool partattached;
        private IModularWeapon imw;
        private string curpart;

        private Vector3 oldCenter;
        private Vector3 oldSize;

        private Transform oldtrig;
        private FVRPhysicalObject.InterpStyle oldTriggerInterp;
        private OpenBoltReceiver.InterpStyle oldTriggerInterpOB;
        private float oldfirethres;
        private float oldresetthreshold;
        private float olddualstagethreshold;

        public enum WeaponTypeEnum
        {
            Handgun,
            ClosedBolt,
            OpenBolt
        }

        private void Start()
        {
            imw = ModularWeapon.GetComponent<IModularWeapon>();
            switch(WeaponType)
            {
                case WeaponTypeEnum.Handgun:
                    hg = GetComponent<Handgun>();
                    oldtrig = hg.Trigger;
                    oldTriggerInterp = hg.TriggerInterp;
                    oldresetthreshold = hg.TriggerResetThreshold;
                    oldfirethres = hg.TriggerBreakThreshold;
                    break;
                case WeaponTypeEnum.ClosedBolt:
                    cbw = GetComponent<ClosedBoltWeapon>();
                    oldtrig = cbw.Trigger;
                    oldTriggerInterp = cbw.TriggerInterpStyle;
                    oldresetthreshold = cbw.TriggerResetThreshold;
                    oldfirethres = cbw.TriggerFiringThreshold;
                    olddualstagethreshold = cbw.TriggerDualStageThreshold;
                    break;
                case WeaponTypeEnum.OpenBolt:
                    obr = GetComponent<OpenBoltReceiver>();
                    oldtrig = obr.Trigger;
                    oldTriggerInterpOB = obr.TriggerInterpStyle;
                    oldfirethres = obr.TriggerFiringThreshold;
                    oldresetthreshold = obr.TriggerResetThreshold;
                    break;
                default:
                    localscripts.Logger.LogError("ModifyOnModularPartsAttached, Start: No weapon type assigned. please set one.");
                    break;
            }

            if(BC)
            {
                oldCenter = BC.center;
                oldSize = BC.size;
            }
        }

        private void Update()
        {
            curpart = imw.GetModularFVRFireArm.AllAttachmentPoints[PartGroupID].SelectedModularWeaponPart;

            foreach(string part in PartNames)
            {
                if (part == curpart)
                {
                    partattached = true;
                }
                else
                {
                    partattached = false;
                }
            }

            if(partattached)
            {
                if(EnableGOs)
                {
                    foreach(GameObject g in ObjsToEnable)
                    {
                        g.SetActive(true);
                    }
                }

                if (DisableGOs)
                {
                    foreach (GameObject g in ObjsToEnable)
                    {
                        g.SetActive(false);
                    }
                }

                if(ModifyBoxCollider)
                {
                    BC.center = NewCenter;
                    BC.size = NewSize;
                }

                if(ModifyTrigger)
                {
                    UpdateTrigger(true);
                }
            }
            else
            {
                if (EnableGOs)
                {
                    foreach (GameObject g in ObjsToEnable)
                    {
                        g.SetActive(false);
                    }
                }

                if (DisableGOs)
                {
                    foreach (GameObject g in ObjsToEnable)
                    {
                        g.SetActive(true);
                    }
                }

                if(ModifyBoxCollider)
                {
                    BC.center = oldCenter;
                    BC.size = oldSize;
                }

                if(ModifyTrigger)
                {
                    UpdateTrigger(false);
                }
            }
        }

        public void UpdateTrigger(bool add)
        {
            if(add)
            {
                switch (WeaponType)
                {
                    case WeaponTypeEnum.Handgun:
                        hg.Trigger = Trigger;
                        hg.TriggerInterp = TriggerInterp;
                        hg.TriggerResetThreshold = TriggerResetThreshold;
                        hg.TriggerBreakThreshold = TriggerFireThreshold;
                        break;
                    case WeaponTypeEnum.ClosedBolt:
                        cbw.Trigger = Trigger;
                        cbw.TriggerInterpStyle = TriggerInterp;
                        cbw.TriggerResetThreshold = TriggerResetThreshold;
                        cbw.TriggerFiringThreshold = TriggerFireThreshold;
                        cbw.TriggerDualStageThreshold = TriggerDualStageThreshold;
                        break;
                    case WeaponTypeEnum.OpenBolt:
                        obr.Trigger = Trigger;
                        obr.TriggerInterpStyle = TriggerInterpOpenBolt;
                        obr.TriggerFiringThreshold = TriggerFireThreshold;
                        obr.TriggerResetThreshold = TriggerResetThreshold;
                        break;
                    default:
                        localscripts.Logger.LogError("ModifyOnModularPartsAttached, UpdateTrigger: No weapon type assigned. please set one.");
                        break;
                }
            }
            else
            {
                switch (WeaponType)
                {
                    case WeaponTypeEnum.Handgun:
                        hg.Trigger = oldtrig;
                        hg.TriggerInterp = oldTriggerInterp;
                        hg.TriggerResetThreshold = oldresetthreshold;
                        hg.TriggerBreakThreshold = oldfirethres;
                        break;
                    case WeaponTypeEnum.ClosedBolt:
                        cbw.Trigger = oldtrig;
                        cbw.TriggerInterpStyle = oldTriggerInterp;
                        cbw.TriggerResetThreshold = oldresetthreshold;
                        cbw.TriggerFiringThreshold = oldfirethres;
                        cbw.TriggerDualStageThreshold = olddualstagethreshold;
                        break;
                    case WeaponTypeEnum.OpenBolt:
                        obr.Trigger = oldtrig;
                        obr.TriggerInterpStyle = oldTriggerInterpOB;
                        obr.TriggerFiringThreshold = oldfirethres;
                        obr.TriggerResetThreshold = oldresetthreshold;
                        break;
                    default:
                        localscripts.Logger.LogError("ModifyOnModularPartsAttached, UpdateTrigger: No weapon type assigned. please set one.");
                        break;
                }
            }
        }
    }
}