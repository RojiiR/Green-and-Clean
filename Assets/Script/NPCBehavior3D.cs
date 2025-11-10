using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CapsuleCollider))]
public class NPCBehavior3D : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 1.5f;
    public float runSpeed = 4f;
    public Vector3 moveDirection = Vector3.forward;
    public bool faceMoveDirection = true;

    [Header("Cycle Durations")]
    public float walkDuration = 5f;
    public float idleDuration = 5f;
    public float throwAnimApproxDuration = 2.5f;

    [Header("Player Detection")]
    public float detectRange = 2.5f;
    public KeyCode interactKey = KeyCode.F;
    public Transform player;

    [Header("Spawn Settings")] // 🧩 BAGIAN BARU
    public GameObject[] trashPrefabs; // 3 prefab sampah
    public float spawnOffsetY = 0.5f; // biar gak nempel tanah

    private Rigidbody rb;
    private Animator anim;

    private bool isWalking;
    private bool isRunning;
    private bool isThrowing;
    private bool canThrow = true;
    private bool grounded = true;
    private bool playerNearby;

    private Coroutine mainRoutine;
    private Coroutine runRoutine;

    private int hashIsWalking;
    private int hashIsRunning;
    private int hashDoThrow;
    private int hashRunState;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();

        hashIsWalking = Animator.StringToHash("isWalking");
        hashIsRunning = Animator.StringToHash("isRunning");
        hashDoThrow = Animator.StringToHash("doThrow");
        hashRunState = Animator.StringToHash("Run");

        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p) player = p.transform;
        }

        rb.constraints = RigidbodyConstraints.FreezeRotation;
        rb.useGravity = true;
    }

    void Start()
    {
        anim.applyRootMotion = false;
        mainRoutine = StartCoroutine(MainSequence());
    }

    void Update()
    {
        grounded = Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.down, 0.3f, LayerMask.GetMask("Default"));

        bool inRange = playerNearby || (player != null && Vector3.Distance(transform.position, player.position) <= detectRange);

        if (inRange && Input.GetKeyDown(interactKey))
        {
            if (!isRunning)
                StartRun();
        }

        if (faceMoveDirection && rb.velocity.magnitude > 0.1f)
        {
            Vector3 dir = rb.velocity;
            dir.y = 0;
            if (dir.sqrMagnitude > 0.01f)
                transform.rotation = Quaternion.LookRotation(dir.normalized);
        }
    }

    private IEnumerator MainSequence()
    {
        // 1️⃣ Jalan
        isWalking = true;
        anim.SetBool(hashIsWalking, true);
        anim.SetBool(hashIsRunning, false);
        float t = 0f;
        while (t < walkDuration && !isRunning)
        {
            MoveForward(walkSpeed);
            t += Time.deltaTime;
            yield return null;
        }
        if (isRunning) yield break;

        // 2️⃣ Diam
        StopMoving();
        anim.SetBool(hashIsWalking, false);
        t = 0f;
        while (t < idleDuration && !isRunning)
        {
            t += Time.deltaTime;
            yield return null;
        }
        if (isRunning) yield break;

        // 3️⃣ Lempar
        if (canThrow)
        {
            isThrowing = true;
            anim.SetTrigger(hashDoThrow);
            yield return StartCoroutine(WaitThrowComplete());
            canThrow = false;
        }
        if (isRunning) yield break;

        // 4️⃣ Jalan terus
        isWalking = true;
        anim.SetBool(hashIsWalking, true);
        while (!isRunning)
        {
            MoveForward(walkSpeed);
            yield return null;
        }
    }

    private IEnumerator WaitThrowComplete()
    {
        float timer = 0f;
        while (isThrowing && timer < throwAnimApproxDuration)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        isThrowing = false;
    }

    private void MoveForward(float speed)
    {
        if (!grounded) return;
        Vector3 worldDir = transform.TransformDirection(moveDirection.normalized);
        Vector3 vel = worldDir * speed;
        vel.y = rb.velocity.y;
        rb.velocity = vel;
    }

    private void StopMoving()
    {
        rb.velocity = new Vector3(0f, rb.velocity.y, 0f);
    }

    // 🧩 DIPANGGIL OLEH EVENT ANIMASI LEMPAR
    public void OnThrowAnimationComplete()
    {
        isThrowing = false;

        // 🧩 BAGIAN BARU: Spawn objek di posisi player
        if (player != null && trashPrefabs != null && trashPrefabs.Length > 0)
        {
            int rand = Random.Range(0, trashPrefabs.Length);
            Vector3 spawnPos = transform.position + transform.forward * 0.5f + Vector3.up * spawnOffsetY;
            Instantiate(trashPrefabs[rand], spawnPos, Quaternion.identity);
        }
    }

    private void StartRun()
    {
        if (mainRoutine != null)
        {
            StopCoroutine(mainRoutine);
            mainRoutine = null;
        }

        isRunning = true;
        isWalking = false;
        isThrowing = false;
        canThrow = false;

        anim.ResetTrigger(hashDoThrow);
        anim.SetBool(hashIsWalking, false);
        anim.SetBool(hashIsRunning, true);
        anim.CrossFade(hashRunState, 0.1f, 0, 0f);

        if (runRoutine != null) StopCoroutine(runRoutine);
        runRoutine = StartCoroutine(RunForward());
    }

    private IEnumerator RunForward()
    {
        float smoothVel = 0f;
        while (isRunning)
        {
            Vector3 targetVel = transform.TransformDirection(moveDirection.normalized) * runSpeed;
            targetVel.y = rb.velocity.y;
            rb.velocity = Vector3.Lerp(rb.velocity, targetVel, Time.deltaTime * 5f);
            yield return null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            playerNearby = true;
        if (other.CompareTag("Ground"))
            grounded = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            playerNearby = false;
        if (other.CompareTag("Ground"))
            grounded = false;
    }
}
