using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Player : MonoBehaviour
{



    #region variable
    public int playerLevel = 1;
    public int Level = 1;
    public int Exp;
    public float jumpForce = 2;
    public float jumpAccelereta = 0.5f;
    public float jumpAcceleretaDown = 0.2f;
    public float maxJumpAccelereta = 4;
    public float curAcceleretaJump = 0;
    public float CurAcceleretaSpeedForward;
    public float CurAcceleretaSpeedRight;
    public float SpeedAccelereta;
    public float MaxSpeedAccelereta;
    public bool jump = false;
    public bool max = false;
    public bool IsGrounded = false;
    public bool Moveforward;
    public bool MoveRight;
    public bool LastMoveForward;
    public bool LastMoveRight;
    public int maxBlock;
    public int curBlock;
    public bool CanGoOnNextLevel = false;
    public float Score;
    public float BonusScore;
    public int TimeBonus;
    public float TimeWithoutStop;
    public float TimeGame;
    public int TempBonusScore;
    public float TimeToDie;
    float BonusSpeed;
    float TimeSpeed;
    int chanceSpeedBonus = 1;
    string o1;
    public bool Win = false;
    bool PressEsc;
    public float ColorBonusScore;
    GUIStyle TextStile;
    GameObject select = null;
    Component select2;
    FieldInfo field;
    PropertyInfo property;
    GameObject Collision;
    List<GameObject> Levels = new List<GameObject>();
    Vector2 posmain1;
    Rect _rect1 = new Rect(0, 0, 200, 200);
    Vector2 posmain2;
    Rect _rect2 = new Rect(300, 300, 200, 200);
    Vector2 posmain3;
    Rect _rect3 = new Rect(300, 300, 200, 200);
    Vector2 posmain4;
    Rect _rect4 = new Rect(0, 0, 200, 200);
    #endregion

    #region methods
    void Start()
    {
        Cursor.visible = false;
        TextStile = new GUIStyle();
        TextStile.normal.textColor = Color.black;
        TextStile.fontSize = 25;
        int i = 1;
        while (true)
        {
            Levels.Add(GameObject.Find("Level" + i));
            if (i > 1)
            {
                if (Levels[i - 1])
                    Levels[i - 1].SetActive(false);
                else
                    break;
            }
            i++;
        }
       // LoadNextLevel();
        StartCoroutine(Spawn());
        TimeToDie = 10;
    }

    void Update()
    {
        Cursor.visible = PressEsc;
        if (Win || CanGoOnNextLevel)
        {
            Cursor.visible = true;
            return;
        }
        if (TimeToDie < 0)
            SceneManager.LoadScene(0);
        TimeToDie -= Time.deltaTime;
        TimeGame += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Escape))
            PressEsc = !PressEsc;
        Cursor.visible = false;
        if (transform.position.y <= -15)
            SceneManager.LoadScene(0);
        if (IsGrounded)
        {
            if (Input.GetKeyDown(KeyCode.Space))
                jump = true;
        }

        if (Moveforward = Input.GetKey(KeyCode.S))
        {
            if (CurAcceleretaSpeedForward >= -MaxSpeedAccelereta)
            {
                CurAcceleretaSpeedForward -= SpeedAccelereta;
            }
        }
        else if (Moveforward = Input.GetKey(KeyCode.W))
        {
            if (CurAcceleretaSpeedForward <= MaxSpeedAccelereta)
            {
                CurAcceleretaSpeedForward += SpeedAccelereta;
            }
        }

        if (MoveRight = Input.GetKey(KeyCode.D))
        {
            if (CurAcceleretaSpeedRight < MaxSpeedAccelereta)
            {
                CurAcceleretaSpeedRight += SpeedAccelereta;
            }
        }
        else if (MoveRight = Input.GetKey(KeyCode.A))
        {
            if (CurAcceleretaSpeedRight > -MaxSpeedAccelereta)
            {
                CurAcceleretaSpeedRight -= SpeedAccelereta;
            }
        }

        if (Exp >= playerLevel * 5)
        {
            Exp = 0;
            playerLevel++;
        }

        if (playerLevel > 7)
        {
            CanGoOnNextLevel = true;
            Score *= (180 - TimeGame) / 25;
        }
        if (TimeSpeed > 0)
        {
            BonusSpeed = 1.5f;
            TimeSpeed -= Time.deltaTime;
        }
        else BonusSpeed = 1;
    }

    void FixedUpdate()
    {
        if (Win || CanGoOnNextLevel)
            return;
        if (MoveRight && Moveforward)
        {
            CurAcceleretaSpeedForward *= 0.92f;
            CurAcceleretaSpeedRight *= 0.92f;
        }
        if (CurAcceleretaSpeedRight != 0 || CurAcceleretaSpeedForward != 0)
        {
            if (TimeBonus < 5)
            {
                TimeWithoutStop += Time.deltaTime;
            }
            if (playerLevel > 3)
                BonusScore += (TimeWithoutStop / ((7 - playerLevel - TimeBonus) <= 0 ? 1 : (7 - playerLevel - TimeBonus)) * (TimeSpeed > 0 ? 2 : 1)) / 15;
            if (TimeWithoutStop >= TimeBonus * 5 && TimeBonus < 5)
                TimeBonus++;
            transform.position += new Vector3(Camera.main.transform.forward.x * Time.deltaTime * 5 * playerLevel * CurAcceleretaSpeedForward * BonusSpeed,
                                           0, Camera.main.transform.forward.z * Time.deltaTime * 5 * playerLevel * CurAcceleretaSpeedForward * BonusSpeed);

            transform.position += new Vector3(Camera.main.transform.right.x * Time.deltaTime * 5 * playerLevel * CurAcceleretaSpeedRight * BonusSpeed,
                                           0, Camera.main.transform.right.z * Time.deltaTime * 5 * playerLevel * CurAcceleretaSpeedRight * BonusSpeed);
        }
        else
        {
            TimeBonus = 1;
            TimeWithoutStop = 0;
        }
        if (!MoveRight)
        {
            if (CurAcceleretaSpeedRight > 0)
            {
                LastMoveRight = true;
                CurAcceleretaSpeedRight -= SpeedAccelereta;
            }
            else if (CurAcceleretaSpeedRight < 0)
            {
                if (!LastMoveRight)
                    CurAcceleretaSpeedRight += SpeedAccelereta;
                else CurAcceleretaSpeedRight = 0;
            }
        }
        if (!Moveforward)
        {
            if (CurAcceleretaSpeedForward > 0)
            {
                LastMoveForward = true;
                CurAcceleretaSpeedForward -= SpeedAccelereta;
            }
            else if (CurAcceleretaSpeedForward < 0)
            {
                if (!LastMoveForward)
                    CurAcceleretaSpeedForward += SpeedAccelereta;
                else CurAcceleretaSpeedForward = 0;
            }
        }
        if (jump)
        {
            IsGrounded = false;
            if (curAcceleretaJump < maxJumpAccelereta && !max)
            {
                curAcceleretaJump += jumpAccelereta * jumpForce;
            }
            else if (curAcceleretaJump > 0)
            {
                max = true;
                curAcceleretaJump -= jumpAcceleretaDown;
            }
            else curAcceleretaJump = 0;
        }
        transform.position += new Vector3(0, curAcceleretaJump * Time.deltaTime, 0);

        if (curAcceleretaJump == 0)
        {
            max = false;
            jump = false;
          //  jump = true;


        }
        if (Collision != null && !jump)
        {
            Vector3 vec = Collision.transform.position;
            if (transform.position.y > Collision.transform.position.y)
                transform.position = new Vector3(transform.position.x, Collision.transform.position.y + transform.localScale.y, transform.position.z);
        }
    }

    private void LoadNextLevel()
    {
        if (Level + 1 > Levels.Count)
            Win = true;
        Levels[Level - 1].SetActive(false);
        playerLevel = 1;
        Exp = 0;
        Level++;
        Levels[Level - 1].SetActive(true);
        transform.position = new Vector3(0, 2.43f, 0);
        IsGrounded = false;
        CanGoOnNextLevel = false;
        foreach (var obj in GameObject.FindGameObjectsWithTag("Block"))
        {
            Destroy(obj);
        }
    }

    void OnTriggerEnter(Collider trigger)
    {
        if (!CanGoOnNextLevel)
        {
            if (trigger.name == "Bonus")
            {
                Score += 10 * TimeBonus;
                curBlock--;
                Exp++;
                TimeToDie = 10 - playerLevel;
                Destroy(trigger.gameObject);
            }
            else if (trigger.gameObject.name == "SpeedBonus")
            {
                Score += 30 * TimeBonus;
                TimeSpeed = 10f;
                Destroy(trigger.gameObject);
            }
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (!CanGoOnNextLevel)
        {
            if (other.gameObject.name == "LOSE")
                SceneManager.LoadScene(0);
            if (!IsGrounded && curAcceleretaJump == 0)
                IsGrounded = true;
            else IsGrounded = false;
            if (other.gameObject.name == "BLOCK")
                Collision = other.gameObject;
        }
    }
    #endregion

    IEnumerator Spawn()
    {
        if (Win)
            yield return null;
        if (curBlock < maxBlock && !CanGoOnNextLevel)
        {
            curBlock++;
            int a = UnityEngine.Random.Range(0, 100);
            int z = UnityEngine.Random.Range(-20, 20);
            int x = UnityEngine.Random.Range(-20, 20);
            GameObject spawn = GameObject.CreatePrimitive(PrimitiveType.Cube);
            spawn.transform.position = new Vector3(x, 0.5f, z);
            spawn.GetComponent<MeshRenderer>().material.color = new Color32((byte)UnityEngine.Random.Range(0, 255), (byte)UnityEngine.Random.Range(0, 255), (byte)UnityEngine.Random.Range(0, 255), 255);
            spawn.GetComponent<BoxCollider>().isTrigger = true;
            spawn.GetComponent<BoxCollider>().size = new Vector3(0.95f, 0.95f, 0.95f);
            spawn.AddComponent<CheckCollision>();
            spawn.AddComponent<Rigidbody>();
            spawn.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            spawn.tag = "Block";
            if (a <= chanceSpeedBonus)
            {
                spawn.AddComponent<ParticleSystem>();
                spawn.name = "SpeedBonus";
            }
            else spawn.name = "Bonus";
        }
        yield return new WaitForSeconds(1);
        StartCoroutine(Spawn());
    }

    #region gui
    void OnGUI()
    {
        GUILayout.Label("Level : " + Level, TextStile);
        GUILayout.Label("Player Level : " + playerLevel + "/8", TextStile);
        GUILayout.Label("EXP : " + Exp + "/" + playerLevel * 5, TextStile);
        GUILayout.Label("Score :" + (int)Score, TextStile);
        GUILayout.Label("Time Life :" + (int)TimeToDie, TextStile);
        if ((int)BonusScore > 0)
        {
            ColorBonusScore = 1;
            TextStile.normal.textColor = Color.yellow;
            GUI.Label(new Rect(90 + (12 * (int)Score.ToString().Length), 83.5f, 100, 0), "  +" + (int)BonusScore, TextStile);
            TextStile.normal.textColor = Color.black;
        }
        if (TimeWithoutStop < 1 && ColorBonusScore > 0)
        {
            if (TempBonusScore == 0)
               TempBonusScore = (int)BonusScore;
            if ((int)BonusScore > 0)
            {
                float num = TempBonusScore / 5 * Time.deltaTime;
                if (num == 0)
                    num = 1;
                BonusScore -= num;
                Score += num;
                if (BonusScore < 0)
                    BonusScore = 0;
            }
            else {
                TempBonusScore = 0;
                ColorBonusScore -= 0.02f;
                if (TextStile.normal.textColor.a > 0)
                    TextStile.normal.textColor = new Color(1f, 0.92f, 0.016f, ColorBonusScore);
                GUI.Label(new Rect(90 + (12 * (int)Score.ToString().Length), 83.5f, 100, 0), "  +" + (int)BonusScore, TextStile);
                TextStile.normal.textColor = Color.black;
            }
        }
        
        if (Win)
        {
            TextStile.normal.textColor = Color.white;
            TextStile.fontSize = 35;
            GUI.Box(new Rect(Screen.width / 2 - 200, Screen.height / 2 - 100, 
                             Screen.width / 2 * 0.5f, Screen.height / 2 * 0.5f), "");
            GUI.Label(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 50, 100, 100), "You Win", TextStile);
            TextStile.fontSize = 25;
            TextStile.normal.textColor = Color.black;
        }
        else if (CanGoOnNextLevel)
        {
            TextStile.normal.textColor = Color.white;
            TextStile.fontSize = 35;
            GUI.Box(new Rect(Screen.width / 2 - 200, Screen.height / 2 - 100,
                             Screen.width / 2 * 0.5f, Screen.height / 2 * 0.5f), "");
            if (GUI.Button(new Rect(Screen.width / 2 - 120, Screen.height / 2 - 40,
                                    Screen.width / 2 * 0.5f, Screen.height / 2 * 0.5f), "Next Level", TextStile))
                LoadNextLevel();
            TextStile.fontSize = 25;
            TextStile.normal.textColor = Color.black;
        }
      //  if (Win)

        //  _rect4 = GUI.Window(0, _rect4, Main1, "");
        //if (select)
        //    _rect1 = GUI.Window(1, _rect1, Main2, "");
        //if (select2)
        //    _rect2 = GUI.Window(2, _rect2, Main3, "");
        //if (field != null || property != null)
        //    _rect3 = GUI.Window(3, _rect3, Main4, "");
    }
    
    void Main1(int id)
    {

        //posmain1 = GUILayout.BeginScrollView(posmain1);
        //foreach (var obj in FindObjectsOfType(typeof(GameObject)))
        //{
        //    if (GUILayout.Button(obj.name))
        //    {
        //        select = obj as GameObject;
        //    }
        //}
        //GUILayout.EndScrollView();
    }

    void Main2(int id)
    {
        posmain2 = GUILayout.BeginScrollView(posmain2);
        foreach (var obj in select.GetComponents<Component>())
        {
            if (GUILayout.Button(obj.GetType().Name))
            {
                select2 = obj as Component;
            }
        }
        GUILayout.EndScrollView();
    }

    void Main3(int id)
    {
        posmain3 = GUILayout.BeginScrollView(posmain3);
        foreach (var obj in select2.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
        {
            if (GUILayout.Button(obj.Name))
            {
                field = obj;
                property = null;
            }
        }
        foreach (var obj in select2.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
        {
            if (GUILayout.Button(obj.Name))
            {
                property = obj;
                field = null;
            }
        }
        GUILayout.EndScrollView();
    }

    void Main4(int id)
    {
        posmain4 = GUILayout.BeginScrollView(posmain4);
        object obj = null;
        try
        {
            if (field != null)
                obj = field.GetValue(select);
            else obj = property.GetValue(select);
        
        GUILayout.Label(obj.GetType().Name);
        o1 = GUILayout.TextField(o1);
        if (GUILayout.Button("Set Value"))
        {
            switch (obj.GetType().Name)
            {
                case "System.Single":
                    if (field != null)
                        field.SetValue(select, float.Parse(o1));
                    else property.SetValue(select, float.Parse(o1), null);
                    break;
                case "System.String":
                    if (field != null)
                        field.SetValue(select, o1);
                    else property.SetValue(select, o1, null);
                    break;
                case "System.Int32":
                    if (field != null)
                        field.SetValue(select, int.Parse(o1));
                    else property.SetValue(select, int.Parse(o1), null);
                    break;
                default:
                    break;
            }
        }
        }
        catch { }
        GUILayout.EndScrollView();

    }

    #endregion
}
