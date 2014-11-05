using UnityEngine;
using System;
using System.Collections.Generic;
using System.Text;

namespace CamOn.Utils {
	public static class BoundsExtension
	{
	    public static Vector3[] ScreenSpaceArrayCache = new Vector3[9];
		
	    public static Vector3[] GetScreenSpaceArray(this Bounds bounds, Camera camera)
	    {
			bounds.ToArray();
			for (int i=0;i<9;i++)
				BoundsExtension.ScreenSpaceArrayCache[i] = camera.WorldToScreenPoint(ArrayCache[i]);
	        return BoundsExtension.ScreenSpaceArrayCache;
	    }
	
		
		public static Vector3[] ArrayCache = new Vector3[9];
	    public static Vector3[] ToArray(this Bounds bounds)
	    {
			if (BoundsExtension.ArrayCache[0] != bounds.center){
		        BoundsExtension.ArrayCache[0] = bounds.center;
		        BoundsExtension.ArrayCache[1] = bounds.center + new Vector3 (bounds.extents.x, bounds.extents.y, bounds.extents.z);
		        BoundsExtension.ArrayCache[2] = bounds.center + new Vector3(bounds.extents.x, -bounds.extents.y, bounds.extents.z);
		        BoundsExtension.ArrayCache[3] = bounds.center + new Vector3(bounds.extents.x, bounds.extents.y, -bounds.extents.z);
		        BoundsExtension.ArrayCache[4] = bounds.center + new Vector3(bounds.extents.x, -bounds.extents.y, -bounds.extents.z);
		        BoundsExtension.ArrayCache[5] = bounds.center + new Vector3(-bounds.extents.x, bounds.extents.y, bounds.extents.z);
		        BoundsExtension.ArrayCache[6] = bounds.center + new Vector3(-bounds.extents.x, -bounds.extents.y, bounds.extents.z);
		        BoundsExtension.ArrayCache[7] = bounds.center + new Vector3(-bounds.extents.x, bounds.extents.y, -bounds.extents.z);
		        BoundsExtension.ArrayCache[8] = bounds.center + new Vector3 (-bounds.extents.x, -bounds.extents.y, -bounds.extents.z);
			}
	        return BoundsExtension.ArrayCache;
	    }
	}
}