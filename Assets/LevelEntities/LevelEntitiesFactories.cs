using System.Collections.Generic;
using UnityEngine;

using LevelEntityFactoriesDictionary = System.Collections.Generic.Dictionary<
  LevelEntityType,
  System.Func<UnityEngine.GameObject>
>;

public class LevelEntitiesFactories {
  // Not meant to be instantiated.
  private LevelEntitiesFactories() {}

  public static LevelEntityFactoriesDictionary make(GameObject asteroidPrefab, GameObject blackHolePrefab) {
    var factory = new LevelEntityFactoriesDictionary();

    factory[LevelEntityType.Asteroid] = () => {
      var asteroid = GameObject.Instantiate(asteroidPrefab);

      return asteroid;
    };

    factory[LevelEntityType.BlackHole] = () => {
      var blackHole = GameObject.Instantiate(blackHolePrefab);

      return blackHole;
    };

    return factory;
  }
}
