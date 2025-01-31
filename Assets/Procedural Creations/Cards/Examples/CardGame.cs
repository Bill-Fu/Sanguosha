using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class CardGame : MonoBehaviour
{
    //体力最大值为4，目前规则下是这样子的
    #region 常量定义
    const int Tili_Max = 4;
    const int Button_Max = 9;
    const int Xuanjiang_Max = 3;
    const int Shoupai_Max = 23;
    const float Shoupai_Juli = 2.5f;
    const int Shoupaiperround = 2;
    const float FlyTime = 0.4f;
    const float DealTime = 0.4f;
    const float StayTime = 2f;
    const float QiehuanTime = 0f;
    const int Biyue = 1;
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
    bool Player2_ShaSign = false;

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
        State_Resolving_Player1,
        State_Resolving_Player2,
        State_Binsi_Player1,
        State_Binsi_Player2,
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

    Sanguosha_GameState pre_state;//只在判断濒死状态的时候用一下
    Sanguosha_Round pre_round;
    #endregion

    #region 玩家手牌列表和选将框列表定义
    //玩家选将列表武将卡牌
    List<Card> Xuanjiang_Cards = new List<Card>();
    //存放使用过的手牌
    List<Card> Used_Cards = new List<Card>();
    //存放弃牌列表
    List<Card> Qipai_Cards = new List<Card>();
    //存放弃牌在手牌列表中的序号
    List<int> Qipai_Index;
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
    GameObject Sanguosha_Image_Playerwin;
    GameObject Sanguosha_Image_Diannaowin;
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
    //玩家1区域卡牌定义
    Card player1_panding1_card = null;
    Card player1_panding2_card = null;
    Card player1_wuqi_card = null;
    Card player1_fangju_card = null;
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
    //玩家2区域卡牌定义
    Card player2_panding1_card = null;
    Card player2_panding2_card = null;
    Card player2_wuqi_card = null;
    Card player2_fangju_card = null;
    //玩家2手牌血量定义
    int player2_manxue;
    int player2_tili;
    int player2_shoupaishangxian;
    int player2_shoupai;
    #endregion
    
    void Start()
    {
        //设置全屏
        Screen.SetResolution(1366, 768, false);
        //设置初始状态
        #region 设置初始状态为 游戏状态：无效状态 游戏回合：无效回合 游戏阶段：无效阶段
        game_state = Sanguosha_GameState.State_Invalid;
        game_round = Sanguosha_Round.Round_Invalid;
        game_stage = Sanguosha_Stage.Stage_Invalid;
        game_jiesuan = Sanguosha_Jiesuan.Jiesuan_None;
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
        Sanguosha_Buttons[6] = this.transform.Find("Player2_Wuqi_Button").gameObject;
        Sanguosha_Buttons[7] = this.transform.Find("Player2_Fangju_Button").gameObject;
        Sanguosha_Buttons[8] = this.transform.Find("Player2_Shoupai_Button").gameObject;
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
        Sanguosha_Image_Playerwin = this.transform.Find("Canvas/Player_Win").gameObject;
        Sanguosha_Image_Diannaowin = this.transform.Find("Canvas/Diannao_Win").gameObject;
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
        Sanguosha_Image_Playerwin.SetActive(false);
        Sanguosha_Image_Diannaowin.SetActive(false);
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

    //就我这部分代码是想通过键盘控制按三个button
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
        foreach(Card card in Used_Cards)
        {
            GameObject.DestroyImmediate(card.gameObject);
        }
        foreach (Card card in Xuanjiang_Cards)
        {
            GameObject.DestroyImmediate(card.gameObject);
        }
        foreach(Card card in Qipai_Cards)
        {
            GameObject.DestroyImmediate(card.gameObject);
        }

        //清空玩家1区域卡牌
        if (player1_panding1_card != null)
        {
            GameObject.DestroyImmediate(player1_panding1_card.gameObject);
            player1_panding1_card = null;
        }

        if (player1_panding2_card != null)
        {
            GameObject.DestroyImmediate(player1_panding2_card.gameObject);
            player1_panding2_card = null;
        }

        if (player1_wuqi_card != null)
        {
            GameObject.DestroyImmediate(player1_wuqi_card.gameObject);
            player1_wuqi_card = null;
        }

        if (player1_fangju_card != null)
        {
            GameObject.DestroyImmediate(player1_fangju_card.gameObject);
            player1_fangju_card = null;
        }

        //清空玩家2区域卡牌
        if (player2_panding1_card != null)
        {
            GameObject.DestroyImmediate(player2_panding1_card.gameObject);
            player2_panding1_card = null;
        }

        if (player2_panding2_card != null)
        {
            GameObject.DestroyImmediate(player2_panding2_card.gameObject);
            player2_panding2_card = null;
        }

        if (player2_wuqi_card != null)
        {
            GameObject.DestroyImmediate(player2_wuqi_card.gameObject);
            player2_wuqi_card = null;
        }

        if (player2_fangju_card != null)
        {
            GameObject.DestroyImmediate(player2_fangju_card.gameObject);
            player2_fangju_card = null;
        }

        Player1_Cards.Clear();
        Player2_Cards.Clear();
        Used_Cards.Clear();
        Xuanjiang_Cards.Clear();
        Qipai_Cards.Clear();

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
        if (tmpCard == null)
        {
            Deck.RenewCard(Used_Cards);
        }

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
        if (tmpCard == null)
        {
            Deck.RenewCard(Used_Cards);
        }

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
            Sanguosha_Image_Playerwin.SetActive(false);
            Sanguosha_Image_Diannaowin.SetActive(false);
            //所有的手牌按键设置为不启用
            for (int i = 0; i < Shoupai_Max; ++i)
            {
                Sanguosha_Shoupai[i].SetActive(false);
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
    //摸牌阶段给玩家摸牌的函数
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

        #region 更新状态
        UpdatePlayerStatus();
        UpdateDiannaoStatus();
        StartCoroutine(UpdateUsedCards());
        #endregion
    }
    //摸牌阶段给电脑摸牌的函数
    IEnumerator DiannaoMopai(int MoPaiNum)
    {
        if (game_state == Sanguosha_GameState.State_Playing && game_round == Sanguosha_Round.Round_Player2)
        {
            for (int i = 0; i < MoPaiNum; ++i)
            {
                Diannao_Fapai();
                UpdatePlayerStatus();
                UpdateDiannaoStatus();
                yield return new WaitForSeconds(DealTime);
            }
        }

        #region 更新状态
        UpdatePlayerStatus();
        UpdateDiannaoStatus();
        StartCoroutine(UpdateUsedCards());
        #endregion
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
    //现在也要用来判断是否有角色死亡，游戏结束
    void UpdatePlayerStatus()
    {
        Debug.Log("剩余手牌堆数量" + Deck.RestCards().ToString());

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
        if (player1_wuqi_card != null)
        {
            player1_wuqi_card.transform.position = new Vector3(20, 0, 0);
        }
        if (player1_fangju_card != null)
        {
            player1_fangju_card.transform.position = new Vector3(20, 0, 0);
        }
        #endregion

        #region 更新玩家判定状态
        Sanguosha_Text_Player1_Panding1.GetComponent<TextMesh>().text = string_player1_panding1;
        Sanguosha_Text_Player1_Panding2.GetComponent<TextMesh>().text = string_player1_panding2;
        if (player1_panding1_card != null)
        {
            player1_panding1_card.transform.position = new Vector3(20, 0, 0);
            if (player1_panding2_card != null)
            {
                player1_panding2_card.transform.position = new Vector3(20, 0, 0);
            }
        }
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
        Debug.Log("玩家手牌数量" + Player1_Cards.Count);
        #endregion

        #region 更新玩家手牌状态
        SortShoupai();
        #endregion

        #region 判断玩家1是否濒死
        if (player1_tili == 0)
        {
            pre_state = game_state;
            game_state = Sanguosha_GameState.State_Binsi_Player1;

            State_Handler("");

        }
        #endregion
    }

    //更新电脑的装备状态，判定状态，手牌状态和体力状态
    void UpdateDiannaoStatus()
    {
        Debug.Log("剩余手牌堆数量" + Deck.RestCards().ToString());

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
        if (player2_wuqi_card != null)
        {
            player2_wuqi_card.transform.position = new Vector3(20, 0, 0);
        }
        if (player2_fangju_card != null)
        {
            player2_fangju_card.transform.position = new Vector3(20, 0, 0);
        }
        #endregion

        #region 更新玩家判定状态
        Sanguosha_Text_Player2_Panding1.GetComponent<TextMesh>().text = string_player2_panding1;
        Sanguosha_Text_Player2_Panding2.GetComponent<TextMesh>().text = string_player2_panding2;
        if (player2_panding1_card != null)
        {
            player2_panding1_card.transform.position = new Vector3(20, 0, 0);
            if (player2_panding2_card != null)
            {
                player2_panding2_card.transform.position = new Vector3(20, 0, 0);
            }
        }
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
        Debug.Log("电脑手牌数量" + Player2_Cards.Count);
        #endregion

        #region 更新电脑手牌
        SortDiannaoShoupai();
        #endregion

        #region 判断玩家2是否濒死
        if (player2_tili == 0)
        {
            pre_state = game_state;
            game_state = Sanguosha_GameState.State_Binsi_Player2;
            
            State_Handler("");
            
        }
        #endregion
    }

    IEnumerator UpdateUsedCards()
    {
        yield return new WaitForSeconds(DealTime);
        for(int i = 0; i < Used_Cards.Count; ++i)
        {
            Used_Cards[i].transform.position = new Vector3(20, 0, 0);
        }
        Debug.Log("弃牌堆卡牌数量" + Used_Cards.Count.ToString());
    }

    //根据手牌数量更新牌的位置和Buttons的位置
    void SortDiannaoShoupai()
    {
        for(int i = 0; i < Player2_Cards.Count; ++i)
        {
            Player2_Cards[i].transform.position = new Vector3(0, 6, 1);
        }
    }

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
        //所有功能button都启用除了最后两个对面武将装备以及手牌的按键，只在过河拆桥和顺手牵羊的时候使用
        for (int i = 1; i < Sanguosha_Buttons.Length - 3; ++i)
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
        game_round = Sanguosha_Round.Round_Player2;
        game_stage = Sanguosha_Stage.Stage_Zhunbei;
        Huihe_Num = 1;
        #endregion

        State_Handler("");
    }
    
    void EndGame(string msg)
    {
        switch (msg)
        {
            case "Playerwin":
                Sanguosha_Image_Playerwin.SetActive(true);
                break;
            case "Diannaowin":
                Sanguosha_Image_Diannaowin.SetActive(true);
                break;
        }
    }

    IEnumerator Binsi_Player2_Process(string msg)
    {
        Jiesuan_Num = 0;

        for (int i = 0; i < Player2_Cards.Count; ++i)
        {
            if (Player2_Cards[i].Definition.CardName == "Tao")
            {
                Debug.Log("电脑濒死出桃");
                Player2_Chupai(i);

                yield return new WaitForSeconds(StayTime);

                player2_tili++;

                //状态机重新转回出牌阶段
                game_state = pre_state;
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
                Debug.Log("电脑无桃可出死亡，游戏结束，玩家胜利");

                //电脑不出闪结束万箭齐发结算
                game_state = Sanguosha_GameState.State_Group1Wins;
                game_round = Sanguosha_Round.Round_Player1;
                game_stage = Sanguosha_Stage.Stage_Chupai;
                game_jiesuan = Sanguosha_Jiesuan.Jiesuan_None;

                //这里直接跳游戏结束画面
                EndGame("Playerwin");
            }
        }

        if (Player2_Cards.Count == 0)
        {
            Debug.Log("电脑无桃可出死亡，游戏结束，玩家胜利!!!");

            //电脑不出闪结束万箭齐发结算
            game_state = Sanguosha_GameState.State_Group1Wins;

            //电脑不出闪结束万箭齐发结算
            game_state = Sanguosha_GameState.State_Group1Wins;
            game_round = Sanguosha_Round.Round_Player1;
            game_stage = Sanguosha_Stage.Stage_Chupai;
            game_jiesuan = Sanguosha_Jiesuan.Jiesuan_None;

            //这里直接跳游戏结束画面
            EndGame("Playerwin");
        }

        yield return new WaitForSeconds(QiehuanTime);
    }

    IEnumerator Binsi_Player1_Process(string msg)
    {
        Debug.Log("玩家无桃可出死亡，游戏结束，电脑胜利");
        
        game_state = Sanguosha_GameState.State_Group2Wins;
        game_round = Sanguosha_Round.Round_Player2;
        game_stage = Sanguosha_Stage.Stage_Chupai;
        game_jiesuan = Sanguosha_Jiesuan.Jiesuan_None;

        //这里直接跳游戏结束画面
        EndGame("Diannaowin");

        yield return new WaitForSeconds(QiehuanTime);
    }

    IEnumerator Player1_Zhunbei_Process(string msg)
    {
        #region 准备阶段
        //主要是觉得不能放在要被重复调用的Chupai处理函数中
        
        //一些状态可以在这里初始化，如杀的使用次数
        //player1_tili--;
        Player1_ShaSign = false;
        Card_Select = -1;

        #region 更新玩家和电脑状态
        UpdatePlayerStatus();
        UpdateDiannaoStatus();
        //StartCoroutine(UpdateUsedCards());
        #endregion

        #region 状态机变化
        game_state = Sanguosha_GameState.State_Playing;
        game_round = Sanguosha_Round.Round_Player1;
        game_stage = Sanguosha_Stage.Stage_Panding;
        game_jiesuan = Sanguosha_Jiesuan.Jiesuan_None;
        #endregion
        
        yield return new WaitForSeconds(QiehuanTime);

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
            Used_Cards.Add(player1_panding2_card);
            player1_panding2_card = null;
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
            Used_Cards.Add(player1_panding1_card);
            player1_panding1_card = null;
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
            game_jiesuan = Sanguosha_Jiesuan.Jiesuan_None;

            if (player1_shoupai > player1_shoupaishangxian)
            {
                Qipai_Cards = new List<Card>(player1_shoupai - player1_shoupaishangxian);
                Qipai_Index = new List<int>(player1_shoupai - player1_shoupaishangxian);
            }
            else
            {
                State_Handler("");
            }
        }
        else
        {
            game_state = Sanguosha_GameState.State_Playing;
            game_round = Sanguosha_Round.Round_Player1;
            game_stage = Sanguosha_Stage.Stage_Chupai;
            game_jiesuan = Sanguosha_Jiesuan.Jiesuan_None;

            State_Handler("");
        }

        Player1_LebusishuSign = false;
        #endregion



        yield return new WaitForSeconds(QiehuanTime);
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
                                Debug.Log("玩家出杀");
                                if (Player1_ShaSign == false || string_player1_wuqi == "诸葛连弩" || string_player1_wujiang=="Zhangfei")
                                {
                                    Player1_Cards.RemoveAt(Card_Select);
                                    tmpCard.transform.position = new Vector3(2 - Jiesuan_Num * Shoupai_Juli, 1, 0);
                                    Used_Cards.Add(tmpCard);

                                    Jiesuan_Num++;
                                    Card_Select = -1;
                                    Player1_ShaSign = true;

                                    if (string_player2_fangju != "防具" && string_player1_wuqi!="青釭剑")
                                    {
                                        if (string_player2_fangju == "仁王盾")
                                        {
                                            if (tmpCard.Definition.Huase == "Meihua" || tmpCard.Definition.Huase == "Heitao")
                                            {
                                                //如果是梅花和黑桃的话则触发仁王盾效果，直接结束
                                                Debug.Log("电脑发动仁王盾效果");

                                                #region 更新玩家和电脑状态
                                                UpdatePlayerStatus();
                                                UpdateDiannaoStatus();
                                                #endregion

                                                Jiesuan_Num = 0;
                                            }
                                            else
                                            {
                                                //如果是红杀的话则直接忽略跟正常一样结算就好了
                                                //状态机变化
                                                game_state = Sanguosha_GameState.State_Resolving_Player2;
                                                game_round = Sanguosha_Round.Round_Player1;
                                                game_stage = Sanguosha_Stage.Stage_Chupai;
                                                game_jiesuan = Sanguosha_Jiesuan.Jiesuan_Sha;

                                                #region 更新玩家和电脑状态
                                                UpdatePlayerStatus();
                                                UpdateDiannaoStatus();
                                                #endregion
                                                State_Handler("");
                                                Jiesuan_Num = 0;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        //状态机变化
                                        game_state = Sanguosha_GameState.State_Resolving_Player2;
                                        game_round = Sanguosha_Round.Round_Player1;
                                        game_stage = Sanguosha_Stage.Stage_Chupai;
                                        game_jiesuan = Sanguosha_Jiesuan.Jiesuan_Sha;
                                        
                                        #region 更新玩家和电脑状态
                                        UpdatePlayerStatus();
                                        UpdateDiannaoStatus();
                                        #endregion
                                        State_Handler("");
                                        Jiesuan_Num = 0;
                                    }
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
                                    Debug.Log("玩家出桃");
                                    Player1_Cards.RemoveAt(Card_Select);
                                    tmpCard.transform.position = new Vector3(2 - Jiesuan_Num * Shoupai_Juli, 1, 0);
                                    Used_Cards.Add(tmpCard);
                                    Jiesuan_Num++;

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
                                Debug.Log("玩家出无中生有");
                                Player1_Cards.RemoveAt(Card_Select);
                                tmpCard.transform.position = new Vector3(2 - Jiesuan_Num * Shoupai_Juli, 1, 0);
                                Used_Cards.Add(tmpCard);
                                Jiesuan_Num++;

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
                                Debug.Log("玩家出万箭齐发");
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
                                Jiesuan_Num = 0;
                                break;
                                #endregion
                            case "Nanmanruqin":
                                #region 出牌阶段出南蛮入侵
                                Debug.Log("玩家出南蛮入侵");
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
                                Jiesuan_Num = 0;
                                break;
                                #endregion
                            case "Juedou":
                                #region 出牌阶段出决斗
                                Debug.Log("玩家出决斗");
                                Player1_Cards.RemoveAt(Card_Select);
                                tmpCard.transform.position = new Vector3(2 - Jiesuan_Num * Shoupai_Juli, 1, 0);
                                Used_Cards.Add(tmpCard);

                                //状态机变化
                                game_state = Sanguosha_GameState.State_Resolving_Player2;
                                game_stage = Sanguosha_Stage.Stage_Chupai;
                                game_jiesuan = Sanguosha_Jiesuan.Jiesuan_Juedou;

                                Card_Select = -1;
                                Jiesuan_Num += 1;

                                #region 更新玩家和电脑状态
                                UpdatePlayerStatus();
                                UpdateDiannaoStatus();
                                #endregion
                                State_Handler("");
                                Jiesuan_Num = 0;
                                break;
                            #endregion
                            case "Guohechaiqiao":
                                #region 出牌阶段出过河拆桥
                                if(Player2_Cards.Count!=0 || string_player2_wuqi!="武器" || string_player2_fangju != "防具")
                                {
                                    Debug.Log("玩家出过河拆桥");
                                    Player1_Cards.RemoveAt(Card_Select);
                                    tmpCard.transform.position = new Vector3(2 - Jiesuan_Num * Shoupai_Juli, 1, 0);
                                    Used_Cards.Add(tmpCard);


                                    //状态机变化
                                    game_state = Sanguosha_GameState.State_Resolving_Player1;
                                    game_round = Sanguosha_Round.Round_Player1;
                                    game_stage = Sanguosha_Stage.Stage_Chupai;
                                    game_jiesuan = Sanguosha_Jiesuan.Jiesuan_Guohechaiqiao;

                                    Card_Select = -1;
                                    Jiesuan_Num++;

                                    #region 更新玩家和电脑状态
                                    UpdatePlayerStatus();
                                    UpdateDiannaoStatus();
                                    #endregion

                                    //启用相应选择的button
                                    if (Player2_Cards.Count != 0)
                                    {
                                        Sanguosha_Buttons[8].SetActive(true);
                                    }

                                    if (string_player2_wuqi != "武器")
                                    {
                                        Sanguosha_Buttons[6].SetActive(true);
                                    }

                                    if (string_player2_fangju != "防具")
                                    {
                                        Sanguosha_Buttons[7].SetActive(true);
                                    }
                                }
                                else
                                {
                                    Debug.Log("对面没有可供过河拆桥的牌");
                                }
                                #endregion
                                break;
                            case "Shunshouqianyang":
                                #region 出牌阶段出顺手牵羊
                                if (Player2_Cards.Count != 0 || string_player2_wuqi != "武器" || string_player2_fangju != "防具")
                                {
                                    Debug.Log("玩家出顺手牵羊");
                                    Player1_Cards.RemoveAt(Card_Select);
                                    tmpCard.transform.position = new Vector3(2 - Jiesuan_Num * Shoupai_Juli, 1, 0);
                                    Used_Cards.Add(tmpCard);


                                    //状态机变化
                                    game_state = Sanguosha_GameState.State_Resolving_Player1;
                                    game_round = Sanguosha_Round.Round_Player1;
                                    game_stage = Sanguosha_Stage.Stage_Chupai;
                                    game_jiesuan = Sanguosha_Jiesuan.Jiesuan_Shunshouqianyang;

                                    Card_Select = -1;
                                    Jiesuan_Num++;

                                    #region 更新玩家和电脑状态
                                    UpdatePlayerStatus();
                                    UpdateDiannaoStatus();
                                    #endregion

                                    //启用相应选择的button
                                    if (Player2_Cards.Count != 0)
                                    {
                                        Sanguosha_Buttons[8].SetActive(true);
                                    }

                                    if (string_player2_wuqi != "武器")
                                    {
                                        Sanguosha_Buttons[6].SetActive(true);
                                    }

                                    if (string_player2_fangju != "防具")
                                    {
                                        Sanguosha_Buttons[7].SetActive(true);
                                    }
                                }
                                else
                                {
                                    Debug.Log("对面没有可供顺手牵羊的牌");
                                }

                                #endregion
                                break;
                            case "Lebusishu":
                                Debug.Log("玩家出乐不思蜀");
                                #region 出牌阶段出乐不思蜀
                                Player1_Cards.RemoveAt(Card_Select);
                                tmpCard.transform.position = new Vector3(2 - Jiesuan_Num * Shoupai_Juli, 1, 0);
                                
                                //因为是非延时锦囊牌，实际上就是暂时贴到对方玩家那里了，先不放到弃牌堆，等到结算的时候再放入弃牌堆

                                Jiesuan_Num++;

                                if (string_player2_panding1 == "1")
                                {
                                    string_player2_panding1 = "乐";
                                    player2_panding1_card = tmpCard;
                                }
                                else
                                {
                                    string_player2_panding2 = "乐";
                                    player2_panding2_card = tmpCard;
                                }

                                Card_Select = -1;
                                Jiesuan_Num = 0;

                                yield return new WaitForSeconds(StayTime);

                                #region 更新玩家和电脑状态
                                UpdatePlayerStatus();
                                UpdateDiannaoStatus();

                                #endregion
                                #endregion
                                break;
                            case "Bingliangcunduan":
                                #region 出牌阶段出兵粮寸断
                                Debug.Log("玩家出兵粮寸断");
                                Player1_Cards.RemoveAt(Card_Select);
                                tmpCard.transform.position = new Vector3(2 - Jiesuan_Num * Shoupai_Juli, 1, 0);
                                //因为是非延时锦囊牌，实际上就是暂时贴到对方玩家那里了，先不放到弃牌堆
                                Jiesuan_Num++;

                                if (string_player2_panding1 == "1")
                                {
                                    string_player2_panding1 = "兵";
                                    player2_panding1_card = tmpCard;
                                }
                                else
                                {
                                    string_player2_panding2 = "兵";
                                    player2_panding2_card = tmpCard;
                                }

                                Card_Select = -1;
                                Jiesuan_Num = 0;

                                yield return new WaitForSeconds(StayTime);

                                #region 更新玩家和电脑状态
                                UpdatePlayerStatus();
                                UpdateDiannaoStatus();
                                #endregion
                                
                                #endregion
                                break;
                            case "Zhugeliannu":
                                #region 出牌阶段装备诸葛连弩
                                Debug.Log("玩家装备了诸葛连弩");

                                Player1_Cards.RemoveAt(Card_Select);
                                tmpCard.transform.position = new Vector3(2 - Jiesuan_Num * Shoupai_Juli, 1, 0);
                                Jiesuan_Num++;
                                //因为是装备牌，实际上就是暂时贴到己方玩家那里了，先不放到弃牌堆

                                if (string_player1_wuqi != "武器")
                                {
                                    player1_wuqi_card.transform.position = new Vector3(2 - Jiesuan_Num * Shoupai_Juli, 1, 0);
                                    Used_Cards.Add(player1_wuqi_card);
                                    player1_wuqi_card = tmpCard;
                                    string_player1_wuqi = "诸葛连弩";
                                }
                                else
                                {
                                    player1_wuqi_card = tmpCard;
                                    string_player1_wuqi = "诸葛连弩";
                                }

                                Card_Select = -1;
                                Jiesuan_Num = 0;

                                #region 更新玩家和电脑状态
                                UpdatePlayerStatus();
                                UpdateDiannaoStatus();
                                StartCoroutine(UpdateUsedCards());
                                #endregion

                                #endregion
                                break;
                            case "Guanshifu":
                                #region 出牌阶段装备贯石斧
                                Debug.Log("玩家装备了贯石斧");
                                Player1_Cards.RemoveAt(Card_Select);
                                tmpCard.transform.position = new Vector3(2 - Jiesuan_Num * Shoupai_Juli, 1, 0);
                                Jiesuan_Num++;
                                //因为是装备牌，实际上就是暂时贴到己方玩家那里了，先不放到弃牌堆

                                if (string_player1_wuqi != "武器")
                                {
                                    player1_wuqi_card.transform.position = new Vector3(2 - Jiesuan_Num * Shoupai_Juli, 1, 0);
                                    Used_Cards.Add(player1_wuqi_card);
                                    player1_wuqi_card = tmpCard;
                                    string_player1_wuqi = "贯石斧";
                                }
                                else
                                {
                                    player1_wuqi_card = tmpCard;
                                    string_player1_wuqi = "贯石斧";
                                }

                                Card_Select = -1;
                                Jiesuan_Num = 0;

                                #region 更新玩家和电脑状态
                                UpdatePlayerStatus();
                                UpdateDiannaoStatus();
                                StartCoroutine(UpdateUsedCards());
                                #endregion
                                #endregion
                                break;
                            case "Qinggangjian":
                                #region 出牌阶段装备青釭剑
                                Debug.Log("玩家装备了青釭剑");
                                Player1_Cards.RemoveAt(Card_Select);
                                tmpCard.transform.position = new Vector3(2 - Jiesuan_Num * Shoupai_Juli, 1, 0);
                                Jiesuan_Num++;
                                //因为是装备牌，实际上就是暂时贴到己方玩家那里了，先不放到弃牌堆

                                if (string_player1_wuqi != "武器")
                                {
                                    player1_wuqi_card.transform.position = new Vector3(2 - Jiesuan_Num * Shoupai_Juli, 1, 0);
                                    Used_Cards.Add(player1_wuqi_card);
                                    player1_wuqi_card = tmpCard;
                                    string_player1_wuqi = "青釭剑";
                                }
                                else
                                {
                                    player1_wuqi_card = tmpCard;
                                    string_player1_wuqi = "青釭剑";
                                }

                                Card_Select = -1;
                                Jiesuan_Num = 0;

                                #region 更新玩家和电脑状态
                                UpdatePlayerStatus();
                                UpdateDiannaoStatus();
                                StartCoroutine(UpdateUsedCards());
                                #endregion
                                #endregion
                                break;
                            case "Hanbingjian":
                                Debug.Log("玩家装备了寒冰剑");
                                #region 出牌阶段装备寒冰剑
                                Player1_Cards.RemoveAt(Card_Select);
                                tmpCard.transform.position = new Vector3(2 - Jiesuan_Num * Shoupai_Juli, 1, 0);
                                Jiesuan_Num++;
                                //因为是装备牌，实际上就是暂时贴到己方玩家那里了，先不放到弃牌堆

                                if (string_player1_wuqi != "武器")
                                {
                                    player1_wuqi_card.transform.position = new Vector3(2 - Jiesuan_Num * Shoupai_Juli, 1, 0);
                                    Used_Cards.Add(player1_wuqi_card);
                                    player1_wuqi_card = tmpCard;
                                    string_player1_wuqi = "寒冰剑";
                                }
                                else
                                {
                                    player1_wuqi_card = tmpCard;
                                    string_player1_wuqi = "寒冰剑";
                                }

                                Card_Select = -1;
                                Jiesuan_Num = 0;

                                #region 更新玩家和电脑状态
                                UpdatePlayerStatus();
                                UpdateDiannaoStatus();
                                StartCoroutine(UpdateUsedCards());
                                #endregion

                                #endregion
                                break;
                            case "Baguazhen":
                                Debug.Log("玩家装备了八卦阵");
                                #region 出牌阶段装备八卦阵
                                Player1_Cards.RemoveAt(Card_Select);
                                tmpCard.transform.position = new Vector3(2 - Jiesuan_Num * Shoupai_Juli, 1, 0);
                                Jiesuan_Num++;
                                //因为是装备牌，实际上就是暂时贴到己方玩家那里了，先不放到弃牌堆

                                if (string_player1_fangju != "防具")
                                {
                                    player1_fangju_card.transform.position = new Vector3(2 - Jiesuan_Num * Shoupai_Juli, 1, 0);
                                    Used_Cards.Add(player1_fangju_card);
                                    player1_fangju_card = tmpCard;
                                    string_player1_fangju = "八卦阵";
                                }
                                else
                                {
                                    player1_fangju_card = tmpCard;
                                    string_player1_fangju = "八卦阵";
                                }

                                Card_Select = -1;
                                Jiesuan_Num = 0;

                                #region 更新玩家和电脑状态
                                UpdatePlayerStatus();
                                UpdateDiannaoStatus();
                                StartCoroutine(UpdateUsedCards());
                                #endregion
                                #endregion
                                break;
                            case "Renwangdun":
                                Debug.Log("玩家装备了仁王盾");
                                #region 出牌阶段装备仁王盾
                                Player1_Cards.RemoveAt(Card_Select);
                                tmpCard.transform.position = new Vector3(2 - Jiesuan_Num * Shoupai_Juli, 1, 0);
                                Jiesuan_Num++;
                                //因为是装备牌，实际上就是暂时贴到己方玩家那里了，先不放到弃牌堆

                                if (string_player1_fangju != "防具")
                                {
                                    player1_fangju_card.transform.position = new Vector3(2 - Jiesuan_Num * Shoupai_Juli, 1, 0);
                                    Used_Cards.Add(player1_fangju_card);
                                    player1_fangju_card = tmpCard;
                                    string_player1_fangju = "仁王盾";
                                }
                                else
                                {
                                    player1_fangju_card = tmpCard;
                                    string_player1_fangju = "仁王盾";
                                }

                                Card_Select = -1;
                                Jiesuan_Num = 0;

                                #region 更新玩家和电脑状态
                                UpdatePlayerStatus();
                                UpdateDiannaoStatus();
                                StartCoroutine(UpdateUsedCards());
                                #endregion
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
                    game_jiesuan = Sanguosha_Jiesuan.Jiesuan_None;
                    #endregion

                    if (player1_shoupai > player1_shoupaishangxian)
                    {
                        Qipai_Cards = new List<Card>(player1_shoupai - player1_shoupaishangxian);
                        Qipai_Index = new List<int>(player1_shoupai - player1_shoupaishangxian);
                    }
                    else
                    {
                        State_Handler("");
                    }
                }
                break;
        }

        yield return new WaitForSeconds(QiehuanTime);
    }

    IEnumerator Player1_Qipai_Process(string msg)
    {
        bool sign = false;
        if (game_state == Sanguosha_GameState.State_Playing)
        {
            if (player1_shoupaishangxian < player1_shoupai)
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
                        //先判断来的卡牌是不是在存的要弃牌的列表中
                        int Selected = int.Parse(msg.Substring(7));

                        for (int i = 0; i < Qipai_Index.Count; i++)
                        {
                            //如果点击的牌已经在了，那么将他从记录的List中去掉，然后下降一点
                            if (Qipai_Index[i] == Selected)
                            {
                                Qipai_Cards.RemoveAt(i);
                                Qipai_Index.RemoveAt(i);
                                Player1_Cards[Selected].transform.position = new Vector3(Player1_Cards[Selected].transform.position.x, Player1_Cards[Selected].transform.position.y - 1, Player1_Cards[Selected].transform.position.z);
                                Debug.Log("现在选中了" + Qipai_Cards.Count.ToString() + "张卡牌要弃");
                                sign = true;
                                break;
                            }
                        }

                        if (sign == false)
                        {
                            //然后分两种情况，一种情况是没选超，一种情况是选超了，选超的话需要把List最前面一个去掉然后再最后添加上这个
                            if (Qipai_Index.Count < Qipai_Index.Capacity)
                            {
                                //这种情况就是没有选超，那么只要把选中的那张牌记录下来然后升高一点就好了
                                Qipai_Index.Add(Selected);
                                Qipai_Cards.Add(Player1_Cards[Selected]);
                                //抬高卡牌
                                Player1_Cards[Selected].transform.position = new Vector3(Player1_Cards[Selected].transform.position.x, Player1_Cards[Selected].transform.position.y + 1, Player1_Cards[Selected].transform.position.z);
                                Debug.Log("现在选中了" + Qipai_Cards.Count.ToString() + "张卡牌要弃");
                            }
                            else
                            {
                                //这种情况就是选超了，那么就要把List中的第一张从队列中出来，然后最后一张补上
                                Player1_Cards[Qipai_Index[0]].transform.position = new Vector3(Player1_Cards[Qipai_Index[0]].transform.position.x, Player1_Cards[Qipai_Index[0]].transform.position.y - 1, Player1_Cards[Qipai_Index[0]].transform.position.z);
                                Qipai_Index.RemoveAt(0);
                                Qipai_Cards.RemoveAt(0);

                                Qipai_Index.Add(Selected);
                                Qipai_Cards.Add(Player1_Cards[Selected]);
                                Player1_Cards[Selected].transform.position = new Vector3(Player1_Cards[Selected].transform.position.x, Player1_Cards[Selected].transform.position.y + 1, Player1_Cards[Selected].transform.position.z);
                                Debug.Log("现在选中了" + Qipai_Cards.Count.ToString() + "张卡牌要弃");
                            }
                        }
                        break;
                    case "Queding":
                        if (Qipai_Index.Count == Qipai_Index.Capacity)
                        {
                            Jiesuan_Num = 0;
                            //选中的牌达到了应弃牌的数量
                            foreach(Card card in Qipai_Cards)
                            {
                                Player1_Cards.Remove(card);
                                Used_Cards.Add(card);
                                card.transform.position = new Vector3(2 - Jiesuan_Num * Shoupai_Juli, 1, 0);
                                Jiesuan_Num++;
                            }

                            Qipai_Cards.Clear();
                            Jiesuan_Num = 0;

                            #region 更新玩家和电脑状态
                            UpdatePlayerStatus();
                            UpdateDiannaoStatus();
                            StartCoroutine(UpdateUsedCards());
                            yield return new WaitForSeconds(StayTime);
                            #endregion

                            #region 状态机变化
                            game_state = Sanguosha_GameState.State_Playing;
                            game_round = Sanguosha_Round.Round_Player1;
                            game_stage = Sanguosha_Stage.Stage_Jieshu;
                            game_jiesuan = Sanguosha_Jiesuan.Jiesuan_None;
                            #endregion

                            State_Handler("");
                        }
                        break;
                    default:
                        break;
                }
            }
            else
            {
                #region 状态机变化
                game_state = Sanguosha_GameState.State_Playing;
                game_round = Sanguosha_Round.Round_Player1;
                game_stage = Sanguosha_Stage.Stage_Jieshu;
                game_jiesuan = Sanguosha_Jiesuan.Jiesuan_None;
                #endregion

                State_Handler("");
            }

        }

        //临时输出，后面删除
        yield return new WaitForSeconds(QiehuanTime);
    }

    IEnumerator Player1_Jieshu_Process(string msg)
    {
        if (string_player1_wujiang == "Diaochan")
        {
            StartCoroutine(PlayerMopai(Biyue));

            yield return new WaitForSeconds(Biyue * DealTime);

            #region 更新玩家和电脑状态
            UpdatePlayerStatus();
            UpdateDiannaoStatus();
            #endregion
        }

        #region 状态机变化
        game_state = Sanguosha_GameState.State_Playing;
        game_round = Sanguosha_Round.Round_Player2;
        game_stage = Sanguosha_Stage.Stage_Zhunbei;
        game_jiesuan = Sanguosha_Jiesuan.Jiesuan_None;
        #endregion

        State_Handler("");

        //临时输出，后面删除
        yield return new WaitForSeconds(QiehuanTime);
    }

    IEnumerator Player2_Zhunbei_Process(string msg)
    {
        #region 准备阶段
        //主要是觉得不能放在要被重复调用的Chupai处理函数中

        //一些状态可以在这里初始化，如杀的使用次数
        //player1_tili--;
        Player2_ShaSign = false;
        Card_Select = -1;

        #region 更新玩家和电脑状态
        UpdatePlayerStatus();
        UpdateDiannaoStatus();
        //StartCoroutine(UpdateUsedCards());
        #endregion

        #region 状态机变化
        game_state = Sanguosha_GameState.State_Playing;
        game_round = Sanguosha_Round.Round_Player2;
        game_stage = Sanguosha_Stage.Stage_Panding;
        game_jiesuan = Sanguosha_Jiesuan.Jiesuan_None;
        #endregion

        yield return new WaitForSeconds(QiehuanTime);

        State_Handler("");
        #endregion

    }

    IEnumerator Player2_Panding_Process(string msg)
    {
        if (string_player2_panding2 != "2")
        {
            Card tmpCard;
            tmpCard = Panding();
            if (string_player2_panding2 == "乐")
            {
                if (tmpCard.Definition.Huase != "Hongtao")
                {
                    Player2_LebusishuSign = true;
                }
            }
            else if (string_player2_panding2 == "兵")
            {
                if (tmpCard.Definition.Huase != "Meihua")
                {
                    Player2_BingliangcunduanSign = true;
                }
            }
            string_player2_panding2 = "2";
            Used_Cards.Add(tmpCard);
            Used_Cards.Add(player2_panding2_card);
            player2_panding2_card = null;
            yield return new WaitForSeconds(StayTime);
            tmpCard.transform.position = new Vector3(3, 0, 0);
        }

        UpdatePlayerStatus();
        UpdateDiannaoStatus();

        if (string_player2_panding1 != "1")
        {
            Card tmpCard;
            tmpCard = Panding();
            if (string_player2_panding1 == "乐")
            {
                if (tmpCard.Definition.Huase != "Hongtao")
                {
                    Player2_LebusishuSign = true;
                }
            }
            else if (string_player2_panding1 == "兵")
            {
                if (tmpCard.Definition.Huase != "Meihua")
                {
                    Player2_BingliangcunduanSign = true;
                }
            }
            string_player2_panding1 = "1";
            Used_Cards.Add(tmpCard);
            Used_Cards.Add(player2_panding1_card);
            player2_panding1_card = null;
            yield return new WaitForSeconds(StayTime);

        }
        #region 更新玩家和电脑状态
        UpdatePlayerStatus();
        UpdateDiannaoStatus();
        StartCoroutine(UpdateUsedCards());
        #endregion

        #region 状态机变化
        if (Player2_BingliangcunduanSign)
        {
            if (Player2_LebusishuSign)
            {
                game_state = Sanguosha_GameState.State_Playing;
                game_round = Sanguosha_Round.Round_Player2;
                game_stage = Sanguosha_Stage.Stage_Qipai;
                game_jiesuan = Sanguosha_Jiesuan.Jiesuan_None;
            }
            else
            {
                game_state = Sanguosha_GameState.State_Playing;
                game_round = Sanguosha_Round.Round_Player2;
                game_stage = Sanguosha_Stage.Stage_Chupai;
                game_jiesuan = Sanguosha_Jiesuan.Jiesuan_None;
            }
        }
        else
        {
            //后面摸牌阶段结束再判断一下乐不思蜀状态
            game_state = Sanguosha_GameState.State_Playing;
            game_round = Sanguosha_Round.Round_Player2;
            game_stage = Sanguosha_Stage.Stage_Mopai;
            game_jiesuan = Sanguosha_Jiesuan.Jiesuan_None;
        }
        Player2_BingliangcunduanSign = false;
        #endregion

        State_Handler("");
    }

    IEnumerator Player2_Mopai_Process(string msg)
    {
        StartCoroutine(DiannaoMopai(Shoupaiperround));

        yield return new WaitForSeconds(Shoupaiperround * DealTime);

        #region 更新玩家和电脑状态
        UpdatePlayerStatus();
        UpdateDiannaoStatus();
        #endregion

        #region 状态机变化
        if (Player2_LebusishuSign)
        {
            game_state = Sanguosha_GameState.State_Playing;
            game_round = Sanguosha_Round.Round_Player2;
            game_stage = Sanguosha_Stage.Stage_Qipai;
            game_jiesuan = Sanguosha_Jiesuan.Jiesuan_None;

            if (player2_shoupai > player2_shoupaishangxian)
            {
                Qipai_Cards = new List<Card>(player2_shoupai - player2_shoupaishangxian);
                Qipai_Index = new List<int>(player2_shoupai - player2_shoupaishangxian);
            }

            State_Handler("");

        }
        else
        {
            game_state = Sanguosha_GameState.State_Playing;
            game_round = Sanguosha_Round.Round_Player2;
            game_stage = Sanguosha_Stage.Stage_Chupai;
            game_jiesuan = Sanguosha_Jiesuan.Jiesuan_None;

            State_Handler("");
        }

        Player2_LebusishuSign = false;
        #endregion



        yield return new WaitForSeconds(QiehuanTime);
    }

    int BI()
    {
        for(int i = 0; i < Player2_Cards.Count; ++i)
        {
            if (Player2_Cards[i].Definition.CardName == "Sha")
            {
                return i;
            }
        }
        return -1;
    }

    int AI()
    {
        List<int> Values = new List<int>();

        //先根据状态定义value的值
        int Hongsha_Value;
        int Heisha_Value;
        int Shan_Value;
        int Tao_Value;
        int Juedou_Value;
        int Nanmanruqin_Value;
        int Wanjianqifa_Value;
        int Wuzhongshengyou_Value;
        int Shunshouqianyang_Value;
        int Guohechaiqiao_Value;
        int Lebusishu_Value;
        int Bingliangcunduan_Value;
        int Zhugeliannu_Value;
        int Baguazhen_Value;
        int Renwangdun_Value;
        int Qinggangjian_Value;

        #region 根据当前状态确定不同牌的价值
        //无中生有价值为100
        Wuzhongshengyou_Value = 100;
        //顺手牵羊价值为90
        Shunshouqianyang_Value = 90;
        if (Player1_Cards.Count == 0 && string_player1_fangju == "防具" && string_player1_wuqi == "武器")
        {
            Shunshouqianyang_Value = -90;
        }
        //过河拆桥价值计算：优先防守，如果对面武器装备了青釭剑而我有防具时，优先使用过河拆桥拆掉青釭剑
        #region 过河拆桥价值
        Guohechaiqiao_Value = 80;
        if (string_player1_wuqi == "青釭剑")
        {
            for(int i = 0; i < Player2_Cards.Count; ++i)
            {
                if(Player2_Cards[i].Definition.CardName=="Baguanzhen"|| Player2_Cards[i].Definition.CardName == "Renwangdun")
                {
                    Guohechaiqiao_Value = 95;
                }    
            }

            if (string_player2_fangju != "武器")
            {
                Guohechaiqiao_Value = 95;
            }
        }
        if (Player1_Cards.Count == 0 && string_player1_fangju == "防具" && string_player1_wuqi == "武器")
        {
            Guohechaiqiao_Value = -80;
        }
        #endregion
        //乐不思蜀价值
        Lebusishu_Value = 70;
        //兵粮寸断价值
        Bingliangcunduan_Value = 60;
        //决斗价值
        #region 决斗价值
        Juedou_Value = 50;
        //通过比较双方杀的数量来决定决斗的价值，如果少于对方的话则为负数
        {
            int Player1_Sha_Num=0;
            int Player2_Sha_Num=0;
            for(int i = 0; i < Player1_Cards.Count; ++i)
            {
                if (Player1_Cards[i].Definition.CardName == "Sha")
                {
                    Player1_Sha_Num++;
                }
            }

            for(int i = 0; i < Player2_Cards.Count; ++i)
            {
                if (Player2_Cards[i].Definition.CardName == "Sha")
                {
                    Player2_Sha_Num++;
                }
            }

            if (Player2_Sha_Num < Player1_Sha_Num)
            {
                Juedou_Value = -50;
            }
        }
        #endregion
        //南蛮价值
        Nanmanruqin_Value = 40;
        //黑杀价值
        Heisha_Value = 30;
        //如果对面有仁王盾而我没有大宝剑，则黑杀价值为负数
        if (string_player1_fangju == "仁王盾" && string_player2_wuqi != "青釭剑")
        {
            Heisha_Value = -30;
        }
        if (Player2_ShaSign == true)
        {
            Heisha_Value = -30;
        }
        //红杀价值
        Hongsha_Value = 20;
        if (Player2_ShaSign == true)
        {
            Hongsha_Value = -20;
        }
        Wanjianqifa_Value = 10;
        Tao_Value = 5;
        if (player2_tili == player2_manxue)
        {
            Tao_Value = -5;
        }
        Renwangdun_Value = 28;
        if (string_player2_fangju != "防具")
        {
            Renwangdun_Value = -28;
        }
        Baguazhen_Value = 26;
        if (string_player2_fangju != "防具")
        {
            Baguazhen_Value = -26;
        }

        Qinggangjian_Value = 24;
        Zhugeliannu_Value = 22;
        if (string_player1_fangju != "防具")
        {
            Qinggangjian_Value = 24;
            Zhugeliannu_Value = -22;
        }
        else if(string_player2_wuqi!="武器")
        {
            Qinggangjian_Value = -24;
            Zhugeliannu_Value = -22;
        }
        else
        {
            Qinggangjian_Value = 24;
            Zhugeliannu_Value = 22;
        }

        Shan_Value = -100;
        #endregion
        
        //根据卡牌名称确定在当前条件下每张牌的价值
        #region 根据卡牌名称确定在当前条件下每张牌的价值
        for (int i = 0; i < Player2_Cards.Count; ++i)
        {
            switch (Player2_Cards[i].Definition.CardName)
            {
                case "Wuzhongshengyou":
                    Values.Add(Wuzhongshengyou_Value);
                    break;
                case "Shunshouqianyang":
                    Values.Add(Shunshouqianyang_Value);
                    break;
                case "Guohechaiqiao":
                    Values.Add(Guohechaiqiao_Value);
                    break;
                case "Lebusishu":
                    Values.Add(Lebusishu_Value);
                    break;
                case "Bingliangcunduan":
                    Values.Add(Bingliangcunduan_Value);
                    break;
                case "Juedou":
                    Values.Add(Juedou_Value);
                    break;
                case "Nanmanruqin":
                    Values.Add(Nanmanruqin_Value);
                    break;
                case "Wanjianqifa":
                    Values.Add(Wanjianqifa_Value);
                    break;
                case "Tao":
                    Values.Add(Tao_Value);
                    break;
                case "Zhugeliannu":
                    Values.Add(Zhugeliannu_Value);
                    break;
                case "Baguazhen":
                    Values.Add(Baguazhen_Value);
                    break;
                case "Renwangdun":
                    Values.Add(Renwangdun_Value);
                    break;
                case "Qinggangjian":
                    Values.Add(Qinggangjian_Value);
                    break;
                case "Sha":
                    if(Player2_Cards[i].Definition.Huase=="Hongtao"|| Player2_Cards[i].Definition.Huase == "Fangkuai")
                    {
                        Values.Add(Hongsha_Value);
                    }
                    else
                    {
                        Values.Add(Heisha_Value);
                    }
                    break;
                case "Shan":
                    Values.Add(Shan_Value);
                    break;
            }
        }
        #endregion

        if (Values.Count == 0)
        {
            //对应没手牌的情况，当然就不出了
            return -1;
        }
        else
        {
            int MAX = 0;
            int MAX_idx=-1;
            for(int i = 0; i < Values.Count; ++i)
            {
                if (Values[i] > MAX)
                {
                    MAX = Values[i];
                    MAX_idx = i;
                }
            }
            if (MAX == 0)
            {
                //没有大于阈值价值=0的牌，表示无牌可出了
                return -1;
            }
            else
            {
                //有牌可出的话返回其在玩家手牌中的序号
                return MAX_idx;
            }
        }
    }

    IEnumerator Player2_Chupai_Process(string msg)
    {
        game_jiesuan = Sanguosha_Jiesuan.Jiesuan_None;
        int AI_Card_Idx;
        Card AI_Card;

        #region 进入电脑出牌阶段
        //通过AI函数获得
        AI_Card_Idx = AI();
        Debug.Log("AI_Card_Idx=" + AI_Card_Idx.ToString());
        if (AI_Card_Idx == -1)
        {
            //无牌可出直接结束出牌阶段
            #region 状态机变化
            game_state = Sanguosha_GameState.State_Playing;
            game_round = Sanguosha_Round.Round_Player2;
            game_stage = Sanguosha_Stage.Stage_Qipai;
            game_jiesuan = Sanguosha_Jiesuan.Jiesuan_None;
            #endregion

            if (player2_shoupai > player2_shoupaishangxian)
            {
                Qipai_Cards = new List<Card>(player2_shoupai - player2_shoupaishangxian);
                Qipai_Index = new List<int>(player2_shoupai - player2_shoupaishangxian);
            }

            State_Handler("");
        }
        else
        {
            AI_Card = Player2_Cards[AI_Card_Idx];
            switch (AI_Card.Definition.CardName)
            {
                case "Sha":
                    Debug.LogError("电脑出杀");
                    #region 电脑出杀
                    if (Player2_ShaSign == false || string_player2_wuqi == "诸葛连弩" || string_player2_wujiang == "Zhangfei")
                    {
                        Player2_Cards.RemoveAt(AI_Card_Idx);
                        AI_Card.transform.position = new Vector3(2 - Jiesuan_Num * Shoupai_Juli, 1, 0);
                        Used_Cards.Add(AI_Card);

                        Jiesuan_Num++;
                        Player2_ShaSign = true;

                        if (string_player1_fangju != "防具" && string_player2_wuqi != "青釭剑")
                        {
                            if (string_player1_fangju == "仁王盾")
                            {
                                if (AI_Card.Definition.Huase == "Meihua" || AI_Card.Definition.Huase == "Heitao")
                                {
                                    //如果是梅花和黑桃的话则触发仁王盾效果，直接结束
                                    Debug.Log("玩家发动仁王盾效果");

                                    #region 更新玩家和电脑状态
                                    UpdatePlayerStatus();
                                    UpdateDiannaoStatus();
                                    #endregion

                                    Jiesuan_Num = 0;

                                    State_Handler("");
                                }
                                else
                                {
                                    //如果是红杀的话则直接忽略跟正常一样结算就好了
                                    //状态机变化
                                    game_state = Sanguosha_GameState.State_Resolving_Player1;
                                    game_round = Sanguosha_Round.Round_Player2;
                                    game_stage = Sanguosha_Stage.Stage_Chupai;
                                    game_jiesuan = Sanguosha_Jiesuan.Jiesuan_Sha;

                                    #region 更新玩家和电脑状态
                                    UpdatePlayerStatus();
                                    UpdateDiannaoStatus();
                                    #endregion
                                }
                            }
                        }
                        else
                        {
                            //状态机变化
                            game_state = Sanguosha_GameState.State_Resolving_Player1;
                            game_round = Sanguosha_Round.Round_Player2;
                            game_stage = Sanguosha_Stage.Stage_Chupai;
                            game_jiesuan = Sanguosha_Jiesuan.Jiesuan_Sha;

                            #region 更新玩家和电脑状态
                            UpdatePlayerStatus();
                            UpdateDiannaoStatus();
                            #endregion
                        }
                    }
                    #endregion
                    break;
                case "Wuzhongshengyou":
                    Debug.LogError("电脑出无中生有");
                    #region 电脑出无中生有
                    Player2_Cards.RemoveAt(AI_Card_Idx);
                    AI_Card.transform.position = new Vector3(2 - Jiesuan_Num * Shoupai_Juli, 1, 0);
                    Used_Cards.Add(AI_Card);
                    Jiesuan_Num++;

                    StartCoroutine(DiannaoMopai(Shoupaiperround));

                    yield return new WaitForSeconds(2*DealTime);

                    #region 更新玩家和电脑状态
                    UpdatePlayerStatus();
                    UpdateDiannaoStatus();
                    StartCoroutine(UpdateUsedCards());
                    #endregion

                    State_Handler("");

                    Jiesuan_Num = 0;

                    break;
                #endregion
                case "Tao":
                    Debug.LogError("电脑出桃");
                    #region 电脑出桃
                    if (player2_tili != player2_manxue)
                    {
                        Player2_Cards.RemoveAt(AI_Card_Idx);
                        AI_Card.transform.position = new Vector3(2 - Jiesuan_Num * Shoupai_Juli, 1, 0);
                        Used_Cards.Add(AI_Card);
                        Jiesuan_Num++;

                        player2_tili += 1;
                        
                        Jiesuan_Num = 0;

                        #region 更新玩家和电脑状态
                        UpdatePlayerStatus();
                        UpdateDiannaoStatus();
                        StartCoroutine(UpdateUsedCards());
                        #endregion

                        yield return new WaitForSeconds(StayTime);

                        State_Handler("");
                    }
                    else
                    {
                        Debug.Log("满血无法吃桃！");
                    }
                    break;
                #endregion
                case "Lebusishu":
                    Debug.LogError("电脑出乐不思蜀");
                    #region 出牌阶段出乐不思蜀
                    Player2_Cards.RemoveAt(AI_Card_Idx);
                    AI_Card.transform.position = new Vector3(2 - Jiesuan_Num * Shoupai_Juli, 1, 0);
                    //因为是非延时锦囊牌，实际上就是暂时贴到对方玩家那里了，先不放到弃牌堆，等到结算的时候再放入弃牌堆

                    Jiesuan_Num++;

                    if (string_player1_panding1 == "1")
                    {
                        string_player1_panding1 = "乐";
                        player1_panding1_card = AI_Card;
                    }
                    else
                    {
                        string_player1_panding2 = "乐";
                        player1_panding2_card = AI_Card;
                    }
                    
                    Jiesuan_Num = 0;

                    yield return new WaitForSeconds(StayTime);

                    #region 更新玩家和电脑状态
                    UpdatePlayerStatus();
                    UpdateDiannaoStatus();
                    #endregion
                    
                    State_Handler("");
                    #endregion
                    break;
                case "Bingliangcunduan":
                    Debug.LogError("电脑出兵粮寸断");
                    #region 出牌阶段出兵粮寸断
                    Player2_Cards.RemoveAt(AI_Card_Idx);
                    AI_Card.transform.position = new Vector3(2 - Jiesuan_Num * Shoupai_Juli, 1, 0);
                    //因为是非延时锦囊牌，实际上就是暂时贴到对方玩家那里了，先不放到弃牌堆，等到结算的时候再放入弃牌堆

                    Jiesuan_Num++;

                    if (string_player1_panding1 == "1")
                    {
                        string_player1_panding1 = "兵";
                        player1_panding1_card = AI_Card;
                    }
                    else
                    {
                        string_player1_panding2 = "兵";
                        player1_panding2_card = AI_Card;
                    }

                    Jiesuan_Num = 0;

                    yield return new WaitForSeconds(StayTime);

                    #region 更新玩家和电脑状态
                    UpdatePlayerStatus();
                    UpdateDiannaoStatus();
                    #endregion
                    
                    State_Handler("");
                    #endregion
                    break;
                case "Wanjianqifa":
                    Debug.LogError("电脑出万箭齐发");
                    #region 出牌阶段出万箭齐发
                    Player2_Cards.RemoveAt(AI_Card_Idx);
                    AI_Card.transform.position = new Vector3(2 - Jiesuan_Num * Shoupai_Juli, 1, 0);
                    Used_Cards.Add(AI_Card);

                    //状态机变化
                    game_state = Sanguosha_GameState.State_Resolving_Player1;
                    game_round = Sanguosha_Round.Round_Player2;
                    game_stage = Sanguosha_Stage.Stage_Chupai;
                    game_jiesuan = Sanguosha_Jiesuan.Jiesuan_Wanjianqifa;
                    
                    Jiesuan_Num += 1;

                    #region 更新玩家和电脑状态
                    UpdatePlayerStatus();
                    UpdateDiannaoStatus();
                    #endregion
                    #endregion
                    break;
                case "Nanmanruqin":
                    Debug.LogError("电脑出南蛮入侵");
                    #region 出牌阶段出南蛮入侵
                    Player2_Cards.RemoveAt(AI_Card_Idx);
                    AI_Card.transform.position = new Vector3(2 - Jiesuan_Num * Shoupai_Juli, 1, 0);
                    Used_Cards.Add(AI_Card);

                    //状态机变化
                    game_state = Sanguosha_GameState.State_Resolving_Player1;
                    game_round = Sanguosha_Round.Round_Player2;
                    game_stage = Sanguosha_Stage.Stage_Chupai;
                    game_jiesuan = Sanguosha_Jiesuan.Jiesuan_Nanmanruqin;

                    Jiesuan_Num += 1;

                    #region 更新玩家和电脑状态
                    UpdatePlayerStatus();
                    UpdateDiannaoStatus();
                    #endregion
                    #endregion
                    break;
                case "Guohechaiqiao":
                    #region 出牌阶段出过河拆桥
                    Debug.LogError("电脑出过河拆桥");
                    if (Player1_Cards.Count != 0 || string_player1_wuqi != "武器" || string_player1_fangju != "防具")
                    {
                        Player2_Cards.RemoveAt(AI_Card_Idx);
                        AI_Card.transform.position = new Vector3(2 - Jiesuan_Num * Shoupai_Juli, 1, 0);
                        Used_Cards.Add(AI_Card);
                        
                        Jiesuan_Num++;

                        //有防具拆防具
                        if (string_player1_fangju != "防具")
                        {
                            player1_fangju_card.transform.position = new Vector3(2 - Jiesuan_Num * Shoupai_Juli, 1, 0);
                            Used_Cards.Add(player1_fangju_card);
                            player1_fangju_card = null;
                            string_player1_fangju = "防具";
                            
                            yield return new WaitForSeconds(StayTime);

                            #region 更新状态
                            UpdatePlayerStatus();
                            UpdateDiannaoStatus();
                            StartCoroutine(UpdateUsedCards());
                            yield return new WaitForSeconds(StayTime);
                            #endregion

                            //状态机变化

                            game_state = Sanguosha_GameState.State_Playing;
                            game_round = Sanguosha_Round.Round_Player2;
                            game_stage = Sanguosha_Stage.Stage_Chupai;
                            game_jiesuan = Sanguosha_Jiesuan.Jiesuan_None;
                            
                            Jiesuan_Num = 0;

                            State_Handler("");

                        }
                        else if(string_player1_wuqi != "武器")
                        {
                            player1_wuqi_card.transform.position = new Vector3(2 - Jiesuan_Num * Shoupai_Juli, 1, 0);
                            Used_Cards.Add(player1_wuqi_card);
                            player1_wuqi_card = null;
                            string_player1_wuqi = "武器";

                            yield return new WaitForSeconds(StayTime);
                            
                            #region 更新状态
                            UpdatePlayerStatus();
                            UpdateDiannaoStatus();
                            StartCoroutine(UpdateUsedCards());
                            yield return new WaitForSeconds(StayTime);
                            #endregion

                            game_state = Sanguosha_GameState.State_Playing;
                            game_round = Sanguosha_Round.Round_Player2;
                            game_stage = Sanguosha_Stage.Stage_Chupai;
                            game_jiesuan = Sanguosha_Jiesuan.Jiesuan_None;

                            Jiesuan_Num = 0;

                            State_Handler("");
                        }
                        else
                        {
                            int random_card = Random.Range(0, Player1_Cards.Count);
                            Card tmpCard = Player1_Cards[random_card];
                            Player1_Cards.RemoveAt(random_card);
                            tmpCard.transform.position = new Vector3(2 - Jiesuan_Num * Shoupai_Juli, 1, 0);
                            Used_Cards.Add(tmpCard);

                            yield return new WaitForSeconds(StayTime);

                            #region 更新状态
                            UpdatePlayerStatus();
                            UpdateDiannaoStatus();
                            StartCoroutine(UpdateUsedCards());
                            yield return new WaitForSeconds(StayTime);
                            #endregion

                            //状态机变化

                            game_state = Sanguosha_GameState.State_Playing;
                            game_round = Sanguosha_Round.Round_Player2;
                            game_stage = Sanguosha_Stage.Stage_Chupai;
                            game_jiesuan = Sanguosha_Jiesuan.Jiesuan_None;

                            Jiesuan_Num = 0;

                            State_Handler("");
                        }
                    }
                    else
                    {
                        Debug.Log("对面没有可供过河拆桥的牌");
                    }
                    #endregion
                    break;
                case "Shunshouqianyang":
                    #region 出牌阶段出顺手牵羊
                    Debug.LogError("电脑出顺手牵羊");
                    if (Player1_Cards.Count != 0 || string_player1_wuqi != "武器" || string_player1_fangju != "防具")
                    {
                        Player2_Cards.RemoveAt(AI_Card_Idx);
                        AI_Card.transform.position = new Vector3(2 - Jiesuan_Num * Shoupai_Juli, 1, 0);
                        Used_Cards.Add(AI_Card);

                        Jiesuan_Num++;

                        //有防具拆防具
                        if (string_player1_fangju != "防具")
                        {
                            Player2_Cards.Add(player1_fangju_card);
                            player1_fangju_card = null;
                            string_player1_fangju = "防具";

                            yield return new WaitForSeconds(StayTime);

                            #region 更新状态
                            UpdatePlayerStatus();
                            UpdateDiannaoStatus();
                            StartCoroutine(UpdateUsedCards());
                            yield return new WaitForSeconds(StayTime);
                            #endregion

                            //状态机变化

                            game_state = Sanguosha_GameState.State_Playing;
                            game_round = Sanguosha_Round.Round_Player2;
                            game_stage = Sanguosha_Stage.Stage_Chupai;
                            game_jiesuan = Sanguosha_Jiesuan.Jiesuan_None;

                            Jiesuan_Num = 0;

                            State_Handler("");

                        }
                        else if (string_player1_wuqi != "武器")
                        {
                            Player2_Cards.Add(player1_wuqi_card);
                            player1_wuqi_card = null;
                            string_player1_wuqi = "武器";

                            yield return new WaitForSeconds(StayTime);

                            #region 更新状态
                            UpdatePlayerStatus();
                            UpdateDiannaoStatus();
                            StartCoroutine(UpdateUsedCards());
                            yield return new WaitForSeconds(StayTime);
                            #endregion

                            game_state = Sanguosha_GameState.State_Playing;
                            game_round = Sanguosha_Round.Round_Player2;
                            game_stage = Sanguosha_Stage.Stage_Chupai;
                            game_jiesuan = Sanguosha_Jiesuan.Jiesuan_None;

                            Jiesuan_Num = 0;

                            State_Handler("");
                        }
                        else
                        {
                            int random_card = Random.Range(0, Player1_Cards.Count);
                            Card tmpCard = Player1_Cards[random_card];
                            Player1_Cards.RemoveAt(random_card);
                            Player2_Cards.Add(tmpCard);

                            yield return new WaitForSeconds(StayTime);

                            #region 更新状态
                            UpdatePlayerStatus();
                            UpdateDiannaoStatus();
                            StartCoroutine(UpdateUsedCards());
                            yield return new WaitForSeconds(StayTime);
                            #endregion

                            //状态机变化

                            game_state = Sanguosha_GameState.State_Playing;
                            game_round = Sanguosha_Round.Round_Player2;
                            game_stage = Sanguosha_Stage.Stage_Chupai;
                            game_jiesuan = Sanguosha_Jiesuan.Jiesuan_None;

                            Jiesuan_Num = 0;

                            State_Handler("");
                        }
                    }
                    else
                    {
                        Debug.Log("对面没有可供过河拆桥的牌");
                    }
                    #endregion
                    break;
                case "Zhugeliannu":
                    #region 出牌阶段装备诸葛连弩
                    Debug.LogError("电脑装备了诸葛连弩");

                    Player2_Cards.RemoveAt(AI_Card_Idx);
                    AI_Card.transform.position = new Vector3(2 - Jiesuan_Num * Shoupai_Juli, 1, 0);
                    Jiesuan_Num++;
                    //因为是装备牌，实际上就是暂时贴到己方玩家那里了，先不放到弃牌堆

                    if (string_player2_wuqi != "武器")
                    {
                        player2_wuqi_card.transform.position = new Vector3(2 - Jiesuan_Num * Shoupai_Juli, 1, 0);
                        Used_Cards.Add(player2_wuqi_card);
                        player2_wuqi_card = AI_Card;
                        string_player2_wuqi = "诸葛连弩";
                    }
                    else
                    {
                        player2_wuqi_card = AI_Card;
                        string_player2_wuqi = "诸葛连弩";
                    }
                    
                    Jiesuan_Num = 0;

                    #region 更新玩家和电脑状态
                    UpdatePlayerStatus();
                    UpdateDiannaoStatus();
                    StartCoroutine(UpdateUsedCards());
                    #endregion

                    State_Handler("");

                    #endregion
                    break;
                case "Qinggangjian":
                    #region 出牌阶段装备青釭剑
                    Debug.LogError("电脑装备了青釭剑");

                    Player2_Cards.RemoveAt(AI_Card_Idx);
                    AI_Card.transform.position = new Vector3(2 - Jiesuan_Num * Shoupai_Juli, 1, 0);
                    Jiesuan_Num++;
                    //因为是装备牌，实际上就是暂时贴到己方玩家那里了，先不放到弃牌堆

                    if (string_player2_wuqi != "武器")
                    {
                        player2_wuqi_card.transform.position = new Vector3(2 - Jiesuan_Num * Shoupai_Juli, 1, 0);
                        Used_Cards.Add(player2_wuqi_card);
                        player2_wuqi_card = AI_Card;
                        string_player2_wuqi = "青釭剑";
                    }
                    else
                    {
                        player2_wuqi_card = AI_Card;
                        string_player2_wuqi = "青釭剑";
                    }

                    Jiesuan_Num = 0;

                    #region 更新玩家和电脑状态
                    UpdatePlayerStatus();
                    UpdateDiannaoStatus();
                    StartCoroutine(UpdateUsedCards());
                    #endregion

                    State_Handler("");

                    #endregion
                    break;
                case "Renwangdun":
                    #region 出牌阶段装备仁王盾
                    Debug.LogError("电脑装备了仁王盾");

                    Player2_Cards.RemoveAt(AI_Card_Idx);
                    AI_Card.transform.position = new Vector3(2 - Jiesuan_Num * Shoupai_Juli, 1, 0);
                    Jiesuan_Num++;
                    //因为是装备牌，实际上就是暂时贴到己方玩家那里了，先不放到弃牌堆

                    if (string_player2_fangju != "防具")
                    {
                        player2_fangju_card.transform.position = new Vector3(2 - Jiesuan_Num * Shoupai_Juli, 1, 0);
                        Used_Cards.Add(player2_fangju_card);
                        player2_fangju_card = AI_Card;
                        string_player2_fangju = "仁王盾";
                    }
                    else
                    {
                        player2_fangju_card = AI_Card;
                        string_player2_fangju = "仁王盾";
                    }

                    Jiesuan_Num = 0;

                    #region 更新玩家和电脑状态
                    UpdatePlayerStatus();
                    UpdateDiannaoStatus();
                    StartCoroutine(UpdateUsedCards());
                    #endregion

                    State_Handler("");

                    #endregion
                    break;
                case "Juedou":
                    #region 出牌阶段出决斗
                    Debug.LogError("电脑出了决斗");

                    Player2_Cards.RemoveAt(AI_Card_Idx);
                    AI_Card.transform.position = new Vector3(2 - Jiesuan_Num * Shoupai_Juli, 1, 0);
                    Used_Cards.Add(AI_Card);
                    Jiesuan_Num++;
                    
                    game_state = Sanguosha_GameState.State_Resolving_Player1;
                    game_stage = Sanguosha_Stage.Stage_Chupai;
                    game_jiesuan = Sanguosha_Jiesuan.Jiesuan_Juedou;

                    #region 更新玩家和电脑状态
                    UpdatePlayerStatus();
                    UpdateDiannaoStatus();
                    #endregion
                    #endregion
                    break;
                default:
                    Debug.LogError("未知如何处理的牌！:"+AI_Card.Definition.CardName);
                    break;
            }
        }


        #endregion

        yield return new WaitForSeconds(QiehuanTime);
    }

    IEnumerator Player2_Qipai_Process(string msg)
    {
        if (player2_shoupaishangxian < player2_shoupai)
        {
            //这里我先随机弃牌，卡牌堆最前的几张弃掉

            for(int i = 0; i < Qipai_Cards.Capacity; ++i)
            {
                Qipai_Cards.Add(Player2_Cards[i]);
            }

            Jiesuan_Num = 0;

            foreach (Card card in Qipai_Cards)
            {
                Player2_Cards.Remove(card);
                Used_Cards.Add(card);
                card.transform.position = new Vector3(2 - Jiesuan_Num * Shoupai_Juli, 1, 0);
                Jiesuan_Num++;
            }

            Qipai_Cards.Clear();
            Jiesuan_Num = 0;

            #region 更新玩家和电脑状态
            UpdatePlayerStatus();
            UpdateDiannaoStatus();
            StartCoroutine(UpdateUsedCards());
            #endregion



            #region 状态机变化
            game_state = Sanguosha_GameState.State_Playing;
            game_round = Sanguosha_Round.Round_Player2;
            game_stage = Sanguosha_Stage.Stage_Jieshu;
            game_jiesuan = Sanguosha_Jiesuan.Jiesuan_None;
            #endregion

            State_Handler("");
        }
        else
        {
            #region 状态机变化
            game_state = Sanguosha_GameState.State_Playing;
            game_round = Sanguosha_Round.Round_Player2;
            game_stage = Sanguosha_Stage.Stage_Jieshu;
            game_jiesuan = Sanguosha_Jiesuan.Jiesuan_None;
            #endregion

            State_Handler("");
        }

        yield return new WaitForSeconds(QiehuanTime);
    }

    IEnumerator Player2_Jieshu_Process(string msg)
    {
        if (string_player2_wujiang == "Diaochan")
        {
            StartCoroutine(DiannaoMopai(Biyue));

            yield return new WaitForSeconds(Biyue * DealTime);

            #region 更新玩家和电脑状态
            UpdatePlayerStatus();
            UpdateDiannaoStatus();
            #endregion

        }

        #region 状态机变化
        game_state = Sanguosha_GameState.State_Playing;
        game_round = Sanguosha_Round.Round_Player1;
        game_stage = Sanguosha_Stage.Stage_Zhunbei;
        game_jiesuan = Sanguosha_Jiesuan.Jiesuan_None;
        #endregion

        State_Handler("");

        Huihe_Num++;

        //临时输出，后面删除
        yield return new WaitForSeconds(QiehuanTime);
    }

    void Resolving_Player2_Process(string msg)
    {
        switch (game_jiesuan)
        {
            case Sanguosha_Jiesuan.Jiesuan_Sha:
                #region 结算杀牌
                //这里先随便写写电脑的决策，暂时的话是有闪出闪
                if (Player2_Cards.Count == 0)
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

                if (Player2_Cards.Count == 0)
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

                if (Player2_Cards.Count == 0)
                {
                    Debug.Log("掉血");
                    player2_tili -= 1;

                    //电脑不出杀结束决斗结算
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
                //很丑陋，就是为了解决在玩家没牌的时候卡住的bug
                if (Player2_Cards.Count == 0)
                {
                    Debug.Log("掉血");
                    player2_tili -= 1;

                    //电脑不出杀结束决斗结算
                    game_state = Sanguosha_GameState.State_Playing;
                    game_stage = Sanguosha_Stage.Stage_Chupai;
                    game_jiesuan = Sanguosha_Jiesuan.Jiesuan_None;
                    #region 更新玩家和电脑状态
                    UpdatePlayerStatus();
                    UpdateDiannaoStatus();
                    StartCoroutine(UpdateUsedCards());
                    Jiesuan_Num = 0;
                    #endregion

                    if (game_round == Sanguosha_Round.Round_Player2)
                    {
                        State_Handler("");
                    }

                    break;
                }
                for (int i = 0; i < Player2_Cards.Count; ++i)
                {
                    if (Player2_Cards[i].Definition.CardName == "Sha")
                    {
                        Debug.Log("出杀");
                        Player2_Chupai(i);

                        //状态机重新转回玩家，等待玩家操作驱动
                        game_state = Sanguosha_GameState.State_Resolving_Player1;
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
                        game_state = Sanguosha_GameState.State_Playing;
                        game_stage = Sanguosha_Stage.Stage_Chupai;
                        game_jiesuan = Sanguosha_Jiesuan.Jiesuan_None;
                        #region 更新玩家和电脑状态
                        UpdatePlayerStatus();
                        UpdateDiannaoStatus();
                        StartCoroutine(UpdateUsedCards());
                        Jiesuan_Num = 0;
                        #endregion
                        if (game_round == Sanguosha_Round.Round_Player2)
                        {
                            State_Handler("");
                        }
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
                //选择手牌
                switch (game_jiesuan)
                {
                    case Sanguosha_Jiesuan.Jiesuan_Juedou:
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
                    case Sanguosha_Jiesuan.Jiesuan_Sha:
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
                    case Sanguosha_Jiesuan.Jiesuan_Wanjianqifa:
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
                    case Sanguosha_Jiesuan.Jiesuan_Nanmanruqin:
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
                    default:
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
                }
                break;
            case "Player2_Wuqi_Button":
                #region 过河拆桥或是顺手牵羊中选择武器按键
                switch (game_jiesuan)
                {
                    case Sanguosha_Jiesuan.Jiesuan_Guohechaiqiao:
                        player2_wuqi_card.transform.position = new Vector3(2 - Jiesuan_Num * Shoupai_Juli, 1, 0);
                        Used_Cards.Add(player2_wuqi_card);
                        player2_wuqi_card = null;
                        string_player2_wuqi = "武器";

                        #region 更新状态
                        UpdatePlayerStatus();
                        UpdateDiannaoStatus();
                        StartCoroutine(UpdateUsedCards());
                        #endregion

                        //将这些buttons设置为不可用
                        Sanguosha_Buttons[6].SetActive(false);
                        Sanguosha_Buttons[7].SetActive(false);
                        Sanguosha_Buttons[8].SetActive(false);

                        //状态机变化
                        game_state = Sanguosha_GameState.State_Playing;
                        game_round = Sanguosha_Round.Round_Player1;
                        game_stage = Sanguosha_Stage.Stage_Chupai;
                        game_jiesuan = Sanguosha_Jiesuan.Jiesuan_None;

                        Jiesuan_Num = 0;

                        break;
                    case Sanguosha_Jiesuan.Jiesuan_Shunshouqianyang:
                        Player1_Cards.Add(player2_wuqi_card);
                        player2_wuqi_card = null;
                        string_player2_wuqi = "武器";

                        #region 更新状态
                        UpdatePlayerStatus();
                        UpdateDiannaoStatus();
                        StartCoroutine(UpdateUsedCards());
                        #endregion

                        //将这些buttons设置为不可用
                        Sanguosha_Buttons[6].SetActive(false);
                        Sanguosha_Buttons[7].SetActive(false);
                        Sanguosha_Buttons[8].SetActive(false);

                        //状态机变化
                        game_state = Sanguosha_GameState.State_Playing;
                        game_round = Sanguosha_Round.Round_Player1;
                        game_stage = Sanguosha_Stage.Stage_Chupai;
                        game_jiesuan = Sanguosha_Jiesuan.Jiesuan_None;

                        Jiesuan_Num = 0;

                        break;
                }
                #endregion
                break;
            case "Player2_Fangju_Button":
                #region 过河拆桥或是顺手牵羊中选择防具按键
                switch (game_jiesuan)
                {
                    case Sanguosha_Jiesuan.Jiesuan_Guohechaiqiao:
                        player2_fangju_card.transform.position = new Vector3(2 - Jiesuan_Num * Shoupai_Juli, 1, 0);
                        Used_Cards.Add(player2_fangju_card);
                        player2_fangju_card = null;
                        string_player2_fangju = "防具";

                        #region 更新状态
                        UpdatePlayerStatus();
                        UpdateDiannaoStatus();
                        StartCoroutine(UpdateUsedCards());
                        #endregion

                        //将这些buttons设置为不可用
                        Sanguosha_Buttons[6].SetActive(false);
                        Sanguosha_Buttons[7].SetActive(false);
                        Sanguosha_Buttons[8].SetActive(false);

                        //状态机变化

                        game_state = Sanguosha_GameState.State_Playing;
                        game_round = Sanguosha_Round.Round_Player1;
                        game_stage = Sanguosha_Stage.Stage_Chupai;
                        game_jiesuan = Sanguosha_Jiesuan.Jiesuan_None;

                        //
                        Jiesuan_Num = 0;
                        break;
                    case Sanguosha_Jiesuan.Jiesuan_Shunshouqianyang:
                        Player1_Cards.Add(player2_fangju_card);
                        player2_fangju_card = null;
                        string_player2_fangju = "防具";

                        #region 更新状态
                        UpdatePlayerStatus();
                        UpdateDiannaoStatus();
                        StartCoroutine(UpdateUsedCards());
                        #endregion

                        //将这些buttons设置为不可用
                        Sanguosha_Buttons[6].SetActive(false);
                        Sanguosha_Buttons[7].SetActive(false);
                        Sanguosha_Buttons[8].SetActive(false);

                        //状态机变化

                        game_state = Sanguosha_GameState.State_Playing;
                        game_round = Sanguosha_Round.Round_Player1;
                        game_stage = Sanguosha_Stage.Stage_Chupai;
                        game_jiesuan = Sanguosha_Jiesuan.Jiesuan_None;

                        //
                        Jiesuan_Num = 0;
                        break;
                }
                #endregion
                break;
            case "Player2_Shoupai_Button":
                #region 过河拆桥或是顺手牵羊中选择手牌按键
                switch (game_jiesuan)
                {
                    case Sanguosha_Jiesuan.Jiesuan_Guohechaiqiao:
                        {
                            #region 过河拆桥
                            int random_card = Random.Range(0, Player2_Cards.Count);
                            Card tmpCard = Player2_Cards[random_card];
                            Player2_Cards.RemoveAt(random_card);
                            tmpCard.transform.position = new Vector3(2 - Jiesuan_Num * Shoupai_Juli, 1, 0);
                            Used_Cards.Add(tmpCard);

                            #region 更新状态
                            UpdatePlayerStatus();
                            UpdateDiannaoStatus();
                            StartCoroutine(UpdateUsedCards());
                            #endregion

                            //将这些buttons设置为不可用
                            Sanguosha_Buttons[6].SetActive(false);
                            Sanguosha_Buttons[7].SetActive(false);
                            Sanguosha_Buttons[8].SetActive(false);

                            //状态机变化

                            game_state = Sanguosha_GameState.State_Playing;
                            game_round = Sanguosha_Round.Round_Player1;
                            game_stage = Sanguosha_Stage.Stage_Chupai;
                            game_jiesuan = Sanguosha_Jiesuan.Jiesuan_None;

                            Jiesuan_Num = 0;
                            #endregion
                        }

                        break;
                    case Sanguosha_Jiesuan.Jiesuan_Shunshouqianyang:
                        {
                            #region 顺手牵羊
                            int random_card = Random.Range(0, Player2_Cards.Count);
                            Card tmpCard = Player2_Cards[random_card];
                            Player2_Cards.RemoveAt(random_card);
                            Player1_Cards.Add(tmpCard);

                            #region 更新状态
                            UpdatePlayerStatus();
                            UpdateDiannaoStatus();
                            StartCoroutine(UpdateUsedCards());
                            #endregion

                            //将这些buttons设置为不可用
                            Sanguosha_Buttons[6].SetActive(false);
                            Sanguosha_Buttons[7].SetActive(false);
                            Sanguosha_Buttons[8].SetActive(false);

                            //状态机变化

                            game_state = Sanguosha_GameState.State_Playing;
                            game_round = Sanguosha_Round.Round_Player1;
                            game_stage = Sanguosha_Stage.Stage_Chupai;
                            game_jiesuan = Sanguosha_Jiesuan.Jiesuan_None;

                            Jiesuan_Num = 0;
                            #endregion
                        }

                        break;
                }
                #endregion

                break;
            case "Queding":
                #region 确定按键
                if (Card_Select != -1)
                {
                    Card tmpCard = Player1_Cards[Card_Select];
                    switch (game_jiesuan)
                    {
                        case Sanguosha_Jiesuan.Jiesuan_Juedou:
                            #region 结算决斗阶段
                            if (Player1_Cards[Card_Select].Definition.CardName == "Sha")
                            {
                                Player1_Cards.RemoveAt(Card_Select);
                                Used_Cards.Add(tmpCard);
                                tmpCard.transform.position = new Vector3(2 - Jiesuan_Num * Shoupai_Juli, 1, 0);

                                //状态机变化
                                game_state = Sanguosha_GameState.State_Resolving_Player2;
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
                            #endregion
                            break;
                        case Sanguosha_Jiesuan.Jiesuan_Sha:
                            #region 结算杀阶段
                            if (Player1_Cards[Card_Select].Definition.CardName == "Shan")
                            {
                                Player1_Cards.RemoveAt(Card_Select);
                                Used_Cards.Add(tmpCard);
                                tmpCard.transform.position = new Vector3(2 - Jiesuan_Num * Shoupai_Juli, 1, 0);

                                //状态机变化
                                game_state = Sanguosha_GameState.State_Playing;
                                game_round = Sanguosha_Round.Round_Player2;
                                game_stage = Sanguosha_Stage.Stage_Chupai;
                                game_jiesuan = Sanguosha_Jiesuan.Jiesuan_None;

                                Card_Select = -1;
                                Jiesuan_Num = 0;

                                #region 更新玩家和电脑状态
                                UpdatePlayerStatus();
                                UpdateDiannaoStatus();
                                #endregion

                                State_Handler("");
                            }
                            {
                                Debug.Log("请出闪");
                            }
                            #endregion
                            break;
                        case Sanguosha_Jiesuan.Jiesuan_Wanjianqifa:
                            #region 结算万箭齐发阶段
                            if (Player1_Cards[Card_Select].Definition.CardName == "Shan")
                            {
                                Player1_Cards.RemoveAt(Card_Select);
                                Used_Cards.Add(tmpCard);
                                tmpCard.transform.position = new Vector3(2 - Jiesuan_Num * Shoupai_Juli, 1, 0);

                                //状态机变化
                                game_state = Sanguosha_GameState.State_Playing;
                                game_round = Sanguosha_Round.Round_Player2;
                                game_stage = Sanguosha_Stage.Stage_Chupai;
                                game_jiesuan = Sanguosha_Jiesuan.Jiesuan_None;

                                Card_Select = -1;
                                Jiesuan_Num = 0;

                                #region 更新玩家和电脑状态
                                UpdatePlayerStatus();
                                UpdateDiannaoStatus();
                                #endregion

                                State_Handler("");
                            }
                            #endregion
                            break;
                        case Sanguosha_Jiesuan.Jiesuan_Nanmanruqin:
                            #region 结算南蛮入侵阶段
                            if (Player1_Cards[Card_Select].Definition.CardName == "Sha")
                            {
                                Player1_Cards.RemoveAt(Card_Select);
                                Used_Cards.Add(tmpCard);
                                tmpCard.transform.position = new Vector3(2 - Jiesuan_Num * Shoupai_Juli, 1, 0);

                                //状态机变化
                                game_state = Sanguosha_GameState.State_Playing;
                                game_round = Sanguosha_Round.Round_Player2;
                                game_stage = Sanguosha_Stage.Stage_Chupai;
                                game_jiesuan = Sanguosha_Jiesuan.Jiesuan_None;

                                Card_Select = -1;
                                Jiesuan_Num = 0;

                                #region 更新玩家和电脑状态
                                UpdatePlayerStatus();
                                UpdateDiannaoStatus();
                                #endregion

                                State_Handler("");
                            }
                            #endregion
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
                            game_stage = Sanguosha_Stage.Stage_Chupai;
                            game_jiesuan = Sanguosha_Jiesuan.Jiesuan_None;

                            #region 更新玩家和电脑状态
                            UpdatePlayerStatus();
                            UpdateDiannaoStatus();
                            StartCoroutine(UpdateUsedCards());
                            Jiesuan_Num = 0;
                            #endregion

                            if (game_round == Sanguosha_Round.Round_Player2)
                            {
                                State_Handler("");
                            }
                            break;
                        case Sanguosha_Jiesuan.Jiesuan_Sha:
                            Debug.Log("杀对玩家造成伤害");

                            player1_tili -= 1;

                            game_state = Sanguosha_GameState.State_Playing;
                            game_round = Sanguosha_Round.Round_Player2;
                            game_stage = Sanguosha_Stage.Stage_Chupai;
                            game_jiesuan = Sanguosha_Jiesuan.Jiesuan_None;

                            #region 更新玩家和电脑状态
                            UpdatePlayerStatus();
                            UpdateDiannaoStatus();
                            StartCoroutine(UpdateUsedCards());
                            Jiesuan_Num = 0;
                            #endregion
                            State_Handler("");
                            break;
                        case Sanguosha_Jiesuan.Jiesuan_Wanjianqifa:
                            Debug.Log("杀对玩家造成伤害");

                            player1_tili -= 1;

                            game_state = Sanguosha_GameState.State_Playing;
                            game_round = Sanguosha_Round.Round_Player2;
                            game_stage = Sanguosha_Stage.Stage_Chupai;
                            game_jiesuan = Sanguosha_Jiesuan.Jiesuan_None;

                            #region 更新玩家和电脑状态
                            UpdatePlayerStatus();
                            UpdateDiannaoStatus();
                            StartCoroutine(UpdateUsedCards());
                            Jiesuan_Num = 0;
                            #endregion
                            State_Handler("");
                            break;
                        case Sanguosha_Jiesuan.Jiesuan_Nanmanruqin:
                            Debug.Log("杀对玩家造成伤害");

                            player1_tili -= 1;

                            game_state = Sanguosha_GameState.State_Playing;
                            game_round = Sanguosha_Round.Round_Player2;
                            game_stage = Sanguosha_Stage.Stage_Chupai;
                            game_jiesuan = Sanguosha_Jiesuan.Jiesuan_None;

                            #region 更新玩家和电脑状态
                            UpdatePlayerStatus();
                            UpdateDiannaoStatus();
                            StartCoroutine(UpdateUsedCards());
                            Jiesuan_Num = 0;
                            #endregion
                            State_Handler("");
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
            case Sanguosha_GameState.State_Binsi_Player1:
                #region 玩家进入濒死状态
                Debug.Log("玩家进入濒死状态");
                StartCoroutine(Binsi_Player1_Process(msg));
                #endregion
                break;
            case Sanguosha_GameState.State_Binsi_Player2:
                #region
                Debug.Log("电脑进入濒死状态");
                StartCoroutine(Binsi_Player2_Process(msg));
                #endregion
                break;
            case Sanguosha_GameState.State_ChooseWujiang:
                Debug.Log("->选择武将");
                StartCoroutine(Xuanzewujiang_Process(msg));
                break;
            case Sanguosha_GameState.State_Resolving:
                break;
            case Sanguosha_GameState.State_Resolving_Player1:
                Debug.Log("->玩家选择应对");
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
                                Debug.Log("->准备阶段");
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
                                StartCoroutine(Player1_Qipai_Process(msg));
                                break;
                                #endregion
                            case Sanguosha_Stage.Stage_Jieshu:
                                #region 结束阶段
                                Debug.Log("->结束阶段");
                                StartCoroutine(Player1_Jieshu_Process(msg));
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
                                #region 准备阶段
                                Debug.Log("进入电脑准备阶段");
                                StartCoroutine(Player2_Zhunbei_Process(msg));
                                #endregion
                                break;
                            case Sanguosha_Stage.Stage_Panding:
                                #region 判定阶段
                                Debug.Log("进入电脑判定阶段");
                                StartCoroutine(Player2_Panding_Process(msg));
                                #endregion
                                break;
                            case Sanguosha_Stage.Stage_Mopai:
                                #region 摸牌阶段
                                Debug.Log("->进入电脑摸牌阶段");
                                StartCoroutine(Player2_Mopai_Process(msg));
                                #endregion
                                break;
                            case Sanguosha_Stage.Stage_Chupai:
                                #region 出牌阶段
                                Debug.Log("->进入电脑出牌阶段");
                                StartCoroutine(Player2_Chupai_Process(msg));
                                #endregion
                                break;
                            case Sanguosha_Stage.Stage_Qipai:
                                #region 弃牌阶段
                                Debug.Log("->进入电脑弃牌阶段");
                                StartCoroutine(Player2_Qipai_Process(msg));
                                #endregion
                                break;
                            case Sanguosha_Stage.Stage_Jieshu:
                                #region 结束阶段
                                Debug.Log("->进入电脑结束阶段");
                                StartCoroutine(Player2_Jieshu_Process(msg));
                                #endregion
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
            case "Player2_Wuqi_Button":
            case "Player2_Fangju_Button":
            case "Player2_Shoupai_Button":
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
