using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CardGame : MonoBehaviour
{
	public CardDeck Deck;
	//List<CardDefinition> m_deck = new List<CardDefinition>();
	
	List<Card> m_dealer = new List<Card>();
	List<Card> m_player = new List<Card>();

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

	GameState m_state;
	
	GameObject [] Buttons;
	
	// Use this for initialization
	void Start ()
	{
        //���ó�ʼ״̬
		m_state = GameState.Invalid;
        //������Deck����˼Ӧ�þ��ǿ��Ƶ���˼�����������ʼ����������ʲô��û��
		Deck.Initialize();
        //��ȡ�ҵ�����Text�Ķ���
		PlayerWins = this.transform.Find("MessagePlayerWins").gameObject;
		DealerWins = this.transform.Find("MessageDealerWins").gameObject;
		NobodyWins = this.transform.Find("MessageTie").gameObject;
        //������Textȫ������Ϊ����
		PlayerWins.SetActive(false);
		DealerWins.SetActive(false);
		NobodyWins.SetActive(false);
        //��ʼ����Ű�����Buttons����
		Buttons = new GameObject[3];
        //��ȡ�ҵ�����Button�Ķ���
		Buttons[0] = this.transform.Find("Button1").gameObject;
		Buttons[1] = this.transform.Find("Button2").gameObject;
		Buttons[2] = this.transform.Find("Button3").gameObject;
        //�����������������button����ɫ��״̬����Լ����ÿһ����Ҫ����button״̬��ʱ�����
		UpdateButtons();
	}
	//��������button����ɫ״̬
	void UpdateButtons()
	{
		Buttons[0].GetComponent<Renderer>().material.color = Color.blue;
		Buttons[1].GetComponent<Renderer>().material.color = (m_state == GameState.Started) ? Color.blue : Color.red;
		Buttons[2].GetComponent<Renderer>().material.color = (m_state == GameState.Started || m_state == GameState.PlayerBusted) ? Color.blue : Color.red;
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
		if (Input.GetKeyDown(KeyCode.F1))
		{
			OnReset();
		}
		else if (Input.GetKeyDown(KeyCode.F2))
		{
			OnHitMe();
		}
		else if (Input.GetKeyDown(KeyCode.F3))
		{
			OnStop();
		}
		UpdateButtons();
	}
	//shuffle�Ļ�Ӧ����ϴ�Ƶĺ���
	void Shuffle()
	{
		if (m_state != GameState.Invalid)
		{
		}
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
	
	void HitDealer()
	{
        //��û�ù����ƶ���ȡ��һ����
		CardDef c1 = Deck.Pop();
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
            //ϴ��
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
			UpdateButtons();
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
	//����ĺ������ṩ��button�ⲿ���õ�
	public void OnButton(string msg)
	{
		Debug.Log("OnButton = "+msg);
		switch (msg)
		{
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
		}
	}
}
