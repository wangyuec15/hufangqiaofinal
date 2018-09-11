using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CA_Grid : MonoBehaviour {

	// VARIABLES

	// Texture to be used as start of CA input
	public Texture2D seedImage;

	// Time frame
	public int timeEnd = 100;
	int currentFrame = 0;

	// Array for storing voxels
	int width;
	int length;
	int height;
	GameObject[,,] voxelGrid;
    ArrayList cellCollection = new ArrayList();

	// Reference to the voxel we are using
	public GameObject voxelPrefab;

	// Spacing between voxels
	float spacing = 1f;

	// Voxel trace line points
	List<Vector3> linePoints;
	public GameObject tracedLines;
	public Color tracedLinesColorStart = Color.red;
	public Color tracedLinesColorEnd = Color.blue;

   // private int _type;
	// FUNCTIONS

	// Use this for initialization
	void Start () {
		// Read the image width and height
		width = seedImage.width;
		length = seedImage.height;
		height = timeEnd;
		// Create a new CA grid
		CreateGrid ();
	}
	
	// Update is called once per frame
	void Update () {

        /* for each voxel
         * voxel.standardposition
         * Vector3(randomX, randomY, randomZ)
         * Vector3 toCenter = voxel.standardposition-voxel.currentpositiom
         * if (toCenter.length() >= treshold){
         *      voxel.transform(toCenter);
         *      
         * } else {
         *      voxel.transform(Vector3(random.range(-1,1), random.range(-1,1),random.range(-1,1)));
         * }
 
        */

        // Calculate the CA state, save the new state, display the CA and increment time frame
        if (currentFrame < timeEnd - 1)
        {
            // Calculate the future state of the voxels
            CalculateCA();
            // Update the voxels that are printing
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < length; j++)
                {
                    GameObject currentVoxel = voxelGrid[i, j, 0];
                    currentVoxel.GetComponent<Voxel>().UpdateVoxel();
                }
            }
            // Save the CA state
            SaveCA();
            // Display the printed voxels
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < length; j++)
                {
                    for (int k = 1; k < height; k++)
                    {
                        voxelGrid[i, j, k].GetComponent<Voxel>().VoxelDisplay();
                    }
                }
            }
            // Increment the current frame count
            currentFrame++;
        }
        // Spin the CA if spacebar is pressed (be careful, GPU instancing will be lost!)
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (gameObject.GetComponent<ModelDisplay>() == null)
            {
                gameObject.AddComponent<ModelDisplay>();
            }
            else 
            {
                Destroy(gameObject.GetComponent<ModelDisplay>());
            }
        }
		// Trace the top voxels and show/hide them
		if(currentFrame == timeEnd-1){
			if(Input.GetKeyDown(KeyCode.T)){
				TraceCA ();
				gameObject.SetActive (false);
			}
		}
    }

	// Create grid function
	void CreateGrid(){
		// Allocate space in memory for the array
		voxelGrid = new GameObject[width, length, height];
		// Populate the array with voxels from a base image
		for (int i = 0; i < width; i++) {
			for (int j = 0; j < length; j++) {
                for (int k = 0; k < timeEnd; k++) {
                    // Create values for the transform of the new voxel
                    //    Vector3 currentVoxelPos = new Vector3(0f, 0f, spacing * 4);
                    Quaternion currentVoxelRot = Quaternion.identity;
                    if (k == 3)
                    {
                        currentVoxelRot = new Quaternion(0, 0, 180f, 1);
                    }

                 //   transform.rotation = currentVoxelRot;
                    Vector3 currentVoxelPos = new Vector3(i * spacing * (float)6.928203230275509+ (j % 2) * 3.464101615137755f * spacing, k * spacing * 3.265986323710904f, j * spacing * (float)6);
                    GameObject currentVoxel = Instantiate (voxelPrefab, currentVoxelPos, currentVoxelRot);

                    //currentVoxel.transform.Rotate(0f,k*0.5f,0f);

                    //currentVoxel.transform.RotateAroundLocal(new Vector3(0f, 1f, 0f), Random.Range(0f, 1f)*0.5f);



                    int _type = 0;
                    if (k % 3 == 1)
                        {
                            if (j % 2 == 1)
                            {
                                if (i % 3 == 1)
                                {
                                    _type = 1;
                                }
                                else if (i % 3 == 2)
                                {
                                    _type = 2;
                                }
                                else
                                {
                                    _type = 3;
                                }
                            }
                            else
                            {
                                if (i % 3 == 0)
                                {
                                    _type = 1;
                                }
                                else if (i % 3 == 1)
                                {
                                    _type = 2;
                                }
                                else
                                {
                                    _type = 3;
                                }
                            }
                        }
                        else if (k % 3 == 2)
                        {
                            if (j % 2 == 1)
                            {
                                if (i % 3 == 1)
                                {
                                    _type = 4;
                                }
                                else if (i % 3 == 2)
                                {
                                    _type = 5;
                                }
                                else
                                {
                                    _type = 6;
                                }
                            }
                            else
                            {
                                if (i % 3 == 0)
                                {
                                    _type = 4;
                                }
                                else if (i % 3 == 1)
                                {
                                    _type = 5;
                                }
                                else
                                {
                                    _type = 6;
                                }
                            }
                        }
                        else
                        {
                            if (j % 2 == 1)
                            {
                                if (i % 3 == 1)
                                {
                                    _type = 7;
                                }
                                else if (i % 3 == 2)
                                {
                                    _type = 8;
                                }
                                else
                                {
                                    _type = 9;
                                }
                            }
                            else
                            {
                                if (i % 3 == 0)
                                {
                                    _type = 7;
                                }
                                else if (i % 3 == 1)
                                {
                                    _type = 8;
                                }
                                else
                                {
                                    _type = 9;
                                }
                            }
                        }


                    currentVoxel.GetComponent<Voxel>().SetupVoxel(i,j,k,_type);
                    // Set the state of the voxels
                    if (k <= 3) {						
						// Create a new state based on the input image
                        int currentVoxelState = (int)(seedImage.GetPixel (i, j).grayscale+0.4);
						currentVoxel.GetComponent<Voxel> ().SetState (currentVoxelState);
					} else {
                        // Set the state to death
                        currentVoxel.GetComponent<MeshRenderer>().enabled = false;
                        currentVoxel.GetComponent<Voxel> ().SetState (0);
					}
					// Save the current voxel in the voxelGrid array
					voxelGrid[i,j,k] = currentVoxel;
					// Attach the new voxel to the grid game object
					currentVoxel.transform.parent = gameObject.transform;
				}
			}
		}
	}

	// Calculate CA function
	void CalculateCA(){
		// Go over all the voxels stored in the voxels array
       
		for (int i = 1; i < width-1; i++) {
			for (int j = 1; j < length-1; j++) {
				GameObject currentVoxel = voxelGrid[i,j,0];
				int currentVoxelState = currentVoxel.GetComponent<Voxel> ().GetState ();
				int aliveNeighbours = 0;

				// Calculate how many alive neighbours are around the current voxel
				for (int x = -1; x <= 1; x++) {
					for (int y = -1; y <= 1; y++) {
						GameObject currentNeigbour = voxelGrid [i + x, j + y,0];
						int currentNeigbourState = currentNeigbour.GetComponent<Voxel> ().GetState();
						aliveNeighbours += currentNeigbourState;
					}
				}
				aliveNeighbours -= currentVoxelState;
				// Rule Set 1: for voxels that are alive
		/*		
                        if (currentFrame>6&&currentFrame<10)
                        {

                            for (int x = -1; x <= 1; x++)
                            {
                                for (int y = -1; y <= 1; y++)
                                {
                                    int yVoxelState = (int)seedImage.GetPixel(i + x, j + y).grayscale;
                                    int yNeighbours = 0;
                                    yNeighbours += yVoxelState;
                                    if (yNeighbours == 9)
                                    {
                                        currentVoxel.GetComponent<Voxel>().SetFutureState(1);
                                    }
                                    else if(yNeighbours>=7){
                                        currentVoxel.GetComponent<Voxel>().SetFutureState((int)Random.Range(0.3f, 1.99f));
                                    }
                                    else if(yNeighbours >=3)
                                    {
                                        currentVoxel.GetComponent<Voxel>().SetFutureState((int)Random.Range(0.01f, 1.99f));
                                    }
                                }
                            }
                              //  int yVoxelState = (int)seedImage.GetPixel(i, j).grayscale;
                        }
                        */
                if (currentVoxelState == 1)
                {
                    // If there are less than two neighbours I am going to die
                    if (aliveNeighbours < 2)
                    {
                        currentVoxel.GetComponent<Voxel>().SetFutureState(0);
                    }
                    // If there are two or three neighbours alive I am going to stay alive
                    if (aliveNeighbours == 2 || aliveNeighbours == 3)
                    {
                        currentVoxel.GetComponent<Voxel>().SetFutureState(1);
                    }
                    // If there are more than three neighbours I am going to die
                    if (aliveNeighbours > 3 && aliveNeighbours < 8)
                    {
                        currentVoxel.GetComponent<Voxel>().SetFutureState(0);
                    }
                    if (aliveNeighbours ==8 )
                    {
                        if (currentFrame > 6 && currentFrame < 10)
                        {
                            for (int x = -1; x <= 1; x++)
                            {
                                for (int y = -1; y <= 1; y++)
                                {
                                    int yVoxelState = (int)seedImage.GetPixel(i + x, j + y).grayscale;
                                    int yNeighbours = 0;
                                    yNeighbours += yVoxelState;
                                    currentVoxel = voxelGrid[i, j, 0];
                                    if (yNeighbours == 9)
                                    {
                                        currentVoxel.GetComponent<Voxel>().SetFutureState(1);
                                    }
                                }
                            }
                        }
                    }
                    if (aliveNeighbours == 9){
                        currentVoxel.GetComponent<Voxel>().SetFutureState(1);

                    }

                }
                // Rule Set 2: for voxels that are death
                if (currentVoxelState == 0)
                {
                    // If there are exactly three alive neighbours I will become alive
                    if (aliveNeighbours == 3)
                    {
                        currentVoxel.GetComponent<Voxel>().SetFutureState(1);

                    }

                    //  int yVoxelState = (int)seedImage.GetPixel(i, j).grayscale;

                }

			}
		}
	}

    // Save the CA states
	void SaveCA(){
		for(int i =0; i< width; i++){
			for (int j = 0; j < length; j++) {
                GameObject currentVoxel = voxelGrid[i, j, 0];
                int currentVoxelState = currentVoxel.GetComponent<Voxel>().GetState();
                // Save the voxel state
                GameObject savedVoxel = voxelGrid[i, j, currentFrame];
                savedVoxel.GetComponent<Voxel> ().SetState (currentVoxelState);                
                // Save the voxel age if voxel is alive
                if (currentVoxelState == 1) {
                    int currentVoxelAge = currentVoxel.GetComponent<Voxel>().GetAge();
                    savedVoxel.GetComponent<Voxel>().SetAge(currentVoxelAge);
                }
			}
		}
	}

	// Trace CA
	void TraceCA(){
		// Save in a list all the alive voxels from the last layer
		List<GameObject> aliveVoxels = new List<GameObject> ();
		for (int i = 0; i < width; i++) {
			for (int j = 0; j < length; j++) {
				GameObject currentVoxel = voxelGrid [i,j,height-2];
				if(currentVoxel.GetComponent<Voxel>().GetState() == 1){
					aliveVoxels.Add (currentVoxel);
				}
			}
		}
		// Get a random alive voxel from the last layer
		for (int i = 0; i < aliveVoxels.Count; i++) {
			GameObject randomVoxel = aliveVoxels[i];
			// Initalize the list that will save the polyline points
			linePoints = new List<Vector3> ();
			linePoints.Add (randomVoxel.GetComponent<Transform>().position);
			// Generate path below
			int currentHeight = height-2;
			while (currentHeight > 1) {
				// Get the oldest voxel below the currentVoxel
				int currentI = (int) randomVoxel.GetComponent<Voxel> ().address.x;
				int currentJ = (int) randomVoxel.GetComponent<Voxel> ().address.y;
				int currentK = (int) randomVoxel.GetComponent<Voxel> ().address.z;
				Vector2 oldestVoxelBelowAddress = PathCalculator (currentI, currentJ, currentK);
				GameObject oldestVoxelBelow = voxelGrid [(int)oldestVoxelBelowAddress.x, (int)oldestVoxelBelowAddress.y, (int)currentK-1];
				// Add the position of the oldest voxel below the the polyline list
				linePoints.Add(oldestVoxelBelow.GetComponent<Transform>().position);
				// Set the current voxel to the oldest voxel below
				randomVoxel = oldestVoxelBelow;
				// Start calculation for the layer below
				currentHeight--;
			}
			// Create a line render based on the traced path
			GameObject currentLine = new GameObject();
			currentLine.name = "line(" + i.ToString ()+")";
			currentLine.transform.parent = tracedLines.transform;
			LineRenderer lineRenderer = currentLine.AddComponent<LineRenderer>();
			lineRenderer.material = new Material (Shader.Find("Particles/Multiply"));
			lineRenderer.widthMultiplier = 0.2f;
			lineRenderer.positionCount = linePoints.Count;
			float alpha = 1.0f;
			Gradient gradient = new Gradient();
			gradient.SetKeys(
				new GradientColorKey[] { new GradientColorKey(tracedLinesColorStart, 0.0f), new GradientColorKey(tracedLinesColorEnd, 1.0f) },
				new GradientAlphaKey[] { new GradientAlphaKey(alpha, 0.5f), new GradientAlphaKey(alpha, 1.0f) }
			);
			lineRenderer.colorGradient = gradient;
			lineRenderer.SetPositions (linePoints.ToArray());
		}
	}

	// Path calculator
	Vector2 PathCalculator(int i, int j, int k){
		int oldestAge = 0;
		Vector2 voxelWithOldestAgeAddress = new Vector2(0,0);
		for (int x = -1; x <= 1; x++) {
			for (int y = -1; y <= 1; y++) {
				GameObject currentVoxel = voxelGrid [i + x, j + y, k - 1];
				int currentVoxelAge = currentVoxel.GetComponent<Voxel> ().GetAge();
				if (currentVoxelAge > oldestAge) {
					oldestAge = currentVoxelAge;
					voxelWithOldestAgeAddress.x = i + x;
					voxelWithOldestAgeAddress.y = j + y;
				}
			}
		}
		return voxelWithOldestAgeAddress;
	}
}
