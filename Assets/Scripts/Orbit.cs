using UnityEngine;

public class Orbit : MonoBehaviour
{
	public Transform Pivot;
	public float Speed = 1;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.RotateAround(Pivot.position, Vector3.up, Speed);
	}
}
