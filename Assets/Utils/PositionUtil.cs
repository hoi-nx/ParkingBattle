using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Position util.
/// </summary>
public class PositionUtil {
	/// <summary>
	/// Gets the relative position.
	/// </summary>
	/// <returns>The relative position.</returns>
	/// <param name="origin">Origin.</param>
	/// <param name="target">Target.</param>
	public static Vector3 GetRelativePosition(Transform origin, Transform target) {
		Vector3 distance = target.position - origin.position;
		Vector3 relativePosition = Vector3.zero;
		relativePosition.x = Vector3.Dot(distance, origin.right.normalized);
		relativePosition.y = Vector3.Dot(distance, origin.up.normalized);
		relativePosition.z = Vector3.Dot(distance, origin.forward.normalized);
		return relativePosition;
	}
	/// <summary>
	/// Chuyen doi toa do mot diem tu he toa do Oxy sang OXY voi goc quay alpha. Alpha in deg
	/// </summary>
	/// <returns>The from Oxy to OXY.</returns>
	/// <param name="angle">Angle.</param>
	public static Vector3 PoitFromOxyToOXY(Vector3 point, float alpha) {
		if (point == null) {
			return Vector3.zero;
		}
		float x = point.x;
		float y = point.y;
		float z = point.z;
		float X = x * Mathf.Cos(alpha * Mathf.PI / 180f) - y * Mathf.Sin(alpha * Mathf.PI / 180f);
		float Y = x * Mathf.Sin(alpha * Mathf.PI / 180f) + y * Mathf.Cos(alpha * Mathf.PI / 180f);
		float Z = z;
		return new Vector3(X, Y, Z);
	}
}