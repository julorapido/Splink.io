using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using UnityEngine;
using PathCreation.Utility;

public class PlayerCollisions : MonoBehaviour
{
    // Start is called before the first frame update
    //public Collider mmbr_collider;
    public string[] colsions_values = new string[4]{"ground", "frontwall","sidewall", "slider"};
    [Header ("Selected Collision")]
    public string slcted_clsion;
    [Header ("Start Delay")]
    public float strt_delay;
    private bool can_trgr = false;
    public Transform ply_transform;
    private Vector3 currentVelocity;
    //ImmutableList<string> colors = ImmutableList.Create("Red", "Green", "Blue");
    private void Start(){
        StartCoroutine(delay_trgrs(strt_delay));
    }

    private void OnTriggerEnter(Collider collision) {
        if(slcted_clsion.Length > 0 && can_trgr){
            Vector3 _size = collision.bounds.size;
            switch (slcted_clsion){
                case "ground":
                    // Grnd hit
                    if(collision.gameObject.tag == "ground"){
                        FindObjectOfType<PlayerMovement>().animateCollision("groundHit", _size);
                        FindObjectOfType<CameraMovement>().fly_dynm(false);
                    }
                    // Obstcl hit
                    if(collision.gameObject.tag == "obstacle"){
                        FindObjectOfType<PlayerMovement>().animateCollision("obstacleHit", _size);
                    }
                    // Launcher jmp
                    if(collision.gameObject.tag == "launcher"){ FindObjectOfType<PlayerMovement>().animateCollision("launcherHit", _size);}
                    // Tyro hit
                    if(collision.gameObject.tag == "tyro"){
                        FindObjectOfType<PlayerMovement>().tyro_movement(collision.gameObject);
                    }
                    break;
                case "frontwall":
                    // front wall gameover
                    if(collision.gameObject.tag == "ground"){
                        FindObjectOfType<PlayerMovement>().animateCollision("frontWallHit", _size);
                    }
                    break;
                case "sidewall":
                    // Sidewall hit
                    if(collision.gameObject.tag == "ground"){
                        Vector3 targetDir = collision.gameObject.transform.position - ply_transform.position;
                        float angle = Vector3.Angle(targetDir, transform.forward);
                        FindObjectOfType<PlayerMovement>().animateCollision("wallRunHit", _size);
                    }
                    break; 
                case "slider":
                    if(collision.gameObject.tag == "slider"){
                        //LeanTween.scale(collision.gameObject, collision.gameObject.transform.localScale * 1.05f, 0.4f).setEasePunch();
                        Vector3 dwned = collision.gameObject.transform.position;
                        dwned.y -= 0.1f;
                        Vector3 smoothDwn_ = Vector3.SmoothDamp(collision.gameObject.transform.position, dwned, ref currentVelocity, 0.4f); 
                        FindObjectOfType<PlayerMovement>().animateCollision("sliderHit", _size);
                    }  
                    break;
                default:
                    break;
            }
        }
    }
    
    private void OnTriggerExit(Collider collision) {
        if(slcted_clsion.Length > 0 && can_trgr){
            Vector3 _size = collision.bounds.size;
            switch (slcted_clsion)
            {
                case "ground":
                    if(collision.gameObject.tag == "ground"){
                        FindObjectOfType<PlayerMovement>().animateCollision("groundLeave", _size);
                        FindObjectOfType<CameraMovement>().fly_dynm(true);
                        //FindObjectOfType<PlayerMovement>().animateCollision("groundLeave");
                    }
                    if(collision.gameObject.tag == "obstacle"){
                        FindObjectOfType<PlayerMovement>().animateCollision("obstacleLeave", _size);
                    }
                    break;
                case "sidewall":
                    if(collision.gameObject.tag == "ground"){
                        FindObjectOfType<PlayerMovement>().animateCollision("wallRunExit", _size);
                    }
                    break; 
                case "slider":
                    if(collision.gameObject.tag == "slider"){
                        LeanTween.scale(collision.gameObject, collision.gameObject.transform.localScale * 1.08f, 1f).setEasePunch();
                        Vector3 upped = collision.gameObject.transform.position;
                        upped.y += 0.1f;
                        Vector3 smoothUp_ = Vector3.SmoothDamp(collision.gameObject.transform.position, upped, ref currentVelocity, 0.4f); 
                        FindObjectOfType<PlayerMovement>().animateCollision("sliderLeave", _size);
                    }  
                    break;
                default:
                    break;
            }
        }
    }

    private IEnumerator delay_trgrs(float dl){
        yield return new WaitForSeconds(dl);
        can_trgr = true;
    }
 
}
