using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Animator _anim;
    BoxCollider2D _collider;

    [SerializeField]
    private float _enemySpeed = 4f;

    [SerializeField]
    private GameObject _laserPrefab;

    private float _fireRate = 3.0f;
    private float _canFire = -1;

    private Player _player;

    private AudioSource _enemyAudioSource;
    [SerializeField]
    private AudioClip _explosionSound;

     
    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        if (_player == null)
            Debug.LogError("No Player Component");

        _anim = GetComponent<Animator>();

        if (_anim == null)
            Debug.LogError("Animator is null");

        _collider = GetComponent<BoxCollider2D>();

        if (_collider == null)
            Debug.LogError("Collider is null");

        _enemyAudioSource = GetComponent<AudioSource>();
        if (_enemyAudioSource == null)
            Debug.LogError("No enemy audiosource");
        else
            _enemyAudioSource.clip = _explosionSound;
     
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();

        if (Time.time > _canFire)
        {
            _fireRate = Random.Range(3f, 7f);
            _canFire = Time.time + _fireRate;
            GameObject enemyLaser = Instantiate(_laserPrefab, transform.position, Quaternion.identity);
            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();

            foreach (Laser i in lasers)
                i.AssignEnemyLaser();
        }

    }

    void CalculateMovement()
    {
        transform.Translate(Vector3.down * _enemySpeed * Time.deltaTime);

        if (transform.position.y < -5f)
        {
            float randomX = Random.Range(-8f, 8f);
            transform.position = new Vector3(randomX, 7, 0);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();

            if (player != null)
            {
                player.Damage();
            }

            _anim.SetTrigger("OnEnemyDeath");
            _collider.enabled = false; 
            //_enemySpeed = 0;
            _enemyAudioSource.Play();
            Destroy(this.gameObject, 2.4f);
        }
        
        if (other.tag == "Laser")
        {
            if (_player != null)
                _player.AddScore(10);

            Destroy(other.gameObject);
            _anim.SetTrigger("OnEnemyDeath");
            _collider.enabled = false;
            //_enemySpeed = 0;
            _enemyAudioSource.Play();
            Destroy(this.gameObject, 2.4f);
        } 
    

    }

}
