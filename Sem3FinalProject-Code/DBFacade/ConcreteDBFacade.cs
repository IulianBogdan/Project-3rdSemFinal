using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sem3FinalProject_Code.Models;
using System.IO;
using System.Diagnostics;
using System.Data.SqlClient;
using System.Data;

namespace Sem3FinalProject_Code.DBFacade
{
    public class ConcreteDBFacade : IDBFacade
    {
        private struct DBProperty
        {
            public int id;
            public string name;

            public DBProperty(int id, string name)
            {
                this.id = id;
                this.name = name;
            }
        }

        private struct DBItemType
        {
            public int id;
            public string name;

            public DBItemType(int id, string name)
            {
                this.id = id;
                this.name = name;
            }
        }

        private struct DBItem
        {
            public int id;
            public string name;
            public string productNumber;
            public string itemType;

            public DBItem(int id, string name, string productNumber, string itemType)
            {
                this.id = id;
                this.name = name;
                this.productNumber = productNumber;
                this.itemType = itemType;
            }
        }

        private SqlConnection connection;
        private string server;
        private string database;
        private string uid;
        private string password;

        public ConcreteDBFacade()
        {
            StreamReader secretFile = new StreamReader(new FileStream("C:\\Users\\utente\\Desktop\\FinalProject\\Sem3FinalProject-Code\\dbconnect.secret", FileMode.Open));
            server = secretFile.ReadLine();
            database = secretFile.ReadLine();
            uid = secretFile.ReadLine();
            password = secretFile.ReadLine();
            string connectionString = "Server=" + server + "; " + "Database=" + database + "; " + "User ID=" + uid + "; " + "Password=" + password + ";";
            Debug.Print(connectionString);
            connection = new SqlConnection(connectionString);
        }

        private void OpenConnection()
        {
            connection.Open();
        }

        private void CloseConnection()
        {
            connection.Close();
        }

        private IDictionary<string, DBProperty> GetDBProperties()
        {
            string query = "SELECT * FROM Property";
            IDictionary<string, DBProperty> properties = new Dictionary<string, DBProperty>();

            if (connection.State == System.Data.ConnectionState.Open)
            {
                SqlCommand cmd = new SqlCommand(query, connection);
                SqlDataReader dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    int id = dataReader.GetInt32(0);
                    string name = dataReader.GetString(1);
                    DBProperty p = new DBProperty(id, name);
                    properties.Add(name, p);
                }

                dataReader.Close();
            }

            return properties;
        }

        private int? GetItemId(string productNumber, string producerEmail)
        {
            int? id = null;

            string query = "SELECT ItemId FROM Item WHERE ProductNumber=@ProductNumber AND ProducerEmail=@ProducerEmail";

            if (connection.State == System.Data.ConnectionState.Open)
            {
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.Add(new SqlParameter("ProductNumber", productNumber));
                cmd.Parameters.Add(new SqlParameter("ProducerEmail", producerEmail));
                SqlDataReader dataReader = cmd.ExecuteReader();

                if (dataReader.Read())
                {
                    id = dataReader.GetInt32(0);
                }

                dataReader.Close();
            }

            return id;
        }

        private IDictionary<string, DBItemType> GetDBItemTypes()
        {
            string query = "SELECT * FROM ItemType";
            IDictionary<string, DBItemType> itemTypes = new Dictionary<string, DBItemType>();

            if (connection.State == System.Data.ConnectionState.Open)
            {
                SqlCommand cmd = new SqlCommand(query, connection);
                SqlDataReader dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    int id = dataReader.GetInt32(0);
                    string name = dataReader.GetString(1);
                    DBItemType it = new DBItemType(id, name);
                    itemTypes.Add(name, it);
                }

                dataReader.Close();
            }

            return itemTypes;
        }

        public void AddProducer(string producerEmail, string producerName)
        {
            OpenConnection();

            string query = "INSERT INTO Producer VALUES (@Email, @Name)";

            SqlCommand cmd = new SqlCommand(query, connection);

            cmd.Parameters.Add(new SqlParameter("Email", producerEmail));
            cmd.Parameters.Add(new SqlParameter("Name", producerName));
            cmd.ExecuteNonQuery();

            CloseConnection();
        }

        public void AddItems(Item[] items, string producerEmail)
        {
            OpenConnection();

            IDictionary<string, DBProperty> properties = GetDBProperties();
            IDictionary<string, DBItemType> itemTypes = GetDBItemTypes();

            string queryItem = "INSERT INTO Item (Name, ProductNumber, ItemTypeId, ProducerEmail) VALUES (@Name, @ProductNumber, @ItemTypeId, @ProducerEmail);SELECT CAST(scope_identity() AS int)";
            string queryProperty = "INSERT INTO ItemPropertyValue (PropertyId, ItemId, Value) VALUES (@PropertyId, @ItemId, @Value)";

            SqlCommand cmd = new SqlCommand("", connection);
            for (int i = 0; i < items.Length; i++)
            {
                cmd.CommandText = queryItem;
                cmd.Parameters.Add(new SqlParameter("Name", items[i].Name));
                cmd.Parameters.Add(new SqlParameter("ProductNumber", items[i].ProductNumber));
                cmd.Parameters.Add(new SqlParameter("ItemTypeId", itemTypes[items[i].Type.Name].id));
                cmd.Parameters.Add(new SqlParameter("ProducerEmail", producerEmail));
                int itemId = (int)cmd.ExecuteScalar();

                foreach (KeyValuePair<string, DBProperty> kvp in properties)
                {
                    Property p = items[i].GetProperty(kvp.Key);
                    if (p != null)
                    {
                        cmd.CommandText = queryProperty;
                        cmd.Parameters.Add(new SqlParameter("PropertyId", kvp.Value.id));
                        cmd.Parameters.Add(new SqlParameter("ItemId", itemId));
                        cmd.Parameters.Add(new SqlParameter("Value", p.Value));

                        cmd.ExecuteNonQuery();
                    }
                    cmd.Parameters.Clear();
                }
                cmd.Parameters.Clear();
            }

            CloseConnection();
        }

        public void DeleteItems(Item[] items, string producerEmail)
        {
            string query = "UPDATE Item SET Active=0 WHERE ProductNumber=@ProductNumber AND ProducerEmail=@ProducerEmail";

            OpenConnection();

            SqlCommand cmd = new SqlCommand("", connection);
            for (int i = 0; i < items.Length; i++)
            {
                cmd.CommandText = query;
                cmd.Parameters.Add(new SqlParameter("ProductNumber", items[i].ProductNumber));
                cmd.Parameters.Add(new SqlParameter("ProducerEmail", producerEmail));

                cmd.ExecuteNonQuery();

                cmd.Parameters.Clear();
            }

            CloseConnection();
        }

        public IList<Item> GetItems(string producerEmail)
        {
            string itemQuery = "SELECT Item.ItemId,Item.Name,Item.ProductNumber,ItemType.Name FROM Item,ItemType WHERE ProducerEmail=@ProducerEmail AND Item.ItemTypeId=ItemType.ItemTypeId AND Item.Active = 1";
            string propertiesQuery = "SELECT ItemPropertyValue.Value, Property.Name FROM ItemPropertyValue,Property WHERE ItemId=@ItemId AND Property.PropertyId = ItemPropertyValue.PropertyId";
            IList<Item> items = new List<Item>();
            IList<DBItem> dBItems = new List<DBItem>();
            IDictionary<int, IDictionary<string, string>> itemsProperties = new Dictionary<int, IDictionary<string, string>>();
            PropertyTypeFactory propertyTypeFactory = new PropertyTypeFactory();

            OpenConnection();

            IDictionary<string, DBItemType> itemTypes = GetDBItemTypes();

            SqlCommand cmd = new SqlCommand(itemQuery, connection);
            cmd.Parameters.Add(new SqlParameter("ProducerEmail", producerEmail));
            SqlDataReader dataReader = cmd.ExecuteReader();

            while (dataReader.Read())
            {
                int id = dataReader.GetInt32(0);
                string name = dataReader.GetString(1);
                string productNumber = dataReader.GetString(2);
                string itemType = dataReader.GetString(3);

                dBItems.Add(new DBItem(id, name, productNumber, itemType));
            }

            dataReader.Close();

            foreach (DBItem dBItem in dBItems)
            {
                IDictionary<string, string> properties = new Dictionary<string, string>();

                cmd.CommandText = propertiesQuery;
                cmd.Parameters.Add(new SqlParameter("ItemId", dBItem.id));

                dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    string propertyName = dataReader.GetString(1);
                    string propertyValue = dataReader.GetString(0);
                    properties.Add(propertyName, propertyValue);
                }

                cmd.Parameters.Clear();
                dataReader.Close();
                itemsProperties.Add(dBItem.id, properties);
            }

            CloseConnection();

            foreach (DBItem dBItem in dBItems)
            {
                Item i = new Item(dBItem.name, dBItem.productNumber, GetItemType(dBItem.itemType), itemsProperties[dBItem.id]);
                items.Add(i);
            }

            return items;
        }

        public ItemType GetItemType(string typeName)
        {
            ItemType type = null;

            if (connection.State == System.Data.ConnectionState.Closed)
                OpenConnection();
            IDictionary<string, DBItemType> itemTypes = GetDBItemTypes();

            if (itemTypes.ContainsKey(typeName))
            {
                IDictionary<string, Property> defaultProp = new Dictionary<string, Property>();
                PropertyTypeFactory propertyTypeFactory = new PropertyTypeFactory();
                string query = "Select * From ItemTypeDefaultProperty,Property,PropertyType WHERE  ItemTypeId = @ItemTypeId AND ItemTypeDefaultProperty.PropertyId = Property.PropertyId AND Property.PropertyTypeId = PropertyType.PropertyTypeId";

                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.Add(new SqlParameter("ItemTypeId", itemTypes[typeName].id));

                SqlDataReader dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    string value = dataReader.GetString(2);
                    string propertyName = dataReader.GetString(4);
                    string propertyTypeName = dataReader.GetString(7);
                    IPropertyType propertyType = propertyTypeFactory.GetPropertyType(propertyTypeName);
                    Property p = new Property(value, propertyName, propertyType);
                    defaultProp.Add(propertyName, p);
                }

                cmd.Parameters.Clear();
                dataReader.Close();

                type = new ItemType(itemTypes[typeName].name, defaultProp);
            }
            if (connection.State == ConnectionState.Open)
                CloseConnection();

            return type;
        }

        public bool HasItem(Item item, string producerEmail)
        {
            string query = "SELECT * FROM Item WHERE ProducerEmail=@ProducerEmail AND ProductNumber=@ProductNumber";

            OpenConnection();

            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.Add(new SqlParameter("ProducerEmail", producerEmail));
            cmd.Parameters.Add(new SqlParameter("ProductNumber", item.ProductNumber));
            SqlDataReader dataReader = cmd.ExecuteReader();

            bool exists = dataReader.HasRows;

            dataReader.Close();
            CloseConnection();

            return exists;
        }

        public void UpdateItems(Item[] items, string producerEmail)
        {
            OpenConnection();

            IDictionary<string, DBProperty> properties = GetDBProperties();
            IDictionary<string, DBItemType> itemTypes = GetDBItemTypes();

            string queryItem = "UPDATE Item SET Name=@ProductName, ItemTypeId=@ItemTypeId WHERE ProductNumber=@ProductNumber AND ProducerEmail=@ProducerEmail";
            string queryProperty = "UPDATE ItemPropertyValue SET Value=@PropertyValue WHERE PropertyId=@PropertyId AND ItemId=@ItemId";


            SqlCommand cmd = new SqlCommand("", connection);
            for (int i = 0; i < items.Length; i++)
            {
                int? itemId = GetItemId(items[i].ProductNumber, producerEmail);
                if (itemId == null)
                    throw new KeyNotFoundException("The item with PN:" + items[i].ProductNumber + " does not exist in the database");

                cmd.CommandText = queryItem;
                cmd.Parameters.Add(new SqlParameter("ProductName", items[i].Name));
                cmd.Parameters.Add(new SqlParameter("ItemTypeId", itemTypes[items[i].Type.Name].id));
                cmd.Parameters.Add(new SqlParameter("ProductNumber", items[i].ProductNumber));
                cmd.Parameters.Add(new SqlParameter("ProducerEmail", producerEmail));
                cmd.ExecuteNonQuery();

                foreach (KeyValuePair<string, DBProperty> kvp in properties)
                {
                    Property p = items[i].GetProperty(kvp.Key);
                    if (p != null)
                    {
                        cmd.CommandText = queryProperty;
                        cmd.Parameters.Add(new SqlParameter("PropertyId", kvp.Value.id));
                        cmd.Parameters.Add(new SqlParameter("ItemId", itemId));
                        cmd.Parameters.Add(new SqlParameter("PropertyValue", p.Value));
                        cmd.ExecuteNonQuery();

                    }
                    cmd.Parameters.Clear();
                }
                cmd.Parameters.Clear();
            }

            CloseConnection();
        }
    }
}