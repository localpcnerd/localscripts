using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FistVR;
using System;
using BepInEx.Logging;

namespace localscripts
{
    public class ModifyFireSelector : MonoBehaviour
    {
        [Header("Used to change fire selector modes of a handgun when new ModulWorkshop parts are added. Add one of ")]
        public Transform FireSelector;
        public FVRPhysicalObject.InterpStyle interp;
        public FVRPhysicalObject.Axis axis;
        public Handgun.FireSelectorMode[] modesToSetHandgun;
        public int defaultMode;

        private Handgun hg;

        public enum weaponType
        {
            handgun,
            closedBolt,
            openBolt
        }

        void Start()
        {
            hg = GetComponentInParent<Handgun>();
            UpdateSelectorHG();
        }

        private void UpdateSelectorHG()
        {
            hg.HasFireSelector = true;
            hg.FireSelector = FireSelector;
            hg.FireSelectorInterpStyle = interp;
            hg.FireSelectorAxis = axis;
            hg.FireSelectorModes = modesToSetHandgun;
            hg.m_fireSelectorMode = defaultMode;
        }
    }
}