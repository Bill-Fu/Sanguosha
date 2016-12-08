using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class CardGame : MonoBehaviour
{
    //�������ֵΪ4��Ŀǰ�������������ӵ�
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

    //�Ѿ������佫�Ƶ�����
    int Wujiang_num;

    #region ����ɱ��Ϸ����״̬������
    /*
     * ״̬����Ϊ����
     * ��һ�㣺��Ϸ״̬
     * ��Ϊ ��Ч״̬��ѡ���佫��������Ϸ����Ϸ��������Ӫ1��ʤ����Ӫ2��ʤ����Ӫ3��ʤ,���״̬
     * ����һ�£�����Ľ��״̬�ǲο�����ݴ���Ķ��壬�о���������ڽ���һЩ����Ч����ʱ��Ҫ��������
     * �ڶ��㣺�غ�״̬
     * ��Ϊ ��Ч�غϣ����1�غϣ����2�غϣ����3�غϣ����4�غϣ����5�غϣ����6�غϣ����7�غϣ����8�غ�
     * �����㣺�غ��н׶�״̬
     * ��Ϊ ��Ч�׶Σ�׼���׶Σ��ж��׶Σ����ƽ׶Σ����ƽ׶Σ����ƽ׶Σ������׶�
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

    #region ״̬����������
    Sanguosha_GameState game_state;
    Sanguosha_Round game_round;
    Sanguosha_Stage game_stage;
    #endregion

    #region ��������б��ѡ�����б���
    //���ѡ���б��佫����
    List<Card> Xuanjiang_Cards = new List<Card>();
    //���1���г��еĿ���
    List<Card> Player1_Cards = new List<Card>();
    //���2���г��еĿ���
    List<Card> Player2_Cards = new List<Card>();
    //���3���г��еĿ���
    List<Card> Player3_Cards = new List<Card>();
    //���4���г��еĿ���
    List<Card> Player4_Cards = new List<Card>();
    //���5���г��еĿ���
    List<Card> Player5_Cards = new List<Card>();
    //���6���г��еĿ���
    List<Card> Player6_Cards = new List<Card>();
    //���7���г��еĿ���
    List<Card> Player7_Cards = new List<Card>();
    //���8���г��еĿ���
    List<Card> Player8_Cards = new List<Card>();
    #endregion

    #region ���徲̬Button
    GameObject[] Sanguosha_Buttons;
    #endregion

    #region ���徲̬�ı�Text
    //���1�ж�����
    GameObject Sanguosha_Text_Player1_Panding1;
    GameObject Sanguosha_Text_Player1_Panding2;
    //���1װ������
    GameObject Sanguosha_Text_Player1_Zhuangbei1;
    GameObject Sanguosha_Text_Player1_Zhuangbei2;
    //���1��������
    GameObject Sanguosha_Text_Player1_Shoupai;
    //���2�ж�����
    GameObject Sanguosha_Text_Player2_Panding1;
    GameObject Sanguosha_Text_Player2_Panding2;
    //���2װ������
    GameObject Sanguosha_Text_Player2_Zhuangbei1;
    GameObject Sanguosha_Text_Player2_Zhuangbei2;
    //���2��������
    GameObject Sanguosha_Text_Player2_Shoupai;
    #endregion

    #region ���徲̬image
    GameObject[] Sanguosha_Image_Player1_Tili;
    GameObject[] Sanguosha_Image_Player2_Tili;
    #endregion

    // Use this for initialization
    void Start ()
	{
        //����ȫ��
        Screen.SetResolution(1366, 768, false);
        //���ó�ʼ״̬
        #region ���ó�ʼ״̬Ϊ ��Ϸ״̬����Ч״̬ ��Ϸ�غϣ���Ч�غ� ��Ϸ�׶Σ���Ч�׶�
        game_state = Sanguosha_GameState.State_Invalid;
        game_round = Sanguosha_Round.Round_Invalid;
        game_stage = Sanguosha_Stage.Stage_Invalid;
        #endregion

        //Deck����˼Ӧ�þ��ǿ��Ƶ���˼�����������ʼ����������ʲô��û��
        //��ʼ���ƶ���Ϣ
		Deck.Initialize();

        Wujiang_num = 0;

        #region ��ʼ����ž�̬������Sanguosha_Buttons����
        Sanguosha_Buttons = new GameObject[Button_Max];
        Sanguosha_Buttons[0] = this.transform.Find("Restart").gameObject;
        Sanguosha_Buttons[1] = this.transform.Find("Queding").gameObject;
        Sanguosha_Buttons[2] = this.transform.Find("Quxiao").gameObject;
        Sanguosha_Buttons[3] = this.transform.Find("Jieshu").gameObject;
        Sanguosha_Buttons[4] = this.transform.Find("Jineng1").gameObject;
        Sanguosha_Buttons[5] = this.transform.Find("Jineng2").gameObject;
        #endregion

        #region ��ʼ����Ϸ�е��ı�����
        //���1������ı�����
        Sanguosha_Text_Player1_Panding1 = this.transform.Find("Player1_Panding1").gameObject;
        Sanguosha_Text_Player1_Panding2 = this.transform.Find("Player1_Panding2").gameObject;
        Sanguosha_Text_Player1_Zhuangbei1 = this.transform.Find("Player1_Zhuangbei1").gameObject;
        Sanguosha_Text_Player1_Zhuangbei2 = this.transform.Find("Player1_Zhuangbei2").gameObject;
        Sanguosha_Text_Player1_Shoupai = this.transform.Find("Player1_Shoupai").gameObject;
        //���2������ı�����
        Sanguosha_Text_Player2_Panding1 = this.transform.Find("Player2_Panding1").gameObject;
        Sanguosha_Text_Player2_Panding2 = this.transform.Find("Player2_Panding2").gameObject;
        Sanguosha_Text_Player2_Zhuangbei1 = this.transform.Find("Player2_Zhuangbei1").gameObject;
        Sanguosha_Text_Player2_Zhuangbei2 = this.transform.Find("Player2_Zhuangbei2").gameObject;
        Sanguosha_Text_Player2_Shoupai = this.transform.Find("Player2_Shoupai").gameObject;
        #endregion

        #region ��ʼ����Ϸ�е�image����
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

        #region ���������¿�ʼ��Ϸbutton�����image��text��buttonȫ������Ϊ���ɼ�
        //����text�����ɼ�
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
        //����button�����ɼ�
        for(int i = 1; i < Sanguosha_Buttons.Length; ++i)
        {
            Sanguosha_Buttons[i].SetActive(false);
        }
        //����image�����ɼ�
        for(int i = 0; i < Tili_Max; ++i)
        {
            Sanguosha_Image_Player1_Tili[i].SetActive(false);
            Sanguosha_Image_Player2_Tili[i].SetActive(false);
        }
        #endregion

        #region Ҫɾ����֮ǰ��Ϸ�Ĵ���
        m_state = GameState.Invalid;
        //��ȡ�ҵ�����Text�Ķ���
        PlayerWins = this.transform.Find("MessagePlayerWins").gameObject;
		DealerWins = this.transform.Find("MessageDealerWins").gameObject;
		NobodyWins = this.transform.Find("MessageTie").gameObject;
        //������Textȫ������Ϊ����
		PlayerWins.SetActive(false);
		DealerWins.SetActive(false);
		NobodyWins.SetActive(false);
        #endregion
    }
    //��ʾ��ʤ����Ϣ
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
    //���ҵĲ²��ⲿ�ִ�������ͨ�����̿��ư�����button
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
	//shuffle�Ļ�Ӧ����ϴ�Ƶĺ���
	void Shuffle()
	{
		if (m_state != GameState.Invalid)
		{
		}
	}
    //����Clear()����
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
	//�����������ƺ���
	void Clear()
	{
        //��������m_dealer list�е�Ԫ��
		foreach (Card c in m_dealer)
		{
			GameObject.DestroyImmediate(c.gameObject);
		}
        //��������m_player list�е�Ԫ��
		foreach (Card c in m_player)
		{
			GameObject.DestroyImmediate(c.gameObject);
		}
        //���List
		m_dealer.Clear();
		m_player.Clear();
        //���´ӿ��ƿ�����ӿ���
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
        //��û�ù�����Ϸ�ƶ���ȡ��һ����
		CardDef c1 = Deck.Pop();
        //����һ���佫�Ƶ�Ч��
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
	
    //����Reset����֮����õĺ���
	IEnumerator OnReset()
	{
        //���ҵ����resolvingָ��״̬Ӧ���ǵ��Կ��Ʒ����Լ�����Hit��״̬
		if (m_state != GameState.Resolving)
		{
			m_state = GameState.Resolving;
            //������ʾ˭��ʤ����ʾ�ں�
			ShowMessage("");
            //����װ�����õ���
            Clear();
            //ϴ�佫��
            Deck.WujiangShuffle();
            //ϴ��Ϸ��
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
    //����Hit����֮����õĵĺ���
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
    //����Stop����֮����õĺ���
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
    //�ṩ�����Թ��ܵ�Test button����
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
    //����ĺ������ṩ��button�ⲿ���õ�
    
    IEnumerator OnRestart()
    {
        if (game_state != Sanguosha_GameState.State_Resolving)
        {
            #region ��Ҫ����һϵ�е�״̬�ĸ�λ
            game_state = Sanguosha_GameState.State_Resolving;
            Wujiang_num = 0;
            #region ���������¿�ʼ��Ϸbutton�����image��text��buttonȫ������Ϊ���ɼ�
            //����text�����ɼ�
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
            //����button�����ɼ�
            for (int i = 1; i < Sanguosha_Buttons.Length; ++i)
            {
                Sanguosha_Buttons[i].SetActive(false);
            }
            //����image�����ɼ�
            for (int i = 0; i < Tili_Max; ++i)
            {
                Sanguosha_Image_Player1_Tili[i].SetActive(false);
                Sanguosha_Image_Player2_Tili[i].SetActive(false);
            }
            #endregion
            #endregion
            //����װ�����ƶ�
            Sanguosha_Clear();
            //ϴ�佫��
            Deck.WujiangShuffle();
            //ϴ��Ϸ��
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
                //StartCoroutine()�����ڿ�һ���߳��������������
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
