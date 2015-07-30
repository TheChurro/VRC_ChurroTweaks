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
	    public override VRC_CT_CustomEvent Create(VRC_EventHandler.VrcEvent e)
	    {
			VRC_CT_ObjectTeleportEvent EventContents = new VRC_CT_ObjectTeleportEvent();
			EventContents.SetEvent(e);
			return EventContents;
	    }
	}

    public class VRC_CT_ObjectTeleportEvent : VRC_CT_CustomEvent
	{
	    public override void TriggerEvent()
	    {
			VRC_CT_ObjectTags tags = EventContents.ParameterObject.transform.parent.gameObject.GetComponent<VRC_CT_ObjectTags>();

            string teleportTag = EventContents.ParameterString == "" ? "TeleportLocation" : EventContents.ParameterString;

            if (tags != null && tags.hasTag(teleportTag))
	        {
				EventContents.ParameterObject.transform.position = EventContents.ParameterObject.transform.parent.position;
	        }
	        else
	        {
				for (int i = 0; i < EventContents.ParameterObject.transform.childCount; i++)
	            {
					tags = null;
					tags = EventContents.ParameterObject.transform.GetChild(i).GetComponent<VRC_CT_ObjectTags>();
                    if (tags != null && tags.hasTag(teleportTag))
	                {
						EventContents.ParameterObject.transform.position = EventContents.ParameterObject.transform.GetChild(i).position;
	                    break;
	                }
	            }
	        }

			if (EventContents.ParameterBoolOp.Equals(VRC_EventHandler.VrcBooleanOp.True))
	        {
				Rigidbody rigidbody = EventContents.ParameterObject.GetComponent<Rigidbody>();
	            rigidbody.velocity = Vector3.zero;
	            rigidbody.angularVelocity = Vector3.zero;
	        }
	    }
	}
}