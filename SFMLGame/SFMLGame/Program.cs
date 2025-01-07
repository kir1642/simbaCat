using System;
using SFML.Learning;
using SFML.Graphics;
using SFML.Window;
using SFML.Audio;

internal class Program : Game
{
    static string bgTexture = LoadTexture("bg.png");
    static string plTexture = LoadTexture("pl.png");
    static string foodTexture = LoadTexture("food.png");
    
    static string meow = LoadSound("wildcat.ogg");
    static string crash = LoadSound("cat_crash_sound.ogg");
    static string bgMusic = LoadMusic("nfs.ogg");
    static string track1 = LoadMusic("getLow.ogg");
    static Music myMusic = new Music(bgMusic); // создание объекта 
    
    static float playerX = 300;
    static float playerY = 220;
    static int playerSize = 56;

    static float playerSpeed = 80;
    static int playerDirection = 1;

    static int playerScore = 0;

    static float foodX;
    static float foodY;
    static int foodSize = 32;

    static bool isLoose = false;
    static bool start = false;

    static int foodRecord = 0;
    static float speedRecord = 0;

    static void PlayerMove()
    {
        if (GetKey(Keyboard.Key.W) == true) playerDirection = 0;
        if (GetKey(Keyboard.Key.S) == true) playerDirection = 1;
        if (GetKey(Keyboard.Key.A) == true) playerDirection = 2;
        if (GetKey(Keyboard.Key.D) == true) playerDirection = 3;

        if (playerDirection == 0) playerY -= playerSpeed * DeltaTime;
        if (playerDirection == 1) playerY += playerSpeed * DeltaTime;
        if (playerDirection == 2) playerX -= playerSpeed * DeltaTime;
        if (playerDirection == 3) playerX += playerSpeed * DeltaTime;

    }
    static void DrawPlayer()
    {
        if (playerDirection == 0) DrawSprite(plTexture, playerX, playerY, 64, 64, playerSize,playerSize );
        if (playerDirection == 1) DrawSprite(plTexture, playerX, playerY, 0, 64, playerSize,playerSize );
        if (playerDirection == 2) DrawSprite(plTexture, playerX, playerY, 64, 0, playerSize,playerSize );
        if (playerDirection == 3) DrawSprite(plTexture, playerX, playerY, 0, 0, playerSize,playerSize );
    }

    static void Main(string[] args)
    {
        InitWindow(800, 600, "SimbaCat");
        SetFont("minecraft.ttf");

        Random rnd = new Random();
        foodX = rnd.Next(0, 800 - foodSize);
        foodY = rnd.Next(180, 600 - foodSize);


        myMusic.Loop = true;
        myMusic.Volume = 30;
        myMusic.Play();                    
        while (true)
        {   
            // 1. расчет
            DispatchEvents(); 
            // игровая логика
            if (GetKeyDown(Keyboard.Key.Space) == true) start = true;
            if (GetKeyDown(Keyboard.Key.F) == true)
            {
                myMusic.Stop();
                PlayMusic(track1, 70);
            }

            if (isLoose == false && start)
            {
                PlayerMove();

                if (playerX + playerSize > foodX && playerX < foodX + foodSize &&
                playerY + playerSize > foodY && playerY < foodY + foodSize)
                {
                    foodX = rnd.Next(50, 750 - foodSize);
                    foodY = rnd.Next(180, 550 - foodSize);

                    playerScore += 1;
                    playerSpeed += Convert.ToSingle(80) / Convert.ToSingle(playerScore);

                    PlaySound(meow, rnd.Next(50, 70));

                    /* Console.Clear();
                    Console.Write("sCORE " + playerScore);
                    Console.Write("\nplayerSpeed " + playerSpeed); */
                }
                // loose check 
                if (playerX + playerSize > 800 || playerX < 0 || playerY + playerSize > 600 || playerY < 160)
                {
                    isLoose = true;
                    if (playerScore > foodRecord) 
                    {
                        foodRecord = playerScore;
                    }
                    if (playerSpeed > speedRecord)
                    {
                        speedRecord = playerSpeed;
                    }
                    PlaySound(crash, 70);

                    /* Console.Clear();
                    Console.Write("sCORE " + playerScore);
                    Console.WriteLine("\nVSEKAPEZ");
                    Console.Write("\nplayerSpeed " + playerSpeed); */

                }
            }
            
            // 2. очистка
            ClearWindow();
            // 3. отрисовка
            DrawSprite(bgTexture, 0,0);
            DrawPlayer();
            DrawSprite(foodTexture, foodX, foodY, 0, 0, foodSize, foodSize);
            
            if (start == false)
            {
                SetFillColor(101, 99, 102);
                DrawText(202, 280, "Управление:\nW - вверх\nS - вниз\nA - влево\nD - вправо\n", 26);
                SetFillColor(164, 62, 214);
                DrawText(202, 443, "Жмякни \"Пробел\" чтобы начать", 20);
                
            }
            
            if (isLoose == true)
            {
                SetFillColor(101, 99, 102);
                DrawText(202, 300, "Вот и настал этот час...", 32);
                SetFillColor(19, 171, 9);
                DrawText(202, 370, "Рекорд еды = "+foodRecord.ToString(), 28);
                DrawText(202, 400, "Рекорд скорости = " + speedRecord.ToString(), 28);

                SetFillColor(164, 62, 214);
                DrawText(202, 447, "Жмякни \"Пробел\" для перезапуска", 18);

                if (GetKeyDown(Keyboard.Key.Space) == true)
                {
                    isLoose = false;
                    playerX = 333;
                    playerY = 222;
                    playerSpeed = 80;
                    playerDirection = 1;
                    playerScore = 0;
                }
            }

            SetFillColor(79, 65, 11);
            DrawText(20, 4, "Поглощено кошачьей еды:  "+playerScore.ToString(), 16);
            DrawText(480, 4, "Скорость кошачка:  " + playerSpeed.ToString(), 16);

            DisplayWindow();
            // 4. ожидание
            Delay(1);
        }
    }
}

