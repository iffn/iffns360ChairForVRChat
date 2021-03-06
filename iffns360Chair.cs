using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace iffnsStuff.iffnsVRCStuff
{
    [RequireComponent(typeof(VRCStation))]
    public class iffns360Chair : UdonSharpBehaviour
    {
        //Variable assignment
        [Header("Sets the side movement to look over your shoulders:")]
        [SerializeField] float HeadXOffset = 0.25f;

        //Runtime variables
        Transform seatTransform;
        float initialXOffset;

        #if UNITY_EDITOR
        public bool isSeated = false;
        #else
        VRCPlayerApi seatedPlayer;
        #endif

        private void Start()
        {
            VRCStation station = (VRCStation)GetComponent(typeof(VRCStation));

            seatTransform = station.stationEnterPlayerLocation;

            initialXOffset = seatTransform.localPosition.x;
        }

        public override void OnStationEntered(VRCPlayerApi player)
        {
            #if UNITY_EDITOR
            isSeated = true;
            #else
            seatedPlayer = player;
            #endif
        }

        public override void OnStationExited(VRCPlayerApi player)
        {
            seatTransform.localRotation = Quaternion.identity;
            seatTransform.localPosition = new Vector3(initialXOffset, seatTransform.localPosition.y, seatTransform.localPosition.z);
            
            #if UNITY_EDITOR
            isSeated = false;
            #else
            seatedPlayer = null;
            #endif
        }

        public float Remap(float iMin, float iMax, float oMin, float oMax, float iValue)
        {
            float t = Mathf.InverseLerp(iMin, iMax, iValue);
            return Mathf.Lerp(oMin, oMax, t);
        }

        private void Update()
        {
            #if UNITY_EDITOR
            if (!isSeated) return;
            #else
            if (seatedPlayer == null) return;
            if (seatedPlayer.IsUserInVR()) return;
            #endif

            Quaternion headRotation;

            //Rotation:
            #if UNITY_EDITOR
            headRotation = Networking.LocalPlayer.GetTrackingData(VRCPlayerApi.TrackingDataType.Head).rotation;
            #else
            headRotation = seatedPlayer.GetBoneRotation(HumanBodyBones.Head);
            #endif


            Quaternion relativeHeadRotation = Quaternion.Inverse(seatTransform.rotation) * headRotation;

            float headHeading = relativeHeadRotation.eulerAngles.y;

            //Debug.Log(headRotation.eulerAngles + " - " + transform.rotation.eulerAngles);

            seatTransform.localRotation = Quaternion.Euler(headHeading * Vector3.up);

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

            seatTransform.localPosition = new Vector3(initialXOffset + xOffset, seatTransform.localPosition.y, seatTransform.localPosition.z);
        }
    }
}