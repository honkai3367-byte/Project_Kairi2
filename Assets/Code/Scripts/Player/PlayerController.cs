using UnityEngine;
using UnityEngine.InputSystem;
using EnumType;		// GlobalEnum

public class PlayerController : MonoBehaviour, IDamageable
{
	// 플레이어 정보
	private Rigidbody2D rigid;
	private SpriteRenderer sprite;
	private PlayerState state;      // 플레이어 상태
	private bool isGrounded;        // 땅 여부
	private bool isInvincible;		// 무적 여부 (구르기)

	// 이동값
	private Vector2 inputVec;   // 입력된 플레이어 이동값 (-1, 0, 1)
	private float speed;      // 플레이어 이동 속도

	private void Awake()
	{
		rigid = GetComponent<Rigidbody2D>();
		sprite = GetComponent<SpriteRenderer>();
	}

	private void Start()
	{
		isGrounded = true;
		isInvincible = false;
		state = PlayerState.Idle;
		speed = GameManager.Instance.playerStatsRuntime.speed;
	}

	/* Update */
	private void Update()
	{
		rigid.linearVelocity = new Vector2(inputVec.x * speed, rigid.linearVelocityY);

	}

	// 플레이어 스프라이트 업데이트
	private void UpdateSprite()
	{
		// 좌우 플립
		if (inputVec.x > 0)
			sprite.flipX = false;
		else if (inputVec.x < 0)
			sprite.flipX = true;
	}

	/* Input System */
	void OnMove(InputValue val)
	{
		inputVec = val.Get<Vector2>();
	}

	void OnJump(InputValue val)
	{
		if(isGrounded)
		{
			rigid.AddForce(Vector2.up * GameManager.Instance.playerStatsRuntime.jumpForce, ForceMode2D.Impulse);
			isGrounded = false;
		}
	}

	/* Interface */
	public void TakeDamage(int attack)	// 데미지
	{
		if (isInvincible) return;   // 무적일 경우 리턴

		GameManager.Instance.playerStatsRuntime.currentHP -= attack;    // 체력 감소
		
		if (GameManager.Instance.playerStatsRuntime.currentHP <= 0)     // 체력이 0 이하일 때
		{
			return;
		}
	}

	public void Die()	// 죽음 처리
	{

	}
}
