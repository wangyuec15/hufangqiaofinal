﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Voxel : MonoBehaviour {

	// VARIABLES
	private int state = 0;
    private int futureState = 0;
    private int age = 0;
    private int timeEndReference;
    private MaterialPropertyBlock props;
    private new MeshRenderer renderer;
	public Vector3 address;
    //public MeshFilter type1Mesh, type2Mesh, type3Mesh,type4Mesh, type5Mesh, type6Mesh;
    public Mesh Mesh_1, Mesh_2, Mesh_3, Mesh_4, Mesh_5, Mesh_6,Mesh_7,Mesh_8,Mesh_9;
	int type;

    // FUNCTIONS

	public void SetupVoxel(int i, int j, int k, int _type)
    {
        timeEndReference = GameObject.Find("CA_Grid").GetComponent<CA_Grid>().timeEnd;
        props = new MaterialPropertyBlock();
        renderer = gameObject.GetComponent<MeshRenderer>();
        renderer.enabled = true;
		address = new Vector3 (i,j,k);
		type = _type;
		switch (type) {
		case 1:
			MeshFilter setMesh = gameObject.GetComponent<MeshFilter> ();

                setMesh.mesh = Mesh_1;
            //Debug.Log("yes it works");
			break;
		case 2:
			MeshFilter setMesh2 = gameObject.GetComponent<MeshFilter> ();
                setMesh2.mesh = Mesh_2;
			break;
		case 3:
			MeshFilter setMesh3 = gameObject.GetComponent<MeshFilter> ();
                setMesh3.mesh = Mesh_3;
            break;	
        case 4:
            MeshFilter setMesh4 = gameObject.GetComponent<MeshFilter> ();
                setMesh4.mesh = Mesh_4;
            break;
        case 5:
            MeshFilter setMesh5 = gameObject.GetComponent<MeshFilter> ();
                setMesh5.mesh = Mesh_5;
                break;  
        case 6:
            MeshFilter setMesh6 = gameObject.GetComponent<MeshFilter> ();
                setMesh6.mesh = Mesh_6;
            break;
        case 7:
            MeshFilter setMesh7 = gameObject.GetComponent<MeshFilter>();
                setMesh7.mesh = Mesh_7;
            break;
        case 8:
            MeshFilter setMesh8 = gameObject.GetComponent<MeshFilter>();
                setMesh8.mesh = Mesh_8;
            break;
        case 9:
            MeshFilter setMesh9 = gameObject.GetComponent<MeshFilter>();
                setMesh9.mesh = Mesh_9;
            break;
		default:
			MeshFilter setMeshDefault = gameObject.GetComponent<MeshFilter> ();
                setMeshDefault.mesh = Mesh_3;
			break;
		}
    }
	
	// Update function
	public void UpdateVoxel () {
		// Set the future state
		state = futureState;        
        // If voxel is alive update age
        if (state == 1)
        {
            age++;
        }
        // If voxel is death disable the game object mesh renderer and set age to zero
        if (state == 0)
        {
            age = 0;
        }
    }

    // Update the voxel display
    public void VoxelDisplay()
    {
        if (state == 1)
        {            
            // Remap the color to age
            Color ageColor = new Color(Remap(age, 0, timeEndReference, 0.0f, 1.0f), 0, 0, 1);
            props.SetColor("_Color", ageColor);
            // Updated the mesh renderer color
            renderer.enabled = true;
            renderer.SetPropertyBlock(props);
        }
        if(state == 0)
        {
            renderer.enabled = false;
        }
    }

	// Set the state of the voxel
	public void SetState(int _state){
		state = _state;
	}

	// Set the future state of the voxel
	public void SetFutureState(int _futureState){
		futureState = _futureState;
	}

    // Get the age of the voxel
	public void SetAge(int _age){
		age = _age;
	}

	// Get the state of the voxel
	public int GetState(){
		return state;
	}

	// Get the age of the voxel
	public int GetAge(){
		return age;
	}

    // Remap numbers
    private float Remap(float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }
}
