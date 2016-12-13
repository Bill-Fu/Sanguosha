using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CardDeck : MonoBehaviour
{
	[System.Serializable]
	public class DeckItem
	{
		public int Count = 1;
		public CardDef Card;
	}
	
	protected DeckItem [] m_itemList;

    protected DeckItem [] Wujiang_itemList;
	
	// List of cards in live deck
	List<CardDef> m_cards = new List<CardDef>();
    List<CardDef> Wujiang_cards = new List<CardDef>();
	
	public virtual void Initialize()
	{
	}
	
	public void Reset()
	{
		m_cards.Clear();
        Wujiang_cards.Clear();

        foreach(DeckItem item in Wujiang_itemList)
        {
            for(int i = 0; i < item.Count; ++i)
            {
                Wujiang_cards.Add(item.Card);
            }
        }
		
		foreach (DeckItem item in m_itemList)
		{
			for (int i=0; i<item.Count; ++i)
			{
				m_cards.Add(item.Card);
			}
		}
	}
	//下面这个函数Shuffle()是用来洗游戏牌的
	public void Shuffle()
	{
		for (int i=0; i<m_cards.Count; ++i)
		{
			int other = Random.Range(0,m_cards.Count);
			if (other != i)
			{
				CardDef swap = m_cards[i];
				m_cards[i] = m_cards[other];
				m_cards[other] = swap;
			}
		}
	}
    //下面这个函数WujiangShuffle()是用来洗武将牌的
    public void WujiangShuffle()
    {
        for (int i = 0; i < Wujiang_cards.Count; ++i)
        {
            int other = Random.Range(0, Wujiang_cards.Count);
            if (other != i)
            {
                CardDef swap = Wujiang_cards[i];
                Wujiang_cards[i] = Wujiang_cards[other];
                Wujiang_cards[other] = swap;
            }
        }
    }
	
	public CardDef Pop()
	{
        Debug.Log("Pop Youxi");
		int last = m_cards.Count-1;
		if (last >= 0)
		{
			CardDef result = m_cards[last];
			m_cards.RemoveAt(last);
			return result;
		}
		return null;
	}

    public CardDef Wujiangpop()
    {
        Debug.Log("Pop Wujiang");
        int last = Wujiang_cards.Count - 1;
        if (last >= 0)
        {
            CardDef result = Wujiang_cards[last];
            Wujiang_cards.RemoveAt(last);
            return result;
        }
        return null;
    }

    public int RestCards()
    {
        return m_cards.Count;
    }
}
