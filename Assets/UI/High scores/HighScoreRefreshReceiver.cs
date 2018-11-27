using UnityEngine.EventSystems;
using System.Collections.Generic;

public interface HighScoreRefreshReceiver: IEventSystemHandler {
  void refreshScores(List<FinalScore> scores);
}
