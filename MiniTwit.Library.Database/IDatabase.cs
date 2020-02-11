using System;
using
using System.Collections.Generic;

namespace MiniTwit.Library.Database
{
    public interface IDatabase
    {
        string GetUserFromId(int id);

        bool FlagMessage(int messageId);

        bool InitDatabase();

        List<Message> GetAllMessages();

#region delettis

        




#endregion

    


    } 
}
