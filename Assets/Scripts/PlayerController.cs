using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 10.0F;
    private Rigidbody rb;
    public CapsuleCollider col;
    public LayerMask groundLayers;
    public GameObject projectilePrefab;
    public Transform projectileSpawn;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        rb = GetComponent<Rigidbody>();
        col = GetComponent<CapsuleCollider>();
    }
    
    void Update()
    {
        if (Input.GetKeyDown("escape"))
            Cursor.lockState = CursorLockMode.None;
        
        if (IsGrounded())
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                rb.AddForce(Vector3.up * 20, ForceMode.Impulse);
            }
            
            float moveHorizontal = Input.GetAxis("Horizontal") * speed;
            float moveVertical = Input.GetAxis("Vertical") * speed;

            moveHorizontal *= Time.deltaTime;
            moveVertical *= Time.deltaTime;
        
            transform.Translate(moveHorizontal,0,moveVertical);
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            Fire();
        }
    }

    private void Fire()
    {
        GameObject projectile = Instantiate(projectilePrefab, projectileSpawn.position, projectileSpawn.rotation);

        projectile.GetComponent<Rigidbody>().velocity = projectile.transform.forward * speed*2.5f;
        
        Destroy(projectile, 2);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Pick Up"))
        {
            other.gameObject.SetActive(false);
        }
    }

    private bool IsGrounded()
    {
        return Physics.CheckCapsule(col.bounds.center, new Vector3(col.bounds.center.x,
                col.bounds.min.y, col.bounds.center.z), col.radius, groundLayers);
    }
}
