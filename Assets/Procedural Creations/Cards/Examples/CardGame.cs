using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class CardGame : MonoBehaviour
{
    //体力最大值为4，目前规则下是这样子的
    #region 常量定义
    const int Tili_Max = 4;
    const int Button_Max = 8;
    const int Xuanjiang_Max = 3;
    const int Shoupai_Max = 23;
    const float Shoupai_Juli = 2.5f;
    const int Shoupaiperround = 2;
    const float FlyTime = 0.4f;
    const float DealTime = 0.4f;
    const float StayTime = 2f;
    #endregion

    public CardDeck Deck;
    //List<CardDefinition> m_deck = new List<CardDefinition>();

    //已经发的武将牌的数量
    int Wujiang_num;
    int Player_Num = 2;
    int Huihe_Num;
    int Card_Select;
    int Jiesuan_Num = 0;
    bool Player1_LebusishuSign = false;
    bool Player1_BingliangcunduanSign = false;
    bool Player2_LebusishuSign = false;
    bool Player2_BingliangcunduanSign = false;
    bool Player1_ShaSign = false;

    #region 三国杀游戏三层状态机定义，以及结算的状态机
    /*
     * 状态机分为三层
     * 第一层：游戏状态
     * 分为 无效状态，选择武将初始化，正常游戏，游戏结束，阵营1获胜，阵营2获胜，阵营3获胜,解决状态
     * 解释一下，这里的解决状态是参考了这份代码的定义，感觉好像如果在进行一些动画效果的时候要进行锁定
     * 第二层：回合状态
     * 分为 无效回合，玩家1回合，玩家2回合，玩家3回合，玩家4回合，玩家5回合，玩家6回合，玩家7回合，玩家8回合
     * 第三层：回合中阶段状态
     * 分为 无效阶段，准备阶段，判定阶段，摸牌阶段，出牌阶段，弃牌阶段，结束阶段
     */
    enum Sanguosha_GameState
    {
        State_Invalid,
        State_ChooseWujiang,
        State_Playing,
        State_Group1Wins,
        State_Group2Wins,
        State_Group3Wins,
        State_Resolving_Player1,
        State_Resolving_Player2,
        State_Resolving,
    };

    enum Sanguosha_Round
    {
        Round_Invalid,
        Round_Player1,
        Round_Player2,
        Round_Player3,
        Round_Player4,
        Round_Player5,
        Round_Player6,
        Round_Player7,
        Round_Player8,
    };

    enum Sanguosha_Stage
    {
        Stage_Invalid,
        Stage_Zhunbei,
        Stage_Panding,
        Stage_Mopai,
        Stage_Chupai,
        Stage_Qipai,
        Stage_Jieshu,
    };

    enum Sanguosha_Jiesuan
    {
        Jiesuan_None,
        Jiesuan_Panding,
        Jiesuan_Sha,
        Jiesuan_Shan,
        Jiesuan_Tao,
        Jiesuan_Juedou,
        Jiesuan_Wanjianqifa,
        Jiesuan_Nanmanruqin,
        Jiesuan_Guohechaiqiao,
        Jiesuan_Shunshouqianyang,
        Jiesuan_Wuzhongshengyou,
        Jiesuan_Wuxiekeji,
        Jiesuan_Lebusishu,
        Jiesuan_Bingliangcunduan,
        Jiesuan_Zhuangbei,
    }

    #endregion

    #region 状态机变量定义
    Sanguosha_GameState game_state;
    Sanguosha_Round game_round;
    Sanguosha_Stage game_stage;
    Sanguosha_Jiesuan game_jiesuan;

    Sanguosha_GameState preState;
    Sanguosha_Round preRound;
    Sanguosha_Stage preStage;
    Sanguosha_Jiesuan preJiesuan;
    #endregion

    #region 玩家手牌列表和选将框列表定义
    //玩家选将列表武将卡牌
    List<Card> Xuanjiang_Cards = new List<Card>();
    //存放使用过的手牌
    List<Card> Used_Cards = new List<Card>();
    //玩家1手中持有的卡牌
    List<Card> Player1_Cards = new List<Card>();
    //玩家2手中持有的卡牌
    List<Card> Player2_Cards = new List<Card>();
    //玩家3手中持有的卡牌
    List<Card> Player3_Cards = new List<Card>();
    //玩家4手中持有的卡牌
    List<Card> Player4_Cards = new List<Card>();
    //玩家5手中持有的卡牌
    List<Card> Player5_Cards = new List<Card>();
    //玩家6手中持有的卡牌
    List<Card> Player6_Cards = new List<Card>();
    //玩家7手中持有的卡牌
    List<Card> Player7_Cards = new List<Card>();
    //玩家8手中持有的卡牌
    List<Card> Player8_Cards = new List<Card>();
    #endregion

    #region 定义静态Button
    GameObject[] Sanguosha_Buttons;
    GameObject[] Sanguosha_Xuanjiang;
    GameObject[] Sanguosha_Shoupai;
    #endregion

    #region 定义静态文本Text
    //消息提示区域
    GameObject Sanguosha_Text_Message;
    //玩家1判定区域
    GameObject Sanguosha_Text_Player1_Panding1;
    GameObject Sanguosha_Text_Player1_Panding2;
    //玩家1装备区域
    GameObject Sanguosha_Text_Player1_Zhuangbei1;
    GameObject Sanguosha_Text_Player1_Zhuangbei2;
    //玩家1手牌区域
    GameObject Sanguosha_Text_Player1_Shoupai;
    //玩家1技能区域
    GameObject Sanguosha_Text_Player1_Jineng1;
    GameObject Sanguosha_Text_Player1_Jineng2;
    //玩家2判定区域
    GameObject Sanguosha_Text_Player2_Panding1;
    GameObject Sanguosha_Text_Player2_Panding2;
    //玩家2装备区域
    GameObject Sanguosha_Text_Player2_Zhuangbei1;
    GameObject Sanguosha_Text_Player2_Zhuangbei2;
    //玩家2手牌区域
    GameObject Sanguosha_Text_Player2_Shoupai;
    //玩家2技能区域
    GameObject Sanguosha_Text_Player2_Jineng1;
    GameObject Sanguosha_Text_Player2_Jineng2;
    #endregion

    #region 定义静态image
    GameObject[] Sanguosha_Image_Player1_Tili;
    GameObject[] Sanguosha_Image_Player2_Tili;
    #endregion

    #region 定义存储玩家和电脑武将状态变量
    //玩家1区域字符串定义
    string string_player1_wujiang;
    string string_player1_panding1;
    string string_player1_panding2;
    string string_player1_tili;
    string string_player1_wuqi;
    string string_player1_fangju;
    string string_player1_jineng1;
    string string_player1_jineng2;
    //玩家1手牌血量定义
    /*
     * player1_manxue:武将最一开始的满血状态
     * player1_tili:武将当前血量
     * player1_shoupaishangxian:武将当前手牌数量上限，等于武将当前血量
     * player1_shoupai:武将当前手牌数量
     */
    int player1_manxue;
    int player1_tili;
    int player1_shoupaishangxian;
    int player1_shoupai;
    //玩家2区域字符串定义
    string string_player2_wujiang;
    string string_player2_panding1;
    string string_player2_panding2;
    string string_player2_tili;
    string string_player2_wuqi;
    string string_player2_fangju;
    string string_player2_jineng1;
    string string_player2_jineng2;
    //玩家2手牌血量定义
    int player2_manxue;
    int player2_tili;
    int player2_shoupaishangxian;
    int player2_shoupai;
    #endregion

    // Use this for initialization
    void Start()
    {
        //设置全屏
        Screen.SetResolution(1366, 768, false);
        //设置初始状态
        #region 设置初始状态为 游戏状态：无效状态 游戏回合：无效回合 游戏阶段：无效阶段
        game_state = Sanguosha_GameState.State_Invalid;
        game_round = Sanguosha_Round.Round_Invalid;
        game_stage = Sanguosha_Stage.Stage_Invalid;
        #endregion

        //Deck的意思应该就是卡牌的意思，但是这个初始化函数好像什么都没干
        //初始化牌堆信息
        Deck.Initialize();

        Wujiang_num = 0;

        Huihe_Num = 0;

        #region 初始化存放静态按键的Sanguosha_Buttons数组
        Sanguosha_Buttons = new GameObject[Button_Max];
        Sanguosha_Buttons[0] = this.transform.Find("Restart").gameObject;
        Sanguosha_Buttons[1] = this.transform.Find("Queding").gameObject;
        Sanguosha_Buttons[2] = this.transform.Find("Quxiao").gameObject;
        Sanguosha_Buttons[3] = this.transform.Find("Jieshu").gameObject;
        Sanguosha_Buttons[4] = this.transform.Find("Jineng1").gameObject;
        Sanguosha_Buttons[5] = this.transform.Find("Jineng2").gameObject;
        Sanguosha_Buttons[6] = this.transform.Find("Player2_Zhuangbei1_Button").gameObject;
        Sanguosha_Buttons[7] = this.transform.Find("Player2_Zhuangbei2_Button").gameObject;
        #endregion

        #region 初始化存放选将按钮的Sanguosha_Xuanjiang数组
        Sanguosha_Xuanjiang = new GameObject[Xuanjiang_Max];
        Sanguosha_Xuanjiang[0] = this.transform.Find("Wujiang1").gameObject;
        Sanguosha_Xuanjiang[1] = this.transform.Find("Wujiang2").gameObject;
        Sanguosha_Xuanjiang[2] = this.transform.Find("Wujiang3").gameObject;
        #endregion

        #region 初始化存放卡牌button的Sanguosha_Shoupai数组
        Sanguosha_Shoupai = new GameObject[Shoupai_Max];
        for(int i = 0; i < Shoupai_Max; ++i)
        {
            Sanguosha_Shoupai[i] = this.transform.Find("Shoupai" + i.ToString()).gameObject;
        }
        #endregion

        #region 初始化游戏中的文本对象
        //游戏消息提示区域
        Sanguosha_Text_Message = this.transform.Find("TishiMessage").gameObject;
        //玩家1区域的文本对象
        Sanguosha_Text_Player1_Panding1 = this.transform.Find("Player1_Panding1").gameObject;
        Sanguosha_Text_Player1_Panding2 = this.transform.Find("Player1_Panding2").gameObject;
        Sanguosha_Text_Player1_Zhuangbei1 = this.transform.Find("Player1_Zhuangbei1").gameObject;
        Sanguosha_Text_Player1_Zhuangbei2 = this.transform.Find("Player1_Zhuangbei2").gameObject;
        Sanguosha_Text_Player1_Shoupai = this.transform.Find("Player1_Shoupai").gameObject;
        Sanguosha_Text_Player1_Jineng1 = this.transform.Find("Jineng1/Text").gameObject;
        Sanguosha_Text_Player1_Jineng2 = this.transform.Find("Jineng2/Text").gameObject;
        //玩家2区域的文本对象
        Sanguosha_Text_Player2_Panding1 = this.transform.Find("Player2_Panding1").gameObject;
        Sanguosha_Text_Player2_Panding2 = this.transform.Find("Player2_Panding2").gameObject;
        Sanguosha_Text_Player2_Zhuangbei1 = this.transform.Find("Player2_Zhuangbei1").gameObject;
        Sanguosha_Text_Player2_Zhuangbei2 = this.transform.Find("Player2_Zhuangbei2").gameObject;
        Sanguosha_Text_Player2_Shoupai = this.transform.Find("Player2_Shoupai").gameObject;
        Sanguosha_Text_Player2_Jineng1 = this.transform.Find("Player2_Jineng1").gameObject;
        Sanguosha_Text_Player2_Jineng2 = this.transform.Find("Player2_Jineng2").gameObject;
        #endregion

        #region 初始化游戏中的image对象
        Sanguosha_Image_Player1_Tili = new GameObject[Tili_Max];
        Sanguosha_Image_Player2_Tili = new GameObject[Tili_Max];
        Sanguosha_Image_Player1_Tili[0] = this.transform.Find("Canvas/Player1_Tili_1").gameObject;
        Sanguosha_Image_Player1_Tili[1] = this.transform.Find("Canvas/Player1_Tili_2").gameObject;
        Sanguosha_Image_Player1_Tili[2] = this.transform.Find("Canvas/Player1_Tili_3").gameObject;
        Sanguosha_Image_Player1_Tili[3] = this.transform.Find("Canvas/Player1_Tili_4").gameObject;
        Sanguosha_Image_Player2_Tili[0] = this.transform.Find("Canvas/Player2_Tili_1").gameObject;
        Sanguosha_Image_Player2_Tili[1] = this.transform.Find("Canvas/Player2_Tili_2").gameObject;
        Sanguosha_Image_Player2_Tili[2] = this.transform.Find("Canvas/Player2_Tili_3").gameObject;
        Sanguosha_Image_Player2_Tili[3] = this.transform.Find("Canvas/Player2_Tili_4").gameObject;
        #endregion

        #region 将除了重新开始游戏button以外的image，text和button全部设置为不启用
        //所有text都不启用
        Sanguosha_Text_Message.SetActive(false);
        Sanguosha_Text_Player1_Panding1.SetActive(false);
        Sanguosha_Text_Player1_Panding2.SetActive(false);
        Sanguosha_Text_Player1_Zhuangbei1.SetActive(false);
        Sanguosha_Text_Player1_Zhuangbei2.SetActive(false);
        Sanguosha_Text_Player1_Shoupai.SetActive(false);
        Sanguosha_Text_Player2_Panding1.SetActive(false);
        Sanguosha_Text_Player2_Panding2.SetActive(false);
        Sanguosha_Text_Player2_Zhuangbei1.SetActive(false);
        Sanguosha_Text_Player2_Zhuangbei2.SetActive(false);
        Sanguosha_Text_Player2_Shoupai.SetActive(false);
        Sanguosha_Text_Player2_Jineng1.SetActive(false);
        Sanguosha_Text_Player2_Jineng2.SetActive(false);
        //除了重新开始以外所有功能button都不启用
        for (int i = 1; i < Sanguosha_Buttons.Length; ++i)
        {
            Sanguosha_Buttons[i].SetActive(false);
        }
        //所有image都不启用
        for (int i = 0; i < Tili_Max; ++i)
        {
            Sanguosha_Image_Player1_Tili[i].SetActive(false);
            Sanguosha_Image_Player2_Tili[i].SetActive(false);
        }
        //三个选将框设置为不启用
        for (int i = 0; i < Xuanjiang_Max; ++i)
        {
            Sanguosha_Xuanjiang[i].SetActive(false);
        }
        //所有的手牌按键设置为不启用
        for(int i = 0; i < Shoupai_Max; ++i)
        {
            Sanguosha_Shoupai[i].SetActive(false);
        }
        #endregion
        
    }
    // Update is called once per frame
    //就我的猜测这部分代码是想通过键盘控制按三个button
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            //OnReset();
            Screen.SetResolution(1366, 768, false);
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            //OnHitMe();
            Screen.SetResolution(1366, 768, true);
        }
        else if (Input.GetKeyDown(KeyCode.F3))
        {
            //OnStop();
        }
    }
    //shuffle的话应该是洗牌的函数

    /*
    void Shuffle()
    {
        if (m_state != GameState.Invalid)
        {
        }
    }
    */


    //仿照Clear()函数
    void Sanguosha_Clear()
    {
        foreach (Card card in Player1_Cards)
        {
            GameObject.DestroyImmediate(card.gameObject);
        }
        foreach (Card card in Player2_Cards)
        {
            GameObject.DestroyImmediate(card.gameObject);
        }
        foreach (Card card in Xuanjiang_Cards)
        {
            GameObject.DestroyImmediate(card.gameObject);
        }
        Player1_Cards.Clear();
        Player2_Cards.Clear();
        Xuanjiang_Cards.Clear();
        Deck.Reset();
    }
    
    Vector3 GetDeckPosition()
    {
        return new UnityEngine.Vector3(-12, 0, 0);
    }

    void Xuanzewujiang()
    {
        Deck.GetComponent<DeckStandard>().Stock.GetComponent<CardStock>().Back = "Back3";
        CardDef tmpCard = Deck.Wujiangpop();
        float x, y, z;

        Debug.Log("Xuanzewujiang");
        if (tmpCard != null)
        {
            GameObject newObj = new GameObject();
            newObj.name = "Player_Wujiang";
            Card newCard = newObj.AddComponent(typeof(Card)) as Card;
            newCard.Definition = tmpCard;
            newObj.transform.parent = Deck.transform;
            newCard.TryBuild();
            x = -3 + Wujiang_num * 3.0f;
            y = 2;
            z = 0;
            Wujiang_num++;
            Vector3 deckPos = GetDeckPosition();
            newObj.transform.position = deckPos;
            Xuanjiang_Cards.Add(newCard);
            newCard.SetFlyTarget(deckPos, new Vector3(x, y, z), FlyTime);
        }
    }

    //电脑选择武将的话就随机一个就好了
    void Diannao_Xuanjiang()
    {
        Deck.GetComponent<DeckStandard>().Stock.GetComponent<CardStock>().Back = "Back3";
        CardDef tmpCard = Deck.Wujiangpop();
        float x, y, z;

        Debug.Log("Diannao Xuanjiang");
        if (tmpCard != null)
        {
            GameObject newObj = new GameObject();
            newObj.name = "Diannao_Wujiang";
            Card newCard = newObj.AddComponent(typeof(Card)) as Card;
            newCard.Definition = tmpCard;
            newObj.transform.parent = Deck.transform;
            newCard.TryBuild();
            x = 0;
            y = 5.5f;
            z = 0;
            Vector3 deckPos = GetDeckPosition();
            newObj.transform.position = deckPos;
            Xuanjiang_Cards.Add(newCard);
            newCard.SetFlyTarget(deckPos, new Vector3(x, y, z), FlyTime);
        }
    }
    //给玩家发一张牌
    void Player_Fapai()
    {
        Deck.GetComponent<DeckStandard>().Stock.GetComponent<CardStock>().Back = "Back2";
        CardDef tmpCard = Deck.Pop();
        float x, y, z;

        Debug.Log("Player Fapai");
        if (tmpCard != null)
        {
            GameObject newObj = new GameObject();
            newObj.name = "Player1_Shoupai";
            Card newCard = newObj.AddComponent(typeof(Card)) as Card;
            newCard.Definition = tmpCard;
            newObj.transform.parent = Deck.transform;
            newCard.TryBuild();
            if (Player1_Cards.Count < 5)
            {
                x = -8.7f + Player1_Cards.Count * Shoupai_Juli;
            }
            else
            {
                x = -8.7f + 4 * Shoupai_Juli;
            }
            y = -5.1f;
            z = -1f;
            Vector3 deckPos = GetDeckPosition();
            newObj.transform.position = deckPos;
            Player1_Cards.Add(newCard);
            newCard.SetFlyTarget(deckPos, new Vector3(x, y, z), FlyTime);
        }
    }
    //给电脑发一张牌
    void Diannao_Fapai()
    {
        Deck.GetComponent<DeckStandard>().Stock.GetComponent<CardStock>().Back = "Back2";
        CardDef tmpCard = Deck.Pop();
        float x, y, z;

        Debug.Log("Diannao Fapai");
        if (tmpCard != null)
        {
            GameObject newObj = new GameObject();
            newObj.name = "Diannao_Shoupai";
            Card newCard = newObj.AddComponent(typeof(Card)) as Card;
            newCard.Definition = tmpCard;
            newObj.transform.parent = Deck.transform;
            newCard.TryBuild();
            x = 0;
            y = 6f;
            z = 1;
            Vector3 deckPos = GetDeckPosition();
            newObj.transform.position = deckPos;
            Player2_Cards.Add(newCard);
            newCard.SetFlyTarget(deckPos, new Vector3(x, y, z), FlyTime);
        }
    }
	
    //提供给测试功能的Test button调用
    /*
    void OnTest()
    {
        Debug.Log("OnTest");
        //Vector3 deckPos = GetDeckPosition();
        //m_player[0].transform.position = deckPos;
        CardDef c1 = Deck.Wujiangpop();
        if (c1 != null)
        {
            GameObject newObj = new GameObject();
            newObj.name = "Card";
            Card newCard = newObj.AddComponent(typeof(Card)) as Card;
            newCard.Definition = c1;
            newObj.transform.parent = Deck.transform;
            newCard.TryBuild();
            float x = -3 + (m_dealer.Count) * 2.0f;
            float z = (m_dealer.Count) * -0.1f;
            Vector3 deckPos = GetDeckPosition();
            newObj.transform.position = deckPos;
            m_dealer.Add(newCard);
            newCard.SetFlyTarget(deckPos, new Vector3(x, 2, z), FlyTime);
        }

    }
    */
    //这里的函数是提供给button外部调用的
    
    

    IEnumerator OnRestart()
    {
        if (game_state != Sanguosha_GameState.State_Resolving)
        {
            #region 还要进行一系列的状态的复位
            game_state = Sanguosha_GameState.State_Resolving;
            Wujiang_num = 0;
            #region 将除了重新开始游戏button以外的image，text和button全部设置为不可见
            //所有text都不可见
            Sanguosha_Text_Message.SetActive(false);
            Sanguosha_Text_Player1_Panding1.SetActive(false);
            Sanguosha_Text_Player1_Panding2.SetActive(false);
            Sanguosha_Text_Player1_Zhuangbei1.SetActive(false);
            Sanguosha_Text_Player1_Zhuangbei2.SetActive(false);
            Sanguosha_Text_Player1_Shoupai.SetActive(false);
            Sanguosha_Text_Player2_Panding1.SetActive(false);
            Sanguosha_Text_Player2_Panding2.SetActive(false);
            Sanguosha_Text_Player2_Zhuangbei1.SetActive(false);
            Sanguosha_Text_Player2_Zhuangbei2.SetActive(false);
            Sanguosha_Text_Player2_Shoupai.SetActive(false);
            Sanguosha_Text_Player2_Jineng1.SetActive(false);
            Sanguosha_Text_Player2_Jineng2.SetActive(false);
            //所有button都不可见
            for (int i = 1; i < Sanguosha_Buttons.Length; ++i)
            {
                Sanguosha_Buttons[i].SetActive(false);
            }
            //所有image都不可见
            for (int i = 0; i < Tili_Max; ++i)
            {
                Sanguosha_Image_Player1_Tili[i].SetActive(false);
                Sanguosha_Image_Player2_Tili[i].SetActive(false);
            }
            #endregion
            #endregion
            //从新装满卡牌堆
            Sanguosha_Clear();
            //洗武将牌
            Deck.WujiangShuffle();
            //洗游戏牌
            Deck.Shuffle();
            //发三个武将
            Xuanzewujiang();
            yield return new WaitForSeconds(DealTime);
            Xuanzewujiang();
            yield return new WaitForSeconds(DealTime);
            Xuanzewujiang();
            yield return new WaitForSeconds(DealTime);
            //把选择武将的button显示出来
            for(int i = 0; i < Xuanjiang_Max; ++i)
            {
                Sanguosha_Xuanjiang[i].SetActive(true);
            }
            game_state = Sanguosha_GameState.State_ChooseWujiang;
        }
    }
    
    IEnumerator ChushiPlayerShoupai()
    {
        if (game_state == Sanguosha_GameState.State_ChooseWujiang)
        {
            for(int i = 0; i < player1_manxue; ++i)
            {
                Player_Fapai();
                UpdatePlayerStatus();
                UpdateDiannaoStatus();
                yield return new WaitForSeconds(DealTime);
                SortShoupai();
            }
        }
    }
    //给玩家发牌的函数
    IEnumerator PlayerMopai(int MoPaiNum)
    {
        if(game_state==Sanguosha_GameState.State_Playing && game_round==Sanguosha_Round.Round_Player1)
        {
            for (int i = 0; i < MoPaiNum; ++i)
            {
                Player_Fapai();
                UpdatePlayerStatus();
                UpdateDiannaoStatus();
                yield return new WaitForSeconds(DealTime);
                SortShoupai();
            }
        }
        UpdatePlayerStatus();
        UpdateDiannaoStatus();
        StartCoroutine(UpdateUsedCards());
    }

    IEnumerator ChushiDiannaoShoupai()
    {
        if (game_state == Sanguosha_GameState.State_ChooseWujiang)
        {
            for(int i = 0; i < player2_manxue; ++i)
            {
                Diannao_Fapai();
                UpdatePlayerStatus();
                UpdateDiannaoStatus();
                yield return new WaitForSeconds(DealTime);
            }
        }
    }

    //这个函数用来更新玩家的装备状态，判定状态，手牌状态和体力状态
    void UpdatePlayerStatus()
    {
        #region 更新玩家技能状态
        if (string_player1_jineng2 == "")
        {
            if (string_player1_jineng1 == "")
            {
                Sanguosha_Buttons[5].SetActive(false);
                Sanguosha_Buttons[4].SetActive(false);
            }
            else
            {
                Sanguosha_Buttons[4].SetActive(true);
                Sanguosha_Buttons[5].SetActive(false);
                Sanguosha_Text_Player1_Jineng1.GetComponent<TextMesh>().text = string_player1_jineng1;
            }
        }
        else
        {
            Sanguosha_Buttons[4].SetActive(true);
            Sanguosha_Buttons[5].SetActive(true);
            Sanguosha_Text_Player1_Jineng1.GetComponent<TextMesh>().text = string_player1_jineng1;
            Sanguosha_Text_Player1_Jineng2.GetComponent<TextMesh>().text = string_player1_jineng2;
        }
        #endregion

        #region 更新玩家装备状态
        Sanguosha_Text_Player1_Zhuangbei1.GetComponent<TextMesh>().text = string_player1_wuqi;
        Sanguosha_Text_Player1_Zhuangbei2.GetComponent<TextMesh>().text = string_player1_fangju;
        #endregion

        #region 更新玩家判定状态
        Sanguosha_Text_Player1_Panding1.GetComponent<TextMesh>().text = string_player1_panding1;
        Sanguosha_Text_Player1_Panding2.GetComponent<TextMesh>().text = string_player1_panding2;
        #endregion

        #region 更新玩家体力状态
        for (int i = 0; i < Sanguosha_Image_Player1_Tili.Length; ++i)
        {
            Sanguosha_Image_Player1_Tili[i].SetActive(false);
        }

        for (int i=0; i < player1_tili; ++i)
        {
            Sanguosha_Image_Player1_Tili[i].SetActive(true);
        }
        #endregion

        #region 更新玩家手牌数量状态
        player1_shoupai = Player1_Cards.Count;
        player1_shoupaishangxian = player1_tili;
        Sanguosha_Text_Player1_Shoupai.GetComponent<TextMesh>().text = player1_shoupai.ToString() + "/" + player1_shoupaishangxian.ToString();
        #endregion

        #region 更新玩家手牌状态
        SortShoupai();
        #endregion
    }

    //更新电脑的装备状态，判定状态，手牌状态和体力状态
    void UpdateDiannaoStatus()
    {
        #region 更新玩家技能状态
        if (string_player2_jineng2 == "")
        {
            if (string_player2_jineng1 == "")
            {
                Sanguosha_Text_Player2_Jineng1.SetActive(false);
                Sanguosha_Text_Player2_Jineng2.SetActive(false);
            }
            else
            {
                Sanguosha_Text_Player2_Jineng1.SetActive(true);
                Sanguosha_Text_Player2_Jineng2.SetActive(false);
                Sanguosha_Text_Player2_Jineng1.GetComponent<TextMesh>().text = string_player2_jineng1;
            }
        }
        else
        {
            Sanguosha_Text_Player2_Jineng1.SetActive(true);
            Sanguosha_Text_Player2_Jineng2.SetActive(true);
            Sanguosha_Text_Player2_Jineng1.GetComponent<TextMesh>().text = string_player2_jineng1;
            Sanguosha_Text_Player2_Jineng2.GetComponent<TextMesh>().text = string_player2_jineng2;
        }
        #endregion

        #region 更新玩家装备状态
        Sanguosha_Text_Player2_Zhuangbei1.GetComponent<TextMesh>().text = string_player2_wuqi;
        Sanguosha_Text_Player2_Zhuangbei2.GetComponent<TextMesh>().text = string_player2_fangju;
        #endregion

        #region 更新玩家判定状态
        Sanguosha_Text_Player2_Panding1.GetComponent<TextMesh>().text = string_player2_panding1;
        Sanguosha_Text_Player2_Panding2.GetComponent<TextMesh>().text = string_player2_panding2;
        #endregion

        #region 更新玩家体力状态
        for(int i = 0; i < Sanguosha_Image_Player2_Tili.Length; ++i)
        {
            Sanguosha_Image_Player2_Tili[i].SetActive(false);
        }
        for (int i = 0; i < player2_tili; ++i)
        {
            Sanguosha_Image_Player2_Tili[i].SetActive(true);
        }
        #endregion

        #region 更新玩家手牌状态
        player2_shoupai = Player2_Cards.Count;
        player2_shoupaishangxian = player2_tili;
        Sanguosha_Text_Player2_Shoupai.GetComponent<TextMesh>().text = player2_shoupai.ToString() + "/" + player2_shoupaishangxian.ToString();
        #endregion
    }

    IEnumerator UpdateUsedCards()
    {
        yield return new WaitForSeconds(StayTime);
        for(int i = 0; i < Used_Cards.Count; ++i)
        {
            Used_Cards[i].transform.position = new Vector3(20, 0, 0);
        }
        
    }

    //根据手牌数量更新牌的位置和Buttons的位置
    void SortShoupai()
    {
        #region 整理图片位置
        if (Player1_Cards.Count <= 5)
        {
            for(int i = 0; i < Player1_Cards.Count; ++i)
            {
                Player1_Cards[i].transform.position = new Vector3(-8.7f + i * Shoupai_Juli, -5.1f, -1f);
            }
        }
        else
        {
            for(int i = 0; i <Player1_Cards.Count-1; ++i)
            {
                Player1_Cards[i].transform.position = new Vector3(-8.7f + i * Shoupai_Juli * 4 / (Player1_Cards.Count - 1), -5.1f, -1f);
            }
            Player1_Cards[Player1_Cards.Count - 1].transform.position = new Vector3(1.3f, -5.1f, -1.1f);
        }
        #endregion

        #region 整理图片后的Button的位置
        //所有的手牌按键设置为不启用
        for (int i = 0; i < Shoupai_Max; ++i)
        {
            Sanguosha_Shoupai[i].SetActive(false);
        }
        //和手牌数量一致的手牌按键设置为启用
        for(int i = 0; i < Player1_Cards.Count; ++i)
        {
            Sanguosha_Shoupai[i].SetActive(true);
        }
        if (Player1_Cards.Count <= 5)
        {
            for(int i = 0; i < Player1_Cards.Count; ++i)
            {
                Sanguosha_Shoupai[i].transform.position = new Vector3(-8.7f + i * Shoupai_Juli, -5.1f, -1f - 0.01f * i);
            }
        }
        else
        {
            for (int i = 0; i < Player1_Cards.Count; ++i)
            {
                Sanguosha_Shoupai[i].transform.position = new Vector3(-8.7f + i * Shoupai_Juli * 4 / (Player1_Cards.Count - 1), -5.1f, -1f - 0.01f * i);
            }
        }
        #endregion
    }

    Card Panding()
    {
        Deck.GetComponent<DeckStandard>().Stock.GetComponent<CardStock>().Back = "Back2";
        CardDef tmpCard = Deck.Pop();
        float x, y, z;

        Debug.Log("Panding");
        if (tmpCard != null)
        {
            GameObject newObj = new GameObject();
            newObj.name = "Panding";
            Card newCard = newObj.AddComponent(typeof(Card)) as Card;
            newCard.Definition = tmpCard;
            newObj.transform.parent = Deck.transform;
            newCard.TryBuild();
            x = 0;
            y = 0;
            z = 0;
            Vector3 deckPos = GetDeckPosition();
            newObj.transform.position = deckPos;
            newCard.SetFlyTarget(deckPos, new Vector3(x, y, z), FlyTime);
            return newCard;
        }
        return null;
    }

    IEnumerator Xuanzewujiang_Process(string msg)
    {
        #region 初始化阶段

        #region 玩家选择武将
        for (int i = 0; i < Xuanjiang_Max; ++i)
        {
            Sanguosha_Xuanjiang[i].SetActive(false);
        }
        Xuanjiang_Cards[0].transform.position = new Vector3(20, -5, 0);
        Xuanjiang_Cards[1].transform.position = new Vector3(20, -5, 0);
        Xuanjiang_Cards[2].transform.position = new Vector3(20, -5, 0);
        switch (msg)
        {
            case "Wujiang1":
                Xuanjiang_Cards[0].transform.position = new Vector3(6.5f, -3.35f, -4);
                GameObject.DestroyImmediate(Xuanjiang_Cards[1].gameObject);
                GameObject.DestroyImmediate(Xuanjiang_Cards[2].gameObject);
                Xuanjiang_Cards.RemoveAt(2);
                Xuanjiang_Cards.RemoveAt(1);
                break;
            case "Wujiang2":
                Xuanjiang_Cards[1].transform.position = new Vector3(6.5f, -3.35f, -4);
                GameObject.DestroyImmediate(Xuanjiang_Cards[0].gameObject);
                GameObject.DestroyImmediate(Xuanjiang_Cards[2].gameObject);
                Xuanjiang_Cards.RemoveAt(2);
                Xuanjiang_Cards.RemoveAt(0);
                break;
            case "Wujiang3":
                Xuanjiang_Cards[2].transform.position = new Vector3(6.5f, -3.35f, -4);
                GameObject.DestroyImmediate(Xuanjiang_Cards[0].gameObject);
                GameObject.DestroyImmediate(Xuanjiang_Cards[1].gameObject);
                Xuanjiang_Cards.RemoveAt(1);
                Xuanjiang_Cards.RemoveAt(0);
                break;
        }
        #endregion

        #region 电脑选择武将
        Diannao_Xuanjiang();
        #endregion

        #region 初始化玩家状态
        string_player1_wujiang = Xuanjiang_Cards[0].Definition.CardName;

        #region 根据玩家武将初始化技能状态和血量上限
        switch (string_player1_wujiang)
        {
            case "Caocao":
                string_player1_jineng1 = "奸雄";
                string_player1_jineng2 = "";
                player1_manxue = 4;
                break;
            case "Simayi":
                string_player1_jineng1 = "反馈";
                string_player1_jineng2 = "鬼才";
                player1_manxue = 3;
                break;
            case "Xiahoudun":
                string_player1_jineng1 = "刚烈";
                string_player1_jineng2 = "";
                player1_manxue = 4;
                break;
            case "Zhangliao":
                string_player1_jineng1 = "突袭";
                string_player1_jineng2 = "";
                player1_manxue = 4;
                break;
            case "Guojia":
                string_player1_jineng1 = "天妒";
                string_player1_jineng2 = "遗计";
                player1_manxue = 3;
                break;
            case "Zhenji":
                string_player1_jineng1 = "倾国";
                string_player1_jineng2 = "洛神";
                player1_manxue = 3;
                break;
            case "Xuhuang":
                string_player1_jineng1 = "断粮";
                string_player1_jineng2 = "";
                player1_manxue = 4;
                break;
            case "Caoren":
                string_player1_jineng1 = "据守";
                string_player1_jineng2 = "";
                player1_manxue = 4;
                break;
            case "Dianwei":
                string_player1_jineng1 = "强袭";
                string_player1_jineng2 = "";
                player1_manxue = 4;
                break;
            case "Guanyu":
                string_player1_jineng1 = "武圣";
                string_player1_jineng2 = "";
                player1_manxue = 4;
                break;
            case "Zhangfei":
                string_player1_jineng1 = "咆哮";
                string_player1_jineng2 = "";
                player1_manxue = 4;
                break;
            case "Zhugeliang":
                string_player1_jineng1 = "观星";
                string_player1_jineng2 = "空城";
                player1_manxue = 3;
                break;
            case "Zhaoyun":
                string_player1_jineng1 = "龙胆";
                string_player1_jineng2 = "";
                player1_manxue = 4;
                break;
            case "Machao":
                string_player1_jineng1 = "铁骑";
                string_player1_jineng2 = "";
                player1_manxue = 4;
                break;
            case "Huangyueying":
                string_player1_jineng1 = "集智";
                string_player1_jineng2 = "奇才";
                player1_manxue = 3;
                break;
            case "Huangzhong":
                string_player1_jineng1 = "裂弓";
                string_player1_jineng2 = "";
                player1_manxue = 4;
                break;
            case "Weiyan":
                string_player1_jineng1 = "狂骨";
                string_player1_jineng2 = "";
                player1_manxue = 4;
                break;
            case "Jiangwei":
                string_player1_jineng1 = "挑衅";
                string_player1_jineng2 = "";
                player1_manxue = 4;
                break;
            case "Menghuo":
                string_player1_jineng1 = "祸首";
                string_player1_jineng2 = "再起";
                player1_manxue = 4;
                break;
            case "Sunquan":
                string_player1_jineng1 = "制衡";
                string_player1_jineng2 = "";
                player1_manxue = 4;
                break;
            case "Ganning":
                string_player1_jineng1 = "奇袭";
                string_player1_jineng2 = "";
                player1_manxue = 4;
                break;
            case "Huanggai":
                string_player1_jineng1 = "苦肉";
                string_player1_jineng2 = "";
                player1_manxue = 4;
                break;
            case "Zhouyu":
                string_player1_jineng1 = "英姿";
                string_player1_jineng2 = "反间";
                player1_manxue = 3;
                break;
            case "Daqiao":
                string_player1_jineng1 = "国色";
                string_player1_jineng2 = "琉璃";
                player1_manxue = 3;
                break;
            case "Luxun":
                string_player1_jineng1 = "谦逊";
                string_player1_jineng2 = "连营";
                player1_manxue = 3;
                break;
            case "Sunshangxiang":
                string_player1_jineng1 = "结姻";
                string_player1_jineng2 = "枭姬";
                player1_manxue = 3;
                break;
            case "Xiaoqiao":
                string_player1_jineng1 = "天香";
                string_player1_jineng2 = "红颜";
                player1_manxue = 3;
                break;
            case "Zhoutai":
                string_player1_jineng1 = "不屈";
                string_player1_jineng2 = "";
                player1_manxue = 4;
                break;
            case "Huatuo":
                string_player1_jineng1 = "急救";
                string_player1_jineng2 = "青囊";
                player1_manxue = 3;
                break;
            case "Lvbu":
                string_player1_jineng1 = "无双";
                string_player1_jineng2 = "";
                player1_manxue = 4;
                break;
            case "Diaochan":
                string_player1_jineng1 = "闭月";
                string_player1_jineng2 = "";
                player1_manxue = 3;
                break;
            case "Pangde":
                string_player1_jineng1 = "猛进";
                string_player1_jineng2 = "";
                player1_manxue = 4;
                break;
            case "Zhangjiao":
                string_player1_jineng1 = "雷击";
                string_player1_jineng2 = "鬼道";
                player1_manxue = 3;
                break;
        }
        #endregion

        string_player1_panding1 = "1";
        string_player1_panding2 = "2";
        string_player1_wuqi = "武器";
        string_player1_fangju = "防具";
        player1_tili = player1_manxue;
        player1_shoupaishangxian = player1_tili;

        #endregion

        #region 初始化电脑状态
        string_player2_wujiang = Xuanjiang_Cards[1].Definition.CardName;

        #region 根据电脑武将初始化技能状态和血量上限
        switch (string_player2_wujiang)
        {
            case "Caocao":
                string_player2_jineng1 = "奸雄";
                string_player2_jineng2 = "";
                player2_manxue = 4;
                break;
            case "Simayi":
                string_player2_jineng1 = "反馈";
                string_player2_jineng2 = "鬼才";
                player2_manxue = 3;
                break;
            case "Xiahoudun":
                string_player2_jineng1 = "刚烈";
                string_player2_jineng2 = "";
                player2_manxue = 4;
                break;
            case "Zhangliao":
                string_player2_jineng1 = "突袭";
                string_player2_jineng2 = "";
                player2_manxue = 4;
                break;
            case "Guojia":
                string_player2_jineng1 = "天妒";
                string_player2_jineng2 = "遗计";
                player2_manxue = 3;
                break;
            case "Zhenji":
                string_player2_jineng1 = "倾国";
                string_player2_jineng2 = "洛神";
                player2_manxue = 3;
                break;
            case "Xuhuang":
                string_player2_jineng1 = "断粮";
                string_player2_jineng2 = "";
                player2_manxue = 4;
                break;
            case "Caoren":
                string_player2_jineng1 = "据守";
                string_player2_jineng2 = "";
                player2_manxue = 4;
                break;
            case "Dianwei":
                string_player2_jineng1 = "强袭";
                string_player2_jineng2 = "";
                player2_manxue = 4;
                break;
            case "Guanyu":
                string_player2_jineng1 = "武圣";
                string_player2_jineng2 = "";
                player2_manxue = 4;
                break;
            case "Zhangfei":
                string_player2_jineng1 = "咆哮";
                string_player2_jineng2 = "";
                player2_manxue = 4;
                break;
            case "Zhugeliang":
                string_player2_jineng1 = "观星";
                string_player2_jineng2 = "空城";
                player2_manxue = 3;
                break;
            case "Zhaoyun":
                string_player2_jineng1 = "龙胆";
                string_player2_jineng2 = "";
                player2_manxue = 4;
                break;
            case "Machao":
                string_player2_jineng1 = "铁骑";
                string_player2_jineng2 = "";
                player2_manxue = 4;
                break;
            case "Huangyueying":
                string_player2_jineng1 = "集智";
                string_player2_jineng2 = "奇才";
                player2_manxue = 3;
                break;
            case "Huangzhong":
                string_player2_jineng1 = "裂弓";
                string_player2_jineng2 = "";
                player2_manxue = 4;
                break;
            case "Weiyan":
                string_player2_jineng1 = "狂骨";
                string_player2_jineng2 = "";
                player2_manxue = 4;
                break;
            case "Jiangwei":
                string_player2_jineng1 = "挑衅";
                string_player2_jineng2 = "";
                player2_manxue = 4;
                break;
            case "Menghuo":
                string_player2_jineng1 = "祸首";
                string_player2_jineng2 = "再起";
                player2_manxue = 4;
                break;
            case "Sunquan":
                string_player2_jineng1 = "制衡";
                string_player2_jineng2 = "";
                player2_manxue = 4;
                break;
            case "Ganning":
                string_player2_jineng1 = "奇袭";
                string_player2_jineng2 = "";
                player2_manxue = 4;
                break;
            case "Huanggai":
                string_player2_jineng1 = "苦肉";
                string_player2_jineng2 = "";
                player2_manxue = 4;
                break;
            case "Zhouyu":
                string_player2_jineng1 = "英姿";
                string_player2_jineng2 = "反间";
                player2_manxue = 3;
                break;
            case "Daqiao":
                string_player2_jineng1 = "国色";
                string_player2_jineng2 = "琉璃";
                player2_manxue = 3;
                break;
            case "Luxun":
                string_player2_jineng1 = "谦逊";
                string_player2_jineng2 = "连营";
                player2_manxue = 3;
                break;
            case "Sunshangxiang":
                string_player2_jineng1 = "结姻";
                string_player2_jineng2 = "枭姬";
                player2_manxue = 3;
                break;
            case "Xiaoqiao":
                string_player2_jineng1 = "天香";
                string_player2_jineng2 = "红颜";
                player2_manxue = 3;
                break;
            case "Zhoutai":
                string_player2_jineng1 = "不屈";
                string_player2_jineng2 = "";
                player2_manxue = 4;
                break;
            case "Huatuo":
                string_player2_jineng1 = "急救";
                string_player2_jineng2 = "青囊";
                player2_manxue = 3;
                break;
            case "Lvbu":
                string_player2_jineng1 = "无双";
                string_player2_jineng2 = "";
                player2_manxue = 4;
                break;
            case "Diaochan":
                string_player2_jineng1 = "闭月";
                string_player2_jineng2 = "";
                player2_manxue = 3;
                break;
            case "Pangde":
                string_player2_jineng1 = "猛进";
                string_player2_jineng2 = "";
                player2_manxue = 4;
                break;
            case "Zhangjiao":
                string_player2_jineng1 = "雷击";
                string_player2_jineng2 = "鬼道";
                player2_manxue = 3;
                break;
        }
        #endregion

        string_player2_panding1 = "1";
        string_player2_panding2 = "2";
        string_player2_wuqi = "武器";
        string_player2_fangju = "防具";
        player2_tili = player2_manxue;
        player2_shoupaishangxian = player2_tili;
        #endregion

        #region 启用显示游戏界面
        //所有text都启用
        Sanguosha_Text_Player1_Panding1.SetActive(true);
        Sanguosha_Text_Player1_Panding2.SetActive(true);
        Sanguosha_Text_Player1_Zhuangbei1.SetActive(true);
        Sanguosha_Text_Player1_Zhuangbei2.SetActive(true);
        Sanguosha_Text_Player1_Shoupai.SetActive(true);
        Sanguosha_Text_Player1_Jineng1.SetActive(true);
        Sanguosha_Text_Player1_Jineng2.SetActive(true);
        Sanguosha_Text_Player2_Panding1.SetActive(true);
        Sanguosha_Text_Player2_Panding2.SetActive(true);
        Sanguosha_Text_Player2_Zhuangbei1.SetActive(true);
        Sanguosha_Text_Player2_Zhuangbei2.SetActive(true);
        Sanguosha_Text_Player2_Shoupai.SetActive(true);
        //所有功能button都启用除了最后两个对面武将装备的按键，只在过河拆桥和顺手牵羊的时候使用
        for (int i = 1; i < Sanguosha_Buttons.Length - 2; ++i)
        {
            Sanguosha_Buttons[i].SetActive(true);
        }
        //所有image都先不启用
        for (int i = 0; i < Tili_Max; ++i)
        {
            Sanguosha_Image_Player1_Tili[i].SetActive(false);
            Sanguosha_Image_Player2_Tili[i].SetActive(false);
        }
        #endregion

        #region 给玩家发初始手牌
        StartCoroutine(ChushiPlayerShoupai());
        #endregion

        yield return new WaitForSeconds(StayTime);

        #region 给电脑发初始手牌
        StartCoroutine(ChushiDiannaoShoupai());
        #endregion

        yield return new WaitForSeconds(StayTime);

        #endregion

        #region 更新玩家和电脑状态
        UpdatePlayerStatus();
        UpdateDiannaoStatus();
        #endregion

        #region 状态机变化
        game_state = Sanguosha_GameState.State_Playing;
        game_round = Sanguosha_Round.Round_Player1;
        game_stage = Sanguosha_Stage.Stage_Zhunbei;
        Huihe_Num = 1;
        #endregion

        State_Handler("");
    }

    IEnumerator Player1_Zhunbei_Process(string msg)
    {
        #region 准备阶段
        //主要是觉得不能放在要被重复调用的Chupai处理函数中
        
        //一些状态可以在这里初始化，如杀的使用次数
        player1_tili--;
        Player1_ShaSign = false;

        Card_Select = -1;

        #region 更新玩家和电脑状态
        UpdatePlayerStatus();
        UpdateDiannaoStatus();
        StartCoroutine(UpdateUsedCards());
        yield return new WaitForSeconds(DealTime);
        #endregion

        #region 状态机变化
        game_state = Sanguosha_GameState.State_Playing;
        game_round = Sanguosha_Round.Round_Player1;
        game_stage = Sanguosha_Stage.Stage_Panding;
        #endregion

        State_Handler("");

        #endregion
    }

    IEnumerator Player1_Panding_Process(string msg)
    {
        if (string_player1_panding2 != "2")
        {
            Card tmpCard;
            tmpCard = Panding();
            if (string_player1_panding2 == "乐")
            {
                if (tmpCard.Definition.Huase != "Hongtao")
                {
                    Player1_LebusishuSign = true;
                }
            }
            else if (string_player1_panding2 == "兵")
            {
                if (tmpCard.Definition.Huase != "Meihua")
                {
                    Player1_BingliangcunduanSign = true;
                }
            }
            string_player1_panding2 = "2";
            Used_Cards.Add(tmpCard);
            yield return new WaitForSeconds(StayTime);
            tmpCard.transform.position = new Vector3(3, 0, 0);
        }

        UpdatePlayerStatus();
        UpdateDiannaoStatus();

        if (string_player1_panding1 != "1")
        {
            Card tmpCard;
            tmpCard = Panding();
            if (string_player1_panding1 == "乐")
            {
                if (tmpCard.Definition.Huase != "Hongtao")
                {
                    Player1_LebusishuSign = true;
                }
            }
            else if (string_player1_panding1 == "兵")
            {
                if (tmpCard.Definition.Huase != "Meihua")
                {
                    Player1_BingliangcunduanSign = true;
                }
            }
            string_player1_panding1 = "1";
            Used_Cards.Add(tmpCard);
            yield return new WaitForSeconds(StayTime);

        }
        #region 更新玩家和电脑状态
        UpdatePlayerStatus();
        UpdateDiannaoStatus();
        StartCoroutine(UpdateUsedCards());
        #endregion

        #region 状态机变化
        if (Player1_BingliangcunduanSign)
        {
            if (Player1_LebusishuSign)
            {
                game_state = Sanguosha_GameState.State_Playing;
                game_round = Sanguosha_Round.Round_Player1;
                game_stage = Sanguosha_Stage.Stage_Qipai;
            }
            else
            {
                game_state = Sanguosha_GameState.State_Playing;
                game_round = Sanguosha_Round.Round_Player1;
                game_stage = Sanguosha_Stage.Stage_Chupai;
            }
        }
        else
        {
            //后面摸牌阶段结束再判断一下乐不思蜀状态
            game_state = Sanguosha_GameState.State_Playing;
            game_round = Sanguosha_Round.Round_Player1;
            game_stage = Sanguosha_Stage.Stage_Mopai;
        }
        Player1_BingliangcunduanSign = false;
        #endregion

        State_Handler("");
    }

    IEnumerator Player1_Mopai_Process(string msg)
    {
        StartCoroutine(PlayerMopai(Shoupaiperround));

        yield return new WaitForSeconds(Shoupaiperround * DealTime);

        #region 更新玩家和电脑状态
        UpdatePlayerStatus();
        UpdateDiannaoStatus();
        #endregion

        #region 状态机变化
        if (Player1_LebusishuSign)
        {
            game_state = Sanguosha_GameState.State_Playing;
            game_round = Sanguosha_Round.Round_Player1;
            game_stage = Sanguosha_Stage.Stage_Qipai;
        }
        else
        {
            game_state = Sanguosha_GameState.State_Playing;
            game_round = Sanguosha_Round.Round_Player1;
            game_stage = Sanguosha_Stage.Stage_Chupai;
        }

        Player1_LebusishuSign = false;
        #endregion

        State_Handler("");
    }

    IEnumerator Player1_Chupai_Process(string msg)
    {
        game_jiesuan = Sanguosha_Jiesuan.Jiesuan_None;

        switch (msg)
        {
            case "Shoupai0":
            case "Shoupai1":
            case "Shoupai2":
            case "Shoupai3":
            case "Shoupai4":
            case "Shoupai5":
            case "Shoupai6":
            case "Shoupai7":
            case "Shoupai8":
            case "Shoupai9":
            case "Shoupai10":
            case "Shoupai11":
            case "Shoupai12":
            case "Shoupai13":
            case "Shoupai14":
            case "Shoupai15":
            case "Shoupai16":
            case "Shoupai17":
            case "Shoupai18":
            case "Shoupai19":
            case "Shoupai20":
            case "Shoupai21":
            case "Shoupai22":
                if (game_state == Sanguosha_GameState.State_Playing)
                {
                    if (Card_Select != -1)
                    {
                        Player1_Cards[Card_Select].transform.position = new Vector3(Player1_Cards[Card_Select].transform.position.x, Player1_Cards[Card_Select].transform.position.y - 1, Player1_Cards[Card_Select].transform.position.z);
                    }
                    Card_Select = int.Parse(msg.Substring(7));
                    Player1_Cards[Card_Select].transform.position = new Vector3(Player1_Cards[Card_Select].transform.position.x, Player1_Cards[Card_Select].transform.position.y + 1, Player1_Cards[Card_Select].transform.position.z);
                }
                break;
            case "Queding":
                if (game_state == Sanguosha_GameState.State_Playing)
                {
                    if (Card_Select != -1)
                    {
                        Card tmpCard = Player1_Cards[Card_Select];

                        switch (tmpCard.Definition.CardName)
                        {
                            case "Sha":
                                #region 出牌阶段出杀
                                if (Player1_ShaSign == false)
                                {
                                    Player1_Cards.RemoveAt(Card_Select);
                                    tmpCard.transform.position = new Vector3(2 - Jiesuan_Num * Shoupai_Juli, 1, 0);
                                    Used_Cards.Add(tmpCard);

                                    //状态机变化
                                    game_state = Sanguosha_GameState.State_Resolving_Player2;
                                    game_round = Sanguosha_Round.Round_Player1;
                                    game_stage = Sanguosha_Stage.Stage_Chupai;
                                    game_jiesuan = Sanguosha_Jiesuan.Jiesuan_Sha;

                                    Card_Select = -1;
                                    Jiesuan_Num += 1;
                                    Player1_ShaSign = true;

                                    #region 更新玩家和电脑状态
                                    UpdatePlayerStatus();
                                    UpdateDiannaoStatus();
                                    #endregion
                                    State_Handler("");
                                }
                                else
                                {
                                    Debug.Log("一回合只能出一张杀！");
                                }
                                break;
                            #endregion
                            case "Tao":
                                #region 出牌阶段出桃
                                if (player1_tili != player1_manxue)
                                {
                                    Player1_Cards.RemoveAt(Card_Select);
                                    tmpCard.transform.position = new Vector3(2 - Jiesuan_Num * Shoupai_Juli, 1, 0);
                                    Used_Cards.Add(tmpCard);

                                    player1_tili += 1;

                                    Card_Select = -1;
                                    Jiesuan_Num = 0;

                                    #region 更新玩家和电脑状态
                                    UpdatePlayerStatus();
                                    UpdateDiannaoStatus();
                                    StartCoroutine(UpdateUsedCards());
                                    #endregion
                                }
                                else
                                {
                                    Debug.Log("满血无法吃桃！");
                                }
                                break;
                            #endregion
                            case "Wuzhongshengyou":
                                #region 出牌阶段出无中生有
                                Player1_Cards.RemoveAt(Card_Select);
                                tmpCard.transform.position = new Vector3(2 - Jiesuan_Num * Shoupai_Juli, 1, 0);
                                Used_Cards.Add(tmpCard);

                                StartCoroutine(PlayerMopai(Shoupaiperround));

                                yield return new WaitForSeconds(2);

                                Card_Select = -1;
                                Jiesuan_Num = 0;

                                #region 更新玩家和电脑状态
                                UpdatePlayerStatus();
                                UpdateDiannaoStatus();
                                StartCoroutine(UpdateUsedCards());
                                #endregion
                                break;
                            #endregion
                            case "Wanjianqifa":
                                #region 出牌阶段出万箭齐发
                                Player1_Cards.RemoveAt(Card_Select);
                                tmpCard.transform.position = new Vector3(2 - Jiesuan_Num * Shoupai_Juli, 1, 0);
                                Used_Cards.Add(tmpCard);

                                //状态机变化
                                game_state = Sanguosha_GameState.State_Resolving_Player2;
                                game_round = Sanguosha_Round.Round_Player1;
                                game_stage = Sanguosha_Stage.Stage_Chupai;
                                game_jiesuan = Sanguosha_Jiesuan.Jiesuan_Wanjianqifa;

                                Card_Select = -1;
                                Jiesuan_Num += 1;

                                #region 更新玩家和电脑状态
                                UpdatePlayerStatus();
                                UpdateDiannaoStatus();
                                #endregion
                                State_Handler("");
                                break;

                                #endregion
                            case "Nanmanruqin":
                                #region 出牌阶段出南蛮入侵
                                Player1_Cards.RemoveAt(Card_Select);
                                tmpCard.transform.position = new Vector3(2 - Jiesuan_Num * Shoupai_Juli, 1, 0);
                                Used_Cards.Add(tmpCard);

                                //状态机变化
                                game_state = Sanguosha_GameState.State_Resolving_Player2;
                                game_round = Sanguosha_Round.Round_Player1;
                                game_stage = Sanguosha_Stage.Stage_Chupai;
                                game_jiesuan = Sanguosha_Jiesuan.Jiesuan_Nanmanruqin;

                                Card_Select = -1;
                                Jiesuan_Num += 1;

                                #region 更新玩家和电脑状态
                                UpdatePlayerStatus();
                                UpdateDiannaoStatus();
                                #endregion
                                State_Handler("");
                                break;
                                #endregion
                            case "Juedou":
                                #region 出牌阶段出决斗
                                Player1_Cards.RemoveAt(Card_Select);
                                tmpCard.transform.position = new Vector3(2 - Jiesuan_Num * Shoupai_Juli, 1, 0);
                                Used_Cards.Add(tmpCard);

                                //状态机变化
                                game_state = Sanguosha_GameState.State_Resolving_Player2;
                                game_round = Sanguosha_Round.Round_Player1;
                                game_stage = Sanguosha_Stage.Stage_Chupai;
                                game_jiesuan = Sanguosha_Jiesuan.Jiesuan_Juedou;

                                Card_Select = -1;
                                Jiesuan_Num += 1;

                                #region 更新玩家和电脑状态
                                UpdatePlayerStatus();
                                UpdateDiannaoStatus();
                                #endregion
                                State_Handler("");
                                break;
                            #endregion
                            case "Guohechaiqiao":
                                #region 出牌阶段出过河拆桥

                                #endregion
                                break;
                        }
                    }
                }
                break;
            case "Quxiao":
                if (game_state == Sanguosha_GameState.State_Playing)
                {
                    if (Card_Select != -1)
                    {
                        Player1_Cards[Card_Select].transform.position = new Vector3(Player1_Cards[Card_Select].transform.position.x, Player1_Cards[Card_Select].transform.position.y - 1, Player1_Cards[Card_Select].transform.position.z);
                    }
                    Card_Select = -1;
                }
                break;
            case "Jieshu":
                //结束进入弃牌阶段
                if (game_state == Sanguosha_GameState.State_Playing)
                {
                    if (Card_Select != -1)
                    {
                        Player1_Cards[Card_Select].transform.position = new Vector3(Player1_Cards[Card_Select].transform.position.x, Player1_Cards[Card_Select].transform.position.y - 1, Player1_Cards[Card_Select].transform.position.z);
                    }
                    Card_Select = -1;

                    #region 状态机变化
                    game_state = Sanguosha_GameState.State_Playing;
                    game_round = Sanguosha_Round.Round_Player1;
                    game_stage = Sanguosha_Stage.Stage_Qipai;
                    #endregion

                    State_Handler("");
                }
                break;
        }

        yield return new WaitForSeconds(DealTime);
    }

    void Resolving_Player2_Process(string msg)
    {
        switch (game_jiesuan)
        {
            case Sanguosha_Jiesuan.Jiesuan_Sha:
                #region 结算杀牌
                //这里先随便写写电脑的决策，暂时的话是有闪出闪
                for (int i = 0; i < Player2_Cards.Count; ++i)
                {
                    if (Player2_Cards[i].Definition.CardName == "Shan")
                    {
                        Debug.Log("出闪");
                        Player2_Chupai(i);

                        //状态机重新转回出牌阶段
                        game_state = Sanguosha_GameState.State_Playing;
                        game_round = Sanguosha_Round.Round_Player1;
                        game_stage = Sanguosha_Stage.Stage_Chupai;
                        game_jiesuan = Sanguosha_Jiesuan.Jiesuan_None;

                        #region 更新玩家和电脑状态
                        UpdatePlayerStatus();
                        UpdateDiannaoStatus();
                        StartCoroutine(UpdateUsedCards());
                        Jiesuan_Num = 0;
                        #endregion
                        break;
                    }
                    if (i == Player2_Cards.Count - 1)
                    {
                        Debug.Log("掉血");
                        player2_tili -= 1;

                        //电脑不出闪结束万箭齐发结算
                        game_state = Sanguosha_GameState.State_Playing;
                        game_round = Sanguosha_Round.Round_Player1;
                        game_stage = Sanguosha_Stage.Stage_Chupai;
                        game_jiesuan = Sanguosha_Jiesuan.Jiesuan_None;
                        #region 更新玩家和电脑状态
                        UpdatePlayerStatus();
                        UpdateDiannaoStatus();
                        StartCoroutine(UpdateUsedCards());
                        Jiesuan_Num = 0;
                        #endregion
                    }
                }
                break;
            #endregion
            case Sanguosha_Jiesuan.Jiesuan_Wanjianqifa:
                #region 结算万箭齐发（类似杀）
                //这里也随便写写，大概就是有闪出闪好了
                for (int i = 0; i < Player2_Cards.Count; ++i)
                {
                    if (Player2_Cards[i].Definition.CardName == "Shan")
                    {
                        Debug.Log("出闪");
                        Player2_Chupai(i);

                        //状态机重新转回出牌阶段
                        game_state = Sanguosha_GameState.State_Playing;
                        game_round = Sanguosha_Round.Round_Player1;
                        game_stage = Sanguosha_Stage.Stage_Chupai;
                        game_jiesuan = Sanguosha_Jiesuan.Jiesuan_None;

                        #region 更新玩家和电脑状态
                        UpdatePlayerStatus();
                        UpdateDiannaoStatus();
                        StartCoroutine(UpdateUsedCards());
                        Jiesuan_Num = 0;
                        #endregion
                        break;
                    }
                    if (i == Player2_Cards.Count - 1)
                    {
                        Debug.Log("掉血");
                        player2_tili -= 1;

                        //电脑不出闪结束杀结算
                        game_state = Sanguosha_GameState.State_Playing;
                        game_round = Sanguosha_Round.Round_Player1;
                        game_stage = Sanguosha_Stage.Stage_Chupai;
                        game_jiesuan = Sanguosha_Jiesuan.Jiesuan_None;
                        #region 更新玩家和电脑状态
                        UpdatePlayerStatus();
                        UpdateDiannaoStatus();
                        StartCoroutine(UpdateUsedCards());
                        Jiesuan_Num = 0;
                        #endregion
                    }
                }
                break;
                #endregion
            case Sanguosha_Jiesuan.Jiesuan_Nanmanruqin:
                #region 结算南蛮入侵
                for (int i = 0; i < Player2_Cards.Count; ++i)
                {
                    if (Player2_Cards[i].Definition.CardName == "Sha")
                    {
                        Debug.Log("出杀");
                        Player2_Chupai(i);

                        //状态机重新转回出牌阶段
                        game_state = Sanguosha_GameState.State_Playing;
                        game_round = Sanguosha_Round.Round_Player1;
                        game_stage = Sanguosha_Stage.Stage_Chupai;
                        game_jiesuan = Sanguosha_Jiesuan.Jiesuan_None;

                        #region 更新玩家和电脑状态
                        UpdatePlayerStatus();
                        UpdateDiannaoStatus();
                        StartCoroutine(UpdateUsedCards());
                        Jiesuan_Num = 0;
                        #endregion
                        break;
                    }
                    if (i == Player2_Cards.Count - 1)
                    {
                        Debug.Log("掉血");
                        player2_tili -= 1;

                        //电脑不出杀结束南蛮入侵结算
                        game_state = Sanguosha_GameState.State_Playing;
                        game_round = Sanguosha_Round.Round_Player1;
                        game_stage = Sanguosha_Stage.Stage_Chupai;
                        game_jiesuan = Sanguosha_Jiesuan.Jiesuan_None;
                        #region 更新玩家和电脑状态
                        UpdatePlayerStatus();
                        UpdateDiannaoStatus();
                        StartCoroutine(UpdateUsedCards());
                        Jiesuan_Num = 0;
                        #endregion
                    }
                }
                #endregion
                break;
            case Sanguosha_Jiesuan.Jiesuan_Juedou:
                #region 结算决斗
                //这里也是随便写写，大概就是被决斗了有杀出杀
                for (int i = 0; i < Player2_Cards.Count; ++i)
                {
                    if (Player2_Cards[i].Definition.CardName == "Sha")
                    {
                        Debug.Log("出杀");
                        Player2_Chupai(i);

                        //状态机重新转回玩家
                        game_state = Sanguosha_GameState.State_Resolving_Player1;
                        game_round = Sanguosha_Round.Round_Player1;
                        game_stage = Sanguosha_Stage.Stage_Chupai;
                        game_jiesuan = Sanguosha_Jiesuan.Jiesuan_Juedou;

                        #region 更新玩家和电脑状态
                        UpdatePlayerStatus();
                        UpdateDiannaoStatus();
                        #endregion
                        break;
                    }
                    if (i == Player2_Cards.Count - 1)
                    {
                        Debug.Log("掉血");
                        player2_tili -= 1;

                        //电脑不出杀结束决斗结算
                        game_state = Sanguosha_GameState.State_Resolving_Player1;
                        game_round = Sanguosha_Round.Round_Player1;
                        game_stage = Sanguosha_Stage.Stage_Chupai;
                        game_jiesuan = Sanguosha_Jiesuan.Jiesuan_None;
                        #region 更新玩家和电脑状态
                        UpdatePlayerStatus();
                        UpdateDiannaoStatus();
                        StartCoroutine(UpdateUsedCards());
                        Jiesuan_Num = 0;
                        #endregion
                    }
                }
                break;
                #endregion
        }
        #region 更新玩家和电脑状态
        UpdatePlayerStatus();
        UpdateDiannaoStatus();
        #endregion
    }

    void Resolving_Player1_Process(string msg)
    {
        switch (msg)
        {
            case "Shoupai0":
            case "Shoupai1":
            case "Shoupai2":
            case "Shoupai3":
            case "Shoupai4":
            case "Shoupai5":
            case "Shoupai6":
            case "Shoupai7":
            case "Shoupai8":
            case "Shoupai9":
            case "Shoupai10":
            case "Shoupai11":
            case "Shoupai12":
            case "Shoupai13":
            case "Shoupai14":
            case "Shoupai15":
            case "Shoupai16":
            case "Shoupai17":
            case "Shoupai18":
            case "Shoupai19":
            case "Shoupai20":
            case "Shoupai21":
            case "Shoupai22":
                if (game_state == Sanguosha_GameState.State_Resolving_Player1)
                {
                    if (Card_Select != -1)
                    {
                        Player1_Cards[Card_Select].transform.position = new Vector3(Player1_Cards[Card_Select].transform.position.x, Player1_Cards[Card_Select].transform.position.y - 1, Player1_Cards[Card_Select].transform.position.z);
                    }
                    Card_Select = int.Parse(msg.Substring(7));
                    Player1_Cards[Card_Select].transform.position = new Vector3(Player1_Cards[Card_Select].transform.position.x, Player1_Cards[Card_Select].transform.position.y + 1, Player1_Cards[Card_Select].transform.position.z);
                }
                break;
            case "Queding":
                #region 确定按键
                if (Card_Select != -1)
                {
                    Card tmpCard = Player1_Cards[Card_Select];
                    switch (game_jiesuan)
                    {
                        case Sanguosha_Jiesuan.Jiesuan_Juedou:
                            if (Player1_Cards[Card_Select].Definition.CardName == "Sha")
                            {
                                Player1_Cards.RemoveAt(Card_Select);
                                Used_Cards.Add(tmpCard);
                                tmpCard.transform.position = new Vector3(2 - Jiesuan_Num * Shoupai_Juli, 1, 0);

                                //状态机变化
                                game_state = Sanguosha_GameState.State_Resolving_Player2;
                                game_round = Sanguosha_Round.Round_Player1;
                                game_stage = Sanguosha_Stage.Stage_Chupai;
                                game_jiesuan = Sanguosha_Jiesuan.Jiesuan_Juedou;

                                Card_Select = -1;
                                Jiesuan_Num += 1;

                                #region 更新玩家和电脑状态
                                UpdatePlayerStatus();
                                UpdateDiannaoStatus();
                                #endregion

                                State_Handler("");
                            }
                            {
                                Debug.Log("请出杀");
                            }
                            break;
                    }
                }
                #endregion
                break;
            case "Quxiao":
                if (game_state == Sanguosha_GameState.State_Resolving_Player1)
                {
                    if (Card_Select != -1)
                    {
                        Player1_Cards[Card_Select].transform.position = new Vector3(Player1_Cards[Card_Select].transform.position.x, Player1_Cards[Card_Select].transform.position.y - 1, Player1_Cards[Card_Select].transform.position.z);
                    }
                    Card_Select = -1;
                    switch (game_jiesuan)
                    {
                        case Sanguosha_Jiesuan.Jiesuan_Juedou:
                            Debug.Log("决斗对玩家造成伤害");

                            player1_tili -= 1;

                            game_state = Sanguosha_GameState.State_Playing;
                            game_round = Sanguosha_Round.Round_Player1;
                            game_stage = Sanguosha_Stage.Stage_Chupai;
                            game_jiesuan = Sanguosha_Jiesuan.Jiesuan_None;

                            #region 更新玩家和电脑状态
                            UpdatePlayerStatus();
                            UpdateDiannaoStatus();
                            StartCoroutine(UpdateUsedCards());
                            Jiesuan_Num = 0;
                            #endregion
                            break;
                    }
                }
                break;
            case "Jieshu":
                break;
        }
    }

    void Player2_Chupai(int index)
    {
        Card tmpcard = Player2_Cards[index];
        Player2_Cards.RemoveAt(index);
        tmpcard.transform.position = new Vector3(2 - Jiesuan_Num * Shoupai_Juli, 1, 0);
        Jiesuan_Num += 1;
        Used_Cards.Add(tmpcard);
    }

    bool JinengTrigger(string msg)
    {
        switch (game_state)
        {
            case Sanguosha_GameState.State_Playing:
                switch (game_round)
                {
                    case Sanguosha_Round.Round_Invalid:
                        break;
                    case Sanguosha_Round.Round_Player1:
                        switch (game_stage)
                        {
                            case Sanguosha_Stage.Stage_Invalid:
                                break;
                            case Sanguosha_Stage.Stage_Zhunbei:
                                #region 准备阶段
                                switch (string_player1_wujiang)
                                {   
                                    case "Zhenji":
                                        #region 甄姬洛神
                                        switch (msg)
                                        {
                                            case "":
                                                Sanguosha_Text_Message.SetActive(true);
                                                Sanguosha_Text_Message.GetComponent<TextMesh>().text = "";
                                                Sanguosha_Text_Message.GetComponent<TextMesh>().text = "是否发动洛神？";
                                                return true;
                                            case "Queding":
                                                Card tmpCard;
                                                tmpCard = Panding();
                                                //触发改判
                                                //game_state = Sanguosha_GameState.State_Resolving_Player1;
                                                //JinengTrigger("Panding");
                                                if (tmpCard.Definition.Huase == "Heitao" || tmpCard.Definition.Huase == "Meihua")
                                                {
                                                    Player1_Cards.Add(tmpCard);
                                                    StartCoroutine(UpdateUsedCards());
                                                    UpdateDiannaoStatus();
                                                    UpdatePlayerStatus();
                                                    return true;
                                                }
                                                else
                                                {
                                                    Used_Cards.Add(tmpCard);
                                                    StartCoroutine(UpdateUsedCards());
                                                    UpdateDiannaoStatus();
                                                    UpdatePlayerStatus();
                                                    Sanguosha_Text_Message.SetActive(false);
                                                    return false;
                                                }
                                            case "Quxiao":
                                                StartCoroutine(UpdateUsedCards());
                                                UpdateDiannaoStatus();
                                                UpdatePlayerStatus();
                                                Sanguosha_Text_Message.SetActive(false);
                                                return false;
                                        }

                                        return true;
                                    #endregion
                                    case "Zhugeliang":
                                        #region 诸葛亮观星
                                        break;
                                    #endregion
                                    default:
                                        break;
                                }
                                break;
                            #endregion
                            case Sanguosha_Stage.Stage_Panding:
                                #region 判定阶段
                                switch (string_player1_wujiang)
                                {
                                    //是郭嘉的话直接返回真，在函数外面获得那张判定牌
                                    case "Guojia":
                                        //return true;
                                    default:
                                        return false;
                                }
                                #endregion
                            case Sanguosha_Stage.Stage_Mopai:
                                break;
                            case Sanguosha_Stage.Stage_Chupai:
                                break;
                            case Sanguosha_Stage.Stage_Qipai:
                                break;
                            case Sanguosha_Stage.Stage_Jieshu:
                                break;
                        }
                        break;
                    case Sanguosha_Round.Round_Player2:
                        switch (game_stage)
                        {
                            case Sanguosha_Stage.Stage_Invalid:
                                break;
                            case Sanguosha_Stage.Stage_Zhunbei:
                                break;
                            case Sanguosha_Stage.Stage_Panding:
                                break;
                            case Sanguosha_Stage.Stage_Mopai:
                                break;
                            case Sanguosha_Stage.Stage_Chupai:
                                break;
                            case Sanguosha_Stage.Stage_Qipai:
                                break;
                            case Sanguosha_Stage.Stage_Jieshu:
                                break;
                        }
                        break;
                    default:
                        break;
                }
                break;
            case Sanguosha_GameState.State_Resolving_Player1:
                break;

        }
            
        

        return false;
    }

    void State_Handler(string msg)
    {
        switch (game_state)
        {
            case Sanguosha_GameState.State_Invalid:
                break;
            case Sanguosha_GameState.State_Group1Wins:
                break;
            case Sanguosha_GameState.State_Group2Wins:
                break;
            case Sanguosha_GameState.State_Group3Wins:
                break;
            case Sanguosha_GameState.State_ChooseWujiang:
                Debug.Log("->选择武将");
                StartCoroutine(Xuanzewujiang_Process(msg));
                break;
            case Sanguosha_GameState.State_Resolving:
                break;
            case Sanguosha_GameState.State_Resolving_Player1:
                Debug.Log("->玩家应对");
                Resolving_Player1_Process(msg);
                break;
            case Sanguosha_GameState.State_Resolving_Player2:
                Debug.Log("->电脑应对");
                Resolving_Player2_Process(msg);
                break;
            case Sanguosha_GameState.State_Playing:
                switch (game_round)
                {
                    case Sanguosha_Round.Round_Invalid:
                        break;
                    case Sanguosha_Round.Round_Player1:
                        switch (game_stage)
                        {
                            case Sanguosha_Stage.Stage_Invalid:
                                #region 错误阶段
                                Debug.Log("Error stage");
                                break;
                                #endregion
                            case Sanguosha_Stage.Stage_Zhunbei:
                                #region 开始阶段
                                Debug.Log("->开始阶段");
                                StartCoroutine(Player1_Zhunbei_Process(msg));
                                break;
                                #endregion
                            case Sanguosha_Stage.Stage_Panding:
                                #region 判定阶段
                                Debug.Log("->判定阶段");
                                StartCoroutine(Player1_Panding_Process(msg));
                                break;
                                #endregion
                            case Sanguosha_Stage.Stage_Mopai:
                                #region 摸牌阶段
                                Debug.Log("->摸牌阶段");
                                StartCoroutine(Player1_Mopai_Process(msg));
                                break;
                                #endregion
                            case Sanguosha_Stage.Stage_Chupai:
                                #region 出牌阶段
                                Debug.Log("->出牌阶段");
                                StartCoroutine(Player1_Chupai_Process(msg));
                                break;
                                #endregion
                            case Sanguosha_Stage.Stage_Qipai:
                                #region 弃牌阶段
                                Debug.Log("->弃牌阶段");
                                break;
                                #endregion
                            case Sanguosha_Stage.Stage_Jieshu:
                                #region 结束阶段
                                Debug.Log("->结束阶段");
                                break;
                                #endregion
                        }
                        break;
                    case Sanguosha_Round.Round_Player2:
                        switch (game_stage)
                        {
                            case Sanguosha_Stage.Stage_Invalid:
                                break;
                            case Sanguosha_Stage.Stage_Zhunbei:
                                break;
                            case Sanguosha_Stage.Stage_Panding:
                                break;
                            case Sanguosha_Stage.Stage_Mopai:
                                break;
                            case Sanguosha_Stage.Stage_Chupai:
                                break;
                            case Sanguosha_Stage.Stage_Qipai:
                                break;
                            case Sanguosha_Stage.Stage_Jieshu:
                                break;
                        }
                        break;
                }
                break;
        }
    }

    public void OnButton(string msg)
	{
		Debug.Log("OnButton = "+msg);
		switch (msg)
		{
            case "Restart":
                StartCoroutine(OnRestart());
                break;
            case "Queding":
                State_Handler(msg);
                break;
            case "Quxiao":
                State_Handler(msg);
                break;
            case "Jieshu":
                State_Handler(msg);
                break;
            case "Wujiang1":
            case "Wujiang2":
            case "Wujiang3":
                State_Handler(msg);
                break;
            case "Shoupai0":
            case "Shoupai1":
            case "Shoupai2":
            case "Shoupai3":
            case "Shoupai4":
            case "Shoupai5":
            case "Shoupai6":
            case "Shoupai7":
            case "Shoupai8":
            case "Shoupai9":
            case "Shoupai10":
            case "Shoupai11":
            case "Shoupai12":
            case "Shoupai13":
            case "Shoupai14":
            case "Shoupai15":
            case "Shoupai16":
            case "Shoupai17":
            case "Shoupai18":
            case "Shoupai19":
            case "Shoupai20":
            case "Shoupai21":
            case "Shoupai22":
                State_Handler(msg);
                break;
        }
	}
}
