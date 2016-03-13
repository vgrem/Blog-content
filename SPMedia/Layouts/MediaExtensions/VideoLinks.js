function openYTPlayerWin(url) {
    var wnd = window.open(url, 'YouTube', 'height=445,width=697,top=200,left=300,resizable');
    wnd.focus();
    return false;
}
