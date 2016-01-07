using UnityEngine;
using VRCSDK2;

namespace VRC_ChurroTweaks
{
    /**
     * <summary>
     * This class will call and event after a certain time has elapsed after it loaded.
     * </summary>
     **/
	public class VRC_CT_OnWorldLoadTrigger : MonoBehaviour
	{
	    public string EventToLoad;
	    public float delay = 0;
	    private VRC_EventHandler handler;
	    private bool shouldUpdate = true;

	    void Start()
	    {
	        handler = gameObject.GetComponent<VRC_EventHandler>();
            if (delay == 0)
            {
                shouldUpdate = false;
                handler.TriggerEvent(EventToLoad, VRC_EventHandler.VrcBroadcastType.Always);
            }
	    }

	    void Update()
	    {
	        if (shouldUpdate)
	        {
	            if (delay > 0)
	            {
	                delay -= Time.deltaTime;
	                return;
	            }
	            handler.TriggerEvent(EventToLoad, VRC_EventHandler.VrcBroadcastType.Always);
	            shouldUpdate = false;
	        }
	    }
	}
}