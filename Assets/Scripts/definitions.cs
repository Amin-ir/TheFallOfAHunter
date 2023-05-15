using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace Assets.Scripts
{
    public enum SwipeDirection
    {
        up, right, down, left, none
    }
    public enum Styles
    {
        idol, axe, sword
    };
    public enum PuzzleType  
    { 
        gate,fallingSprites,spinningSaw
    }
    public static class CustomTools
    {
        public readonly static float init_maxHealth = 100, init_HFist = 1f, init_LFist = 2.5f,
            init_HSword = 7, init_LSword = 9, init_LAxe = 3f, init_HAxe = 5f,init_throwingAxe = 7f;
        public static Dictionary<int, float> upgrades = new Dictionary<int, float>()
        {
            {0, 0f },
            {1,.10f},
            {2,.15f},
            {3,.25f },
            {4, .30f },
            {5, .35f }
        };
        public static int oppositeDirection(Transform objSelf, float distance)
        {
            if (Mathf.Sign(objSelf.localScale.x * distance) > 0)
                return -1; // localScale must change
            else return 1; // localScale doesn't need to change;
        }
        public static void StoreGameSavingParameters
            (int level,
            int checkpoint,
            int xp,
            int armorUpgradeLevel,
            int swordUpgradeLevel,
            int axeUpgradeLevel,
            int arrow)
        {
            PlayerPrefs.SetInt("xp", xp);
            PlayerPrefs.SetInt("level", level);
            PlayerPrefs.SetInt("checkpoint", checkpoint);
            PlayerPrefs.SetInt("armorUpgrade",armorUpgradeLevel);
            PlayerPrefs.SetInt("swordUpgrade",swordUpgradeLevel);
            PlayerPrefs.SetInt("axeUpgrade",axeUpgradeLevel);
            PlayerPrefs.SetInt("arrow",arrow);
        }
        public static void FetchNextLevelInstantly(){
            int nextLevelIndex = SceneManager.GetActiveScene().buildIndex + 1;
            StoreGameSavingParameters
            (   nextLevelIndex,
                0,
                PlayerPrefs.GetInt("xp"),
                PlayerPrefs.GetInt("armorUpgrade"),
                PlayerPrefs.GetInt("swordUpgrade"),
                PlayerPrefs.GetInt("axeUpgrade"),
                PlayerPrefs.GetInt("arrow"));
            SceneManager.LoadScene(nextLevelIndex);
        }
        public static SwipeDirection DetectSwipe(Vector2 start, Vector2 end)
        {
            var direction = end - start;
            if (Mathf.Abs(direction.y) > Mathf.Abs(direction.x))
                if (direction.y > 0)
                    return SwipeDirection.up;
                else return SwipeDirection.down;
            else if (direction.x > 0)
                return SwipeDirection.right;
                else return SwipeDirection.left;
        }
        public static void PrintPlayerPrefs()
        {
            string[] PlayerPrefsKeys = { "xp", "arrow", "axeUpgrade", "swordUpgrade", "armorUpgrade", "level", "checkpoint" };
            for (int i = 0; i < PlayerPrefsKeys.Length; i++)
            {
                if (PlayerPrefs.HasKey(PlayerPrefsKeys[i]))
                    Debug.Log(PlayerPrefsKeys[i] + " : " + PlayerPrefs.GetInt(PlayerPrefsKeys[i]));
                else Debug.Log(PlayerPrefsKeys[i] + " does not exist");
            }
        }
        public static GameObject GetAscendantParent(this GameObject obj, int depth){
            GameObject target = obj;
            for (int i = 0; i < depth; i++)
                target = target.transform.parent.gameObject;
            return target;
        }
    }
    [System.Serializable]
    public class codexPage
    {
        public string Title;
        public string content;
        public Sprite additionalImage;
    }
}
