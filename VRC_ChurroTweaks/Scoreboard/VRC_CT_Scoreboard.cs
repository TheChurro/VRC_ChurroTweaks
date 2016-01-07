using System;
using System.Collections.Generic;
using UnityEngine;
using VRCSDK2;

namespace VRC_ChurroTweaks
{

    /**
     * <summary>
     * This class holds a TextMesh and and a ScoreboardValue Name.
     * When a scoreboard value changes, the TextMesh will be updated with a string representation of the value
     * 
     * The method UpdateValue(string valueName, string valueDisplay) is virtual and can be overriden for custom displays
     * </summary>
     **/
	public class VRC_CT_Scoreboard : MonoBehaviour
	{
	    public string valueName = "";
	    public TextMesh textArea = null;

	    public virtual void UpdateValue(string valueName, string valueDisplay)
	    {
	        if (this.valueName == valueName)
	        {
	            textArea.text = valueDisplay;
	        }
	    }
	}
}