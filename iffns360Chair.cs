using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.SDKBase.Editor.Attributes;
using VRC.Udon;
using VRC.Udon.Common;

namespace iffnsStuff.iffnsVRCStuff
{
    [RequireComponent(typeof(VRCStation))]
    public class iffns360Chair : UdonSharpBehaviour
    {
        //Variable assignment
        [HelpBox("Note: It is recommended to set the Player Enter Location of the attached station to a sepparate object, since the chair will otherwise also rotate.", order = 0)]
        [Header("Sets the side movement to look over your shoulders:", order = 1)]
        [SerializeField] float HeadXOffset = 0.25f;

        //Runtime variables
        Transform seatTransform;

        Vector3 initialPosition = Vector3.zero;
        Quaternion initialRotaiton = Quaternion.identity;

        public VRCStation LinkedStation { get; private set; }
        public VRCPlayerApi SeatedPlayer { get; private set; }

        private void Start()
        {
            LinkedStation = (VRCStation)GetComponent(typeof(VRCStation));

            if (LinkedStation.stationEnterPlayerLocation == null) LinkedStation.stationEnterPlayerLocation = transform;
            if (LinkedStation.stationExitPlayerLocation == null) LinkedStation.stationExitPlayerLocation = transform;

            seatTransform = LinkedStation.stationEnterPlayerLocation;

            if(seatTransform == null)
            {
                Debug.Log("stationEnterPlayerLocation = null");
                return;
            }

            initialPosition = seatTransform.localPosition;
            initialRotaiton = seatTransform.localRotation;
        }

        public override void OnStationEntered(VRCPlayerApi player)
        {
            SeatedPlayer = player;

            if (LinkedStation.PlayerMobility == VRCStation.Mobility.Mobile) return;

            initialPosition = seatTransform.localPosition;
            initialRotaiton = seatTransform.localRotation;
        }

        public override void OnStationExited(VRCPlayerApi player)
        {
            SeatedPlayer = null;
            seatTransform.localPosition = initialPosition;
            seatTransform.localRotation = initialRotaiton;
        }

        public float Remap(float iMin, float iMax, float oMin, float oMax, float iValue)
        {
            float t = Mathf.InverseLerp(iMin, iMax, iValue);
            return Mathf.Lerp(oMin, oMax, t);
        }

        private void Update()
        {
            if (SeatedPlayer == null || SeatedPlayer.IsUserInVR()) return;
            if (LinkedStation.PlayerMobility == VRCStation.Mobility.Mobile) return;

            Quaternion headRotation;

            //Rotation:
            #if UNITY_EDITOR
            headRotation = Networking.LocalPlayer.GetTrackingData(VRCPlayerApi.TrackingDataType.Head).rotation;
            #else
            headRotation = SeatedPlayer.GetBoneRotation(HumanBodyBones.Head);
            #endif

            Quaternion relativeHeadRotation = Quaternion.Inverse(seatTransform.rotation) * headRotation;

            float headHeading = relativeHeadRotation.eulerAngles.y;

            //Debug.Log(headRotation.eulerAngles + " - " + transform.rotation.eulerAngles);

            seatTransform.localRotation = Quaternion.Euler(headHeading * Vector3.up) * initialRotaiton;

            //Offset:
            float xOffset = 0;

            if (headHeading > 45 && headHeading < 180)
            {
                xOffset = Remap(iMin: 45, iMax: 90, oMin: 0, oMax: HeadXOffset, iValue: headHeading);
            }
            else if (headHeading < 315 && headHeading > 180)
            {
                xOffset = -Remap(iMin: 315, iMax: 270, oMin: 0, oMax: HeadXOffset, iValue: headHeading);
            }
            //Debug.Log($"{headHeading} -> {xOffset}");

            seatTransform.localPosition = initialPosition + xOffset * Vector3.right ;
        }
    }
}