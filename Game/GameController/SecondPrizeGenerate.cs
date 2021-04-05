using System;
using System.Collections.Generic;
using System.Windows.Forms;
using GameEngine;
using ModelLibrary;


namespace GameController
{
    public class SecondPrizeGenerate
    {
        public DX2D _dx2d;

        public Wrapper<Prize> prizeWpapper;

        public List<Wrapper<Prize>> ListOfPrizes = new List<Wrapper<Prize>>();

        List<PrizeCreator> prizeCreators = new List<PrizeCreator>() {
        new HealthPrizeCreator(),new JumpPrizeCreator(), new VelocityPrizeCreator()};

        Dictionary<Type, string> pathDictionary = new Dictionary<Type, string>()
        {
            { typeof(HealthPrize), "C://Курсовая//Sprites//healthPrize.bmp" },
            { typeof(JumpPrize), "C://Курсовая//Sprites//jumpPrize.bmp" },
            { typeof(VelocityPrize), "C://Курсовая//Sprites//speedPrize.bmp" },
        };

        Dictionary<Type, float> coordByXPrize = new Dictionary<Type, float>()
        {
            { typeof(HealthPrize), 43.0f },
            { typeof(JumpPrize), 43.0f },
            { typeof(VelocityPrize), 43.0f },
        };

        Dictionary<Type, float> coordByYPrize = new Dictionary<Type, float>()
        {
            { typeof(HealthPrize), 13.5f },
            { typeof(JumpPrize), 13.5f },
            { typeof(VelocityPrize), 13.5f },
        };

        public Wrapper<Prize> ReturnPrize(PrizeCreator prizeCreator)
        {
            Prize prize = prizeCreator.Create();
            Wrapper<Prize> prizeWrapper = new Wrapper<Prize>(new Sprite(_dx2d, _dx2d.LoadBitmap(pathDictionary[prize.GetType()]), coordByXPrize[prize.GetType()], coordByYPrize[prize.GetType()], 0.0f), prize);
            return prizeWrapper;
        }

        public Wrapper<Prize> DrawPrize()
        {
            Random random = new Random();
            int y = random.Next(0, 3);
            PrizeCreator obs = prizeCreators[y];
            prizeWpapper = ReturnPrize(obs);
            return prizeWpapper;
        }

        public void setPrizeData(object sender, EventArgs args)
        {
            prizeWpapper = DrawPrize();
            ListOfPrizes.Add(prizeWpapper);
        }
    }
}
