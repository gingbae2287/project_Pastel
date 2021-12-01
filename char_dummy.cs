using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class char_dummy : MonoBehaviour
{
     public float speed=5f;
     public float rotateSpeed=5f;
     Rigidbody rb;
     Vector3 movement;
     float horizontalMove;
     float verticalMove;

//-----Jump------------------//
     bool isJumping;
     int Jumpcount=1;
     public float jumpPower=5f;
//-----------------------------//
    
    //청크//
    Chunk Chunk;
    int playerInChunk_x;
    int playerInChunk_z;


    ///

    void Awake(){
        rb=GetComponent<Rigidbody> ();
    }
    void Start()
    {
        Chunk=GameObject.Find("Chunk").GetComponent<Chunk>();
        //게임 시작시 플레이어 주변 청크 로딩
        playerInChunk_x=(int)(gameObject.transform.position.x/16);
        playerInChunk_z=(int)(gameObject.transform.position.z/16);
        Chunk.ChunkUpdate(playerInChunk_x,playerInChunk_z);
        
    }
    void FixedUpdate(){ 
        Run();
        Turn();
        Jump();
    }

    // Update is called once per frame
    void Update()
    {
        horizontalMove=Input.GetAxisRaw("Horizontal");
        verticalMove=Input.GetAxisRaw("Vertical");
        /*if(Input.GetButton("Jump")) {
            if (isJumping) return;
            isJumping=true;
            //rigidbody.AddForce(Vector3.up*jumpPower,ForceMode.Impulse);
        }*/

        //청크업데이트
        CheckChunkChange();
        
    }

    void Run(){
        movement.Set(horizontalMove,0,verticalMove);
        movement=movement.normalized*speed*Time.deltaTime;
        //normalized = vector의 합? 모든방향을 합친 하나의 방향과 크기
        // Time.deltaTime 프레임 보정시간

        rb.MovePosition(transform.position+movement);
        // transform.position현재 오브젝트 위치

    }

    void Turn(){
        if(horizontalMove==0&& verticalMove==0) return;
        Quaternion newRotation=Quaternion.LookRotation(movement);
        rb.rotation=Quaternion.Slerp(rb.rotation, newRotation,rotateSpeed*Time.deltaTime);

    }

    void Jump(){
        if(Input.GetButton("Jump")){
            if(isJumping) return;
            if(Jumpcount<=0) return;


            rb.AddForce(Vector3.up*jumpPower,ForceMode.Impulse);
            Jumpcount--;
        }
    }

    void OnCollisionEnter(Collision other){
        //if (isJumping==false) return;
        if (other.gameObject.tag=="ground"){
            isJumping=false;
            Jumpcount=100;
        }

    }

    void CheckChunkChange(){
        if(playerInChunk_x!=(int)(gameObject.transform.position.x/16) || playerInChunk_z!=(int)(gameObject.transform.position.z/16)){
            playerInChunk_x=(int)(gameObject.transform.position.x/16);
            playerInChunk_z=(int)(gameObject.transform.position.z/16);
            Chunk.ChunkUpdate(playerInChunk_x,playerInChunk_z);
        }
    }



    ////테스트용함수////
    void SpeedChange(float p){
        speed=p;
    }
}
