using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Channels;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["Db"].ConnectionString);
            sqlConnection.Open();

            if (sqlConnection.State == ConnectionState.Open)
            {
                Console.WriteLine("Подключение успешно!\n");
            }

            Console.Write("Введите Login: ");
            string login = Console.ReadLine();

            while (true)
            {
                Console.WriteLine("\n<=============- МЕНЮ -=============>");
                Console.Write("[1] Зашифровать сообщение\n[2] Получить сообщение\n[3] Выход\n<==================================>\n\nВыберите опцию: ");
                int menu = Convert.ToInt32(Console.ReadLine());

                if (menu == 1)
                {
                    Console.Write("\nВведите текст: ");
                    string text = Console.ReadLine().ToUpper();

                    List<char> textProc = new List<char> { };
                    List<int> textNum = new List<int> { };
                    char[] alphbet = new char[26] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
                    char[] alphbetId = new char[36] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
                    List<char> key = new List<char> { };
                    List<int> keyNum = new List<int> { };
                    List<int> ciphertextNum = new List<int> { };
                    List<char> ciphertextLiter = new List<char> { };
                    List<char> textId = new List<char> { };

                    Dictionary<char, int> cipher = new Dictionary<char, int>
            {
                {'A',0}, {'B',1}, {'C',2}, {'D',3}, {'E',4}, {'F',5}, {'G',6}, {'H',7}, {'I',8}, {'J',9}, {'K',10}, {'L',11}, {'M',12}, {'N',13}, {'O',14}, {'P',15}, {'Q',16}, {'R',17}, {'S',18}, {'T',19}, {'U',20}, {'V',21}, {'W',22}, {'X',23}, {'Y',24}, {'Z',25}
            };
                    Dictionary<int, char> deCipher = new Dictionary<int, char>
            {
                {0,'A'}, {1,'B'}, {2,'C'}, {3,'D'}, {4,'E'}, {5,'F'}, {6,'G'}, {7,'H'}, {8,'I'}, {9,'J'}, {10,'K'}, {11,'L'}, {12,'M'}, {13,'N'}, {14,'O'}, {15,'P'}, {16,'Q'}, {17,'R'}, {18,'S'}, {19,'T'}, {20,'U'}, {21,'V'}, {22,'W'}, {23,'X'}, {24,'Y'}, {25,'Z'}
            };

                    for (int i = 0; i < text.Length; i++) //перебор текста и запись в словарь
                    {
                        if (text[i] != ' ' && text[i] != '\'' && text[i] != ',' && text[i] != '.' && text[i] != '!' && text[i] != '-' && text[i] != ':' && text[i] != ';' && text[i] != '?')
                        {
                            textProc.Add(text[i]);
                        }
                    }
                    foreach (char l in textProc) //перебор текста в цифры
                    {
                        foreach (char i in cipher.Keys)
                        {
                            if (l == i)
                            {
                                textNum.Add(cipher[i]);
                            }
                        }
                    }

                    for (int i = 0; i < textProc.Count; i++) //создание ключа
                    {
                        Random rd = new Random();
                        int randomIndex = rd.Next(0, 26);
                        char randomLiter = alphbet[randomIndex];
                        key.Add(randomLiter);
                    }
                    foreach (char l in key) //перебор ключа в цифры
                    {
                        foreach (char i in cipher.Keys)
                        {
                            if (l == i)
                            {
                                keyNum.Add(cipher[i]);
                            }
                        }
                    }

                    for (int i = 0; i < 8; i++) //создание id
                    {
                        Random rd = new Random();
                        int randomIndex = rd.Next(0, 26);
                        char randomThg = alphbetId[randomIndex];
                        textId.Add(randomThg);
                    }
                    string resId = String.Concat(textId);

                    for (int ex = 0; ex < textNum.Count; ex++) //шифрование и запись текста в числах
                    {
                        int cipherEx = keyNum[ex] + textNum[ex];
                        if (cipherEx > 25)
                        {
                            ciphertextNum.Add((keyNum[ex] + textNum[ex]) - 26);
                        }
                        else
                        {
                            ciphertextNum.Add(keyNum[ex] + textNum[ex]);
                        }
                    }
                    foreach (int ex in ciphertextNum) //перебор и запись шифротекста в буквах
                    {
                        foreach (int el in deCipher.Keys)
                        {
                            if (ex == el)
                            {
                                ciphertextLiter.Add(deCipher[el]);
                            }
                        }
                    }
                    string textRes = String.Concat(ciphertextLiter);
                    string keyRes = String.Concat(key);

                    bool res = false;
                    SqlDataReader reader = null;
                    try
                    {
                        SqlCommand sqlCommand = new SqlCommand($"SELECT Id FROM dataTbl", sqlConnection);
                        reader = sqlCommand.ExecuteReader();

                        while (reader.Read())
                        {
                            if (resId == (string)reader[0])
                            {
                                textId.Clear();
                                while (resId == (string)reader[0])
                                {
                                    for (int i = 0; i < 8; i++) //создание id
                                    {
                                        Random rd = new Random();
                                        int randomIndex = rd.Next(0, 26);
                                        char randomThg = alphbetId[randomIndex];
                                        textId.Add(randomThg);
                                    }
                                    resId = String.Concat(textId);
                                }
                                res = true;
                            }
                            else
                            {
                                res = true;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("\n" + ex.Message);
                    }
                    finally
                    {
                        if (reader != null && !reader.IsClosed)
                        {
                            reader.Close();
                        }
                    }
                    if (res)
                    {
                        SqlCommand command = new SqlCommand("INSERT INTO dataTbl (Id, Login, Ciphertext) VALUES (@Id, @Login, @Ciphertext)", sqlConnection);
                        command.Parameters.AddWithValue("Id", resId);
                        command.Parameters.AddWithValue("Login", login);
                        command.Parameters.AddWithValue("Ciphertext", textRes);
                        command.ExecuteNonQuery();
                    }

                    Console.WriteLine($"\nId сообщения: {resId}\n\nКлюч: {keyRes}\n\nВаш текст: {textRes}");
                }
                if (menu == 2)
                {
                    Console.Write("\nВведите Login отправителя: ");
                    string loginSend = Console.ReadLine();
                    Console.Write("\nВведите Id сообщения: ");
                    string idSend = Console.ReadLine();
                    Console.Write("\nВведите ключ: ");
                    string keySend = Console.ReadLine();
                    string textSend = "";

                    SqlCommand command = new SqlCommand("SELECT Ciphertext FROM dataTbl WHERE (Id = @Id AND Login = @Login)", sqlConnection);
                    command.Parameters.AddWithValue("Id", idSend);
                    command.Parameters.AddWithValue("Login", loginSend);

                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        textSend = (string)reader[0];
                    }

                    string text = textSend;
                    string key = keySend;
                    List<char> textProc = new List<char> { };
                    List<int> textNum = new List<int> { };
                    List<char> keyLiter = new List<char> { };
                    List<int> keyNum = new List<int> { };
                    List<int> deciphertextNum = new List<int> { };
                    List<char> textRes = new List<char> { };

                    Dictionary<char, int> cipher = new Dictionary<char, int>
            {
                {'A',0}, {'B',1}, {'C',2}, {'D',3}, {'E',4}, {'F',5}, {'G',6}, {'H',7}, {'I',8}, {'J',9}, {'K',10}, {'L',11}, {'M',12}, {'N',13}, {'O',14}, {'P',15}, {'Q',16}, {'R',17}, {'S',18}, {'T',19}, {'U',20}, {'V',21}, {'W',22}, {'X',23}, {'Y',24}, {'Z',25}
            };
                    Dictionary<int, char> deCipher = new Dictionary<int, char>
            {
                {0,'A'}, {1,'B'}, {2,'C'}, {3,'D'}, {4,'E'}, {5,'F'}, {6,'G'}, {7,'H'}, {8,'I'}, {9,'J'}, {10,'K'}, {11,'L'}, {12,'M'}, {13,'N'}, {14,'O'}, {15,'P'}, {16,'Q'}, {17,'R'}, {18,'S'}, {19,'T'}, {20,'U'}, {21,'V'}, {22,'W'}, {23,'X'}, {24,'Y'}, {25,'Z'}
            };

                    for (int i = 0; i < text.Length; i++) //перебор шифротекста и запись в словарь
                    {
                        if (text[i] != ' ' && text[i] != '\'' && text[i] != ',' && text[i] != '.' && text[i] != '!' && text[i] != '-' && text[i] != ':' && text[i] != ';' && text[i] != '?')
                        {
                            textProc.Add(text[i]);
                        }
                    }
                    foreach (char l in textProc) //перебор шифротекста в цифры
                    {
                        foreach (char i in cipher.Keys)
                        {
                            if (l == i)
                            {
                                textNum.Add(cipher[i]);
                            }
                        }
                    }

                    for (int i = 0; i < key.Length; i++) //перебор ключ и запись в словарь
                    {
                        if (key[i] != ' ' && key[i] != '\'' && key[i] != ',' && key[i] != '.' && key[i] != '!' && key[i] != '-' && key[i] != ':' && key[i] != ';' && key[i] != '?')
                        {
                            keyLiter.Add(key[i]);
                        }
                    }
                    foreach (char l in keyLiter) //перебор ключа в цифры
                    {
                        foreach (char i in cipher.Keys)
                        {
                            if (l == i)
                            {
                                keyNum.Add(cipher[i]);
                            }
                        }
                    }

                    for (int ex = 0; ex < textNum.Count; ex++) //дешифровка и запись текста в числах
                    {
                        int cipherEx = textNum[ex] - keyNum[ex];
                        if (cipherEx >= 0)
                        {
                            deciphertextNum.Add(cipherEx);
                        }
                        else
                        {
                            deciphertextNum.Add(cipherEx + 26);
                        }
                    }

                    foreach (int ex in deciphertextNum) //перебор и записьтекста в буквах
                    {
                        foreach (int el in deCipher.Keys)
                        {
                            if (ex == el)
                            {
                                textRes.Add(deCipher[el]);
                            }
                        }
                    }

                    string textOut = String.Concat(textRes);
                    Console.WriteLine($"\nВаш текст: {textOut}");
                }
                if (menu == 3)
                {
                    break;
                }
            }
        }
    }
}
