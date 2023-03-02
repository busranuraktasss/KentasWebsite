const activeClass = "-active";
const timeout = 3500;

/**
 * Alert ver
 * @param {any} title Başlık
 * @param {any} explanation İçerik yazısı
 * @param {any} type 1-Success, 2-Warning, 3-Error, 4-Info
 */
const myAlert = (title, explanation, type = 1) => {


    let icon = '<i class="fa fa-check"></i>';
    switch (type) {
        case 1: icon = '<i class="fa fa-check fa-2x" style="color:#00b894"></i>'; break;
        case 2: icon = '<i class="fa fa-exclamation-triangle fa-2x" style="color:#fdcb6e"></i>'; break;
        case 3: icon = '<i class="fa fa-times fa-2x" style="color:#d63031"></i>'; break;
        case 4: icon = '<i class="fa fa-info-circle fa-2x" style="color:#0984e3"></i>'; break;
        default:
    }

    let noti = `<div class="notification">
                    <div class="notification__content">
                        <h2 class="notification__title" id="alert-title">Alert</h2>
                        <p class="notification__message" id="alert-explanation"></p>
                        <div class="notification__footer"><a href="javascript:myAlert_close();" class="notification__button -secondary"><i class="fa fa-times"></i></a></div>
                    </div>
                </div>`;

    $('body').append(noti);
    $('#alert-title').html(`${icon} ${title}`);
    $('#alert-explanation').html(explanation);
    document.querySelector(".notification").classList.add(activeClass);
    setTimeout(() => { myAlert_close(); }, timeout);
}

const myAlert_close = () => {
    if (document.querySelector(".notification") && document.querySelector(".notification").classList.length > 0)
        document.querySelector(".notification").classList.remove(activeClass);

    setTimeout(() => { $('.notification').remove(); }, 800);

}