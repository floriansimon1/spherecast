using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class DamageablePlayer: DamageableByAsteroid, ScoreEntity {
  private Health          health;
  private AudioSource     hitSound;
  private Animator        animator;

  public GameObject       gameOverText;
  public ScoreDisplayText scoreDisplay;
  public HighScores       highScores;
  public GameObject       explosion;
  public GameObject       healthBar;
  public GameObject       hitMarker;
  public SimpleHealthBar  greenBar;
  public SimpleHealthBar  blueBar;

  public bool dead = false;

  public int score { get; set; }

  void Start() {
    score    = 0;
    health   = GetComponent<Health>();
    animator = GetComponent<Animator>();
    hitSound = GetComponents<AudioSource>()[1];
  }

  public void scoreHit(Vector3 position, Bullet bullet) {
    var increase = bullet.pointsGiven;

    score += increase;

    scoreDisplay.displayScore(this);

    showHitMarker(position, bullet.pointsGiven);
  }

  public void showHitMarker(Vector3 position, int pointsGiven) {
    var marker = Instantiate(hitMarker, position, Quaternion.identity);

    var text = marker.GetComponent<TextMesh>();

    marker.GetComponent<HitMarker>().facedObject = gameObject;

    text.text = pointsGiven + "!";
  }

  public override void registerAsteroidHit() {
    if (GetComponent<Invincible>()) {
      return;
    }

    hitSound.Play();

    health.current--;

    greenBar.UpdateBar(health.current, health.maximum);
    blueBar.UpdateBar(health.current, health.maximum);

    if (health.current > 0) {
      // After a hit, you are invincible for a short while.
      var invincible = gameObject.AddComponent<Invincible>();

      invincible.onInvincibilityEndCallback = () => {
        animator.Play("Normal");
      };

      animator.Play("Blink");

      return;
    }

    StartCoroutine("die");
  }

  public IEnumerator die() {
    if (dead) {
      yield break;
    }

    dead = true;

    var rigidBody         = GetComponent<Rigidbody>();
    var explosionInstance = Instantiate(explosion, transform.position, Quaternion.identity) as GameObject;

    explosionInstance.transform.localScale *= 20;

    GetComponent<StartReactors>().destroyThrusters();

    animator.Play("Dead");

    rigidBody.constraints = RigidbodyConstraints.FreezeAll;

    Destroy(GetComponentInChildren<TrailRenderer>());
    Destroy(GetComponent<BulletShooter>());
    Destroy(GetComponent<WasdControl>());
    Destroy(GetComponent<MouseLook>());

    gameOverText.SetActive(true);

    yield return new WaitForSeconds(3.0f);

    highScores.registerScore(new FinalScore {
      points = score,
      name   = ""
    });

    SceneManager.LoadScene("High scores");
}
}
