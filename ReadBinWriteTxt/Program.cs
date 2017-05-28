using ReadBinWriteTxt.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadBinWriteTxt
{
    class Program
    {
        static void Main(string[] args)
        {
            //ReadTxtWriteBin();
            ReadBinWriteTxt();

            Console.ReadLine();
        }

        static void PrintConsole(List<Teacher> ListTeachers)
        {
            // IMPRIMIENDO EN CONSOLA
            foreach (var teacher in ListTeachers)
            {
                Console.WriteLine($"Id : {teacher.Id} Name : {teacher.Name}");
            }
        }
        static void ReadTxtWriteBin()
        {
            // Read File TXT
            List<Teacher> ListTeachers = new List<Teacher>();
            var lines = File.ReadAllLines("./Files/Teachers.txt");
            foreach (var line in lines)
            {
                string[] stringSeparated = line.Split('|');
                if (stringSeparated.Length == 2)
                {
                    ListTeachers.Add(new Teacher()
                    {
                        Id = int.Parse(stringSeparated[0]),
                        Name = stringSeparated[1]
                    });
                }
            }

            PrintConsole(ListTeachers);

            // Write File Bin
            var fileStream = File.Open("./Files/TeachersBinary.bin", FileMode.OpenOrCreate);
            var binFile = new BinaryWriter(fileStream);

            foreach (var teacher in ListTeachers)
            {
                binFile.Write(teacher.Id);
                binFile.Write(teacher.Name);
            }
            binFile.Close();
            fileStream.Close();
        }
        static void ReadBinWriteTxt()
        {
            // Read File Bin
            List<Teacher> ListTeachers = new List<Teacher>();
            var fileStream = File.OpenRead("./Files/TeachersBinary.bin");
            var binReader = new BinaryReader(fileStream);

            int band = 0;
            long length = binReader.BaseStream.Length;

            while (band < length)
            {
                var teacher = new Teacher()
                {
                    Id = binReader.ReadInt32(),
                    Name = binReader.ReadString()
                };
                ListTeachers.Add(teacher);

                band += sizeof(int);
                foreach (char c in teacher.Name)
                    band += sizeof(char);
            }
            binReader.Close();
            fileStream.Close();

            PrintConsole(ListTeachers);

            // Write File TXT
            var fileStream = new StreamWriter("./Files/Teachers.txt");
            foreach (var teacher in ListTeachers)
            {
                fileStream.WriteLine($"{teacher.Id}|{teacher.Name}");
            }
            fileStream.Close();
        }
    }
}
