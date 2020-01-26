using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
   
    [SerializeField]
    private float _powerupSpeed = 3.0f;
    // Triple, Speed, Shield
    [SerializeField]
    private int _powerupID; // TripleShot, Speed, Shield

    private Renderer _rend;
    private AudioSource _audioSource;
    
    // Start is called before the first frame update
    void Start()
    {
        _rend = GetComponent<Renderer>();
        _audioSource = GetComponent<AudioSource>();

        if (_rend == null)
            Debug.LogError(" power up renderer is null");
        if (_audioSource == null)
            Debug.LogError("power up audio source is null");

    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _powerupSpeed * Time.deltaTime);

        if (transform.position.y < -4.5f)
            Destroy(this.gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();
            if (player != null)
            {
                

                switch (_powerupID)
                {
                    case 0:
                        player.TripleShotActive();
                        break;
                    case 1:
                        player.SpeedBoostActive();
                        break;
                    case 2:
                        player.ShieldActive();
                        break;   
                    default:
                        Debug.Log("Unknown Powerup");
                        break;
                }

                _audioSource.Play();
                _rend.enabled = false;
                Destroy(this.gameObject, 0.8f);
            }
        }
    }
}
