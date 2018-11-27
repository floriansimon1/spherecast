using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

using OnSceneLoaded = UnityEngine.Events.UnityAction<
  UnityEngine.SceneManagement.Scene,
  UnityEngine.SceneManagement.LoadSceneMode
>;

public class DamageablePlayer: DamageableByAsteroid, ScoreEntity {
  private Health          health;
  private SimpleHealthBar blueBar;
  private AudioSource     hitSound;
  private SimpleHealthBar greenBar;
  private Animator        animator;

  public GameObject       gameOverText;
  public HighScores       highScores;
  public GameObject       explosion;
  public GameObject       healthBar;
  public GameObject       hitMarker;
  public ScoreDisplayText display;

  public int score { get; set; }

  void Start() {
    score    = 0;
    health   = GetComponent<Health>();
    animator = GetComponent<Animator>();
    hitSound = GetComponents<AudioSource>()[1];

    var bars = healthBar.GetComponentsInChildren<SimpleHealthBar>();

    greenBar = bars[0];
    blueBar  = bars[1];

    hitMarker.GetComponent<HitMarker>().facedObject = gameObject;
  }

  public void scoreHit(Vector3 position, Bullet bullet) {
    var increase = bullet.pointsGiven;

    score += increase;

    display.displayScore(this);

    showHitMarker(position, bullet.pointsGiven);
  }

  public void showHitMarker(Vector3 position, int pointsGiven) {
    var marker = Instantiate(hitMarker, position, Quaternion.identity);

    var text = marker.GetComponent<TextMesh>();

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

    /*
    * TODO: Find how to get rid the callback once it has run once.
    * For now, this leaks DamageablePlayer memory.
    */
    OnSceneLoaded callback = (scene, _) => {
      highScores.registerScore(new FinalScore {
        points = score,
        name   = "ACE"
      });
    };

    SceneManager.sceneLoaded += callback;

    SceneManager.LoadScene("High scores");
  }
}
