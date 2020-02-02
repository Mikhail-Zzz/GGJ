using System;
using System.Collections.Generic;

using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
public class CharacterController : MonoBehaviour
{
    private const KeyCode MoveUpstairsKeyCode = KeyCode.W;
    private const KeyCode MoveDownstairsKeyCode = KeyCode.S;
    private const KeyCode MoveRightKeyCode = KeyCode.D;
    private const KeyCode MoveLeftKeyCode = KeyCode.A;
    private const KeyCode JumpKeyCode = KeyCode.Space;

    private const float MAX_WORK_DISTANCE = 5;

    private enum GoSides
    {
        Left,
        Right
    }

    private GoSides CurrentGoSide { get; set; } = GoSides.Right;

    private bool MoveRight { get; set; }
    private bool MoveLeft { get; set; }
    private bool Grounded { get; set; }

    private bool IsOnLadder { get; set; }
    private bool GoingUp { get; set; }
    private bool GoingDown { get; set; }

    private const float MoveSpeed = 4f;
    private const float JumpMoveSpeed = 1.5f;
    private const float JumpSpeed = 6f;
    private const float LadderUpSpeed = 3f;
    private const float LadderDownSpeed = 4f;

    private Animator CharacterAnimator;
    private Rigidbody2D CharacterRigidbody;
    private Collider2D CharacterCollider;
    private GameObject ProbeBlock;

    public GameObject HandBlock;
    public GameObject ProbeBlockPrefab;
    
    public float TopLeftX;
    public float TopLeftY;

    public float TileWidth;
    public float TileHeight;

    // Start is called before the first frame update
    void Start()
    {
        CharacterAnimator = GetComponent<Animator>();
        CharacterRigidbody = GetComponent<Rigidbody2D>();
        CharacterCollider = GetComponent<Collider2D>();

        ProbeBlock = Instantiate(ProbeBlockPrefab, new Vector3(-999, -999), new Quaternion());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(MoveRightKeyCode))
        {
            CurrentGoSide = GoSides.Right;
            MoveRight = true;
            MoveLeft = false;
            CorrectWatch();
        }

        if (Input.GetKeyUp(MoveRightKeyCode))
            MoveRight = false;

        if (MoveRight)
        {
            CharacterAnimator.SetBool("IsGoing", true);
            Move((Grounded ? MoveSpeed : JumpMoveSpeed) * Time.deltaTime, 0);
        }

        if (Input.GetKeyDown(MoveLeftKeyCode))
        {
            CurrentGoSide = GoSides.Left;
            MoveLeft = true;
            MoveRight = false;
            CorrectWatch();
        }

        if (Input.GetKeyUp(MoveLeftKeyCode))
            MoveLeft = false;

        if (MoveLeft)
        {
            CharacterAnimator.SetBool("IsGoing", true);
            Move(-(Grounded ? MoveSpeed : JumpMoveSpeed) * Time.deltaTime, 0);
        }

        if(!MoveLeft && !MoveRight)
            CharacterAnimator.SetBool("IsGoing", false);

        if (Input.GetKeyDown(JumpKeyCode) && Grounded)
        {
            CharacterAnimator.SetBool("IsInAir", true);
            CharacterRigidbody.AddForce(new Vector2(0, JumpSpeed), ForceMode2D.Impulse);
            Grounded = false;
        }

        if(!Grounded && !IsOnLadder)
            CharacterAnimator.SetBool("IsInAir", true);

        if (IsOnLadder)
        {
            if (Input.GetKeyDown(MoveUpstairsKeyCode))
            {
                CharacterAnimator.speed = 1;
                GoingUp = true;
                GoingDown = false;
            }

            if (Input.GetKeyDown(MoveDownstairsKeyCode))
            {
                CharacterAnimator.speed = 1;
                GoingDown = true;
                GoingUp = false;
            }

            if (Input.GetKeyUp(MoveUpstairsKeyCode))
            {
                CharacterAnimator.speed = 0;
                GoingUp = false;
            }

            if (Input.GetKeyUp(MoveDownstairsKeyCode))
            {
                CharacterAnimator.speed = 0;
                GoingDown = false;
            }

            if (GoingUp)
            {
                CharacterAnimator.speed = 1;
                Move(0, LadderUpSpeed * Time.deltaTime);
            }

            if (GoingDown)
            {
                CharacterAnimator.speed = -1;
                Move(0, -LadderDownSpeed * Time.deltaTime);
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            Vector3 coords = GetTileCoords(Input.mousePosition);

            if(CheckDistance(coords, MAX_WORK_DISTANCE))
                Instantiate(HandBlock, coords, Quaternion.identity);
        }

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 coords = GetTileCoords(Input.mousePosition);

            if (CheckDistance(coords, MAX_WORK_DISTANCE))
                ProbeBlock.transform.position = coords;
        }

        if (Input.GetKeyDown(KeyCode.R))
            gameObject.transform.rotation = Quaternion.identity;
    }

    private bool CheckDistance(Vector3 vector, float maxDistance)
    {
        return Math.Pow(gameObject.transform.position.x - vector.x, 2.0) +
               Math.Pow(gameObject.transform.position.y - vector.y, 2.0) <= maxDistance * maxDistance;
    }

    private Vector3 GetTileCoords(Vector3 mousePos)
    {
        Vector3 inp = new Vector3(
                mousePos.x,
                mousePos.y,
                -Camera.main.transform.position.z + Camera.main.depth
            );

        Vector3 wp = Camera.main.ScreenToWorldPoint(inp);
        Vector3 np = new Vector3(
            (int)((wp.x - TopLeftX) / TileWidth) * TileWidth,
            (int)((wp.y - TopLeftY) / TileHeight) * TileHeight
        );

        return np;
    }

    private HashSet<GameObject> stairs = new HashSet<GameObject>();

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Stairs"))
        {
            CharacterAnimator.SetBool("IsOnLadder", true);

            IsOnLadder = true;
            CharacterRigidbody.gravityScale = 0;
            gameObject.transform.rotation = Quaternion.identity;
            CharacterRigidbody.freezeRotation = true;

            stairs.Add(col.gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Stairs"))
        {
            stairs.Remove(col.gameObject);

            if (stairs.Count == 0)
            {
                CharacterAnimator.SetBool("IsOnLadder", false);
                IsOnLadder = false;
                CharacterRigidbody.gravityScale = 1;
                CharacterRigidbody.freezeRotation = false;
            }
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        CharacterAnimator.SetBool("IsInAir", false);
        Grounded = true;
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        Grounded = false;
    }

    private void Move(float dx, float dy)
    {
        gameObject.transform.Translate(dx, dy, 0);
    }

    private void CorrectWatch()
    {
        gameObject.transform.localScale = new Vector3(CurrentGoSide == GoSides.Right ? 1 : -1, 1);
    }
}
