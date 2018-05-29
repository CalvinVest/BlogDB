using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using Newtonsoft.Json;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace BlogDB.Core
{
    public class FileDB<T> : IBlogDB<T>
    {
        private readonly string DatabaseFilePath;// = Path.Combine(Directory.GetCurrentDirectory(), "blogDatabase.json");

        public FileDB(IConfiguration config){
            DatabaseFilePath = config["DatabaseFilePath"];
        }

        public List<T> ReadAll()
        {
            using (var reader = new StreamReader(new FileStream(DatabaseFilePath, FileMode.OpenOrCreate)))
            {
                var fileContents = reader.ReadToEnd();
                var posts = JsonConvert.DeserializeObject<List<T>>(fileContents);
                if (posts == null)
                {
                    posts = new List<T>();
                }
                return posts;
            }
        }

        public void WriteAll(List<T> listOfPosts)
        {
            // false means overwrite
            using (var writer = new StreamWriter(DatabaseFilePath, false))
            {
                var contentsToWrite = JsonConvert.SerializeObject(listOfPosts);
                writer.Write(contentsToWrite);
            }
        }
    }
}