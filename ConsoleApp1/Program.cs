using System.Text.Json.Nodes;
using System.Threading;

class Programm
{
    static public async Task Main()
    {
        bool exit = true;
        while (exit) 
        {
            Console.WriteLine("Выберите действие: \n1) создание файла\n2)Дозапись в файл\n3)Запись через потоки \n4) отправка на сервер \n5)выход");
            int vibor = int.Parse(Console.ReadLine());
            using (StreamWriter sw = File.AppendText("log.txt"))
            {
                sw.WriteLine("Выберите действие: \n1) создание файла\n2)Дозапись в файл\n3)Запись через потоки \n4) отправка на сервер \n5)выход");
                sw.WriteLine(vibor);
            }
            switch (vibor)
            {
                case 1:
                    {
                        try
                        {
                            Console.WriteLine("Файл создан");
                            using (StreamWriter sw = File.AppendText("log.txt"))  
                                sw.WriteLine("Файл создан");
                            writeText();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Ошибка в создании файла: {ex.Message}");
                        }
                        break;
                    }
                case 2:
                    {
                        try
                        {
                            Console.WriteLine("Файл дозаписан");
                            dozapisanText();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Ошибка записи в файл: {ex.Message}");
                        }
                        break;
                    }
                case 3:
                    {
                        try
                        {
                            potokText();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Ошибка записи в файл: {ex.Message}");
                        }
                        break;
                    }
                case 4:
                    {
                        try
                        {
                            HttpClient client = new HttpClient();
                            string url = $"https://jsonplaceholder.typicode.com/posts";
                            string result = await client.GetStringAsync(url);
                            using (StreamWriter sw = File.AppendText("data.txt"))
                            {
                                sw.WriteLine(result);
                            }
                            vivodText();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Ошибка записи в файл: {ex.Message}");
                        }
                        break;
                    }
                case 5:     
                    {
                        exit = false;
                        break;
                    }
            }
        }
    }
    static void vivodText()
    {
        string[] lines = File.ReadAllLines("data.txt");
        foreach (string line in lines)
        {
            Console.WriteLine(line);
        }
    }
    static void dozapisanText()
    {
        using (StreamWriter sw = File.AppendText("data.txt"))
        {
            sw.WriteLine("Новая строка данных");
        }
        using (StreamWriter sw = File.AppendText("log.txt"))
            sw.WriteLine("Новая строка данных");
        vivodText();
    }
    static void writeText()
    {
        using (StreamWriter sw = File.CreateText("data.txt"))
        {
            sw.WriteLine("Новая строка данных");
        }
        using (StreamWriter sw = File.AppendText("log.txt"))
            sw.WriteLine("Новая строка данных");
        vivodText();
    }
    static void ProcessData()
    {
        Mutex mutex = new Mutex();

        mutex.WaitOne();
        Console.WriteLine($"Сейчас активен поток: {Thread.CurrentThread.Name}");
        mutex.ReleaseMutex();
    }
    static void ThreadProc()
    {
        ProcessData();
    }
    static void potokText()
    {
        for (int i = 0; i < 3; i++)
        {

            Thread thread = new Thread(ThreadProc);
            thread.Name = String.Format("Thread{0}", i + 1);
            using (StreamWriter sw = File.AppendText("log.txt"))
               sw.WriteLine($"Сейчас активен поток: {Thread.CurrentThread.Name}");
            thread.Start();
            dozapisanText();
        }
    }
}