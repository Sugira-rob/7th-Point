using UnityEngine;

public class Sword : MonoBehaviour
{
    private PlayerControlls playerControls;
    private Animator myAnimator;

    private void Awake()
    {
        myAnimator = GetComponent<Animator>();
        playerControls = new PlayerControlls();
    }

    private void OnEnable()
    {
        playerControls.Combat.Attack.started += _=> Attack();
    }

    private void Attack()
    {
        myAnimator.SetTrigger("Attack");
    }
}
