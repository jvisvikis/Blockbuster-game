using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiamondEnemyBehaviour : MonoBehaviour
{
    Rigidbody rigidbody;
    Transform target;
    public string targetTag = "Player";
    public float maxLookDist = 10;

    public GameObject laser;
    public float laserTotalLength;
    private float laserCurrentLength;
    public float laserGrowthRate;

    public float moveSpeed;

    public Vector3[] Positions;
    private Vector3 oldPosition;

    private Vector3 newPosition;
    private bool newPosCheck=false;

    //if turning on isRepeating, make sure the last element in the array is the original position it started at
    public bool isRepeating;

    private int i = 0; //index


    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        target = GameObject.FindGameObjectWithTag(targetTag).transform;
    }

    // Update is called once per frame
    void Update()
    {
            if(laser!=null){
                if(laserCurrentLength<=laserTotalLength){
                    laser.transform.localScale = new Vector3(laserCurrentLength, 1f, 1f);
                    laserCurrentLength += laserGrowthRate*Time.deltaTime;
                }
            }
   
            var step = moveSpeed*Time.deltaTime;

            if(laserCurrentLength>=laserTotalLength){
                if(Positions!=null && Positions.Length != 0){
                    if(newPosCheck==false){             
                        newPosition=transform.position+Positions[i];
                        newPosCheck=true;
                    }
                    if(transform.position!=newPosition){
                        transform.position = Vector3.MoveTowards(transform.position, newPosition, step);
                        
                    }
                    if(transform.position == newPosition){
                        if(isRepeating == true){
                            if(i==Positions.Length-1){
                                i=-1;
                            }
                        }
                        if(i<Positions.Length-1){
                            i++;
                            newPosCheck=false;
                        }

                    }
                }
            }
        }
}

