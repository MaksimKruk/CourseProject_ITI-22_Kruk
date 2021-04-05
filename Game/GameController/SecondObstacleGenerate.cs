using System;
using System.Collections.Generic;
using GameEngine;
using ModelLibrary;
namespace GameController
{
    public class SecondObstacleGenerate
    {
        public DX2D _dx2d;

        public Wrapper<Obstacle> obstacleWrapper;

        public List<Wrapper<Obstacle>> ListOfObstacles = new List<Wrapper<Obstacle>>();

        List<ObstacleCreator> obstacleCreators = new List<ObstacleCreator>() {
        new PitCreator(),new WallCreator(), new AnimalCreator()};

        Dictionary<Type, string> dictionary = new Dictionary<Type, string>()
        {
            { typeof(Pit), "C://Курсовая//Sprites//pit.bmp" },
            { typeof(Wall), "C://Курсовая//WpfApp1//WpfApp1//BasicSprites//wall.bmp" },
            { typeof(Animal), "C://Курсовая//Sprites//animal.bmp" },
        };

        Dictionary<Type, float> coordByX = new Dictionary<Type, float>()
        {
            { typeof(Pit), 40.0f },
            { typeof(Wall), 43.0f },
            { typeof(Animal), 45.0f },
        };

        Dictionary<Type, float> coordByY = new Dictionary<Type, float>()
        {
            { typeof(Pit), 13.0f },
            { typeof(Wall), 15.2f },
            { typeof(Animal), 14.7f },
        };


        public Wrapper<Obstacle> ReturnObstacle(ObstacleCreator obstacleCreator)
        {
            Obstacle obstacle = obstacleCreator.Create();
            Wrapper<Obstacle> obstacleWrapper = new Wrapper<Obstacle>(new Sprite(_dx2d, _dx2d.LoadBitmap(dictionary[obstacle.GetType()]), coordByX[obstacle.GetType()], coordByY[obstacle.GetType()], 0.0f), obstacle);
            return obstacleWrapper;
        }

        public Wrapper<Obstacle> DrawObs()
        {
            Random random = new Random();
            int y = random.Next(0, 3);
            ObstacleCreator obs = obstacleCreators[y];
            obstacleWrapper = ReturnObstacle(obs);
            return obstacleWrapper;
        }

        public void setObsData(object sender, EventArgs args)
        {
            obstacleWrapper = DrawObs();
            ListOfObstacles.Add(obstacleWrapper);
        }
    }
}
