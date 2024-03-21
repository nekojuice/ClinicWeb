//資料更新後的閃爍動畫
function DataChanged_ColorAnimate(node) {
    //node = 該row或該cell html tag選擇器
    node.classList.remove('colorChange');
    requestAnimationFrame(function () {
        node.classList.add('colorChange');
        node.addEventListener('animationend', function () {
            node.classList.remove('colorChange');
        }, {
            once: true
        });
    });
}