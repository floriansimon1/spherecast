using System;
using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

using OnSceneLoaded = UnityEngine.Events.UnityAction<
  UnityEngine.SceneManagement.Scene,
  UnityEngine.SceneManagement.LoadSceneMode
>;

public class Conductor: MonoBehaviour {
  public List<Level>        levels;
	public AudioSource        hitSound;
  public HighScores         highScores;
  public GameObject         playerPrefab;
	public AudioSource        explosionSound;
  public GameObject         asteroidPrefab;
  public GameObject         blackHolePrefab;
  public WaveMessageDisplay waveMessageDisplay;

  public GameObject bulletsContainer;
  public GameObject levelEntitiesRoot;
  public GameObject levelEntitiesClonesRoot;

  public int waveNumber = 1;

  private GameObject player;
  private Level      currentLevel;
  private int        remainingVitalEntities;

  private static Conductor instance;

  public void onVitalEntityDestruction() {
    remainingVitalEntities--;

    if (remainingVitalEntities == 0) {
      StartCoroutine("nextLevel");
    }
  }

  public void updateHighScores() {
    var highScoresDisplay = findSceneObject("High scores table").GetComponent<HighScoresDisplay>();

    highScoresDisplay.refreshScores(highScores.scores);
  }

  public void clearGameScene() {
    GameObject[] containers = {
      levelEntitiesClonesRoot,
      levelEntitiesRoot,
      bulletsContainer
    };

    foreach (var container in containers) {
      foreach (Transform child in container.transform) {
        Destroy(child.gameObject);
      }
    }
  }

  public void Awake() {
    if (!instance) {
      instance = this;

      highScores = GetComponent<HighScores>();

      OnSceneLoaded callback = (scene, _) => {
        clearGameScene();

        if (scene.name == "Game scene") {
          startGame();
        } else if (scene.name == "High scores") {
          updateHighScores();
        }
      };

      SceneManager.sceneLoaded += callback;

      return;
    } else {
      Destroy(gameObject);

      return;
    }
  }

  private void injectPlayerDependencies(GameObject playerObject, ObjectsRegister register) {
    var damageablePlayer = playerObject.GetComponent<DamageablePlayer>();
    var bulletShooter    = playerObject.GetComponent<BulletShooter>();

    damageablePlayer.highScores = highScores;

    bulletShooter.bulletsContainer = bulletsContainer;

    bulletShooter.detectDependencies();

    damageablePlayer.scoreDisplay = register.scoreDisplay;
    damageablePlayer.gameOverText = register.gameOverText;
    damageablePlayer.greenBar     = register.greenBar;
    damageablePlayer.blueBar      = register.blueBar;
  }

  private GameObject findSceneObject(string name) {
    return SceneManager
    .GetActiveScene()
    .GetRootGameObjects()
    .Where(rootGameObject => rootGameObject.name == name)
    .First();
  }

  public void startGame() {
    waveNumber = 1;

    var register = findSceneObject("Objects register").GetComponent<ObjectsRegister>();

    waveMessageDisplay = register.waveMessageDisplay;

    player = Instantiate(playerPrefab);

    injectPlayerDependencies(player, register);

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
