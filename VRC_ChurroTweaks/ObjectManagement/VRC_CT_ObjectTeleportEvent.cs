using System;
using UnityEngine;
using VRCSDK2;

namespace VRC_ChurroTweaks
{
    /**
     * <summary>
     * This class will create a VRC_CT_ObjectTeleportEvent. These events use...
     * -- <value> ParameterString </value> the tag indication where an object can be teleported to --
     * -- <value> ParameterObject </value> the object to teleport to a location --
     * This event moves the ParameterObject to either its parent or first child that has the tag given by its
     * ParameterString
     * </summary>
     **/
	public class VRC_CT_ObjectTeleportEventSpawn : VRC_CT_CustomEventSpawn
	{
        public TeleportMode teleportMode;

	    public override VRC_CT_CustomEvent Create(CT_Event e)
	    {
			VRC_CT_ObjectTeleportEvent EventContents = new VRC_CT_ObjectTeleportEvent(teleportMode);
			EventContents.SetEvent(e);
			return EventContents;
	    }

        public enum TeleportMode {TELEPORT_TO_CHILD_WITH_TAG = 0, TELEPORT_TO_PARENT,
                                    TELEPORT_OBJECT_ONE_TO_OBJECT_TWO };
	}

    public class VRC_CT_ObjectTeleportEvent : VRC_CT_CustomEvent
	{
        private VRC_CT_ObjectTeleportEventSpawn.TeleportMode mode;

        public VRC_CT_ObjectTeleportEvent(VRC_CT_ObjectTeleportEventSpawn.TeleportMode mode)
        {
            this.mode = mode;
        }


	    public override void TriggerEvent()
	    {
            if (mode == VRC_CT_ObjectTeleportEventSpawn.TeleportMode.TELEPORT_TO_PARENT)
            {
                EventContents.getGameObjectPerferred0().transform.position = EventContents.getGameObjectPerferred0().transform.parent.position;
                return;
            }
            if (mode == VRC_CT_ObjectTeleportEventSpawn.TeleportMode.TELEPORT_OBJECT_ONE_TO_OBJECT_TWO)
            {
                if (EventContents.ParameterObject0 != null && EventContents.ParameterObject1 != null)
                    EventContents.ParameterObject0.transform.position = EventContents.ParameterObject1.transform.position;
                return;
            }

            VRC_CT_ObjectTags tags;
            String teleportTag = EventContents.ParameterString.Equals("") ? "TeleportLocation" : EventContents.ParameterString;

            for (int i = 0; i < EventContents.getGameObjectPerferred0().transform.childCount; i++)
	        {
				tags = EventContents.getGameObjectPerferred0().transform.GetChild(i).gameObject.GetComponent<VRC_CT_ObjectTags>();
                if (tags != null && tags.hasTag(teleportTag))
	            {
					EventContents.getGameObjectPerferred0().transform.position = EventContents.getGameObjectPerferred0().transform.GetChild(i).position;
                    break;
	            }
	        }

			if (EventContents.ParameterBoolOp.Equals(VRC_EventHandler.VrcBooleanOp.True))
	        {
				Rigidbody rigidbody = EventContents.getGameObjectPerferred0().GetComponent<Rigidbody>();
	            rigidbody.velocity = Vector3.zero;
	            rigidbody.angularVelocity = Vector3.zero;
	        }
	    }
	}
}