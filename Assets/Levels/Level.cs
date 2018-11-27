using System.Collections.Generic;
using UnityEngine;

public class Level {
  public List<GameObject> entities;
  public List<GameObject> clones;
  
  public int     vitalEntities  = 0;
  public Vector3 playerPosition = Facts.origin;

  public Level() {
    entities = new List<GameObject>();
    clones   = new List<GameObject>();
  }

  public void dispose() {
    foreach (var entity in clones) {
      if (entity.gameObject) {
        GameObject.Destroy(entity.gameObject);
      }
    }
  }

  public void activate(Transform parent) {
    clones.Clear();

    foreach (var entity in entities) {
      var copy = GameObject.Instantiate(entity);

      clones.Add(copy);

      copy.transform.parent = parent;

      var rigidBody = entity.GetComponent<Rigidbody>();

      if (rigidBody) {
        copy.GetComponent<Rigidbody>().velocity = rigidBody.velocity;
      }

      copy.SetActive(true);
    }
  }

  public void registerVitalEntity(LevelEntity entity) {
    entities.Add(entity.gameObject);

    vitalEntities++;

    entity.isVital = true;
  }
}
