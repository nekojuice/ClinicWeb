//author: nkj
//20240203
//RWD擴充
$('#menu_toggle').on('click', function () {
    if ($("#sidebar-menu2").css("display") == "none" && $(document).width() < 975) {
        $("#sidebar-menu2").css("display", "block")
    }
    else {
        $("#sidebar-menu2").css("display", "none")
    }
});

//偵測視窗大小變動
const resizeObserver = new ResizeObserver(entries => {
    if (entries[0].target.clientWidth >= 975) {
        $("#sidebar-menu2").css("display", "none")
    }
    //重繪renderpage粉紅色區域
    outputsize()
})
// start observing a DOM node
//resizeObserver.observe(document.body)
resizeObserver.observe(document.getElementsByTagName("html")[0])

//手機版選單事件
$("#sidebar-menu2").find('a').on('click', function (ev) {
    var $li = $(this).parent();
    var openUpMenu = function () {
        $("#sidebar-menu2").find('li').removeClass('active active-sm');
        $("#sidebar-menu2").find('li ul').slideUp();
    }
    var setContentHeight = function () {
        // reset height
        $RIGHT_COL.css('min-height', $(window).height());

        var bodyHeight = $BODY.outerHeight(),
            footerHeight = $BODY.hasClass('footer_fixed') ? -10 : $FOOTER.height(),
            leftColHeight = $LEFT_COL.eq(1).height() + $SIDEBAR_FOOTER.height(),
            contentHeight = bodyHeight < leftColHeight ? leftColHeight : bodyHeight;

        // normalize content
        contentHeight -= $NAV_MENU.height() + footerHeight;

        $RIGHT_COL.css('min-height', contentHeight);
    };


    if ($li.is('.active')) {
        $li.removeClass('active active-sm');
        $('ul:first', $li).slideUp(function () {
            setContentHeight();
        });
    } else {
        // prevent closing menu if we are on child menu
        if (!$li.parent().is('.child_menu')) {
            openUpMenu();
        } else {
            if ($BODY.is('nav-sm')) {
                if (!$li.parent().is('child_menu')) {
                    openUpMenu();
                }
            }
        }

        $li.addClass('active');

        $('ul:first', $li).slideDown(function () {
            setContentHeight();
        });
    }
});

//動態更改粉紅底色
function outputsize() {
    //console.log(document.body.scrollHeight);
    $RIGHT_COL.css('height', 0);
    $RIGHT_COL.css('height', document.body.scrollHeight - 8)
}
