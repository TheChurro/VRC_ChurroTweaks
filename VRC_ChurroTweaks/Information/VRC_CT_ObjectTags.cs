using System;
using System.Collections.Generic;
using UnityEngine;

namespace VRC_ChurroTweaks
{
    /**
     * <summary>
     * A script that contains a list of tags that can be added to, removed from or checked if contains a tag.
     * This is useful because these tags are independent of project settings and multiple tags can be added to
     * one object.
     * </summary>
     **/
	public class VRC_CT_ObjectTags : MonoBehaviour
	{
		public List<string> tags;

		public void addTag(string tag)
		{
			if (!tags.Contains(tag))
			{
				tags.Add(tag);
			}
		}

		public bool hasTag(string tag)
		{
			return tags.Contains(tag);
		}

		public void removeTag(string tag)
		{
			tags.Remove(tag);
		}
	}
}