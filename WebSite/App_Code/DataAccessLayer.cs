﻿using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for DataAccessLayer
/// </summary>
public class DataAccessLayer
{
    private string connStr;
    private string sql;
    /*
        Tarvitaan pakat käyttäjältä
        Tarvitaan pakan luonti (insertoida kortteja deck_has_card)
        tarvitaan kortit näkyviin(millä logiikalla? eli kui monta ja millä haulla siis)
    */

    public DataAccessLayer()
    {
        connStr = ConfigurationManager.ConnectionStrings["connString"].ConnectionString;
        //connStr = @"Data Source=db-H3408.vm; Database=mydb; User ID=DotnetHarkka; Password=dotnet";
    }

    public void testConnection()
    {
        using (MySqlConnection cnn = new MySqlConnection(connStr))
        {
            cnn.Open();
            //teetesti
            cnn.Close();
        }
    }
    /*
        Pakan luonti eli 1 Insert pakkaluokkaan ja korttien
        lukumäärän verran Inserttejä deck_has_card luokkaan.
    */
    public void writeDeckToDB(Deck deck)
    {
        sql = "INSERT INTO Deck (DeckName,User_idUser)" +
              "Values (@name, @userId);";
        MySqlConnection conn = new MySqlConnection(connStr);
        MySqlCommand cmd = new MySqlCommand(sql, conn);
        conn.Open();

        //Kirjoitetaan deck tauluun pakan nimi ja sen tekijä
        cmd.Parameters.AddWithValue("@name", deck.name);
        cmd.Parameters.AddWithValue("@userId", deck.userId);
        cmd.ExecuteNonQuery();
        //Haetaan juuri luodun pakan id
        long deckId = cmd.LastInsertedId;

        //Kirjoitetaan nyt jokainen pakkaan valittu kortti deck_has_card tauluun.
        foreach (var item in deck.cards)
        {
            cmd.Parameters.Clear();
            cmd.CommandText = "INSERT INTO deck_has_card (Deck_idDeck,Card_idCard,CardCount)" +
                                "Values (@deckId, @cardId, @count);";
            cmd.Parameters.AddWithValue("@deckId", deckId);
            cmd.Parameters.AddWithValue("@cardId", item.cardId);
            cmd.Parameters.AddWithValue("@count", item.count);
            cmd.ExecuteNonQuery();
        }

        //Yhteys suljetaan
        conn.Close();
    }

    /*
        Funktio palauttaa halutun pakan
    */
    public Deck readDeckFromDB(int deckId)
    {
        Deck deck = new Deck();
        deck.deckId = deckId;

        sql = "SELECT * FROM deck WHERE idDeck=" + deckId.ToString();
        MySqlConnection conn = new MySqlConnection(connStr);
        MySqlCommand cmd = new MySqlCommand(sql, conn);

        conn.Open();

        MySqlDataReader reader = cmd.ExecuteReader();
        if (reader.HasRows)
        {
            reader.Read();
            deck.name = reader["DeckName"].ToString();
            deck.userId = int.Parse(reader["User_idUser"].ToString());
        }
        reader.Close();


        List<deckHasCard> temp = new List<deckHasCard>();
        cmd.CommandText = "SELECT * FROM deck_has_card WHERE Deck_idDeck=" + deckId.ToString();
        reader = cmd.ExecuteReader();
        if (reader.HasRows)
        {
            while (reader.Read())
            {
                temp.Add(new deckHasCard
                {
                    cardId = int.Parse(reader["Card_idCard"].ToString()),
                    count = int.Parse(reader["CardCount"].ToString())
                });
            }
        }
        reader.Close();

        conn.Close();
        return deck;
    }

    public List<Card> readCardsFromDB()
    {
        List<Card> cards = new List<Card>();

        sql = " SELECT * FROM card";
        MySqlConnection conn = new MySqlConnection(connStr);
        MySqlCommand cmd = new MySqlCommand(sql, conn);
        
        conn.Open();

        MySqlDataReader reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            cards.Add(new Card
            {
                cardId = int.Parse(reader["idCard"].ToString()),
                name = reader["CardName"].ToString(),
                cardSet = reader["CardSet"].ToString(),
                type = reader["CardType"].ToString(),
                rarity = reader["CardRarity"].ToString(),
                cost = int.Parse(reader["CardCost"].ToString()),
                attack = int.Parse(reader["CardAttack"].ToString()),
                health = int.Parse(reader["CardHealth"].ToString()),
                text = reader["CardText"].ToString(),
                playerClass = reader["CardPlayerClass"].ToString(),
                img = reader["CardImg"].ToString()
            });
        }
        conn.Close();
        return cards;
    }

    public void InsertCardsToDb(List<Card> cards)
    {        
        sql = "INSERT INTO Card (CardName,Cardset,CardType,CardRarity,CardCost,CardAttack,CardHealth,CardText,CardPlayerClass,CardImg)" +  
              "Values (@name, @set, @type, @rarity, @cost, @attack, @health, @text, @playerClass, @img);";
        using (MySqlConnection conn = new MySqlConnection(connStr))
        {
            conn.Open();
            foreach (var item in cards)
            {
                insertCard(conn, item);
            }
            conn.Close();
        }
    }

    public void insertCard(MySqlConnection conn, Card x)
    {
        if (x.type != "Hero")
        {
            using (MySqlCommand cmd = new MySqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@name", x.name);
                cmd.Parameters.AddWithValue("@set", x.cardSet);
                cmd.Parameters.AddWithValue("@type", x.type);
                cmd.Parameters.AddWithValue("@rarity", x.rarity);
                cmd.Parameters.AddWithValue("@cost", x.cost);
                cmd.Parameters.AddWithValue("@attack", x.attack);
                cmd.Parameters.AddWithValue("@health", x.health);
                cmd.Parameters.AddWithValue("@text", x.text);
                cmd.Parameters.AddWithValue("@playerClass", x.playerClass);
                cmd.Parameters.AddWithValue("@img", x.img);
                cmd.ExecuteNonQuery();
            }
        }
    }

}