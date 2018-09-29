using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.WSA;
using Cursor = UnityEngine.Cursor;

public class PlayerController : MonoBehaviour
{
    public float speed = 10.0F;
    private Rigidbody rb;
    public CapsuleCollider col;
    public LayerMask groundLayers;
    public GameObject projectilePrefab;
    public Transform projectileSpawn;
    private int projectileCount;
    public Text projectileCountText;
    public Text winText;
    private bool gameFinished;
    public GameObject TileManager;
    
    
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        rb = GetComponent<Rigidbody>();
        col = GetComponent<CapsuleCollider>();
        projectileCount = 0;
        SetProjectileCount();
        gameFinished = false;
    }
    
    void Update()
    {
        if (gameFinished)
        {
            return;
        }
        
        if (Input.GetKeyDown("escape"))
            Cursor.lockState = CursorLockMode.None;
        
        if (IsGrounded())
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                rb.AddForce(Vector3.up * 15    , ForceMode.Impulse);
            }
            
            if (projectileCount > 0 && Input.GetKeyDown(KeyCode.F))
            {
                Fire();
            }
            
            float moveHorizontal = Input.GetAxis("Horizontal") * speed;
            float moveVertical = Input.GetAxis("Vertical") * speed;

            moveHorizontal *= Time.deltaTime;
            moveVertical *= Time.deltaTime;
        
            transform.Translate(moveHorizontal,0,moveVertical);
        }
    }

    private void Fire()
    {
        projectileCount--;
        SetProjectileCount();
        GameObject projectile = Instantiate(projectilePrefab, projectileSpawn.position, projectileSpawn.rotation);
        projectile.GetComponent<Rigidbody>().velocity = projectile.transform.forward * speed*2.5f;
        projectile.GetComponent<ProjectileController>().SetOnTriggerAction((pos) => CheckProjectilePosition(pos, projectile));
    }

    private bool CheckProjectilePosition(float position, GameObject projectile)
    {
        if (position > TileManager.GetComponent<TileManager>().GetScreenLimit())
        {
            Destroy(projectile);
            TileManager.GetComponent<TileManager>().ConstructNorthWalls();
            return true;
        }

        return false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Pick Up"))
        {
            projectileCount++;
            SetProjectileCount();
            other.gameObject.SetActive(false);
        }
        
        if (other.gameObject.CompareTag("Goal"))
        {
            gameFinished = true;
            gameObject.transform.GetChild(0).GetComponent<camMouseLook>().SetGameFinished(true);
            other.gameObject.SetActive(false);
            winText.gameObject.SetActive(true);
        }
    }

    private bool IsGrounded()
    {
        return Physics.CheckCapsule(col.bounds.center, new Vector3(col.bounds.center.x,
                col.bounds.min.y, col.bounds.center.z), col.radius, groundLayers);
    }

    private void SetProjectileCount()
    {
        projectileCountText.text = "Projectile count: " + projectileCount;
    }
}
