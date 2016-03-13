Type.registerNamespace('MediaExtensions.Ribbon.RTE');

MediaExtensions.Ribbon.RTE.InsertLinkDialogArguments = function () { }
MediaExtensions.Ribbon.RTE.$create_InsertLinkDialogArguments = function () {
    return new MediaExtensions.Ribbon.RTE.InsertLinkDialogArguments();
}

MediaExtensions.Ribbon.RTE.ShareVideoDialog = function () {
    this.closedDialogCallback = Function.createDelegate(this, this.onClosed);
}
MediaExtensions.Ribbon.RTE.ShareVideoDialog.prototype = {

    show: function () {
        var dlgArgs = new RTE.InsertLinkDialogArguments();
        dlgArgs.allowRelativeLinks = false;
        dlgArgs.text = RTE.Cursor.get_range().get_text();
        RTE.DialogUtility.$5Q('MediaExtensions/RteDialog.aspx', 'CreateVideoLink', true, true, dlgArgs, null, this.closedDialogCallback, false, 400);
    },

    onClosed: function (result, retValue) {
        if (result === 1) {
            var range = RTE.Cursor.get_range();

            var playerInfo = { id: 'ytplayer' + (ytplayers.length + 1).toString(),
                height: retValue[2],
                width: retValue[1],
                videoId: retValue[0]
            };
            ytplayersinfolist.push(playerInfo);
            this.insertYTPlayer(range, playerInfo);
            //RTE.RteUtility.showRibbonTab('Ribbon.Link', 'LinkTab');

        }
    },
    insertYTPlayer: function (range, playerInfo) {
        var rangeParent = range.parentElement();
        if (!rangeParent) {
            return null;
        }

        var playerElement = rangeParent.ownerDocument.createElement('DIV');
        playerElement.id = playerInfo.id;
        playerElement.className = "youtube-player";
        range.insertBefore(playerElement);

        var playerInfoElement = playerElement.ownerDocument.createElement('SPAN');
        playerInfoElement.style.display = 'none'
        var playerInfoString = playerInfo.videoId + ';' + playerInfo.width + ';' + playerInfo.height;
        SP.UI.UIUtility.setInnerText(playerInfoElement, playerInfoString);
        playerElement.appendChild(playerInfoElement);


        playerInfo.id = playerInfo.id + '_preview';
        var playerPreeviewElement = rangeParent.ownerDocument.createElement('DIV');
        playerPreeviewElement.id = playerInfo.id;
        range.insertBefore(playerPreeviewElement);

        //range.moveToEndOfNode(videoPlayer);
        RTE.Cursor.update();
        initYTPlayer(playerInfo);
    }
}


MediaExtensions.Ribbon.RTE.VideoPageComponent = function () {
    MediaExtensions.Ribbon.RTE.VideoPageComponent.initializeBase(this);
}

MediaExtensions.Ribbon.RTE.VideoPageComponent.prototype = {
    mediaCommands: ['VideoGroup', 'ShareVideo', 'ShareVideoMenuOpen', 'ShareVideoMenuClose', 'EmbedVideo', 'InsertVideoWeb'],
    registerWithPageManager: function () {
        CUI.Page.PageManager.get_instance().addPageComponent(this);
    }, getFocusedCommands: function () {
        return this.mediaCommands;
    }, getGlobalCommands: function () {
        return this.mediaCommands;
    }, canHandleCommand: function (commandId) {
        return true;
    }, handleCommand: function (commandId, properties, sequence) {
        if (commandId === 'ShareVideo') {
            this.invokeVideoCommand(commandId);
            return true;
        }
        if (commandId === 'InsertVideoWeb') {
            this.invokeVideoCommand(commandId);
            return true;
        }
        return true;
    }, isFocusable: function () {
        return true
    }, receiveFocus: function () {
        return true;
    }, yieldFocus: function () {
        return true;
    }, invokeVideoCommand: function (commandId) {
        RTE.SnapshotManager.takeSnapshot();
        var range = RTE.Cursor.get_range();
        if (!range.get_isEditable()) {
            return;
        }
        var dlg = new MediaExtensions.Ribbon.RTE.ShareVideoDialog();
        dlg.show(commandId);
        RTE.SnapshotManager.takeSnapshot();
    }
}



MediaExtensions.Ribbon.RTE.ShareVideoDialog.registerClass('MediaExtensions.Ribbon.RTE.ShareVideoDialog');
MediaExtensions.Ribbon.RTE.VideoPageComponent.registerClass('MediaExtensions.Ribbon.RTE.VideoPageComponent', CUI.Page.PageComponent);



NotifyScriptLoadedAndExecuteWaitingJobs("sharevideoribbon.js");
function ShareVideoInit() {
    var currentPageManager = SP.Ribbon.PageManager.get_instance();
    currentPageManager.addPageComponent(new MediaExtensions.Ribbon.RTE.VideoPageComponent());
}