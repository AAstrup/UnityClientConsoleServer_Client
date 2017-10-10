﻿using ClientServerSharedGameObjectMessages;

public class MessageHandler_ClientCommand_SpellHit : IMessageHandlerCommandClient
{
    public void Handle(object objData)
    {
        var data = (Message_ClientCommand_SpellHit)objData;
        var spellController = InGameWrapper.instance.spellsWrapper.GetSpellController(data.spellHitGmjID);
        var playerController = InGameWrapper.instance.playersWrapper.GetPlayerByGUID(data.playerGMJHit);
        spellController.Hit(playerController);
    }
}