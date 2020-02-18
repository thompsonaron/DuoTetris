using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

public class HighScore : MonoBehaviour
{
    public List<PlayerScore> playerScores;

    // Start is called before the first frame update
    void Start()
    {
        playerScores = new List<PlayerScore>();
    }

    [Serializable]
    public class PlayerScores
    {
        public List<PlayerScore> playerScores;

        public PlayerScores()
        {
            playerScores = new List<PlayerScore>();
        }
    }
    [Serializable]
    public class PlayerScore
    {
        public string nickname;
        public int score;

        public PlayerScore(string nickname, int score)
        {
            this.nickname = nickname;
            this.score = score;
        }
        public override string ToString()
        {
            return nickname + "   " + score.ToString();
        }
    }

    public void AddPlayerScore(string nick, int score)
    {
        

        PlayerScores ps;
        if (File.Exists("highscore"))
        {
            string jsonString = File.ReadAllText("highscore");
            if (jsonString == string.Empty)
            {
                ps = new PlayerScores();
            }
            else
            {
                ps = JsonUtility.FromJson<PlayerScores>(jsonString);
            }
        }
        else
        {
            File.WriteAllText("highsore", "");
            ps = new PlayerScores();
        }
        
        ps.playerScores.Add(new PlayerScore(nick, score));
        string json = JsonUtility.ToJson(ps);
        File.WriteAllText("highscore", json);
    }

    public string LoadData()
    {
        string jsonString = File.ReadAllText("highscore");
        PlayerScores hs = JsonUtility.FromJson<PlayerScores>(jsonString);
        PlayerScore[] highScores = hs.playerScores.OrderByDescending(x => x.score).ToArray();
        StringBuilder holder = new StringBuilder();
        for (int i = 0; i < highScores.Length; i++)
        {
            holder.AppendLine(highScores[i].ToString());
        }
        return holder.ToString();

    }


}
