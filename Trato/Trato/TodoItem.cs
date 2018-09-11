using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;


namespace Trato
{
    public class TodoItem
    {
        string id;
        string name;
        bool done;

        [JsonProperty(PropertyName = "id")]
        public string Id
        {
            get { return id; }
            set { id = value; }
        }

        [JsonProperty(PropertyName = "text")]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        [JsonProperty(PropertyName = "complete")]
        public bool Done
        {
            get { return done; }
            set { done = value; }
        }

        [Version]
        public string Version { get; set; }
    }
    /*
     
     Finally, use the client library to start working with the TodoItem table in your server project. 
     In your project, find a place where this interaction would make sense and add the following:
            
            CurrentPlatform.Init();
            TodoItem item = new TodoItem { Text = "Awesome item" };
            await MobileService.GetTable<TodoItem>().InsertAsync(item);


    Run the Xamarin project to start working with data in your mobile backend.
     
     */
}
