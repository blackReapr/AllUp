﻿//===== slick Slider Product Quick View
$(document).ready(function () {

    const searchInput = document.querySelector(".searchInput");
    const searchCategory = document.querySelector(".searchCategory");
    const searchResult = document.querySelector(".searchResult");

    searchCategory.addEventListener("change", e => {
        searchResult.innerHTML = "";
    })

    searchInput.addEventListener("keyup", async e => {
        const search = e.target.value;
        searchResult.innerHTML = "";
        if (search.length > 3) {
            const { data } = await axios.get("/product/search", {
                params: {
                    categoryId: searchCategory.value,
                    query: e.target.value,
                }
            })
            searchResult.innerHTML = data;
        }
    })

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