using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class CardGame : MonoBehaviour
{
    //体力最大值为4，目前规则下是这样子的
    const int Tili_Max = 4;
    const int Button_Max = 6;
    const int Xuanjiang_Max = 3;

	public CardDeck Deck;
	//List<CardDefinition> m_deck = new List<CardDefinition>();
	
	List<Card> m_dealer = new List<Card>();
	List<Card> m_player = new List<Card>();

    GameState m_state;

    GameObject PlayerWins;
	GameObject DealerWins;
	GameObject NobodyWins;
	
	enum GameState
	{
		Invalid,
		Started,
		PlayerBusted,
		Resolving,
		DealerWins,
		PlayerWins,
		NobodyWins,
	};

    GameObject[] Buttons;

    //已经发的武将牌的数量
    int Wujiang_num;

    #region 三国杀游戏三层状态机定义
    /*
     * 状态机分为三层
     * 第一层：游戏状态
     * 分为 无效状态，选择武将，正常游戏，游戏结束，阵营1获胜，阵营2获胜，阵营3获胜,解决状态
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

    #endregion

    #region 状态机变量定义
    Sanguosha_GameState game_state;
    Sanguosha_Round game_round;
    Sanguosha_Stage game_stage;
    #endregion

    #region 玩家手牌列表和选将框列表定义
    //玩家选将列表武将卡牌
    List<Card> Xuanjiang_Cards = new List<Card>();
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
    #endregion

    #region 定义静态文本Text
    //玩家1判定区域
    GameObject Sanguosha_Text_Player1_Panding1;
    GameObject Sanguosha_Text_Player1_Panding2;
    //玩家1装备区域
    GameObject Sanguosha_Text_Player1_Zhuangbei1;
    GameObject Sanguosha_Text_Player1_Zhuangbei2;
    //玩家1手牌区域
    GameObject Sanguosha_Text_Player1_Shoupai;
    //玩家2判定区域
    GameObject Sanguosha_Text_Player2_Panding1;
    GameObject Sanguosha_Text_Player2_Panding2;
    //玩家2装备区域
    GameObject Sanguosha_Text_Player2_Zhuangbei1;
    GameObject Sanguosha_Text_Player2_Zhuangbei2;
    //玩家2手牌区域
    GameObject Sanguosha_Text_Player2_Shoupai;
    #endregion

    #region 定义静态image
    GameObject[] Sanguosha_Image_Player1_Tili;
    GameObject[] Sanguosha_Image_Player2_Tili;
    #endregion

    // Use this for initialization
    void Start ()
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

        #region 初始化存放静态按键的Sanguosha_Buttons数组
        Sanguosha_Buttons = new GameObject[Button_Max];
        Sanguosha_Buttons[0] = this.transform.Find("Restart").gameObject;
        Sanguosha_Buttons[1] = this.transform.Find("Queding").gameObject;
        Sanguosha_Buttons[2] = this.transform.Find("Quxiao").gameObject;
        Sanguosha_Buttons[3] = this.transform.Find("Jieshu").gameObject;
        Sanguosha_Buttons[4] = this.transform.Find("Jineng1").gameObject;
        Sanguosha_Buttons[5] = this.transform.Find("Jineng2").gameObject;
        #endregion

        #region 初始化游戏中的文本对象
        //玩家1区域的文本对象
        Sanguosha_Text_Player1_Panding1 = this.transform.Find("Player1_Panding1").gameObject;
        Sanguosha_Text_Player1_Panding2 = this.transform.Find("Player1_Panding2").gameObject;
        Sanguosha_Text_Player1_Zhuangbei1 = this.transform.Find("Player1_Zhuangbei1").gameObject;
        Sanguosha_Text_Player1_Zhuangbei2 = this.transform.Find("Player1_Zhuangbei2").gameObject;
        Sanguosha_Text_Player1_Shoupai = this.transform.Find("Player1_Shoupai").gameObject;
        //玩家2区域的文本对象
        Sanguosha_Text_Player2_Panding1 = this.transform.Find("Player2_Panding1").gameObject;
        Sanguosha_Text_Player2_Panding2 = this.transform.Find("Player2_Panding2").gameObject;
        Sanguosha_Text_Player2_Zhuangbei1 = this.transform.Find("Player2_Zhuangbei1").gameObject;
        Sanguosha_Text_Player2_Zhuangbei2 = this.transform.Find("Player2_Zhuangbei2").gameObject;
        Sanguosha_Text_Player2_Shoupai = this.transform.Find("Player2_Shoupai").gameObject;
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

        #region 将除了重新开始游戏button以外的image，text和button全部设置为不可见
        //所有text都不可见
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
        //所有button都不可见
        for(int i = 1; i < Sanguosha_Buttons.Length; ++i)
        {
            Sanguosha_Buttons[i].SetActive(false);
        }
        //所有image都不可见
        for(int i = 0; i < Tili_Max; ++i)
        {
            Sanguosha_Image_Player1_Tili[i].SetActive(false);
            Sanguosha_Image_Player2_Tili[i].SetActive(false);
        }
        #endregion

        #region 要删除的之前游戏的代码
        m_state = GameState.Invalid;
        //获取找到三个Text的对象
        PlayerWins = this.transform.Find("MessagePlayerWins").gameObject;
		DealerWins = this.transform.Find("MessageDealerWins").gameObject;
		NobodyWins = this.transform.Find("MessageTie").gameObject;
        //将三个Text全都设置为隐藏
		PlayerWins.SetActive(false);
		DealerWins.SetActive(false);
		NobodyWins.SetActive(false);
        #endregion
    }
    //显示获胜方信息
    void ShowMessage(string msg)
	{
		if (msg == "Dealer")
		{
			PlayerWins.SetActive(false);
			DealerWins.SetActive(true);
			NobodyWins.SetActive(false);
		}
		else if (msg == "Player")
		{
			PlayerWins.SetActive(true);
			DealerWins.SetActive(false);
			NobodyWins.SetActive(false);
		}
		else if (msg == "Nobody")
		{
			PlayerWins.SetActive(false);
			DealerWins.SetActive(false);
			NobodyWins.SetActive(true);
		}
		else
		{
			PlayerWins.SetActive(false);
			DealerWins.SetActive(false);
			NobodyWins.SetActive(false);
		}
	}
	
	// Update is called once per frame
    //就我的猜测这部分代码是想通过键盘控制按三个button
	void Update ()
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
	void Shuffle()
	{
		if (m_state != GameState.Invalid)
		{
		}
	}
    //仿照Clear()函数
    void Sanguosha_Clear()
    {
        foreach(Card card in Player1_Cards)
        {
            GameObject.DestroyImmediate(card.gameObject);
        }
        foreach(Card card in Player2_Cards)
        {
            GameObject.DestroyImmediate(card.gameObject);
        }
        foreach(Card card in Xuanjiang_Cards)
        {
            GameObject.DestroyImmediate(card.gameObject);
        }
        Player1_Cards.Clear();
        Player2_Cards.Clear();
        Xuanjiang_Cards.Clear();
        Deck.Reset();
    }
	//？？？清理卡牌函数
	void Clear()
	{
        //依次销毁m_dealer list中的元素
		foreach (Card c in m_dealer)
		{
			GameObject.DestroyImmediate(c.gameObject);
		}
        //依次销毁m_player list中的元素
		foreach (Card c in m_player)
		{
			GameObject.DestroyImmediate(c.gameObject);
		}
        //清空List
		m_dealer.Clear();
		m_player.Clear();
        //重新从卡牌库中添加卡牌
		Deck.Reset();
	}
	
	Vector3 GetDeckPosition()
	{
		return new UnityEngine.Vector3(-7,0,0);
	}
	
	const float FlyTime = 0.5f;
	
    void Xuanzewujiang()
    {
        CardDef tmpCard = Deck.Wujiangpop();
        float x, y, z;

        Debug.Log("Xuanzewujiang");
        if (tmpCard != null)
        {
            GameObject newObj = new GameObject();
            newObj.name = "Wujiang_Card";
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
    
	void HitDealer()
	{
        //从没用过的游戏牌堆里取出一张牌
		CardDef c1 = Deck.Pop();
        //尝试一下武将牌的效果
        //CardDef c1 = Deck.Wujiangpop();
		if (c1 != null)
		{
			GameObject newObj = new GameObject();
			newObj.name = "Card";
			Card newCard = newObj.AddComponent(typeof(Card)) as Card;
			newCard.Definition = c1;
			newObj.transform.parent = Deck.transform;
			newCard.TryBuild();
			float x = -3+(m_dealer.Count)*2.0f;
			float z = (m_dealer.Count)*-0.1f;
			Vector3 deckPos = GetDeckPosition();
			newObj.transform.position = deckPos;
			m_dealer.Add(newCard);
			newCard.SetFlyTarget(deckPos,new Vector3(x,2,z),FlyTime);
		}
	}
	void HitPlayer()
	{
        Debug.Log("HitPlayer");
        CardDef c1 = Deck.Pop();
		if (c1 != null)
		{
            Debug.Log("Deck-Popped");
            GameObject newObj = new GameObject();
			newObj.name = "Card";
			Card newCard = newObj.AddComponent(typeof(Card)) as Card;
			newCard.Definition = c1;
			newObj.transform.parent = Deck.transform;
			newCard.TryBuild();
			float x = -3+(m_player.Count)*1.5f;
			float y = -3-m_player.Count*0.15f;
			float z = (m_player.Count)*-0.1f;
			//newObj.transform.position = new Vector3(x,-3,z);
			m_player.Add(newCard);
			Vector3 deckPos = GetDeckPosition();
			newCard.transform.position = deckPos;
			newCard.SetFlyTarget(deckPos,new Vector3(x,y,z),FlyTime);
		}
	}
	
	static int Value(Card c)
	{
		if (c != null)
		{
			switch (c.Definition.Pattern)
			{
			case 0:
				return 10;
			case 1:
				return 11;
			}
			return c.Definition.Pattern;
		}
		return 0;
	}
	
	static int GetScore(List<Card> cards)
	{
		int score = 0;
		bool ace = false;
		foreach (Card c in cards)
		{
			int s = Value(c);
			if ((score + s) > 21)
			{
				if (s == 11)
				{
					s = 1;
				}
				else if (ace)
				{
					score -= 10;
					ace = false;
				}
			}
			score += s;
			ace |= (s == 11);
		}
		return score;
	}
	
	int GetDealerScore()
	{
		return GetScore(m_dealer);
	}
	
	int GetPlayerScore()
	{
		return GetScore(m_player);
	}
	
	const float DealTime = 0.35f;
	
    //按到Reset按键之后调用的函数
	IEnumerator OnReset()
	{
        //就我的理解resolving指的状态应该是电脑控制发牌以及电脑Hit的状态
		if (m_state != GameState.Resolving)
		{
			m_state = GameState.Resolving;
            //不用显示谁获胜的提示口号
			ShowMessage("");
            //从新装满可用的牌
            Clear();
            //洗武将牌
            Deck.WujiangShuffle();
            //洗游戏牌
			Deck.Shuffle();
			HitDealer();
			yield return new WaitForSeconds(DealTime);
			HitDealer();
			yield return new WaitForSeconds(DealTime);
			HitPlayer();
			yield return new WaitForSeconds(DealTime);
			HitPlayer();
			m_state = GameState.Started;
		}
	}
    //按到Hit按键之后调用的的函数
	void OnHitMe()
	{
        Debug.Log("OnHitMe");
		if (m_state == GameState.Started)
		{
			HitPlayer();
			if (GetPlayerScore() > 21)
			{
				m_state = GameState.PlayerBusted;
			}
		}
	}
	bool TryFinalize(int playerScore, int dealer)
	{
		if (playerScore > 21)
		{
			// Dealer Wins!
			ShowMessage("Dealer");
			m_state = GameState.DealerWins;
			return true;
		}
		if (dealer > 21)
		{
			ShowMessage("Player");
			m_state = GameState.PlayerWins;
			return true;
		}
		if (dealer > playerScore)
		{
			ShowMessage("Dealer");
			m_state = GameState.DealerWins;
			return true;
		}
		// Natural 21 beats everything else.
		bool pn = (playerScore == 21) && (m_player.Count == 2);
		bool dn = (dealer == 21) && (m_dealer.Count == 2);
		if (pn && !dn)
		{
			ShowMessage("Player");
			m_state = GameState.PlayerWins;
			return true;
		}
		if (dn && !pn)
		{
			ShowMessage("Dealer");
			m_state = GameState.DealerWins;
			return true;
		}
		if (dealer > 17)
		{
			if (playerScore == dealer)
			{
				// Nobody Wins!
				ShowMessage("Nobody");
				m_state = GameState.NobodyWins;
				return true;
			}
			else if (dealer < playerScore)
			{
				// Player Wins!
				ShowMessage("Player");
				m_state = GameState.PlayerWins;
				return true;
			}
			else
			{
				// Dealer Wins!
				ShowMessage("Dealer");
				m_state = GameState.DealerWins;
				return true;
			}
		}
		return false;
	}
    //按到Stop按键之后调用的函数
	IEnumerator OnStop()
	{
		if (m_state == GameState.Started || m_state == GameState.PlayerBusted)
		{
			m_state = GameState.Resolving;
			int playerScore = GetPlayerScore();
			while (true)
			{
				int d = GetDealerScore();
				Debug.Log(string.Format("Player={0}  Dealer={1}",playerScore,d));
				if (TryFinalize(playerScore,d))
				{
					break;
				}
				else
				{
					Debug.Log("HitDealer");
					HitDealer();
					yield return new WaitForSeconds(DealTime*1.5f);
				}
			}
		}
	}
    //提供给测试功能的Test button调用
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
            Xuanzewujiang();
            yield return new WaitForSeconds(DealTime);
            Xuanzewujiang();
            yield return new WaitForSeconds(DealTime);
            Xuanzewujiang();
            yield return new WaitForSeconds(DealTime);
            game_state = Sanguosha_GameState.State_ChooseWujiang;
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
            /*
		case "Reset":
                //StartCoroutine()类似于开一个线程来进行这个函数
			StartCoroutine(OnReset());
			break;
		case "Hit":
			OnHitMe();
			break;
		case "Stop":
			StartCoroutine(OnStop());
			break;
        case "Test":
            OnTest();
            break;
            */

		}
	}
}
