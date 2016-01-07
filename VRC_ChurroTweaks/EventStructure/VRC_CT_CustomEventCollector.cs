using System;
using System.Collections.Generic;
using UnityEngine;

namespace VRC_ChurroTweaks
{
    /**
     * <summary>
     * A script that collects all VRC_CT_CustomEventSpawns on it and passes it to all of its decendents.
     * </summary>
     **/
	public class VRC_CT_CustomEventCollector : MonoBehaviour
	{
		private HashSet<VRC_CT_CustomEventSpawn> events;

		void Awake()
		{
			events = new HashSet<VRC_CT_CustomEventSpawn>();
			VRC_CT_CustomEventSpawn[] tempEvents = gameObject.GetComponents<VRC_CT_CustomEventSpawn>();
			foreach (VRC_CT_CustomEventSpawn es in tempEvents)
			{
				events.Add(es);
			}
			UpdateChildren(this.gameObject);
		}

		private void UpdateChildren(GameObject obj)
		{
			for (int i = 0; i < obj.transform.childCount; i++)
			{
				VRC_CT_EventHandler handler = obj.transform.GetChild(i).GetComponent<VRC_CT_EventHandler>();
				if (handler != null)
				{
					handler.FindCustomEvents();
					handler.CustomEvents.UnionWith(events);
				}
				UpdateChildren(obj.transform.GetChild(i).gameObject);
			}
		}
	}
}

