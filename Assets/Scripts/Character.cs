using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    public Rigidbody body;
    public Text score;
    public Transform player;
    
    private bool _jumped;
    private bool _rightMoved;
    private bool _leftMoved;
    private bool _grounded = true;
    private string _pos;
    private bool _collided;
    private const string RestartText = "\nTekrar oynamak için R tuşuna basın.";

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W))
        {
            if (_grounded)
            {
                _jumped = true;
                _grounded = false;
            }
        }
        
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            _rightMoved = true;
        }

        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            _leftMoved = true;
        }

        switch (_collided)
        {
            case false:
            {
                var x = player.position.x;
                _pos = (x < 0) ? "0" : x.ToString("0");
                score.text = _pos;
                break;
            }

            case true when Input.GetKeyDown(KeyCode.R):
                _collided = false;
                body.MovePosition(new Vector3(0, 5));
                break;
        }
    }

    private void FixedUpdate()
    {
        if (_jumped && !_collided)
        {
            body.AddForce(Vector3.up, ForceMode.Impulse);
            _jumped = false;
        }

        if (_rightMoved && !_collided)
        {
            body.AddForce(Vector3.right, ForceMode.Impulse);
            _rightMoved = false;
        }

        if (_leftMoved && !_collided)
        {
            body.AddForce(Vector3.left, ForceMode.Impulse);
            _leftMoved = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        var colliderTag = collision.collider.tag;

        switch (colliderTag)
        {
            case "Respawn" when !_collided:
                _collided = true;
                score.text = "Kaybettiniz!\nSkor : " + _pos + RestartText;
                break;
            
            case "Terrain":
                _grounded = true;
                break;
            
            case "Finish":
                _collided = true;
                score.text = "Tebrikler!\nSkor : " + _pos + RestartText;
                break;
        }
    }
}
