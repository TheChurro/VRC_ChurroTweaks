using System;
using UnityEngine;
using VRCSDK2;

namespace VRC_ChurroTweaks
{
    /**
         * <summary>
         * An Event class that includes more information for custom events than can be included in VrcEvents
         * </summary>
         **/
    public class CT_Event : MonoBehaviour
    {
        public String Name;

        /**
         *
         * <summary>
         * The name of the Custom_Event that this event uses
         * </summary>
         *
         **/
        public String EventTypeName;

        public VRC_EventHandler.VrcEventType RegularEventType;

        public String ParameterString;

        public VRC_EventHandler.VrcBooleanOp ParameterBoolOp;

        [NonSerialized]
        public bool ParameterBool;

        public float ParameterFloat;

        public GameObject ParameterObject0;

        /**
         * <summary>
         * A GameObject parameter that doesn't have to be the child of the game object of the handler
         * </summary>
         **/
        public GameObject ParameterObject1;

        /**
         * <summary>
         * Returns the GameObject that isn't the child of the object with the event handler GameObject
         * if it exists. If it doesn't it returns the GameObject that is child to the EventHandler
         * GameObject
         * </summary> 
         **/
        public GameObject getGameObjectPerferred1()
        {
            return ParameterObject1 == null ? ParameterObject0 : ParameterObject1;
        }

        /**
         * <summary>
         * Returns the GameObject that is the child of the object with the event handler GameObject
         * if it exists. If it doesn't it returns the GameObject that isn't child to the EventHandler
         * GameObject
         * </summary> 
         **/
        public GameObject getGameObjectPerferred0()
        {
            return ParameterObject0 == null ? ParameterObject1 : ParameterObject0;
        }
    }
}
