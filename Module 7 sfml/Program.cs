using System;
using SFML.Learning;
using SFML.Graphics;
using SFML.Audio;
using SFML.Window;
class Program : Game 
{
    static string backgroundTexture = LoadTexture("background.png");
    static string playerTexture = LoadTexture("player.png");
    static string foodTexture = LoadTexture("food.png");

    static string foodSound = LoadSound("foodSound2.wav");
    static string meowSound = LoadSound("meow_sound.wav");
    static string bgMusic = LoadMusic("bg_music.wav");
    
    static float playerX = 30;
    static float playerY = 200;
    static float playerSpeed = 400;
    static int playerSize = 56;
    static int PlayerDirection = 1;
    static int PlayerScore = 0;
    static int PlayerBest = 0;
    static float foodX;
    static float foodY;
    static int foodSize = 32;
    static bool isLose = false;
    static void PlayerMove()
    {
        if (GetKey(Keyboard.Key.W) == true) PlayerDirection = 0;
        if (GetKey(Keyboard.Key.D) == true) PlayerDirection = 1;
        if (GetKey(Keyboard.Key.S) == true) PlayerDirection = 2;
        if (GetKey(Keyboard.Key.A) == true) PlayerDirection = 3;

        if (PlayerDirection == 0) playerY -= playerSpeed*DeltaTime;
        if (PlayerDirection == 1) playerX += playerSpeed*DeltaTime;
        if (PlayerDirection == 2) playerY += playerSpeed*DeltaTime;
        if (PlayerDirection == 3) playerX -= playerSpeed*DeltaTime;
    }

    static void DrawPlayer()
    {
        if (PlayerDirection == 0) DrawSprite(playerTexture, playerX, playerY, 64, 64, playerSize, playerSize);
        if (PlayerDirection == 1) DrawSprite(playerTexture, playerX, playerY, 0, 0, playerSize, playerSize);
        if (PlayerDirection == 2) DrawSprite(playerTexture, playerX, playerY, 0, 64, playerSize, playerSize);
        if (PlayerDirection == 3) DrawSprite(playerTexture, playerX, playerY, 64, 0, playerSize, playerSize);
    }

    static void Main(string[] args)
    {
        InitWindow(800, 600, "Cool Game");

        SetFont("comic.ttf");

        Random rnd = new Random();
        foodX = rnd.Next(0, 800 - foodSize);
        foodY = rnd.Next(200, 600 - foodSize);

        PlayMusic(bgMusic, 7);
        
        while (true)
        {
            DispatchEvents();
            if(isLose == false)
            {
                PlayerMove();

                if (playerX + playerSize > foodX && playerX < foodX + foodSize && playerY + playerSize > foodY && playerY < foodY + foodSize)
                {
                    foodX = rnd.Next(0, 800 - foodSize);
                    foodY = rnd.Next(200, 600 - foodSize);
                    PlayerScore += 1;
                    playerSpeed += 10;

                    PlaySound(foodSound);

                    if (PlayerScore % 5 == 0) PlaySound(meowSound, 10);
                }

                if (playerX + playerSize > 800 || playerX < 0 || playerY + playerSize > 600 || playerY < 150)
                {
                    isLose = true;
                }
            }

            if (isLose == true)
            {
                if (PlayerScore > PlayerBest) PlayerBest = PlayerScore;
                if (GetKeyDown(Keyboard.Key.R) == true)
                {
                    isLose = false;
                    playerX = 300;
                    playerY = 220;
                    playerSpeed = 400;
                    PlayerDirection = 1;
                    PlayerScore = 0; 
                }
            }


            ClearWindow();

            DrawSprite(backgroundTexture,0,0);

            if (isLose == true)
            {
                SetFillColor(Color.Cyan);
                FillRectangle(200, 200, 500, 165);
                SetFillColor(Color.Black);
                DrawText(300, 200, "Вы проиграли!", 38);
                DrawText(200, 250, "Нажми \"R\" чтобы перезапустить игру!", 26);
                DrawText(300, 300, $"Ваш счет: {PlayerScore}.", 24);
                DrawText(300, 335, $"Рекорд: {PlayerBest}.", 24);
            }

            DrawPlayer();

            DrawSprite(foodTexture, foodX, foodY);


            DisplayWindow();

            Delay(1);
        }
        Console.ReadLine();
    }
}

