//===== slick Slider Product Quick View
$(document).ready(function () {

    const productModalLinks = document.querySelectorAll(".productModal");
    productModalLinks.forEach(item => item.addEventListener("click", e => {
        e.preventDefault();
        const url = item.getAttribute("href");
        axios.get(url).then(res => {
            document.querySelector(".modal-dialog").innerHTML = res.data;

            //===== slick Slider Product Quick View

            $('.quick-view-image').slick({
                slidesToShow: 1,
                slidesToScroll: 1,
                arrows: false,
                dots: false,
                fade: true,
                asNavFor: '.quick-view-thumb',
                speed: 400,
            });

            $('.quick-view-thumb').slick({
                slidesToShow: 4,
                slidesToScroll: 1,
                asNavFor: '.quick-view-image',
                dots: false,
                arrows: false,
                focusOnSelect: true,
                speed: 400,
            });
        })
    }))

})