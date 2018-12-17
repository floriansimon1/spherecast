using System.Collections.Generic;
using UnityEngine;

using LevelEntityFactoriesDictionary = System.Collections.Generic.Dictionary<
  LevelEntityType,
  System.Func<UnityEngine.GameObject>
>;

public class Levels {
  // Not meant to be instantiated.
  private Levels() {}

  private static Level makeLevel1(LevelEntityFactoriesDictionary levelEntitiesFactories) {
    var level = new Level();

    var simpleAsteroid = levelEntitiesFactories[LevelEntityType.Asteroid]();

    var simpleAsteroidLevelEntity = simpleAsteroid.GetComponent<LevelEntity>();

    var blackHole = levelEntitiesFactories[LevelEntityType.BlackHole]();

    blackHole.transform.position = Vector3.zero;

    simpleAsteroid.transform.position = new Vector3(0.0f, 0.0f, 100.0f);

    simpleAsteroid.GetComponent<Rigidbody>().velocity = Utils.makeVelocity(new Vector3(0.0f, 0.0f, -1.0f), 20.0f);

    simpleAsteroidLevelEntity.isVital = true;

    simpleAsteroid.transform.localScale = Vector3.one * 50.0f;

    simpleAsteroid.GetComponent<Health>().setMaximum(2);

    level.registerVitalEntity(simpleAsteroidLevelEntity);
    level.entities.Add(blackHole);

    return level;
  }

  private static Level makeLevel2(LevelEntityFactoriesDictionary levelEntitiesFactories) {
    var level = new Level();

    var simpleAsteroid = levelEntitiesFactories[LevelEntityType.Asteroid]();
    var blackHole      = levelEntitiesFactories[LevelEntityType.BlackHole]();

    blackHole.transform.position = new Vector3(150.0f, 150.0f, 150.0f);

    var simpleAsteroidLevelEntity = simpleAsteroid.GetComponent<LevelEntity>();

    simpleAsteroid.transform.position = new Vector3(0.0f, 10.0f, 400.0f);

    simpleAsteroid.GetComponent<Rigidbody>().velocity = Utils.makeVelocity(new Vector3(0.0f, -0.5f, -1.0f), 40.0f);

    simpleAsteroidLevelEntity.isVital = true;

    simpleAsteroid.transform.localScale = Vector3.one * 50.0f;

    simpleAsteroid.GetComponent<Health>().setMaximum(2);

    level.registerVitalEntity(simpleAsteroidLevelEntity);
    level.entities.Add(blackHole);

    return level;
  }

  public static List<Level> make(LevelEntityFactoriesDictionary levelEntitiesFactories) {
    var levels = new List<Level>();

    levels.Add(Levels.makeLevel1(levelEntitiesFactories));
    levels.Add(Levels.makeLevel2(levelEntitiesFactories));

    foreach (var level in levels) {
      foreach (var entity in level.entities) {
        entity.SetActive(false);
      }
    }

    return levels;
  }
}
