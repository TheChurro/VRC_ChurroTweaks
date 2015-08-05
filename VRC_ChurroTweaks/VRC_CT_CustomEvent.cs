using System;
using UnityEngine;
using VRCSDK2;

namespace VRC_ChurroTweaks
{
    /**
     * <summary>
     * An abstract class that can be placed on GameObjects to allow CustomEvent usage.
     * </summary>
     **/
	public abstract class VRC_CT_CustomEventSpawn : MonoBehaviour
	{
        /**
         * <summary>
         * EventTypeName is a unique identifier for the event as supplied by the content creator. This is
         * used by VRC_CT_EventHandler to determine which custom event to use
         * </summary>
         **/
	    public string EventTypeName;

        /**
         * <summary>
         * An abstract method that returns the specific VRC_CT_CustomEvent for the spawn class. For instance
         * a VRC_CT_ScoreboardEventSpawn returns a VRC_CT_ScoreboardEvent. Note: overriding classes need to give
         * the VRC_CT_CustomEvent the passed in VrcEvent through SetEvent();
         * </summary>
         **/
	    public abstract VRC_CT_CustomEvent Create(VRC_EventHandler.VrcEvent e);

        /**
         * <summary>
         * If you need access to the GameObject that has the EventHandler override this and return true
         * </summary>
         **/
        public virtual bool RequiresEventHandlerObject()
        {
            return false;
        }

        /**
         * <summary>
         * HashCode is the EventTypeName's HashCode so that it acts as a unique identifier.
         * </summary>
         **/
		public override int GetHashCode()
		{
			return EventTypeName.GetHashCode();
		}
	}

    /**
     * <summary>
     * The actual CustomEvent created by a VRC_CT_CustomEventSpawn. These are given their Event through the
     * virtual method SetEvent(VrcEvent e) on creation. To add functionality to an Event, override TriggerEvent().
     * </summary>
     **/
	public abstract class VRC_CT_CustomEvent
	{
	    public VRC_EventHandler.VrcEvent EventContents;
		public string EventName;

        private GameObject EventHandlerObject;
        /**
         * <summary>
         * A virtual method that is called when the GameObject holding the EventHandler for this Event is given to the
         * CustomEvent
         * </summary>
         **/
        public virtual void SetEventHandlerGameObject(GameObject obj)
        {
            EventHandlerObject = obj;
        }

	    public abstract void TriggerEvent();

	    public VRC_EventHandler.VrcEvent GetEvent()
	    {
			return EventContents;
		}
		
		public virtual void SetEvent(VRC_EventHandler.VrcEvent EventContents)
		{
			this.EventContents = EventContents;
            this.EventContents.ParameterBool = VRC_EventHandler.BooleanOp(EventContents.ParameterBoolOp, false);
	    }

		public override int GetHashCode()
		{
			return EventName.GetHashCode() + EventContents.ParameterString.GetHashCode();
		}
	}
}