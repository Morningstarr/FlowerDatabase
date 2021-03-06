using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Windows.Media.Imaging;

namespace FlowerClient
{
    class Mediator
    {
        public static Mediator instance = new Mediator();

        public NpgsqlConnection Connection { get; set; }
        public NpgsqlCommand Command { get; set; }
        public string SQL { get; set; }
        public string Login { get; set; }
        public string Role { get; set; }
        public string Path { get; set; }

        public object ConvertQueryToValue()
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(instance.SQL, instance.Connection);
            ds.Reset();
            da.Fill(ds);
            dt = ds.Tables[0];
            return dt.Rows[0].ItemArray[0];
        }

        public DataTable ConvertQueryToTable()
        {
            DataTable dt = new DataTable();
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(SQL, Connection);
            da.Fill(dt);
            return dt;
        }

        public List<string> ConvertQueryToComboBox()
        {
            DataTable dt = new DataTable();
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(SQL, Connection);
            da.Fill(dt);
            List<string> temp = new List<string>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                temp.Add(dt.Rows[i].ItemArray[0].ToString());
            }
            return temp;
        }

        public void Execute()
        {
            instance.Command = new NpgsqlCommand(instance.SQL, instance.Connection);
            instance.Command.ExecuteNonQuery();
        }

        public BitmapImage NonBlockingLoad(string path)
        {
            if (!(File.Exists(path)))
                return null;

            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
            image.UriSource = new Uri(path);
            image.EndInit();
            image.Freeze();
            return image;
        }

        public BitmapImage ByteToImage(byte[] imageData)
        {
            if (imageData == null)
                return null;

            BitmapImage biImg = new BitmapImage();
            MemoryStream ms = new MemoryStream(imageData);
            biImg.BeginInit();
            biImg.StreamSource = ms;
            biImg.EndInit();
            biImg.Freeze();
            return biImg;
        }

    }
}
