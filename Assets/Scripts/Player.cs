using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.5f;

    private float _speedMultiplier = 2f;

    [SerializeField]
    private GameObject _laserPrefab;

    [SerializeField]
    private GameObject _tripleShotPrefab;

    [SerializeField]
    private GameObject _speedPrefab;

    [SerializeField]
    private GameObject _shieldPrefab;

    [SerializeField]
    private GameObject _shield;

    private float _laserOffset = 1.05f;

    [SerializeField]
    private float _fireRate = 0.15f;
    private float _canFire = -1f;
    [SerializeField]
    private int _lives = 3;

    [SerializeField]
    private bool _isTripleShotActive = false;
    [SerializeField]
    private bool _isSpeedBoostActive = false;
    [SerializeField]
    private bool _isShieldActive = false;

    [SerializeField]
    private float _tripleShotCooldown = 5.0f;
    [SerializeField]
    private float _speedBoostCooldown = 5.0f;

    [SerializeField]
    private int _score;

    public int bestScore;

    [SerializeField]
    private GameObject _leftEngine, _rightEngine;

    [SerializeField]
    private AudioClip _laserSound;

    private AudioSource _playerAudioSource;

    private SpawnManager _spawnManager;
    private UI_Manager _uiManager;
   
    // Start is called before the first frame update
    void Start()
    {   
        transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        if (_spawnManager == null)
            Debug.LogError("Spawn Manager is null");

        _uiManager = GameObject.Find("Canvas").GetComponent<UI_Manager>();
        if (_uiManager == null)
            Debug.LogError("UI Manager is null");

        _playerAudioSource = GetComponent<AudioSource>();
        if (_playerAudioSource == null)
            Debug.LogError("No player Audio Source");
        else
            _playerAudioSource.clip = _laserSound;
        
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();

        if (Input.GetKeyDown(KeyCode.Space) && (Time.time > _canFire))
        {
            FireLaser();
        }

    }

    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

        
        transform.Translate(direction * _speed * Time.deltaTime);
        
        
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 0), 0);

        if (transform.position.x > 11.3f)
        {
            transform.position = new Vector3(-11.3f, transform.position.y, 0);
        }
        else if (transform.position.x < -11.3f)
        {
            transform.position = new Vector3(11.3f, transform.position.y, 0);
        }
    }

    void FireLaser()
    {
        _canFire = Time.time + _fireRate;

        if (_isTripleShotActive)
        {
            Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            Instantiate(_laserPrefab, transform.position + new Vector3(0, _laserOffset, 0), Quaternion.identity);
        }

        _playerAudioSource.Play();
        
    }

    public void Damage()
    {
        if (_isShieldActive)
        {
            _isShieldActive = false;
            _shield.SetActive(false);
            return;
        }

        _lives--;
        _uiManager.UpdateLives(_lives);

        if (_lives == 2)
            _leftEngine.SetActive(true);
        else if (_lives == 1)
            _rightEngine.SetActive(true);
            


        if (_lives < 1)
        {
            // Tell spawn manager to stop making enemies
            _spawnManager.OnPlayerDeath();
            CheckForBestScore();
            Destroy(this.gameObject);          
        }
    }

    public void SpeedBoostActive()
    {
        _isSpeedBoostActive = true;
        _speed *= _speedMultiplier;
        StartCoroutine(SpeedBoostPowerDown());
    }

    IEnumerator SpeedBoostPowerDown()
    {
        while (_isSpeedBoostActive)
        {
            yield return new WaitForSeconds(_speedBoostCooldown);
            _speed /= _speedMultiplier;
            _isSpeedBoostActive = false;
        }
    }

    public void ShieldActive()
    {
        _isShieldActive = true;
        _shield.SetActive(true);
    }

    
    public void TripleShotActive()
    {
        _isTripleShotActive = true;
        StartCoroutine(CoolDownRoutine());
    }

    IEnumerator CoolDownRoutine()
    {
        while (_isTripleShotActive)
        {
            yield return new WaitForSeconds(_tripleShotCooldown);
            _isTripleShotActive = false;
        }
    }

    public void AddScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }

    void CheckForBestScore()
    {
        if (_score > PlayerPrefs.GetInt("bestScore"))
            PlayerPrefs.SetInt("bestScore", _score);
    }
}
