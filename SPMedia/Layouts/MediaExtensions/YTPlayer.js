function openYTPlayerWin(url) {
    var wnd = window.open(url, 'YouTube', 'height=445,width=697,top=200,left=300,resizable');
    wnd.focus();
    return false;
}

var ytplayers = [];
var ytplayersinfolist = [];

function registerYTIFrameApi() {
    var tag = document.createElement('script');
    tag.src = "https://www.youtube.com/iframe_api";
    var firstScriptTag = document.getElementsByTagName('script')[0];
    firstScriptTag.parentNode.insertBefore(tag, firstScriptTag);
}

function initYTPlayer(playerInfo) {
    var player = new YT.Player(playerInfo.id, {
        height: playerInfo.height,
        width: playerInfo.width,
        videoId: playerInfo.videoId
    });
    ytplayers.push(player);
}

function loadYTPlayers() {
    _spBodyOnLoadFunctionNames.push('_loadYTPlayers');
}

function _loadYTPlayers() {
    //for (var i = 0; i < ytplayers.length; i++) {
    //    ytplayers[i].destroy();
    //}
    playerInfoList = [];
    $('.youtube-player').each(function () {
        var playerInfoParts = $(this).find('span').text().split(';');
        var playerInfo = { id: $(this).attr('id'),
            height: playerInfoParts[2],
            width: playerInfoParts[1],
            videoId: playerInfoParts[0]
        };
        playerInfoList.push(playerInfo);
    });
    onYouTubeIframeAPIReady();
}

function onYouTubeIframeAPIReady() {
    if (typeof window.playerInfoList === 'undefined')
        return;
    for (var i = 0; i < window.playerInfoList.length; i++) {
        initYTPlayer(window.playerInfoList[i]);
    }
}