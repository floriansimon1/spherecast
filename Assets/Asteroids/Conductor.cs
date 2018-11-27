using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Conductor: MonoBehaviour {
  public List<Level>        levels;
  public GameObject         player;
	public AudioSource        hitSound;
	public AudioSource        explosionSound;
  public GameObject         asteroidPrefab;
  public GameObject         blackHolePrefab;
  public WaveMessageDisplay waveMessageDisplay;

  public GameObject levelEntitiesRoot;
  public GameObject levelEntitiesClonesRoot;

  public int waveNumber = 1;

  private Level currentLevel;
  private int   remainingVitalEntities;

  public void onVitalEntityDestruction() {
    remainingVitalEntities--;

    if (remainingVitalEntities == 0) {
      StartCoroutine("nextLevel");
    }
  }

  public void Awake() {
    var damageableAsteroid = asteroidPrefab.GetComponent<DamageableAsteroid>();

    damageableAsteroid.conductor      = this;
    damageableAsteroid.hitSound       = hitSound;
    damageableAsteroid.explosionSound = explosionSound;

    levels = Levels.make(
      LevelEntitiesFactories.make(asteroidPrefab, blackHolePrefab)
    );

    foreach (var level in levels) {
      foreach (var entity in level.entities) {
        entity.transform.parent = levelEntitiesRoot.transform;
      }
    }

    configureScene(waveNumber);
  }

  public void configureScene(int waveNumber) {
    if (currentLevel != null) {
      currentLevel.dispose();
    }

    currentLevel = levels[(waveNumber - 1) %  levels.Count];

    remainingVitalEntities = currentLevel.vitalEntities;

    player.transform.position         = currentLevel.playerPosition;
    player.transform.localEulerAngles = Vector3.zero;

    player.GetComponent<Rigidbody>().velocity = Vector3.zero;

    currentLevel.activate(levelEntitiesClonesRoot.transform);
  }

  IEnumerator nextLevel() {
    waveNumber++;

    yield return new WaitForSeconds(3.0f);

    waveMessageDisplay.showLevel(waveNumber);

    Time.timeScale = 0.0f;

    yield return new WaitForSecondsRealtime(3.0f);

    configureScene(waveNumber);

    Time.timeScale = 1.0f;
  }
}
