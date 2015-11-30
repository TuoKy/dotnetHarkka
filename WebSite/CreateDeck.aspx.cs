﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class CreateDeck : System.Web.UI.Page
{
    private CardLogic logic;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            logic = new CardLogic();
            Session["logic"] = logic;
            //User id pitää vaihtaa kun saa loginin tehtyä
            logic.startDeck((string)(Session["Class"]),1);
            logic.divideAndConquer((string)(Session["Class"]));
            setPictures(logic.index);
            GridViewDeck.DataSource = logic.cardsInDeck as IEnumerable<Card>;
            GridViewDeck.DataBind();
        }
        else
        {
            logic = (CardLogic)(Session["logic"]);
            GridViewDeck.DataSource = logic.cardsInDeck as IEnumerable<Card>;
            GridViewDeck.DataBind();
        }
    }

    private void setPictures(int i)
    {
        Image1.ImageUrl = logic.smallCardPool[i].img;
        Image1.AlternateText = logic.smallCardPool[i].cardId.ToString();
        Image2.ImageUrl = logic.smallCardPool[i + 1].img;
        Image2.AlternateText = logic.smallCardPool[i + 1].cardId.ToString();
        Image3.ImageUrl = logic.smallCardPool[i + 2].img;
        Image3.AlternateText = logic.smallCardPool[i + 2].cardId.ToString();
        Image4.ImageUrl = logic.smallCardPool[i + 3].img;
        Image4.AlternateText = logic.smallCardPool[i + 3].cardId.ToString();
        Image5.ImageUrl = logic.smallCardPool[i + 4].img;
        Image5.AlternateText = logic.smallCardPool[i + 4].cardId.ToString();
        Image6.ImageUrl = logic.smallCardPool[i + 5].img;
        Image6.AlternateText = logic.smallCardPool[i + 5].cardId.ToString();
        Image7.ImageUrl = logic.smallCardPool[i + 6].img;
        Image7.AlternateText = logic.smallCardPool[i + 6].cardId.ToString();
        Image8.ImageUrl = logic.smallCardPool[i + 7].img;
        Image8.AlternateText = logic.smallCardPool[i + 7].cardId.ToString();

        logic.index = i + 7;
    }

    private void hideButtons()
    {
        
    }

    protected void GridButtons_Command(object sender, GridViewCommandEventArgs e)
    {
        int index = Convert.ToInt32(e.CommandArgument);
        GridViewRow row = GridViewDeck.Rows[index];
        int temp = logic.cardsInDeck.FindIndex(x=>x.name==row.Cells[1].Text);

        logic.deleteCard(temp);
    }


    protected void druidCards_Click(object sender, EventArgs e)
    {
        logic.divideAndConquer("Druid");
        setPictures(logic.index);
    }

    protected void hunterCards_Click(object sender, EventArgs e)
    {
        logic.divideAndConquer("Hunter");
        setPictures(logic.index);
    }

    protected void mageCards_Click(object sender, EventArgs e)
    {
        logic.divideAndConquer("Mage");
        setPictures(logic.index);
    }

    protected void paladinCards_Click(object sender, EventArgs e)
    {
        logic.divideAndConquer("Paladin");
        setPictures(logic.index);
    }

    protected void priestCards_Click(object sender, EventArgs e)
    {
        logic.divideAndConquer("Priest");
        setPictures(logic.index);
    }

    protected void rogueCards_Click(object sender, EventArgs e)
    {
        logic.divideAndConquer("Rogue");
        setPictures(logic.index);
    }

    protected void shamanCards_Click(object sender, EventArgs e)
    {
        logic.divideAndConquer("Shaman");
        setPictures(logic.index);
    }
    protected void warlockCards_Click(object sender, EventArgs e)
    {
        logic.divideAndConquer("Warlock");
        setPictures(logic.index);
    }

    protected void warriorCards_Click(object sender, EventArgs e)
    {
        logic.divideAndConquer("Warrior");
        setPictures(logic.index);
    }

    protected void neutrals_Click(object sender, EventArgs e)
    {
        logic.divideAndConquer("");
        setPictures(logic.index);
    }

    protected void previous_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            setPictures(logic.index - 15);
        }
        catch (Exception)
        {
           
        }
    }

    protected void next_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            setPictures(logic.index + 1);
        }
        catch (Exception)
        {
            
        }
    }


    protected void ImageButtonClick(object sender, ImageClickEventArgs e)
    {
        ImageButton tempButton = (ImageButton)sender;
        //Tähän voi asettaa jotain tarkistuksia alternateText kentälle
        logic.addCard(int.Parse(tempButton.AlternateText));
        GridViewDeck.DataBind();
    }

    protected void SaveButtonClick(object sender, EventArgs e)
    {
        if(deckName.Text != "")
        {
            logic.saveDeck(deckName.Text);
            Response.Redirect("Default.aspx");
        }
    }
}