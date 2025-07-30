using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Managers;

public class MapController : MonoBehaviour {

	public Collider minimapBoundingBox;

    void Start () {
		MinimapManager.Instance.UpdateMinimap(minimapBoundingBox);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
