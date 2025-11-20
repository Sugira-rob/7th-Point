using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Generic Singleton base class for MonoBehaviours.
/// Provides a simple Instance property and ensures a single instance exists.
/// </summary>
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<T>();
            }
            return instance;
        }
    }

    protected virtual void Awake()
    {
        if (instance == null)
        {
            instance = this as T;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }
}

// Minimal stub to replace generated Input System PlayerControls when it's missing.
// This stub provides only the members used by PlayerController and prevents compile errors.
// Replace with the real generated PlayerControls when available.
public class PlayerControls
{
    public MovementActions Movement { get; private set; }
    public CombatActions Combat { get; private set; }

    public PlayerControls()
    {
        Movement = new MovementActions();
        Combat = new CombatActions();
    }

    public void Enable() { /* no-op stub */ }
    public void Disable() { /* no-op stub */ }

    public class MovementActions
    {
        public InputActionStub Move { get; } = new InputActionStub();
    }

    public class CombatActions
    {
        public DashAction Dash { get; } = new DashAction();
    }

    public class DashAction
    {
        // Matches usage: playerControls.Combat.Dash.performed += _ => Dash();
        public event System.Action<object> performed;
        public void Trigger() => performed?.Invoke(null);
    }

    public class InputActionStub
    {
        // Matches usage: playerControls.Movement.Move.ReadValue<Vector2>();
        public T ReadValue<T>() where T : struct
        {
            if (typeof(T) == typeof(Vector2))
            {
                object v = Vector2.zero;
                return (T)v;
            }
            return default;
        }
    }
}

public class Knockback : MonoBehaviour
{
    // Minimal stub used by PlayerController to check if the player is currently being knocked back.
    public bool GettingKnockedBack { get; private set; } = false;

    // Optional helper to set the state from other scripts during testing.
    public void SetKnockback(bool value) => GettingKnockedBack = value;
}

// Minimal stub for ActiveInventory to satisfy PlayerController usage.
// This provides a safe, non-MonoBehaviour Instance with a no-op EquipStartingWeapon.
// If your project has a full ActiveInventory MonoBehaviour, remove or replace this stub.
public class ActiveInventory
{
    private static ActiveInventory instance;
    public static ActiveInventory Instance => instance ?? (instance = new ActiveInventory());

    // No-op stub to avoid runtime errors when PlayerController calls this in Start.
    public void EquipStartingWeapon() { /* intentionally left blank for stub */ }
}

// Minimal stub for PlayerHealth to satisfy PlayerController usage.
// Provides a simple Instance and an isDead flag used by PlayerController.
// Replace with the real PlayerHealth MonoBehaviour when available.
public class PlayerHealth
{
    private static PlayerHealth instance;
    public static PlayerHealth Instance => instance ?? (instance = new PlayerHealth());

    // Indicates whether the player is dead; other systems can set this during tests.
    public bool isDead = false;
}

// Minimal stub for Stamina to satisfy PlayerController usage.
// Provides a simple Instance, CurrentStamina and a UseStamina method.
// Replace with the real Stamina MonoBehaviour when available.
public class Stamina
{
    private static Stamina instance;
    public static Stamina Instance => instance ?? (instance = new Stamina());

    // Simple integer stamina pool; other systems can modify for tests.
    public int CurrentStamina { get; private set; } = 100;

    // Consumes stamina; default amount used when called without arguments.
    public void UseStamina(int amount = 1)
    {
        CurrentStamina = Mathf.Max(0, CurrentStamina - amount);
    }
}

public class PlayerController : Singleton<PlayerController>
{
    public bool FacingLeft { get { return facingLeft; } }
    

    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float dashSpeed = 4f;
    [SerializeField] private TrailRenderer myTrailRenderer;
    [SerializeField] private Transform weaponCollider;

    private PlayerControls playerControls;
    private Vector2 movement;
    private Rigidbody2D rb;
    private Animator myAnimator;
    private SpriteRenderer mySpriteRender;
    private Knockback knockback;
    private float startingMoveSpeed;

    private bool facingLeft = false;
    private bool isDashing = false;

    protected override void Awake() {
        base.Awake();

        playerControls = new PlayerControls();
        rb = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        mySpriteRender = GetComponent<SpriteRenderer>();
        knockback = GetComponent<Knockback>();
    }

    private void Start() {
        playerControls.Combat.Dash.performed += _ => Dash();

        startingMoveSpeed = moveSpeed;

        ActiveInventory.Instance.EquipStartingWeapon();
    }

    private void OnEnable() {
        playerControls.Enable();
    }

    private void OnDisable() {
        playerControls.Disable();
    }

    private void Update() {
        PlayerInput();
    }

    private void FixedUpdate() {
        AdjustPlayerFacingDirection();
        Move();
    }

    public Transform GetWeaponCollider() {
        return weaponCollider;
    }

    private void PlayerInput() {
        movement = playerControls.Movement.Move.ReadValue<Vector2>();

        myAnimator.SetFloat("moveX", movement.x);
        myAnimator.SetFloat("moveY", movement.y);
    }

    private void Move() {
        if (knockback.GettingKnockedBack || PlayerHealth.Instance.isDead) { return; }

        rb.MovePosition(rb.position + movement * (moveSpeed * Time.fixedDeltaTime));
    }

    private void AdjustPlayerFacingDirection() {
        Vector3 mousePos = Input.mousePosition;
        Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(transform.position);

        if (mousePos.x < playerScreenPoint.x) {
            mySpriteRender.flipX = true;
            facingLeft = true;
        } else {
            mySpriteRender.flipX = false;
            facingLeft = false;
        }
    }

    private void Dash() {
        if (!isDashing && Stamina.Instance.CurrentStamina > 0) {
            Stamina.Instance.UseStamina();
            isDashing = true;
            moveSpeed *= dashSpeed;
            myTrailRenderer.emitting = true;
            StartCoroutine(EndDashRoutine());
        }
    }

    private IEnumerator EndDashRoutine() {
        float dashTime = .2f;
        float dashCD = .25f;
        yield return new WaitForSeconds(dashTime);
        moveSpeed = startingMoveSpeed;
        myTrailRenderer.emitting = false;
        yield return new WaitForSeconds(dashCD);
        isDashing = false;
    }
}
