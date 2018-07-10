using UnityEngine;
using System.Collections;

public class CameraUtility : MonoBehaviour {
	public static bool IsRendererInFrustum(Collider2D obj, Camera Cam)
	{
	     Plane[] planes =  GeometryUtility.CalculateFrustumPlanes(Cam);
		 return GeometryUtility.TestPlanesAABB (planes, obj.bounds);          	
	}
}
